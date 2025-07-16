using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using System;
using Unity.Collections;
using System.Collections;
using Zenject;

public class SettlementManager : MonoBehaviour
{

    [Header("Население")]
    [SerializeField] private int startPeopleCount = 30;

    [Header("Управление")]
    [SerializeField] private List<BuildingInstance> buildingList = new List<BuildingInstance>();
    [SerializeField] private SettlementLevel _currentLevel = SettlementLevel.RefugeeCamp;

    public int totalPopulation { get; set; }
    public int TotalAssignedWorkers => buildingList.Sum(b => b.CurrentWorkerCount);
    public int freeWorkers => totalPopulation - TotalAssignedWorkers;

    public event Action<int> OnPopulationChange;
    public event Action<BuildingInstance> OnBuildingRegistered;
    public event Action<BuildingInstance> OnBuildingUnregistered;
    public event Action OnSettlementLevelChanged;
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

    public void TierUp(SettlementLevel newTier)
    {
        _currentLevel = newTier;
        OnSettlementLevelChanged?.Invoke();
        Debug.Log($"Уровень поселения повышен до: {newTier}");
    }
    public void ModifyPopulation(int value)
    {
        totalPopulation = Mathf.Max(0, totalPopulation - value); //todo: make freeWorkers - value, not total
        OnPopulationChange?.Invoke(totalPopulation);
    }
}
