using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Point Data", menuName = "Map/MapPointData")]
public class MapPointData : ScriptableObject
{
    public string pointName;
    public Vector2 pointPosition;
    public bool isUnlocked;
    public Sprite pointIcon;

    public List<MapPointData> nextPoints;
}
