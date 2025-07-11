using UnityEngine;
using System.Collections;
using Zenject;
using System.Linq;
using Unity.VisualScripting;
using System.Collections.Generic;

public class UIController : MonoBehaviour
{
    [Inject] DiContainer container;
    [Inject] SettlementManager settlementManager;
    [Inject] ResourseManager resourseManager;
    public InterfaceUI mainInterfaceUI;

    public GameObject industialUIPrefab;
    public GameObject scientificUIPrefab;
    public GameObject generalUIPrefab;
    public GameObject radiostationUIPrefab;
    private GameObject activeBuildingUI;

    public BuildingInstance currentBuildingInstance;

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
            case BuildingType.Radiostation:
                prefabToInstantiate = radiostationUIPrefab;
                break;
        }

        if (prefabToInstantiate != null)
        {
            activeBuildingUI = container.InstantiatePrefab(prefabToInstantiate, transform);

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
            else if (building.buildingData.buildingType == BuildingType.Radiostation)
            {
                TrainingStationUI ui = activeBuildingUI.GetComponent<TrainingStationUI>();
                ui.Initialize(building.GetComponent<StalkerTrainingStation>());

                //тут тестирую commit
            }
        }
    }

    public void CloseBuildingUI()
    {
        if (activeBuildingUI != null)
        {
            Destroy(activeBuildingUI);
            activeBuildingUI = null;
            currentBuildingInstance = null;

            BuildingInstance.Instance.Deselect();
        }
        
    }

    public void DeleteCurrentBuilding()
    {

        if (currentBuildingInstance != null)
        {

            if (currentBuildingInstance.buildingData.buildingType == BuildingType.Factory)
            {
                int factoryCost = 30;
                int refundAmount = Mathf.RoundToInt(factoryCost * 0.5f);
                var refund = new Dictionary<ResourceType, int> { { ResourceType.Metal, refundAmount } };
                resourseManager.RefundResources(refund);
            }
            else if (currentBuildingInstance.buildingData.buildingType == BuildingType.Laboratory)
            {
                int laboratoryCost = 60;
                int refundAmount = Mathf.RoundToInt(laboratoryCost * 0.5f);
                var refund = new Dictionary<ResourceType, int> { { ResourceType.Metal, refundAmount } };
                resourseManager.RefundResources(refund);
            }

            Destroy(currentBuildingInstance.gameObject);
            currentBuildingInstance = null;
        }

        if (activeBuildingUI != null)
        {
            Destroy(activeBuildingUI);
            activeBuildingUI = null;
        }

        StartCoroutine(DelayedUIUpdate());
    }
    private IEnumerator DelayedUIUpdate()
    {
        yield return null;

        if (mainInterfaceUI != null)
        {
            mainInterfaceUI.UpdateIntefaceLaboratoryUI();
            mainInterfaceUI.UpdateIntefaceFactoryUI();
            mainInterfaceUI.UpdateMetalText(resourseManager.metalAmount);
        }
    }
}