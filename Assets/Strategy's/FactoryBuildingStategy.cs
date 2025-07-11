using UnityEditor.Embree;
using UnityEngine;
using UnityEngine.Rendering;
using Zenject;

public class FactoryBuildingStategy : IBuildStrategy
{
    public BuildingType buildingType => BuildingType.Factory;

    const int FACTORY_COST = 30;

    readonly ResourseManager _resourceManager;
    readonly DiContainer _container;
    readonly InterfaceUI _mainUI;
    readonly GameObject _factoryPrefab;
    readonly SettlementManager _settManager;

    [Inject]
    public FactoryBuildingStategy(ResourseManager resourseManager, SettlementManager settManager, DiContainer container, InterfaceUI mainUI, [Inject(Id = "Factory_Prefab")] GameObject factoryPrefab)
    {
        _resourceManager = resourseManager;
        _container = container;
        _mainUI = mainUI;
        _settManager = settManager;
        _factoryPrefab = factoryPrefab;
    }

    public bool CanBuild()
    {
        if (_resourceManager == null)
        {
            Debug.LogError("_resourceManager == null");
        }

        return _resourceManager.metalAmount >= FACTORY_COST;
    }
    public void SpendCost() => _resourceManager.SpendMetal(FACTORY_COST);

    public BuildingInstance Create(Vector3 position, Quaternion rotation)
    {
        var go = _container.InstantiatePrefab(_factoryPrefab, position, rotation, null);
        return go.GetComponent<BuildingInstance>();
    }

    public void OnBuilt(BuildingInstance instance)
    {
        // _resourceManager.SpendMetal(FACTORY_COST);
        _settManager.RegisterBuilding(instance);
        _mainUI.UpdateIntefaceFactoryUI();
    } 
}

