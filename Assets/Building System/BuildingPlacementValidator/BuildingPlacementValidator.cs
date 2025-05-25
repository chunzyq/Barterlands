using UnityEngine;

public class BuildingPlacementValidator : MonoBehaviour
{
    public bool IsValidPlacement(GameObject building)
    {
        var collider = building.GetComponent<Collider>();
        if (collider == null) return true;
        
        // Временно отключаем коллайдер
        collider.enabled = false;
        
        // Проверяем пересечения
        Collider[] overlaps = Physics.OverlapBox(
            collider.bounds.center,
            collider.bounds.extents,
            building.transform.rotation
        );
        
        // Включаем коллайдер обратно
        collider.enabled = true;
        
        return overlaps.Length == 0;
    }
}
