using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Material validPlacementMaterial;
    [SerializeField] private Material invalidPlacementMaterial;
    
    private bool inBuildMode = false;
    private bool canPlace = true;
    private GameObject buildingPrefab;
    private GameObject currentBuildingPreview;
    private Material originalMaterial;

    void Update()
    {
        
        if (inBuildMode && MenuController.Instance.isPaused == false)
        {
            UpdateBuildingPreview();
            
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


    public void SelectBuilding(GameObject buildingPrefabToPlace)
    {
        if (currentBuildingPreview != null)
        {
            Destroy(currentBuildingPreview);
        }

        buildingPrefab = buildingPrefabToPlace;
        inBuildMode = true;

        CreateBuildingPreview();
    }
    
    private void CreateBuildingPreview()
    {
        if (buildingPrefab == null)
        {
            Debug.LogError("Не назначен префаб здания в инспекторе!");
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
    
    private void DestroyBuildingPreview()
    {
        if (currentBuildingPreview != null)
        {
            Destroy(currentBuildingPreview);
            currentBuildingPreview = null;
        }
    }
    
    private void UpdateBuildingPreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, 1000f, groundLayer) && MenuController.Instance.isPaused == false)
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
        GameObject newBuilding = Instantiate(buildingPrefab, currentBuildingPreview.transform.position, currentBuildingPreview.transform.rotation);
        
        Renderer renderer = newBuilding.GetComponent<Renderer>();
        if (renderer != null && originalMaterial != null)
        {
            renderer.material = originalMaterial;
        }
        DestroyBuildingPreview();
        CreateBuildingPreview();
    
    }
    
    public void ExitBuildMode()
    {
        inBuildMode = false;
        DestroyBuildingPreview();
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
}