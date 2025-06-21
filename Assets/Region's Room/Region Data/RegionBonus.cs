using UnityEngine;

[CreateAssetMenu(fileName = "RegionBonus", menuName = "Bonus/RegionBonus")]
public class RegionBonus : ScriptableObject
{
    public string bonusName;
    public string description;
    public int defenseBonus;
    public int resourceBonus;
    public int attackBonus;
}
