using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

public class BuildingInstance : MonoBehaviour
{
    public static BuildingInstance Instance;
    [Inject] UIController uIController;
    [Inject] SettlementManager settlementManager;
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

        settlementManager.RegisterBuilding(this);
    }

    private void OnDestroy()
    {
        settlementManager.UnregisterBuilding(this);
        allBuildingsInstance.Remove(this);
    }

    public int CurrentWorkerCount
    {
        get
        {
            switch (buildingData.buildingType)
            {
                case BuildingType.Factory:
                    return factorySettings.currentFactoryWorkers;
                case BuildingType.Laboratory:
                    return laboratorySettings.currentLaboratoryWorkers;
                case BuildingType.House:
                    return houseSettings.peopleCount;
                default:
                    return 0;
            }
        }
    }

    public Dictionary<ResourceType, int> GetHourlyProduction()
    {
        var prod = new Dictionary<ResourceType, int>();
        switch (buildingData.buildingType)
        {
            case BuildingType.Factory:
                int metal = factorySettings.currentFactoryWorkers * factorySettings.baseProductionRatePerWorker;
                prod[ResourceType.Metal] = metal;
                break;
            case BuildingType.Laboratory:
                int researchTime = laboratorySettings.currentLaboratoryWorkers * laboratorySettings.baseReduceResearchTimePerWorker;
                prod[ResourceType.ResearchTime] = researchTime;
                break;
            case BuildingType.House:
                break;
        }
        return prod;
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
            uIController.OpenBuildingUI(this);
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

    internal List<BuildingInstance> ToList()
    {
        throw new NotImplementedException();
    }
}