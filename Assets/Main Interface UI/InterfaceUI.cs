using TMPro;
using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class InterfaceUI : MonoBehaviour
{

    public static InterfaceUI Instance;
    public TextMeshProUGUI mettalAmountText;
    public TextMeshProUGUI researchTimeText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        mettalAmountText.text = "Metall: " + ResourseManager.Instance.metalAmount;
        researchTimeText.text = "Research Time: 0";
    }

    public void UpdateIntefaceFactoryUI()
    {
        int totalProduction = 0;

        foreach (BuildingInstance building in BuildingInstance.allBuildingsInstance)
        {
            if (building.buildingData.buildingType == BuildingType.Factory && building.factorySettings != null)
            {
                float efficiencyFactor = building.factorySettings.currentFacEfficiency / 100f;
                totalProduction += Mathf.RoundToInt(ResourseManager.Instance.metalPerFactory * efficiencyFactor);
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

    public void UpdateMetalText(int metal)
    {
        mettalAmountText.text = "Metall: " + metal.ToString();
    }
}
