using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    private string savePath;

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

        // Обходим все здания, кроме превью-объектов
        foreach (var building in BuildingInstance.allBuildingsInstance)
        {
            if (building.isPreview)
                continue;

            saveData.buildingSaveDatas.Add(building.GetSaveData());
        }

        // Сохраняем текущее количество металла
        saveData.metalAmount = ResourseManager.Instance.metalAmount;

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Игра сохранена: " + savePath);
    }

    // Метод загрузки игры
    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            SaveGameData saveData = JsonUtility.FromJson<SaveGameData>(json);

            // Удаляем все текущие здания
            foreach (var building in new List<BuildingInstance>(BuildingInstance.allBuildingsInstance))
            {
                Destroy(building.gameObject);
            }

            // Воссоздаём здания из сохранённых данных
            foreach (var buildingDataSave in saveData.buildingSaveDatas)
            {
                // Получаем BuildingData по buildingID (см. BuildingDatabase)
                BuildingData bd = BuildingDatabase.GetBuildingData(buildingDataSave.buildingID);
                if (bd != null)
                {
                    // Инстанцируем prefab здания
                    GameObject buildingGO = Instantiate(bd.buildingPrefab, buildingDataSave.position, Quaternion.Euler(buildingDataSave.rotation));
                    BuildingInstance instance = buildingGO.GetComponent<BuildingInstance>();
                    if (instance != null)
                    {
                        instance.SetInstanceID(buildingDataSave.instanceID);
                        instance.buildingData = bd;
                    }
                }
                else
                {
                    Debug.LogWarning("Не найден BuildingData с ID: " + buildingDataSave.buildingID);
                }
            }

            // Восстанавливаем количество ресурсов
            ResourseManager.Instance.metalAmount = saveData.metalAmount;
            if (UIController.Instance != null && UIController.Instance.mainInterfaceUI != null)
            {
                UIController.Instance.mainInterfaceUI.UpdateMetalText(saveData.metalAmount);
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
