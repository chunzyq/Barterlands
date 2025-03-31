using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using DG.Tweening;
using NUnit.Framework.Constraints;
using System.Collections;

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

            if (currentBuildingInstance.buildingData.buildingType == BuildingType.Factory)
            {
                int factoryCost = 30;
                int refundAmount = Mathf.RoundToInt(factoryCost * 0.5f);
                ResourseManager.Instance.RefundMetal(refundAmount);
            }
            else if (currentBuildingInstance.buildingData.buildingType == BuildingType.Laboratory)
            {
                int laboratoryCost = 60;
                int refundAmount = Mathf.RoundToInt(laboratoryCost * 0.5f);
                ResourseManager.Instance.RefundMetal(refundAmount);
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
            mainInterfaceUI.UpdateMetalText(ResourseManager.Instance.metalAmount);
        }
    }
}