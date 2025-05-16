using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SettlementManager : MonoBehaviour
{
    [Header("Население")]
    public int startPeopleCount = 30;

    public Dictionary<ResourceType, int> resourceStock;
    public IReadOnlyDictionary<ResourceType, int> ResourceStock => resourceStock;
    public List<BuildingInstance> buildingList = new List<BuildingInstance>();


    void Awake()
    {
        resourceStock = new Dictionary<ResourceType, int>();
        foreach (ResourceType rt in System.Enum.GetValues(typeof(ResourceType)))
        {
            resourceStock[rt] = 0;
        }
    }
    public void RegisterBuilding(BuildingInstance building)
    {
        if (!buildingList.Contains(building))
        {
            buildingList.Add(building);
        }
    }
    public void UnregisterBuilding(BuildingInstance building)
    {
        buildingList.Remove(building);
    }

    public int TotalAssignedWorkers => buildingList.Sum(b => b.CurrentWorkerCount);
    public int FreeWorkers => startPeopleCount - TotalAssignedWorkers;

    public bool CanAfford(Dictionary<ResourceType, int> cost)
    {
        foreach (var kv in cost)
        {
            if (resourceStock[kv.Key] < kv.Value)
            {
                return false;
            }
        }
        return true;
    }

    public bool SpendResources(Dictionary<ResourceType, int> cost)
    {
        if (!CanAfford(cost)) return false;
        foreach (var kv in cost)
        {
            resourceStock[kv.Key] -= kv.Value;
        }
        return true;
    }
    public void AddResources(ResourceType type, int amount)
    {
        resourceStock[type] += amount;
    }
    public Dictionary<ResourceType, int> GetTotalHourlyProduction()
    {
        var total = new Dictionary<ResourceType, int>();
        foreach (var b in buildingList)
        {
            var single = b.GetHourlyProduction();
            foreach (var kv in single)
            {
                if (!total.ContainsKey(kv.Key))
                {
                    total[kv.Key] = 0;
                }
                total[kv.Key] += kv.Value;
            }
        }
        return total;
    }

    public BuildingInstance TryBuild(BuildingData data, Vector3 pos)
    {
        var cost = data.cost.ToDictionary(c => c.type, c => c.amount);
        if (!SpendResources(cost))
        {
            return null;
        }
        var go = Instantiate(data.buildingPrefab, pos, Quaternion.identity);
        return go.GetComponent<BuildingInstance>();
    }

    public BuildingInstance GetBuildingByID(string id)
    {
        return buildingList.FirstOrDefault(b => b.InstanceID == id);
    }

}
