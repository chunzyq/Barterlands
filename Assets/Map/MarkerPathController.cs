using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MarkerPathController : MonoBehaviour
{
    [Header("UI-RectTransform самого маркера")]
    public RectTransform markerRect;

    [Header("UI-RectTransform линии (PathLine)")]
    public RectTransform lineRect;

    [Header("Скорость движения (в ед./сек)")]
    public float moveSpeed = 200f;

    [Inject] private StalkerUnitManager stalkerUnitManager;
    [Inject] private UnitSelectionUIManager unitSelectionUIManager;

    public event Action<MapPointData> OnTargetReached;

    RectTransform targetRect;
    MapPointData targetData;
    bool isMoving = false;

    void Start()
    {
        // линия прячется, пока нет цели
        lineRect.gameObject.SetActive(false);

    }

    void LateUpdate()
    {
        markerRect.SetAsLastSibling();
    }

    // Вызываем из MapManager при клике на точку
    public void SetTarget(RectTransform target, MapPointData data)
    {
        if (isMoving) return;
        targetRect = target;
        targetData = data;
        lineRect.gameObject.SetActive(true);
        DrawLine();
    }

    void DrawLine()
    {
        if (targetRect == null) return;

        // Начало и конец в локальных координатах pointsParent
        Vector2 start = markerRect.anchoredPosition;
        Vector2 end   = targetRect.anchoredPosition;
        Vector2 dir   = end - start;
        float  dist  = dir.magnitude;
        float  angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // позиция линии = начало
        lineRect.anchoredPosition = start;
        // растягиваем по длине
        Vector2 size = lineRect.sizeDelta;
        size.x = dist;
        lineRect.sizeDelta = size;
        // поворачиваем
        lineRect.localEulerAngles = new Vector3(0, 0, angle);
    }

    // Вызываем, когда игрок нажмёт «Go»
    public void MoveAlongLine()
    {
        if (unitSelectionUIManager.HasSelectedUnits && stalkerUnitManager.stalkers.Count >= unitSelectionUIManager.maxAvailableUnits)
        {
            if (targetRect == null || isMoving) return;
            StartCoroutine(Co_Move());
        }
        else
        {
            Debug.Log("Выберите как минимум 2х сталкеров!");
        }
    }

    IEnumerator Co_Move()
    {
        isMoving = true;
        // пока не дошли
        while (Vector2.Distance(markerRect.anchoredPosition, targetRect.anchoredPosition) > 0.01f)
        {
            markerRect.anchoredPosition = Vector2.MoveTowards(
                markerRect.anchoredPosition,
                targetRect.anchoredPosition,
                moveSpeed * Time.deltaTime);

            DrawLine();    // обновляем начало и длину линии
            yield return null;
        }

        isMoving = false;
        // если линия больше не нужна
        lineRect.gameObject.SetActive(false);

        OnTargetReached?.Invoke(targetData);
        
    }
}
