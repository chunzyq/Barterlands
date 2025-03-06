using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New File", menuName = "Data/BuildingData")]
public class BuildingData : ScriptableObject
{
    public string buildingName;
    public Sprite iconSprite;
    public GameObject buildingPrefab_test;
    public string uiPanelTag;
    public int id;
}
