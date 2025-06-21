using UnityEngine;
using Zenject;

public class LaboratoryBuildingStrategy : IBuildStrategy
{
    public BuildingType buildingType => BuildingType.Laboratory;
    const int LAB_COST = 60;

    readonly ResourseManager _resourceManager;
    readonly DiContainer _container;
    readonly InterfaceUI _mainUI;
    readonly GameObject _laboratoryPrefab;
    readonly SettlementManager _settManager;

    [Inject]
    public LaboratoryBuildingStrategy(ResourseManager resourseManager, SettlementManager settManager, DiContainer container, InterfaceUI mainUI, [Inject(Id = "Laboratory_Prefab")] GameObject laboratoryPrefab)
    {
        _resourceManager = resourseManager;
        _container = container;
        _mainUI = mainUI;
        _settManager = settManager;
        _laboratoryPrefab = laboratoryPrefab;
    }

    public bool CanBuild()
    {
        return _resourceManager.metalAmount >= LAB_COST;
    }

    public void SpendCost() => _resourceManager.SpendMetal(LAB_COST);

    public BuildingInstance Create(Vector3 position, Quaternion rotation)
    {
        var go = _container.InstantiatePrefab(_laboratoryPrefab, position, rotation, null);
        return go.GetComponent<BuildingInstance>();
    }

    public void OnBuilt(BuildingInstance instance)
    {
        _resourceManager.SpendMetal(LAB_COST);
        _settManager.RegisterBuilding(instance);
        _mainUI.UpdateIntefaceLaboratoryUI();
    }
}
