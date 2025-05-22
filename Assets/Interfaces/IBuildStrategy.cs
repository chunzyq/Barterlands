using UnityEngine;

public interface IBuildStrategy
{
    BuildingType buildingType { get; }
    bool CanBuild();

    void SpendCost();

    BuildingInstance Create(Vector3 pos, Quaternion rotation);

    void OnBuilt(BuildingInstance instance);
}
