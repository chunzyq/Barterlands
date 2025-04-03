using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class BuildingInstance : MonoBehaviour
{
    public static BuildingInstance Instance;
    public static List<BuildingInstance> allBuildingsInstance = new List<BuildingInstance>();
    public string InstanceID {get; private set;} // создаётся поле для уникального ID здания, у которого есть публичный доступ для ДОСТУПА, но приватный доступ для изменения

    public BuildingData buildingData; // доступ к дате зданий
    public FactorySettings factorySettings;
    public LaboratorySettings laboratorySettings;
    public HouseSettings houseSettings;

    private bool canOpenUI = false;
    public bool isPreview = false;
    public bool isSelected { get; private set; }
    private Outline outline;


    private void Awake()
    {
        Instance = this;
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
        outline = GetComponent<Outline>();

        if (outline != null)
        {
            outline.enabled = false;
        }
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
        data.buildingID = buildingData.buildingID;
        data.position = transform.position;
        data.rotation = transform.eulerAngles;

        data.factorySettings = null;
        data.laboratorySettings = null;
        data.houseSettings = null;


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

    public void ApplySaveData(BuildingSaveData data)
    {
        switch (buildingData.buildingType)
        {
            case BuildingType.Factory:
                if (data.factorySettings != null)
                {
                    factorySettings = data.factorySettings;
                }
                break;
            case BuildingType.Laboratory:
                if (data.laboratorySettings != null)
                {
                    laboratorySettings = data.laboratorySettings;
                }
                break;
            case BuildingType.House:
                if (data.houseSettings != null)
                {
                    houseSettings = data.houseSettings;
                }
                break;
        }
    }

    private void OnMouseDown()
    {
        if (!canOpenUI)
        {
            return;
        }
        
        if (MenuController.Instance.isPaused == false)
        {
            UIController.Instance.OpenBuildingUI(this);
            Debug.Log("Clicked");
            if (outline != null)
            {
                outline.enabled = true;
                Debug.Log("Обводка включена!");
            }

            isSelected = true;
        }
    }
    public void Deselect()
    {
        isSelected = false;
        if (outline != null)
        {
            outline.enabled = false;
        }
    }
}