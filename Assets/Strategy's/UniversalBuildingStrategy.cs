using System.Linq;
using UnityEngine;
using Zenject;

public class UniversalBuildingStrategy : MonoBehaviour
{
    [Inject] private DiContainer _container;
    [Inject] private SettlementManager _settlementManager;
    [Inject] private ResourseManager _resourceManager;
    [Inject] private UIController _uiController;
    
    public bool CanBuild(BuildingData data)
    {
        var cost = data.cost.ToDictionary(c => c.type, c => c.amount);
        return _resourceManager.CanAfford(cost);
    }
    
    public BuildingInstance Create(BuildingData data, Vector3 position, Quaternion rotation)
    {
        var go = _container.InstantiatePrefab(data.buildingPrefab, position, rotation, null);
        var instance = go.GetComponent<BuildingInstance>();
        
        if (instance != null)
        {
            instance.GenerateUniqueID();
        }
        
        return instance;
    }
    
    public void OnBuilt(BuildingInstance instance, BuildingData data)
    {
        var cost = data.cost.ToDictionary(c => c.type, c => c.amount);
        _resourceManager.SpendResources(cost);
        
        _settlementManager.RegisterBuilding(instance);
        
        UpdateUIForBuildingType(data.buildingType);
    }
    
    private void UpdateUIForBuildingType(BuildingType type)
    {
        var mainUI = _uiController.mainInterfaceUI;
        if (mainUI == null) return;
        
        switch (type)
        {
            case BuildingType.Factory:
                mainUI.UpdateIntefaceFactoryUI();
                break;
            case BuildingType.Laboratory:
                mainUI.UpdateIntefaceLaboratoryUI();
                break;
        }
    }
}
