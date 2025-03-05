using System;
using UnityEngine;

public class BuildingPanel : MonoBehaviour
{
    public static BuildingPanel Instance;

    [SerializeField] private LayerMask buildingsLayer;
    // [SerializeField] private GameObject testPanelUI;
    [SerializeField] private BuildingList buildingList;

    public GameObject selectedBuilding;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (BuildManager.Instance.inBuildMode|| MenuController.Instance.isPaused) return;
        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetKey(KeyCode.LeftAlt) && Input.GetMouseButtonDown(0))
        {
            HandleOnBuildingClicked();
        }
    }

    private void HandleOnBuildingClicked()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, buildingsLayer))
        {
            selectedBuilding = hit.collider.gameObject;
            BuildingData buildingData = FindBuildingByName(selectedBuilding);
            Debug.Log(buildingData);

            if (buildingData != null && buildingData.buildingPanelUI != null)
            {
                buildingData.buildingPanelUI.SetActive(true);
            }
            
        }
    }

    private BuildingData FindBuildingByName(GameObject buildingObject)
    {
        if (buildingObject == null) return null;

        string objectName = buildingObject.name.Replace("(Clone)", "").Trim();

        if (buildingList == null) return null;

        foreach (var building in buildingList.containerBuildings)
        {
            if (building != null && building.buildingPrefab_test != null && building.buildingPrefab_test.name == objectName)
            {
                Debug.Log(building);
                return building;
            }
        }
        
        return null;
    }

    // private void CloseMenu()
    // {
    //     testPanelUI.SetActive(false);
    // }
}
