using System.Collections.Generic;
using Mono.Cecil;
using UnityEngine;

[CreateAssetMenu(fileName = "New File", menuName = "Data/Building")]
public class BuildingData : ScriptableObject
{
    [Header("Индентификация")]
    public string buildingID;
    public string buildingName;
    public BuildingType buildingType;

    [Header("Визуализация")]
    public Sprite buildingIcon;
    public GameObject buildingPrefab;
    

    [Header("Стоимость")]
    public List<ResourceCost> cost = new List<ResourceCost>();

    public struct ResourceCost
    {
        public ResourceType type;
        public int amount;
    }
}
