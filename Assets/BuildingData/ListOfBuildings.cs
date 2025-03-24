using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ListOfBuildings : MonoBehaviour
{
    [SerializeField] List<BuildingData> allBuildings = new List<BuildingData>();
    public List<BuildingData> containerBuildings;

    public List<BuildingData> industrialBuildings;
    public List<BuildingData> scientificBuildings;
    public List<BuildingData> generalBuildings;
    public List<BuildingData> economicBuildings;
    public List<BuildingData> medicalBuildings;

    void Awake()
    {
        containerBuildings = new List<BuildingData>();

        for (int i = 0; i < allBuildings.Count; i++)
        {
            AddBuilding(allBuildings[i]);
        }

        industrialBuildings = allBuildings.Where(b => b.buildingID == "0").ToList();
        generalBuildings = allBuildings.Where(b => b.buildingID == "1").ToList();
        scientificBuildings = allBuildings.Where(b => b.buildingID == "2").ToList();
        economicBuildings = allBuildings.Where(b => b.buildingID == "3").ToList();
        medicalBuildings = allBuildings.Where(b => b.buildingID == "4").ToList();

    }
    private void AddBuilding(BuildingData building)
    {
        containerBuildings.Add(building);
    }
}
