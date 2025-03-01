using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingList : MonoBehaviour
{
    [SerializeField] List<BuildingData> allBuildings = new List<BuildingData>();
    public List<BuildingData> containerBuildings {get; private set; }

    void Awake()
    {
        containerBuildings = new List<BuildingData>();
        for (int i = 0; i < allBuildings.Count; i++)
        {
            AddItem(allBuildings[i]);
        }
    }
    public void AddItem(BuildingData item)
    {
        containerBuildings.Add(item);
    }
}
