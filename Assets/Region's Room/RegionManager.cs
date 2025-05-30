using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RegionManager : MonoBehaviour
{
    [SerializeField] private List<GameRegion> allRegions = new List<GameRegion>();
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
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            ChangeLevel();
        }
    }
    private void OnRegionsStateChanged(GameRegion region, RegionState newState)
    {
        Debug.Log($"Регион {region.regionData.coordinates} изменил состояние на {newState}");
    }

    // void Update()
    // {

    //     if (Input.GetKeyDown(KeyCode.B))
    //     {
    //         settlementManager.LevelUp(SettlementLevel.MakeShiftShelter);
    //     }
    // }
    void ChangeLevel()
    {
        settlementManager.TierUp(SettlementLevel.MakeShiftShelter);
    }
}
