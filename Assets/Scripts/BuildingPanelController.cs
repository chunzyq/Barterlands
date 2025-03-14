using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPanelController : MonoBehaviour
{
    [SerializeField] ListOfBuildings listOfBuildings;
    [SerializeField] GameObject panelOfBuildings;
    [SerializeField] RectTransform container;
    [SerializeField] Button buildingButtonPrefag;
    [SerializeField] BuildManager buildManager;

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

        for (int i = 0; i < listOfBuildings.containerBuildings.Count; i++)
        {
            var item = listOfBuildings.containerBuildings[i];
            int buildIndex = i;

            Button buildingBtn;
            buildingBtn = Instantiate(buildingButtonPrefag, container);

            Image image = buildingBtn.GetComponent<Image>();

            buildingBtn.onClick.AddListener(() => OnBuildingButtonClicked(buildIndex));

            image.sprite = item.buildingIcon;
        }
    }
    public void Redraw()
    {
        RedrawAllBuildings();
    }

    public void RedrawBuildingByType(List<BuildingData> buildingToShow)
    {
        foreach (Transform child in container)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < buildingToShow.Count; i++)
        {
            var item = buildingToShow[i];
            var originalIndex = listOfBuildings.containerBuildings.IndexOf(item);

            Button buildingBtn;
            buildingBtn = Instantiate(buildingButtonPrefag, container);

            Image image = buildingBtn.GetComponent<Image>();

            buildingBtn.onClick.AddListener(() => OnBuildingButtonClicked(originalIndex));

            image.sprite = item.buildingIcon;
        }
    }

    private void OnBuildingButtonClicked(int buildingIndex)
    {
        var selectedBuilding = listOfBuildings.containerBuildings[buildingIndex];

        if (selectedBuilding.buildingPrefab != null)
        {
            buildManager.SelectBuilding(selectedBuilding.buildingPrefab);
        }
        else
        {
            Debug.LogError("У здания " + selectedBuilding.buildingName + " не назначен префаб!");
        }
    }

    public void OnScienceBtnClicked()
    {
        RedrawBuildingByType(listOfBuildings.scientificBuildings);
    }
    public void OnIndustrialBtnClicked()
    {
        RedrawBuildingByType(listOfBuildings.industrialBuildings);
    }
    public void OnGeneralBtnClicked()
    {
        RedrawBuildingByType(listOfBuildings.generalBuildings);
    }
    public void OnAllBtnClicked()
    {
        RedrawAllBuildings();
    }
}
