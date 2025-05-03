using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<MapPointData> mapPoints;

    public GameObject pointPrefab;
    public Transform pointsParent;
    public MarkerPathController markerController;
    private Dictionary<MapPointData, PointView> spawned = new Dictionary<MapPointData, PointView>();

    public void Start()
    {
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
        go.GetComponent<RectTransform>().anchoredPosition = data.pointPosition;
        var view = go.GetComponent<PointView>();
        view.Setup(data);

        view.onPointSelected += (selectedData) => {
        // найдём RectTransform этого View
            var rt = go.GetComponent<RectTransform>();
            markerController.SetTarget(rt);
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

        Debug.Log($"Рейд на {mapPointData.pointName}...");
        yield return new WaitForSeconds(2f);
        Debug.Log($"Рейд завершён, открываем следующие точки...");

        foreach (var next in mapPointData.nextPoints)
        {
            if (!next.isUnlocked)
            {
                next.isUnlocked = true;
                SpawnPoint(next);
            }
        }

    }
}
