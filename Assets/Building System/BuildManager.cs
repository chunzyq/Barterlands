using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

public class BuildManager : MonoBehaviour
{
    [Inject] UIController uIController;
    [Inject] DiContainer container;
    [Inject] ResourseManager resourseManager;
    [Inject] SettlementManager settlementManager;

    [Header("Кнопки")]
    public Button enterBuildMode_Btn;
    public Button closeListOfBuildings_Btn;

    [Header("UI панели")]
    public GameObject buildingPanel;

    public bool inBuildMode = false;
    private bool canPlace = false;
    private GameObject buildingPrefab;
    private GameObject currentBuildingPreview;
    private Material originalMaterial;

    [SerializeField] private float gridSize = 1.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask buildingLayer;
    [SerializeField] private Material validPlacementMaterial;
    [SerializeField] private Material invalidPlacementMaterial;

    private GameObject newBuilding;
    private Outline outline;


    void Start()
    {
        enterBuildMode_Btn.onClick.AddListener(OnEnterBuildModeButtonClicked);
        closeListOfBuildings_Btn.onClick.AddListener(OnCloseListOfBuildingsClicked);
    }

    void Update()
    {
        MouseOnBuilding();

        if (inBuildMode && MenuController.Instance.isPaused == false)
        {
            bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

            UpdatePrefabPreview();

            if (Input.GetMouseButtonDown(0) && canPlace && MenuController.Instance.isPaused == false)
            {
                PlaceBuilding();
            }
            if (Input.GetMouseButtonDown(1))
            {
                ExitBuildMode();
            }
        }
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit, 100f, buildingLayer))
            {
                if (uIController.currentBuildingInstance != null)
                {
                    uIController.CloseBuildingUI();
                }
            }
        }
    }

    public void SelectBuilding(GameObject building)
    {

        if (currentBuildingPreview != null)
        {
            Destroy(currentBuildingPreview);
        }

        buildingPrefab = building;
        inBuildMode = true;

        CreateBuildingPreview();
    }

    private void CreateBuildingPreview()
    {

        if (buildingPrefab == null)
        {
            inBuildMode = false;
            return;
        }
        currentBuildingPreview = container.InstantiatePrefab(buildingPrefab, Vector3.zero, Quaternion.identity, null);
        //BuildingInstance var previewInstance = currentBuildingPreview.GetComponent<BuildingInstance>();
        var previewInstance = currentBuildingPreview.GetComponent<BuildingInstance>();
        previewInstance.isPreview = true;
        BuildingInstance.allBuildingsInstance.Remove(previewInstance);


        outline = currentBuildingPreview.GetComponent<Outline>();

        outline.enabled = false;

        if (previewInstance != null)
        {
            previewInstance.isPreview = true;
            BuildingInstance.allBuildingsInstance.Remove(previewInstance);
        }

        Renderer renderer = currentBuildingPreview.GetComponent<Renderer>();

        if (renderer != null)
        {
            originalMaterial = renderer.material;
        }
    }

    private void UpdatePrefabPreview()
    {
        if (currentBuildingPreview == null) return;

        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

        if (isOverUI)
        {
            currentBuildingPreview.SetActive(false);
            return;
        }
        else
        {
            currentBuildingPreview.SetActive(true);
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, groundLayer) && MenuController.Instance.isPaused == false)
        {
            float x = Mathf.Round(hit.point.x / gridSize) * gridSize;
            float z = Mathf.Round(hit.point.z / gridSize) * gridSize;
            float y = hit.point.y;

            Collider collider = currentBuildingPreview.GetComponent<Collider>();

            if (collider != null)
            {
                y += collider.bounds.extents.y;
            }

            Vector3 snappedPosition = new Vector3(x, y, z);
            currentBuildingPreview.transform.position = snappedPosition;

            canPlace = CheckIfPlacementValid();
            UpdatePreviewMaterial();
        }
    }

    private bool CheckIfPlacementValid()
    {
        Collider buildingCollider = currentBuildingPreview.GetComponent<Collider>();
        if (buildingCollider != null)
        {
            buildingCollider.enabled = false;

            Collider[] colliders = Physics.OverlapBox(
                buildingCollider.bounds.center,
                buildingCollider.bounds.extents,
                currentBuildingPreview.transform.rotation
            );

            buildingCollider.enabled = true;

            return colliders.Length == 0;
        }

        return true;
    }

    private void UpdatePreviewMaterial()
    {
        Renderer[] renderers = currentBuildingPreview.GetComponentsInChildren<Renderer>();
        foreach (var renderer in renderers)
        {
            if (renderer != null)
            {
                if (canPlace)
                {
                    if (validPlacementMaterial != null)
                    {
                        renderer.material = validPlacementMaterial;
                    }
                }
                else
                {
                    if (invalidPlacementMaterial != null)
                    {
                        renderer.material = invalidPlacementMaterial;
                    }
                }
            }
        }
    }

    private void PlaceBuilding()
    {
        if (currentBuildingPreview == null) return;

        var previewInstance = currentBuildingPreview.GetComponent<BuildingInstance>();

        if (previewInstance != null && previewInstance.buildingData.buildingType == BuildingType.Factory)
        {
            int factoryCost = 30;
            bool canBuild = resourseManager.SpendMetal(factoryCost);
            if (!canBuild)
            {
                Debug.Log("Недостаточно ресурса: металл");
                Destroy(currentBuildingPreview);
                currentBuildingPreview = null;
                return;
            }
        }
        else if (previewInstance != null && previewInstance.buildingData.buildingType == BuildingType.Laboratory)
        {
            int laboratoryCost = 60;
            bool canBuild = resourseManager.SpendMetal(laboratoryCost);
            if (!canBuild)
            {
                Debug.Log("Недостаточно ресурса: металл");
                Destroy(currentBuildingPreview);
                currentBuildingPreview = null;
                return;
            }
        }

        currentBuildingPreview.GetComponent<BuildingInstance>().isPreview = true;

        newBuilding = container.InstantiatePrefab(buildingPrefab, currentBuildingPreview.transform.position, currentBuildingPreview.transform.rotation, null);

        Renderer renderer = newBuilding.GetComponent<Renderer>();
        if (renderer != null && originalMaterial != null)
        {
            renderer.material = originalMaterial;
        }

        outline = newBuilding.GetComponent<Outline>();
        outline.enabled = false;

        BuildingInstance buildingInstance = newBuilding.GetComponent<BuildingInstance>();

        if (buildingInstance != null)
        {
            buildingInstance.GenerateUniqueID();
            
            if (buildingInstance.buildingData.buildingType == BuildingType.Laboratory)
            {
                uIController.mainInterfaceUI.UpdateIntefaceLaboratoryUI();
            }
            else if (buildingInstance.buildingData.buildingType == BuildingType.Factory)
            {
                uIController.mainInterfaceUI.UpdateIntefaceFactoryUI();
            }
        }

        StartCoroutine(DelayedUIUpdate());
        Destroy(currentBuildingPreview);
        currentBuildingPreview = null;
        inBuildMode = false;
    }

    private void MouseOnBuilding()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, buildingLayer) && MenuController.Instance.isPaused == false)
        {
            BuildingInstance buildingInstance = hit.collider.GetComponent<BuildingInstance>();
            Outline currentOutline = hit.collider.GetComponent<Outline>();
            
            if (currentOutline != null && buildingInstance != null && !buildingInstance.isSelected)
            {
                currentOutline.enabled = true;
            }

            outline = currentOutline;
        }
        else
        {
            if (outline != null)
            {
                BuildingInstance buildingInstance = outline.GetComponent<BuildingInstance>();

                if (buildingInstance != null && !buildingInstance.isSelected)
                {
                    outline.enabled = false;
                }
            }
        }
    }

    private IEnumerator DelayedUIUpdate()
    {
        yield return null;

        uIController.mainInterfaceUI.UpdateMetalText(resourseManager.metalAmount);
        
    }

    private void ExitBuildMode()
    {
        inBuildMode = false;
        DestroyBuildingPreview();
    }

    private void DestroyBuildingPreview()
    {
        if (currentBuildingPreview != null)
        {
            Destroy(currentBuildingPreview);
            currentBuildingPreview = null;
        }

    }
    public void DeleteBuilding(BuildingInstance building)
    {
        Destroy(building);
    }

    private void OnEnterBuildModeButtonClicked()
    {
        buildingPanel.SetActive(true);
    }
    private void OnCloseListOfBuildingsClicked()
    {
        inBuildMode = false;
        buildingPanel.SetActive(false);
    }

    public void TryBuildFactory()
    {
        int factoryCost = 30;
        if (resourseManager.SpendMetal(factoryCost))
        {
            Debug.Log("Завод построен!");
        }
        else
        {
            Debug.Log("Недостаточно ресурса: металл");
        }
    }
}
