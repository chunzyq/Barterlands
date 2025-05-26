using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Unity.Collections;
using System.Collections;

public class SettlementManager : MonoBehaviour
{
    [Header("Население")]
    [SerializeField] private int startPeopleCount = 30;

    [Header("Управление")]
    [SerializeField] private List<BuildingInstance> buildingList = new List<BuildingInstance>();

    public int totalPopulation { get; private set; }
    public int TotalAssignedWorkers => buildingList.Sum(b => b.CurrentWorkerCount);
    public int freeWorkers => totalPopulation - TotalAssignedWorkers;

    public event Action<int> OnPopulationChange;
    public event Action<BuildingInstance> OnBuildingRegistered;
    public event Action<BuildingInstance> OnBuildingUnregistered;

    void Awake()
    {
        totalPopulation = startPeopleCount;
    }

    public void RegisterBuilding(BuildingInstance building)
    {
        if (!buildingList.Contains(building))
        {
            buildingList.Add(building);
            
            OnBuildingRegistered?.Invoke(building);
        }
    }

    public void UnregisterBuilding(BuildingInstance building)
    {
        if (buildingList.Remove(building))
        {
            OnBuildingUnregistered?.Invoke(building);
        }
    }

    public List<BuildingInstance> GetAllBuildings()
    {
        return new List<BuildingInstance>(buildingList);
    }

    public BuildingInstance GetBuildingByID(string id)
    {
        return buildingList.FirstOrDefault(b => b.InstanceID == id);
    }

     public List<BuildingInstance> GetBuildingsByType(BuildingType type)
    {
        return buildingList.Where(b => b.buildingData.buildingType == type).ToList();
    }

    public bool CanAssignWorkers(int amount)
    {
        return freeWorkers >= amount;
    }

    public bool AssignWorkers(BuildingInstance building, int amount)
    {
        if (!CanAssignWorkers(amount)) return false;
        
        // Логика назначения рабочих
        switch (building.buildingData.buildingType)
        {
            case BuildingType.Factory:
                building.factorySettings.currentFactoryWorkers += amount;
                break;
            case BuildingType.Laboratory:
                building.laboratorySettings.currentLaboratoryWorkers += amount;
                break;
        }
        
        return true;
    }

    public void UnassignWorkers(BuildingInstance building, int amount)
    {
        // Логика снятия рабочих
        switch (building.buildingData.buildingType)
        {
            case BuildingType.Factory:
                building.factorySettings.currentFactoryWorkers = 
                    Mathf.Max(0, building.factorySettings.currentFactoryWorkers - amount);
                break;
            case BuildingType.Laboratory:
                building.laboratorySettings.currentLaboratoryWorkers = 
                    Mathf.Max(0, building.laboratorySettings.currentLaboratoryWorkers - amount);
                break;
        }
    }

    public int GetWorkerCountByBuildingType(BuildingType type)
    {
        return buildingList
            .Where(b => b.buildingData.buildingType == type)
            .Sum(b => b.CurrentWorkerCount);
    }

    public float GetAverageEfficiency()
    {
        var factories = GetBuildingsByType(BuildingType.Factory);
        if (factories.Count == 0) return 100f;
        
        return factories.Average(f => f.factorySettings.currentFacEfficiency);
    }

    // public Dictionary<ResourceType, int> resourceStock;
    // public IReadOnlyDictionary<ResourceType, int> ResourceStock => resourceStock;
    // public List<BuildingInstance> buildingList = new List<BuildingInstance>();
    // public event Action<ResourceType, int> OnResourceChange;

    // [SerializeField] private float productionInterval = 15f;


    // void Awake()
    // {
    //     resourceStock = new Dictionary<ResourceType, int>();
    //     foreach (ResourceType rt in System.Enum.GetValues(typeof(ResourceType)))
    //     {
    //         resourceStock[rt] = 0;
    //     }
    // }
    // private void Start()
    // {
    //     StartCoroutine(ProduceResources());
    // }
    // private IEnumerator ProduceResources()
    // {
    //     while (true)
    //     {
    //         yield return new WaitForSeconds(productionInterval);

    //         var production = GetTotalHourlyProduction();
    //         foreach (var kv in production)
    //         {
    //             resourceStock[kv.Key] += kv.Value;
    //             OnResourceChange?.Invoke(kv.Key, resourceStock[kv.Key]);
    //         }
    //     }
    // }
    // public void RegisterBuilding(BuildingInstance building)
    // {
    //     if (!buildingList.Contains(building))
    //     {
    //         buildingList.Add(building);
    //     }
    // }
    // public void UnregisterBuilding(BuildingInstance building)
    // {
    //     buildingList.Remove(building);
    // }

    // public int TotalAssignedWorkers => buildingList.Sum(b => b.CurrentWorkerCount);
    // public int FreeWorkers => startPeopleCount - TotalAssignedWorkers;

    // public bool CanAfford(Dictionary<ResourceType, int> cost)
    // {
    //     return cost.All(kv => resourceStock[kv.Key] >= kv.Value);

    //     // foreach (var kv in cost)
    //     // {
    //     //     if (resourceStock[kv.Key] < kv.Value)
    //     //     {
    //     //         return false;
    //     //     }
    //     // }
    //     // return true;
    // }

    // public bool SpendResources(Dictionary<ResourceType, int> cost)
    // {
    //     if (!CanAfford(cost)) return false;
    //     foreach (var kv in cost)
    //     {
    //         resourceStock[kv.Key] -= kv.Value;
    //         OnResourceChange?.Invoke(kv.Key, resourceStock[kv.Key]);
    //     }
    //     return true;
    // }
    // public void AddResources(ResourceType type, int amount)
    // {
    //     resourceStock[type] += amount;
    //     OnResourceChange?.Invoke(type, resourceStock[type]);
    // }
    // public Dictionary<ResourceType, int> GetTotalHourlyProduction()
    // {
    //     var total = new Dictionary<ResourceType, int>();
    //     foreach (var b in buildingList)
    //     {
    //         var single = b.GetHourlyProduction();
    //         foreach (var kv in single)
    //         {
    //             if (!total.ContainsKey(kv.Key))
    //             {
    //                 total[kv.Key] = 0;
    //             }
    //             total[kv.Key] += kv.Value;
    //         }
    //     }
    //     return total;
    // }

    // public BuildingInstance TryBuild(BuildingData data, Vector3 pos)
    // {
    //     var cost = data.cost.ToDictionary(c => c.type, c => c.amount);
    //     if (!SpendResources(cost))
    //     {
    //         return null;
    //     }
    //     var go = Instantiate(data.buildingPrefab, pos, Quaternion.identity);
    //     var instance = go.GetComponent<BuildingInstance>();

    //     instance.GenerateUniqueID();
    //     RegisterBuilding(instance);
    //     return instance;
    // }

    // public BuildingInstance GetBuildingByID(string id)
    // {
    //     return buildingList.FirstOrDefault(b => b.InstanceID == id);
    // }
    // public void NotifyResourceChange(ResourceType type, int amount)
    // {
    //     OnResourceChange?.Invoke(type, amount);
    // }

}
