using System.Collections.Generic;
using UnityEngine;

public class ApplyRegionBonus : MonoBehaviour
{
    [SerializeField] private SettlementManager settlementManager;
    [SerializeField] private ResourseManager resourseManager;
    [SerializeField] private List<GameRegion> allRegions = new List<GameRegion>();

    void Start()
    {
        foreach (var region in allRegions)
        {
            region.OnStateChanged += OnRegionStateChanged;
        }
    }
    void OnDestroy()
    {
        foreach (var region in allRegions)
        {
            region.OnStateChanged -= OnRegionStateChanged;
        }
    }

    private void OnRegionStateChanged(GameRegion region, RegionState state)
    {
        if (state == RegionState.Chosen)
        {
            RegionBonus bonus = region.regionData.bonus;
            ApplyBonusForRegion(bonus);
        }
    }

    private void ApplyBonusForRegion(RegionBonus bonus)
    {
        if (bonus.defenseBonus != 0)
        {
            resourseManager.AddResource(ResourceType.Metal, bonus.attackBonus);
        }
    }
}
