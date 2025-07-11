using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PointView : MonoBehaviour
{
    public TextMeshProUGUI pointNameText;
    public UnityEngine.UI.Image pointImage;
    public Button button;

    private MapPointData mapPointData;


    public UnityAction<MapPointData> onPointSelected;

    public void Setup(MapPointData data)
    {
        mapPointData = data;
        pointNameText.text = data.pointName;
        pointImage.sprite = data.pointIcon;
        button.onClick.AddListener(() => onPointSelected?.Invoke(mapPointData));
    }
}
