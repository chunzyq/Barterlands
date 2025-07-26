using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Barterlands.Logging;
using Zenject;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.NCalc;
using System;

public class MapManager : MonoBehaviour
{
    public List<MapPointData> mapPoints;
    [Inject] StalkerUnitManager stalkerUnitManager;
    [Inject] ResourseManager resourseManager;
    public GameObject pointPrefab;
    public Transform pointsParent;
    public MarkerPathController markerController;
    bool isRaidBusy = false;
    private Dictionary<MapPointData, PointView> spawned = new Dictionary<MapPointData, PointView>();
    private ILoggerService _logger;

    void Awake()
    {
        _logger = new UnityLogger();
    }

    public void Start()
    {

        markerController.OnTargetReached += OnPointSelected;

        foreach (var data in mapPoints)
        {
            if (data.isUnlocked)
            {
                SpawnPoint(data);
            }
        }
    }

    void SpawnPoint(MapPointData data)
    {
        var go = Instantiate(pointPrefab, pointsParent);
        var rt = go.GetComponent<RectTransform>();
        go.GetComponent<RectTransform>().anchoredPosition = data.pointPosition;
        var view = go.GetComponent<PointView>();
        view.Setup(data);

        view.onPointSelected += selectedData =>
        {

            if (isRaidBusy)
            {
                return;
            }

            markerController.SetTarget(rt, selectedData);
        };

        spawned[data] = view;
    }

    public void OnPointSelected(MapPointData mapPointData)
    {
        if (stalkerUnitManager.stalkers.Count >= 1)
        {
            StartCoroutine(RaidCoroutine(mapPointData));
        }
        else
        {
            _logger.Error("Ошибка! (как ты вообще сюда попал)");
        }
    }

    IEnumerator RaidCoroutine(MapPointData mapPointData)
    {

        isRaidBusy = true;
        _logger.Info($"Рейд на {mapPointData.pointName}...");
        yield return new WaitForSeconds(2f);

        var rewardMessage = new List<string>();

        foreach (var reward in mapPointData.rewards)
        {
            if (reward.amount > 0)
            {
                resourseManager.AddResource(reward.type, reward.amount);
                rewardMessage.Add($"{reward.amount} {GetResourceName(reward.type)}");
            }
        }

        if (rewardMessage.Count > 0)
        {
            _logger.Info($"Получено: {string.Join(", ", rewardMessage)}");
        }
        else
        {
            _logger.Info("Ресурсы не были получены");
        }

        _logger.Info($"Рейд завершён, открываем следующие точки...");

        foreach (var next in mapPointData.nextPoints)
        {
            if (!next.isUnlocked)
            {
                next.isUnlocked = true;
                SpawnPoint(next);
            }
        }

        isRaidBusy = false;

    }

    private object GetResourceName(ResourceType key)
    {
        switch (key)
    {
        case ResourceType.Metal: return "металла";
        default: return key.ToString().ToLower();
    }
    }
}
