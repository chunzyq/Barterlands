using UnityEngine;
using Zenject;

public class BuildingPlacementHandler : MonoBehaviour
{
    [Inject] private DiContainer _container;
    [Inject] private SettlementManager _settlementManager;
    [Inject] private BuildingPlacementValidator _validator;
    [Inject] private BuildPreviewHandler _previewHandler;
    
    // Универсальная стратегия для всех зданий
    [Inject] private UniversalBuildingStrategy _universalStrategy;
    
    public bool TryPlaceBuilding(BuildingData data, Vector3 position, Quaternion rotation)
    {
        if (!_previewHandler.CanPlace) 
        {
            Debug.Log("Невозможно разместить здание в этом месте");
            return false;
        }
        
        // Используем универсальную стратегию
        if (!_universalStrategy.CanBuild(data))
        {
            Debug.Log("Недостаточно ресурсов для строительства");
            return false;
        }
        
        var instance = _universalStrategy.Create(data, position, rotation);
        if (instance != null)
        {
            _universalStrategy.OnBuilt(instance, data);
            return true;
        }
        
        return false;
    }
}
