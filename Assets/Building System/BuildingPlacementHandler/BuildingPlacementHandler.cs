using System;
using UnityEngine;
using Zenject;
using Barterlands.Logging;

public class BuildingPlacementHandler : MonoBehaviour
{

    public event Action OnBuildingPlaced;

    [Inject] private DiContainer _container;
    [Inject] private SettlementManager _settlementManager;
    [Inject] private BuildingPlacementValidator _validator;
    [Inject] private BuildPreviewHandler _previewHandler;   
    [Inject] private UniversalBuildingStrategy _universalStrategy;

    private ILoggerService _logger;

    void Awake()
    {
        _logger = new UnityLogger();      
    }


    public bool TryPlaceBuilding(BuildingData data, Vector3 position, Quaternion rotation)
    {
        if (!_previewHandler.CanPlace)
        {
            _logger.Error("Невозможно разместить здание в этом месте");
            return false;
        }

        if (!_universalStrategy.CanBuild(data))
        {
            _logger.Warning("Недостаточно ресурсов для строительства");
            return false;
        }

        var instance = _universalStrategy.Create(data, position, rotation);
        if (instance != null)
        {
            _universalStrategy.OnBuilt(instance, data);
            // OnBuildingPlaced?.Invoke();
            return true;
        }

        return false;
    }
}
