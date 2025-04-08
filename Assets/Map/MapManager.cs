using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public List<MapPointData> mapPoints;

    public GameObject pointPrefab;
    public Transform pointsParent;

    public void Start()
    {
        InitializeMap();   
    }

    public void InitializeMap()
    {
        foreach (var pointData in mapPoints)
        {
            GameObject pointGO = Instantiate(pointPrefab, pointsParent);

            pointGO.GetComponent<RectTransform>().anchoredPosition = pointData.pointPosition;

            PointView view = pointGO.GetComponent<PointView>();

            if (view != null)
            {
                view.Setup(pointData);
                view.onPointSelected += OnPointSelected;
            }
        }
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
        Debug.Log($"Начало рейда на точку {mapPointData.pointName}...");
        yield return new WaitForSeconds(2.0f);
        Debug.Log($"Рейд на точке {mapPointData.pointName} завершён. Обработка результатов...");
    }
}
