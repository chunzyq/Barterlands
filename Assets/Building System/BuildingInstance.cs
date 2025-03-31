using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInstance : MonoBehaviour
{
    public static List<BuildingInstance> allBuildingsInstance = new List<BuildingInstance>();
    public string InstanceID {get; private set;} // создаётся поле для уникального ID здания, у которого есть публичный доступ для ДОСТУПА, но приватный доступ для изменения

    public BuildingData buildingData; // доступ к дате зданий
    public FactorySettings factorySettings;
    public LaboratorySettings laboratorySettings;
    public HouseSettings houseSettings;

    private bool canOpenUI = false;
    public bool isPreview = false;

    private void Awake()
    {
        if (!isPreview)
        {
            allBuildingsInstance.Add(this);
        }

        switch (buildingData.buildingType)
        {
            case BuildingType.Factory:
                factorySettings = new FactorySettings();
                break;
            case BuildingType.House:
                houseSettings = new HouseSettings();
                break;
            case BuildingType.Laboratory:
                laboratorySettings = new LaboratorySettings();
                break;
        }
    }

    private void OnDestroy()
    {
        allBuildingsInstance.Remove(this);
    }
    private void Start()
    {
        StartCoroutine(EnableUIInteraction());
    }

    private IEnumerator EnableUIInteraction()
    {
        yield return new WaitForSeconds(0.5f);
        canOpenUI = true;
    }
    public void GenerateUniqueID()
    {
        InstanceID = System.Guid.NewGuid().ToString(); // генерируется уникальный ID
        Debug.Log(InstanceID);
    }

    public void SetInstanceID(string id)
    {
        InstanceID = id;
    }

    public BuildingSaveData GetSaveData()
    {
        BuildingSaveData data = new BuildingSaveData();
        data.instanceID = InstanceID;
        data.buildingID = buildingData.buildingID; // предполагается, что в BuildingData есть поле buildingID
        data.position = transform.position;
        data.rotation = transform.eulerAngles;

        // Сохраняем настройки, если они применимы для данного типа здания
        switch (buildingData.buildingType)
        {
            case BuildingType.Factory:
                data.factorySettings = factorySettings;
                break;
            case BuildingType.Laboratory:
                data.laboratorySettings = laboratorySettings;
                break;
            case BuildingType.House:
                data.houseSettings = houseSettings;
                break;
        }

        return data;
    }

    private void OnMouseDown()
    {
        if (!canOpenUI)
        {
            return;
        }

        UIController.Instance.OpenBuildingUI(this); // при нажатии на ЛКМ на здание - инфа о нём передаётся в UIController в функцию
        Debug.Log("Clicked");
    }
}