using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BuildManager : MonoBehaviour
{
    private BuildingData buildingData;
    public static BuildManager Instance;

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
    [SerializeField] private Material validPlacementMaterial;
    [SerializeField] private Material invalidPlacementMaterial;


    void Start()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        enterBuildMode_Btn.onClick.AddListener(OnEnterBuildModeButtonClicked);
        closeListOfBuildings_Btn.onClick.AddListener(OnCloseListOfBuildingsClicked);
    }

    void Update()
    {

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
        currentBuildingPreview = Instantiate(buildingPrefab);

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

        GameObject newBuilding = Instantiate(buildingPrefab, currentBuildingPreview.transform.position, currentBuildingPreview.transform.rotation);

        Renderer renderer = newBuilding.GetComponent<Renderer>();
        if (renderer != null && originalMaterial != null)
        {
            renderer.material = originalMaterial;
        }

        BuildingInstance buildingInstance = newBuilding.GetComponent<BuildingInstance>();
        if (buildingInstance != null)
        {
            buildingInstance.GenerateUniqueID();
        }
        
        inBuildMode = false;
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

    private void OnEnterBuildModeButtonClicked()
    {
        buildingPanel.SetActive(true);
    }
    private void OnCloseListOfBuildingsClicked()
    {
        inBuildMode = false;
        buildingPanel.SetActive(false);
    }
}
