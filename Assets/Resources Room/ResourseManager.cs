using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;

public class ResourseManager : MonoBehaviour
{
    // [Inject] private UIController uIController;
    // [Inject] private SettlementManager settlementManager;

    private UIController uIController;
    private SettlementManager settlementManager;

    [Header("Начальные ресурсы")]
    [SerializeField] private int startingMetal = 300;

    [Header("Производство")]
    [SerializeField] private float productionInterval = 15f;

    private Dictionary<ResourceType, int> resourceStock;
    public IReadOnlyDictionary<ResourceType, int> ResourceStock => resourceStock;

    public event Action<ResourceType, int> OnResourceChange;

    void Awake()
    {
        resourceStock = new Dictionary<ResourceType, int>();

        foreach (ResourceType rt in System.Enum.GetValues(typeof(ResourceType)))
        {
            resourceStock[rt] = 0;
        }

        resourceStock[ResourceType.Metal] = startingMetal;
    }

    private void Start()
    {
        // StartCoroutine(ProductionCycle());

        UpdateUI();
    }

    private IEnumerator ProductionCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(productionInterval);

            var production = CalculateTotalProduction();

            foreach (var kvp in production)
            {
                AddResource(kvp.Key, kvp.Value);
            }
        }
    }

    public Dictionary<ResourceType, int> CalculateTotalProduction()
    {
        var totalProduction = new Dictionary<ResourceType, int>();

        var buildings = settlementManager.GetAllBuildings();

        foreach (var building in buildings)
        {
            var buildingProduction = building.GetHourlyProduction();

            foreach (var kvp in buildingProduction)
            {
                if (!totalProduction.ContainsKey(kvp.Key))
                {
                    totalProduction[kvp.Key] = 0;
                }
                totalProduction[kvp.Key] += kvp.Value;
            }
        }

        return totalProduction;
    }

    public int GetResourceAmount(ResourceType type)
    {
        return resourceStock.ContainsKey(type) ? resourceStock[type] : 0;
    }

    public bool CanAfford(Dictionary<ResourceType, int> cost)
    {
        return cost.All(kvp => GetResourceAmount(kvp.Key) >= kvp.Value);
    }

    public bool SpendResources(Dictionary<ResourceType, int> cost)
    {
        if (!CanAfford(cost)) return false;

        foreach (var kvp in cost)
        {
            resourceStock[kvp.Key] -= kvp.Value;
            OnResourceChange?.Invoke(kvp.Key, resourceStock[kvp.Key]);
        }

        UpdateUI();
        return true;
    }

    public void AddResource(ResourceType type, int amount)
    {
        if (!resourceStock.ContainsKey(type))
        {
            resourceStock[type] = 0;
        }

        resourceStock[type] += amount;
        OnResourceChange?.Invoke(type, resourceStock[type]);
        UpdateUI();
    }

    public void RefundResources(Dictionary<ResourceType, int> refund)
    {
        foreach (var kvp in refund)
        {
            AddResource(kvp.Key, kvp.Value);
        }
    }

    public bool SpendMetal(int cost)
    {
        var costDict = new Dictionary<ResourceType, int> { { ResourceType.Metal, cost } };
        return SpendResources(costDict);
    }

    public void RefundMetal(int amount)
    {
        AddResource(ResourceType.Metal, amount);
    }

    public int metalAmount => GetResourceAmount(ResourceType.Metal);

    private void UpdateUI()
    {
        if (uIController?.mainInterfaceUI != null)
        {
            uIController.mainInterfaceUI.UpdateMetalText(GetResourceAmount(ResourceType.Metal));
        }
    }

    public void InitializeDependecies(UIController _uIController, SettlementManager _settlementManager)
    {
        uIController = _uIController;
        settlementManager = _settlementManager;

        StartCoroutine(ProductionCycle());
    }

    
}