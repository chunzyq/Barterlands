using System.Text;
using TMPro;
using TMPro.Examples;
using Unity.Multiplayer.Center.Common;
using UnityEngine;
using Zenject;

public class InterfaceUI : MonoBehaviour
{
    [Inject] SettlementManager settlementManager;
    [Inject] ResourseManager resourseManager;

    public TextMeshProUGUI mettalAmountText;
    public TextMeshProUGUI researchTimeText;
    public TextMeshProUGUI peopleCountText;

    void Start()
    {
        mettalAmountText.text = "Metall: " + resourseManager.metalAmount;
        researchTimeText.text = "Research Time: 0";
        peopleCountText.text = "People: " + settlementManager.startPeopleCount;

    }
    void Update()
    {
        UpdatePeopleCountText();
    }

    public void UpdateIntefaceFactoryUI()
    {
        int totalProduction = 0;

        foreach (BuildingInstance building in BuildingInstance.allBuildingsInstance)
        {
            if (building.buildingData.buildingType == BuildingType.Factory && building.factorySettings != null)
            {
                float efficiencyFactor = building.factorySettings.currentFacEfficiency / 100f;
                totalProduction += Mathf.RoundToInt(resourseManager.metalPerFactory * efficiencyFactor);
            }
        }

        mettalAmountText.text = "Metall: " + totalProduction.ToString();
    }
    public void UpdateIntefaceLaboratoryUI()
    {
        int totalResearchTime = 0;

        foreach (BuildingInstance building in BuildingInstance.allBuildingsInstance)
        {
            if (building.buildingData.buildingType == BuildingType.Laboratory && building.laboratorySettings != null)
            {
                int researchTime = building.laboratorySettings.baseResearchTime - (building.laboratorySettings.currentLaboratoryWorkers * building.laboratorySettings.baseReduceResearchTimePerWorker);
                totalResearchTime += researchTime;
            }
        }
        researchTimeText.text = "Research Time: " + totalResearchTime.ToString();
    }
    public void UpdatePeopleCountText()
    {
        peopleCountText.text = "People: " + settlementManager.FreeWorkers;
    }

    public void UpdateMetalText(int metal)
    {
        mettalAmountText.text = "Metall: " + metal.ToString();
    }
}