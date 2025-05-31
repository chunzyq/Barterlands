using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChooseRegionForGrow : MonoBehaviour
{
    [SerializeField] private List<GameRegion> allRegions = new List<GameRegion>();

    private const float EPSILON = 0.1f;
    private const float TARGETSTEP = 500f;

    public List<GameRegion> GetChoosenRegions()
    {
        var chosenRegions = allRegions.Where(r => r.regionData.state == RegionState.Chosen).ToList();

        if (chosenRegions.Count == 0) return new List<GameRegion>();

        var neighbourSet = new HashSet<GameRegion>();
        foreach (var chosen in chosenRegions)
        {
            Vector3 pos0 = chosen.transform.position;

            foreach (var region in allRegions)
            {
                if (region.regionData.state != RegionState.Locked)
                {
                    continue;
                }

                Vector3 pos = region.transform.position;

                float dx = Mathf.Abs(pos.x - pos0.x);
                float dz = Mathf.Abs(pos.z - pos0.z);

                bool isXNeighbor = (Mathf.Abs(dx - TARGETSTEP) < EPSILON) && (Mathf.Abs(dz) < EPSILON);
                bool isZNeighbor = (Mathf.Abs(dz - TARGETSTEP) < EPSILON) && (Mathf.Abs(dx) < EPSILON);

                if (isXNeighbor || isZNeighbor)
                {
                    neighbourSet.Add(region);
                }
            }
        }

        var lockedNeighbors = neighbourSet.ToList();
        if (lockedNeighbors.Count <= 2)
            return lockedNeighbors;

        return lockedNeighbors
            .OrderBy(_ => Random.value)
            .Take(2)
            .ToList();
    }

    public void ApplyGrowth(List<GameRegion> candidates)
    {
        if (candidates == null || candidates.Count == 0)
            return;

        foreach (var region in candidates)
        {
            region.ChangeState(RegionState.Chosen);
        }
    }


    // private void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.G))
    //     {
    //         var list = GetChoosenRegions();
    //         ApplyGrowth(list);
    //     }
    // }
}
