using System.Collections.Generic;
using NUnit.Framework.Internal;
using UnityEngine;
using Zenject;

public class RegionManager : MonoBehaviour
{
    [SerializeField] private List<GameRegion> allRegions = new List<GameRegion>();
    [SerializeField] private ChooseRegionForGrow regionSelector;
    [SerializeField] private RegionUIHandler regionUIHandler;
    [Inject] SettlementManager settlementManager;

    void Start()
    {
        foreach (GameRegion region in allRegions)
        {
            region.OnStateChanged += OnRegionsStateChanged;
        }
        settlementManager.OnSettlementLevelChanged += ChangeLevel;
    }
    void OnDestroy()
    {
        settlementManager.OnSettlementLevelChanged -= ChangeLevel;

        foreach (GameRegion region in allRegions)
        {
            region.OnStateChanged -= OnRegionsStateChanged;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            settlementManager.TierUp(SettlementLevel.MakeShiftShelter);
        }
    }
    private void OnRegionsStateChanged(GameRegion region, RegionState newState)
    {
        Debug.Log($"Регион {region.regionData.coordinates} изменил состояние на {newState}");
    }
    void ChangeLevel()
    {
        List<GameRegion> candidates = regionSelector.GetChoosenRegions();
        Debug.Log($"[RegionManager] Найдено кандидатов: {candidates.Count}");

        regionUIHandler.OpenGrowthMenu(candidates);
    }
}
