using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [Header("Настройки")]
    [SerializeField] private GameObject buildingPrefab;
    [SerializeField] private float gridSize = 1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Material validPlacementMaterial;
    [SerializeField] private Material invalidPlacementMaterial;
    
    [Header("Клавиши управления")]
    [SerializeField] private KeyCode buildModeKey = KeyCode.B;
    
    private bool inBuildMode = false;
    private GameObject currentBuildingPreview;
    private bool canPlace = true;
    private Material originalMaterial;

    void Update()
    {
        if (Input.GetKeyDown(buildModeKey) && MenuController.Instance.isPaused == false)
        {
            ToggleBuildMode();
        }
        
        if (inBuildMode && MenuController.Instance.isPaused == false)
        {
            UpdateBuildingPreview();
            
            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                PlaceBuilding();
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                ExitBuildMode();
            }
        }
    }
    
    private void ToggleBuildMode()
    {
        inBuildMode = !inBuildMode;
        
        if (inBuildMode && MenuController.Instance.isPaused == false)
        {
            CreateBuildingPreview();
        }
        else
        {
            DestroyBuildingPreview();
        }
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
        
        SetPreviewTransparency(0.5f);
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
            // Привязка к сетке
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
        Renderer renderer = currentBuildingPreview.GetComponent<Renderer>();
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
    
    private void SetPreviewTransparency(float alpha)
    {
        Renderer[] renderers = currentBuildingPreview.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            Material mat = renderer.material;
            Color color = mat.color;
            color.a = alpha;
            mat.color = color;
            
            mat.SetFloat("_Mode", 3);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            mat.SetInt("_ZWrite", 0);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.EnableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = 3000;
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
    
    }
    
    private void ExitBuildMode()
    {
        inBuildMode = false;
        DestroyBuildingPreview();
    }
}