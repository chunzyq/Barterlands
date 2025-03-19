using TMPro;
using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class InterfaceUI : MonoBehaviour
{
    public TextMeshProUGUI mettalAmountText;
    public TextMeshProUGUI researchTimeText;

    void Start()
    {
        mettalAmountText.text = "Metall: 0";
        researchTimeText.text = "Research Time: 0";
    }

    // public void UpdateIntefaceFactoryUI()
    // {
    //     int totalMetall = 0;

    //     foreach (BuildingInstance building in BuildingInstance.allBuildingsInstance)
    //     {
    //         if (building.buildingData.buildingType == BuildingType.Factory && building.factorySettings != null)
    //         {
    //             totalMetall += building.factorySettings.productionRate;
    //         }
    //     }

    //     mettalAmountText.text = "Metall: " + totalMetall.ToString();
    // }
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

    public void UpdateMetalText(int metal)
    {
        mettalAmountText.text = "Metall: " + metal.ToString();
    }
}
