using System.Collections.Generic;
using UnityEngine;

public static class BuildingDatabase
{
    private static Dictionary<string, BuildingData> dataDict;

    // Инициализация базы данных
    public static void Initialize()
    {
        dataDict = new Dictionary<string, BuildingData>();
        BuildingData[] allData = Resources.LoadAll<BuildingData>("BuildingsData");
        foreach (var data in allData)
        {
            dataDict[data.buildingID] = data;
        }
    }

    public static BuildingData GetBuildingData(string id)
    {
        if (dataDict == null)
        {
            Initialize();
        }
        if (dataDict.ContainsKey(id))
        {
            return dataDict[id];
        }
        return null;
    }
}
