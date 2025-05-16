using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using Zenject;

public class SaveManager : MonoBehaviour
{
    private string savePath;
    [Inject] UIController uIController;

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/savefile.json";
        DontDestroyOnLoad(gameObject);
    }

    // Метод сохранения игры
    public void SaveGame()
    {
        SaveGameData saveData = new SaveGameData();
        saveData.buildingSaveDatas = new List<BuildingSaveData>();

        foreach (var building in BuildingInstance.allBuildingsInstance)
        {
            if (building.isPreview)
                continue;

            saveData.buildingSaveDatas.Add(building.GetSaveData());
        }

        saveData.metalAmount = ResourseManager.Instance.metalAmount;

        JsonSerializerSettings settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        settings.Converters.Add(new Vector3Converter());

        string json = JsonConvert.SerializeObject(saveData, settings);
        File.WriteAllText(savePath, json);
        Debug.Log("Игра сохранена: " + savePath);
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            JsonSerializerSettings settings = new JsonSerializerSettings();
            settings.Converters.Add(new Vector3Converter());
            SaveGameData saveData = JsonUtility.FromJson<SaveGameData>(json);

            foreach (var building in new List<BuildingInstance>(BuildingInstance.allBuildingsInstance))
            {
                Destroy(building.gameObject);
            }


            foreach (var buildingDataSave in saveData.buildingSaveDatas)
            {
                BuildingData bd = BuildingDatabase.GetBuildingData(buildingDataSave.buildingID);
                if (bd != null)
                {

                    GameObject buildingGO = Instantiate(bd.buildingPrefab, buildingDataSave.position, Quaternion.Euler(buildingDataSave.rotation));
                    BuildingInstance instance = buildingGO.GetComponent<BuildingInstance>();
                    if (instance != null)
                    {
                        instance.SetInstanceID(buildingDataSave.instanceID);
                        instance.buildingData = bd;
                        instance.ApplySaveData(buildingDataSave);
                    }
                }
                else
                {
                    Debug.LogWarning("Не найден BuildingData с ID: " + buildingDataSave.buildingID);
                }
            }

            ResourseManager.Instance.metalAmount = saveData.metalAmount;
            if (uIController.mainInterfaceUI != null)
            {
                uIController.mainInterfaceUI.UpdateMetalText(saveData.metalAmount);
            }
            Debug.Log("Игра загружена: " + savePath);
        }
        else
        {
            Debug.Log("Файл сохранения не найден: " + savePath);
        }
    }
}


[System.Serializable]
public class SaveGameData
{
    public List<BuildingSaveData> buildingSaveDatas;
    public int metalAmount;
}

[System.Serializable]
public class BuildingSaveData
{
    public string instanceID;
    public string buildingID;
    public Vector3 position;
    public Vector3 rotation;
    public FactorySettings factorySettings;
    public LaboratorySettings laboratorySettings;
    public HouseSettings houseSettings;
}