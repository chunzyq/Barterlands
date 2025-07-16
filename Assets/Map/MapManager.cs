using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Barterlands.Logging;
using Zenject;

public class MapManager : MonoBehaviour
{
    public List<MapPointData> mapPoints;
    [Inject] StalkerUnitManager stalkerUnitManager;
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

        view.onPointSelected += selectedData => {

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
        if (mapPointData.isUnlocked)
        {
            StartCoroutine(RaidCoroutine(mapPointData));
        }
        else
        {
            Debug.Log($"Точка {mapPointData.pointName} заблокирована!");
        }
    }

    IEnumerator RaidCoroutine(MapPointData mapPointData)
    {

        isRaidBusy = true;
        Debug.Log($"Рейд на {mapPointData.pointName}...");
        yield return new WaitForSeconds(2f);

        // resourseManager.metalAmount += mapPointData.metalReward;

        _logger.Info($"Рейд завершён, открываем следующие точки...");
        _logger.Error($"СТАЛКЕРЫ НА КАРТЕ АААА {stalkerUnitManager.stalkers.Count}");

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
}
