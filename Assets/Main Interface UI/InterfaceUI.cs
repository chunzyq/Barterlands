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
        // mettalAmountText.text = "Metall: " + resourseManager.metalAmount;
        // researchTimeText.text = "Research Time: 0";
        // peopleCountText.text = "People: " + settlementManager.totalPopulation;

        resourseManager.OnResourceChange += OnResourceChange;
        settlementManager.OnPopulationChange += OnPopulationChange;

        UpdateAllUI();

    }

    void OnDestroy()
    {
        resourseManager.OnResourceChange -= OnResourceChange;
        settlementManager.OnPopulationChange -= OnPopulationChange;
    }
    void Update()
    {
        UpdatePeopleCountText();
    }

    private void UpdateAllUI()
    {
        UpdateMetalText(resourseManager.GetResourceAmount(ResourceType.Metal));

        UpdatePeopleCountText();

        UpdateResearchTimeUI();
    }

    private void OnResourceChange(ResourceType type, int amount)
    {
        switch (type)
        {
            case ResourceType.Metal:
                UpdateMetalText(amount);
                break;
        }
    }

    private void OnPopulationChange(int newPopulation)
    {
        UpdatePeopleCountText();
    }

    public void UpdateMetalText(int metal)
    {
        if (mettalAmountText != null)
        {
            mettalAmountText.text = $"Metal: {metal}";
        }
    }

    public void UpdatePeopleCountText()
    {
        if (peopleCountText != null)
        {
            peopleCountText.text = $"People: {settlementManager.freeWorkers}/{settlementManager.totalPopulation}";
        }
    }

    public void UpdateIntefaceFactoryUI()
    {
        var buildings = settlementManager.GetBuildingsByType(BuildingType.Factory);
        int totalProduction = 0;
        
        foreach (var building in buildings)
        {
            if (building.factorySettings != null)
            {
                float efficiencyFactor = building.factorySettings.currentFacEfficiency / 100f;
                int workersCount = building.factorySettings.currentFactoryWorkers;
                int baseProduction = building.factorySettings.baseProductionRatePerWorker;
                
                totalProduction += Mathf.RoundToInt(workersCount * baseProduction * efficiencyFactor);
            }
        }
        
        if (mettalAmountText != null)
        {
            int currentMetal = resourseManager.GetResourceAmount(ResourceType.Metal);
            mettalAmountText.text = $"Metal: {currentMetal} (+{totalProduction}/h)";
        }
    }

    public void UpdateIntefaceLaboratoryUI()
    {
        var buildings = settlementManager.GetBuildingsByType(BuildingType.Laboratory);
        int totalResearchTime = 0;
        
        foreach (var building in buildings)
        {
            if (building.laboratorySettings != null)
            {
                int researchTime = building.laboratorySettings.baseResearchTime - 
                    (building.laboratorySettings.currentLaboratoryWorkers * 
                     building.laboratorySettings.baseReduceResearchTimePerWorker);
                totalResearchTime += Mathf.Max(0, researchTime);
            }
        }
        
        if (researchTimeText != null)
        {
            researchTimeText.text = $"Research Time: {totalResearchTime}";
        }
    }

    public void UpdateResearchTimeUI()
    {
        UpdateIntefaceLaboratoryUI();
    }

    public void ShowResourceChange(ResourceType type, int changeAmount)
    {
        // Можно добавить всплывающий текст или анимацию
        string sign = changeAmount >= 0 ? "+" : "";
        Debug.Log($"{type}: {sign}{changeAmount}");
    }
    
    public void UpdateProductionRates()
    {
        // Обновляем отображение производства для всех типов ресурсов
        var production = resourseManager.CalculateTotalProduction();
        
        // Можно отобразить в UI скорость производства каждого ресурса
        foreach (var kvp in production)
        {
            Debug.Log($"{kvp.Key} production: +{kvp.Value}/h");
        }
    }

    // public void UpdateIntefaceFactoryUI()
    // {
    //     int totalProduction = 0;

    //     foreach (BuildingInstance building in BuildingInstance.allBuildingsInstance)
    //     {
    //         if (building.buildingData.buildingType == BuildingType.Factory && building.factorySettings != null)
    //         {
    //             float efficiencyFactor = building.factorySettings.currentFacEfficiency / 100f;
    //             totalProduction += Mathf.RoundToInt(resourseManager.metalPerFactory * efficiencyFactor);
    //         }
    //     }

    //     mettalAmountText.text = "Metall: " + totalProduction.ToString();
    // }
    // public void UpdateIntefaceLaboratoryUI()
    // {
    //     int totalResearchTime = 0;

    //     foreach (BuildingInstance building in BuildingInstance.allBuildingsInstance)
    //     {
    //         if (building.buildingData.buildingType == BuildingType.Laboratory && building.laboratorySettings != null)
    //         {
    //             int researchTime = building.laboratorySettings.baseResearchTime - (building.laboratorySettings.currentLaboratoryWorkers * building.laboratorySettings.baseReduceResearchTimePerWorker);
    //             totalResearchTime += researchTime;
    //         }
    //     }
    //     researchTimeText.text = "Research Time: " + totalResearchTime.ToString();
    // }
    // public void UpdatePeopleCountText()
    // {
    //     peopleCountText.text = "People: " + settlementManager.freeWorkers;
    // }

    // public void UpdateMetalText(int metal)
    // {
    //     mettalAmountText.text = "Metall: " + metal.ToString();
    // }
}