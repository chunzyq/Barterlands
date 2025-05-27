using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class ResourceMenuController : MonoBehaviour
{
    [Inject] private ResourseManager resourseManager;
    [Inject] private SettlementManager settlementManager;

    [SerializeField] private TextMeshProUGUI metalAmount;
    [SerializeField] private TextMeshProUGUI freeWorkers;
    [SerializeField] private TextMeshProUGUI assignedWorkers;

    [SerializeField] private TextMeshProUGUI totalPopulation;

    private Dictionary<ResourceType, int> cachedProduction;
    private float updateInterval = 0.5f;
    private float lastUpdateTime;

    void Start()
    {
        resourseManager.OnResourceChange += OnResourceChanged;
        settlementManager.OnPopulationChange += OnPopulationChanged;
        settlementManager.OnBuildingRegistered += OnBuildingChanged;
        settlementManager.OnBuildingUnregistered += OnBuildingChanged;
        
        UpdateAllUI();
    }

    void OnDestroy()
    {
        resourseManager.OnResourceChange -= OnResourceChanged;
        settlementManager.OnPopulationChange -= OnPopulationChanged;
        settlementManager.OnBuildingRegistered -= OnBuildingChanged;
        settlementManager.OnBuildingUnregistered -= OnBuildingChanged;
    }

    void Update()
    {
        if (Time.time - lastUpdateTime > updateInterval)
        {
            UpdateProductionRates();
            lastUpdateTime = Time.time;
        }
    }

    private void OnResourceChanged(ResourceType type, int amount)
    {
        UpdateResourceDisplay(type, amount);
    }
    
    private void OnPopulationChanged(int newPopulation)
    {
        UpdatePopulationUI();
    }
    
    private void OnBuildingChanged(BuildingInstance building)
    {
        UpdateProductionRates();
        UpdatePopulationUI();
    }

    private void UpdateAllUI()
    {
        UpdateResourceDisplay(ResourceType.Metal, resourseManager.GetResourceAmount(ResourceType.Metal));
        
        UpdatePopulationUI();
        
        UpdateProductionRates();
    }

    private void UpdateResourceDisplay(ResourceType type, int amount)
    {
        int productionRate = GetProductionRate(type);
        string rateText = productionRate != 0 ? $" ({FormatRate(productionRate)}/h)" : "";
        
        switch (type)
        {
            case ResourceType.Metal:
                if (metalAmount != null)
                    metalAmount.text = $"Metal: {amount}{rateText}";
                break;
        }
    }

    private void UpdatePopulationUI()
    {
        if (freeWorkers != null)
            freeWorkers.text = $"Free People: {settlementManager.freeWorkers}";
            
        if (assignedWorkers != null)
            assignedWorkers.text = $"Assigned Workers: {settlementManager.TotalAssignedWorkers}";
            
        if (totalPopulation != null)
            totalPopulation.text = $"Total Population: {settlementManager.totalPopulation}";
    }

    private void UpdateProductionRates()
    {
        cachedProduction = resourseManager.CalculateTotalProduction();
        
        UpdateResourceDisplay(ResourceType.Metal, resourseManager.GetResourceAmount(ResourceType.Metal));
    }

    private int GetProductionRate(ResourceType type)
    {
        if (cachedProduction == null) return 0;
        return cachedProduction.TryGetValue(type, out var rate) ? rate : 0;
    }
    
    private string FormatRate(int rate)
    {
        if (rate > 0) return $"+{rate}";
        return rate.ToString();
    }

    // public void ShowDetailedStats()
    // {
    //     // Можно вызвать для показа детальной статистики
    //     Debug.Log("=== Resource Production Details ===");
        
    //     var buildings = settlementManager.GetAllBuildings();
    //     foreach (var building in buildings)
    //     {
    //         var production = building.GetHourlyProduction();
    //         if (production.Count > 0)
    //         {
    //             Debug.Log($"{building.buildingData.buildingName}:");
    //             foreach (var kvp in production)
    //             {
    //                 Debug.Log($"  - {kvp.Key}: +{kvp.Value}/h");
    //             }
    //         }
    //     }
        
    //     Debug.Log($"\nTotal Workers: {settlementManager.TotalAssignedWorkers}/{settlementManager.totalPopulation}");
    // }

    // void Update()
    // {
    //     Dictionary<ResourceType, int> prod = settlementManager.Ave();

    //     metalAmount.text = $"Metal: {resourseManager.metalAmount.ToString()} ({GetRate(prod, ResourceType.Metal)}/h)";
    //     freeWorkers.text = "Free People: " + settlementManager.freeWorkers.ToString();
    //     assignedWorkers.text = "Assigned Workers: " + settlementManager.TotalAssignedWorkers.ToString();
    // }

    // private int GetRate(Dictionary<ResourceType, int> d, ResourceType type)
    // {
    //     return d.TryGetValue(type, out var v) ? v : 0;
    // }
}
