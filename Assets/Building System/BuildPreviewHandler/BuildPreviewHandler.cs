using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class BuildPreviewHandler : MonoBehaviour
{
    [Header("Настройка сетки")]
    [SerializeField] private float gridSize = 1.0f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask buildingLayer;

    [Header("Материалы")]
    [SerializeField] private Material validPlacementMaterial;
    [SerializeField] private Material invalidPlacementMaterial;

    [Inject] private DiContainer _container;
    [Inject] private BuildingPlacementValidator _validator;

    private GameObject _currentPreview;
    private BuildingInstance _previewInstance;
    private bool _canPlace;

    public bool CanPlace => _canPlace;

    public Vector3 GetPreviewPosition() => _currentPreview?.transform.position ?? Vector3.zero;
    public Quaternion GetPreviewRotation() => _currentPreview?.transform.rotation ?? Quaternion.identity;

    private void Update()
    {
        UpdatePreviewPosition();
    }

    public void CreatePreview(BuildingData data)
    {
        DestroyPreview();

        if (data?.buildingPrefab == null) return;

        _currentPreview = _container.InstantiatePrefab(data.buildingPrefab);
        _previewInstance = _currentPreview.GetComponent<BuildingInstance>();

        if (_previewInstance != null)
        {
            _previewInstance.isPreview = true;
            BuildingInstance.allBuildingsInstance.Remove(_previewInstance);
        }

        var outline = _currentPreview.GetComponent<Outline>();
        if (outline != null) outline.enabled = false;

        SaveOriginalMaterials();
    }

    public void DestroyPreview()
    {
        if (_currentPreview != null)
        {
            Destroy(_currentPreview);
            _currentPreview = null;
            _previewInstance = null;
        }
    }

    private void UpdatePreviewPosition()
    {
        if (_currentPreview == null) return;

        bool isOverUI = EventSystem.current.IsPointerOverGameObject();

        if (isOverUI)
        {
            _currentPreview.SetActive(false);
            return;
        }

        _currentPreview.SetActive(true);

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 snappedPosition = GetSnappedPosition(hit.point);
            _currentPreview.transform.position = snappedPosition;

            _canPlace = _validator.IsValidPlacement(_currentPreview);
            UpdatePreviewMaterials();
        }
    }

    private Vector3 GetSnappedPosition(Vector3 worldPosition)
    {
        float x = Mathf.Round(worldPosition.x / gridSize) * gridSize;
        float z = Mathf.Round(worldPosition.z / gridSize) * gridSize;
        float y = worldPosition.y;

        var collider = _currentPreview.GetComponent<Collider>();
        if (collider != null)
        {
            y += collider.bounds.extents.y;
        }

        return new Vector3(x, y, z);
    }

    private void SaveOriginalMaterials()
    {
        var renderers = _currentPreview.GetComponentsInChildren<Renderer>();
    }
    
    private void UpdatePreviewMaterials()
    {
        var renderers = _currentPreview.GetComponentsInChildren<Renderer>();
        Material materialToUse = _canPlace ? validPlacementMaterial : invalidPlacementMaterial;
        
        foreach (var renderer in renderers)
        {
            if (renderer != null && materialToUse != null)
            {
                renderer.material = materialToUse;
            }
        }
    }
}
