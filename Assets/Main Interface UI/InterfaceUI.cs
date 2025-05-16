using System.Text;
using TMPro;
using TMPro.Examples;
using Unity.Multiplayer.Center.Common;
using UnityEngine;
using Zenject;

public class InterfaceUI : MonoBehaviour
{

    public static InterfaceUI Instance;
    [Inject] SettlementManager settlementManager;
    public TextMeshProUGUI mettalAmountText;
    public TextMeshProUGUI researchTimeText;
    public TextMeshProUGUI peopleCountText;
    public TextMeshProUGUI testText;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        mettalAmountText.text = "Metall: " + ResourseManager.Instance.metalAmount;
        researchTimeText.text = "Research Time: 0";
        peopleCountText.text = "People: " + settlementManager.startPeopleCount;

    }
    void Update()
    {
        TestFunc();
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
    public void UpdatePeopleCountText()
    {
        peopleCountText.text = "People: " + settlementManager.FreeWorkers;
    }

    public void UpdateMetalText(int metal)
    {
        mettalAmountText.text = "Metall: " + metal.ToString();
    }

    public void TestFunc()
    {
        testText.text = settlementManager.TotalAssignedWorkers.ToString();
        
    }
}