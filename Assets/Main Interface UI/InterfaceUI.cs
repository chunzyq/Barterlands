using System.Text;
using TMPro;
using TMPro.Examples;
using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class InterfaceUI : MonoBehaviour
{

    public static InterfaceUI Instance;
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
        peopleCountText.text = "People: " + SettlementManager.Instance.startPeopleCount;

    }
    void Update()
    {
        TestFunc();
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

    public void TestFunc()
    {
        testText.text = SettlementManager.Instance.TotalAssignedWorkers.ToString();
    }

    // public void TestFunc()
    // {
    //     if (testText == null)
    //     {
    //         Debug.Log("testText не назначен!");
    //     }
    //     var prod = SettlementManager.Instance.GetTotalHourlyProduction();
    //     var sb = new StringBuilder();

    //     foreach (var kv in prod)
    //     {
    //         sb.AppendLine($"{kv.Key}: {kv.Value}/h");
    //     }

    //     testText.text = sb.ToString();
    // }
}