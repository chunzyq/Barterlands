using UnityEngine;

public class BuildingPlacementValidator : MonoBehaviour
{
    public bool IsValidPlacement(GameObject building)
    {
        var collider = building.GetComponent<Collider>();
        if (collider == null) return true;
        
        collider.enabled = false;
        
        Collider[] overlaps = Physics.OverlapBox(
            collider.bounds.center,
            collider.bounds.extents,
            building.transform.rotation
        );
        
        collider.enabled = true;
        
        return overlaps.Length == 0;
    }
}
