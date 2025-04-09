using TMPro.Examples;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapDragZoom : MonoBehaviour, IBeginDragHandler, IDragHandler, IScrollHandler
{
    public float dragSensitivity = 1.0f;

    public float zoomSensitivity = 0.1f;
    public float minZoom = 0.5f;
    public float maxZoom = 2.5f;

    private RectTransform mapRectTransform;
    private Vector2 lastMousePosition;

    void Awake()
    {
        mapRectTransform = GetComponent<RectTransform>();
        if (mapRectTransform == null)
        {
            Debug.LogError("MapDragZoom не обнаружил RectTransform на объекте!");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        lastMousePosition = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.position - lastMousePosition;
        mapRectTransform.anchoredPosition += delta * dragSensitivity;
        lastMousePosition = eventData.position;
    }

    public void OnScroll(PointerEventData eventData)
    {
        float scrollDelta = eventData.scrollDelta.y;
        float newScale = Mathf.Clamp(mapRectTransform.localScale.x + scrollDelta * zoomSensitivity, minZoom, maxZoom);
        mapRectTransform.localScale = new Vector3(newScale, newScale, 1);
    }
}
