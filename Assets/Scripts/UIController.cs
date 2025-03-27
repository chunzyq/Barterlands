using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using DG.Tweening;
using NUnit.Framework.Constraints;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    private BuildingData buildingData;
    public InterfaceUI mainInterfaceUI;

    public GameObject industialUIPrefab;
    public GameObject scientificUIPrefab;
    public GameObject generalUIPrefab;
    private GameObject activeBuildingUI;

    public BuildingInstance currentBuildingInstance;

    private void Awake()
    {
        Instance = this;
    }

    public void OpenBuildingUI(BuildingInstance building)
    {

        currentBuildingInstance = building;

        if (activeBuildingUI != null)
        {
            Destroy(activeBuildingUI);
        }

        GameObject prefabToInstantiate = null;

        switch (building.buildingData.buildingType)
        {
            case BuildingType.Factory:
                prefabToInstantiate = industialUIPrefab;
                break;
            case BuildingType.Laboratory:
                prefabToInstantiate = scientificUIPrefab;
                break;
            case BuildingType.House:
                prefabToInstantiate = generalUIPrefab;
                break;
        }

        if (prefabToInstantiate != null)
        {
            activeBuildingUI = Instantiate(prefabToInstantiate, transform);

            // activeBuildingUI.transform.localScale = Vector3.zero;

            // activeBuildingUI.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutCirc);

            if (building.buildingData.buildingType == BuildingType.Factory)
            {
                FactoryUI ui = activeBuildingUI.GetComponent<FactoryUI>();
                ui.UpdateUI(building.factorySettings);
            }
            else if (building.buildingData.buildingType == BuildingType.Laboratory)
            {
                LaboratoryUI ui = activeBuildingUI.GetComponent<LaboratoryUI>();
                ui.UpdateUI(building.laboratorySettings);
            }
            else if (building.buildingData.buildingType == BuildingType.House)
            {
                HouseUI ui = activeBuildingUI.GetComponent<HouseUI>();
                ui.UpdateUI(building.houseSettings);
            }
        }
    }

    public void DeleteCurrentBuilding()
    {
        if (currentBuildingInstance != null)
        {
            Destroy(currentBuildingInstance.gameObject);
            currentBuildingInstance = null;
        }
        if (activeBuildingUI != null)
        {
            Destroy(activeBuildingUI);
            activeBuildingUI = null;
        }
        if (mainInterfaceUI != null)
        {
            mainInterfaceUI.UpdateIntefaceLaboratoryUI();
        }
    }
}
