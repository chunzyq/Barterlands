using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuildingUI : MonoBehaviour
{
    [SerializeField] BuildingList buildingList;
    [SerializeField] RectTransform container;
    [SerializeField] Button buildingButtonPrefab;
    [SerializeField] private BuildManager buildManager;

    void Start()
    {
        Redraw();
    }
    public void RedrawAllBuildings()
    {

        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        for (var i = 0; i < buildingList.containerBuildings.Count; i++)
        {
            var item = buildingList.containerBuildings[i];
            int buildingIndex = i;

            Button buildingButton;
            buildingButton = Instantiate(buildingButtonPrefab, container);
            
            Image image = buildingButton.GetComponent<Image>();

            buildingButton.onClick.AddListener(() => OnBuildingButtonClicked(buildingIndex));

            image.sprite = item.iconSprite;

        }
    }
    public void Redraw()
    {
        RedrawAllBuildings();
    }

    private void OnBuildingButtonClicked(int buildingIndex)
    {
        var selectedBuilding = buildingList.containerBuildings[buildingIndex];


        if (selectedBuilding.buildingPrefab_test != null)
        {
            buildManager.SelectBuilding(selectedBuilding.buildingPrefab_test);
        }
        else
        {
            Debug.LogError("У здания " + selectedBuilding.buildingName + " не назначен префаб!");
        }
    }
    public void RedrawBuildingList(List<BuildingData> buildingsToShow)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < buildingsToShow.Count; i++)
        {
            var item = buildingsToShow[i];
            var originalIndex = buildingList.containerBuildings.IndexOf(item);

            Button buildingButton;
            buildingButton = Instantiate(buildingButtonPrefab, container);

            Image image = buildingButton.GetComponent<Image>();

            buildingButton.onClick.AddListener(() => OnBuildingButtonClicked(originalIndex));

            image.sprite = item.iconSprite;
        }
    }
    public void OnScienceButtonClicked()
    {
        RedrawBuildingList(buildingList.scientificBuilding);
    }
    public void OnIndustrialButtonClicked()
    {
        RedrawBuildingList(buildingList.industrialBuilding);
    }
    public void OnAllBuildingsButtonClicked()
    {
        RedrawBuildingList(buildingList.containerBuildings);
    }
}
