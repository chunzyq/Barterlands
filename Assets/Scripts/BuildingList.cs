using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingList : MonoBehaviour
{
    private BuildingData buildingData;
    [SerializeField] List<BuildingData> allBuildings = new List<BuildingData>();
    public List<BuildingData> containerBuildings;
    public List<BuildingData> scientificBuilding;
    public List<BuildingData> industrialBuilding;
    public List<BuildingData> residentialBuilding;
    

    void Awake()
    {
        containerBuildings = new List<BuildingData>();
        for (int i = 0; i < allBuildings.Count; i++)
        {
            AddItem(allBuildings[i]);
        }
        industrialBuilding = allBuildings.Where(b => b.id == 0).ToList();
        residentialBuilding = allBuildings.Where(b => b.id == 1).ToList();
        scientificBuilding = allBuildings.Where(b => b.id == 2).ToList();
        
    }
    public void AddItem(BuildingData item)
    {
        containerBuildings.Add(item);
    }
}
