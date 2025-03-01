using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    [SerializeField] BuildingList buildingList;
    [SerializeField] RectTransform container;

    void Start()
    {
        Redraw();
    }
    void Redraw()
    {
        for (var i = 0; i < buildingList.containerBuildings.Count; i++)
        {
            var item = buildingList.containerBuildings[i];
            var icon = new GameObject("Icon");
            icon.AddComponent<Image>().sprite = item.test;

            icon.transform.SetParent(container);
        }
    }
}
