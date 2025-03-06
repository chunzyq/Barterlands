using System;
using UnityEngine;

public class BuildingPanel : MonoBehaviour
{
    public static BuildingPanel Instance;

    [SerializeField] private LayerMask buildingsLayer;

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
        if (BuildManager.Instance.inBuildMode || MenuController.Instance.isPaused) return;
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
            Building selectedBuilding = hit.collider.GetComponent<Building>();
            
            if (selectedBuilding != null && selectedBuilding.currentData != null)
            {
                UIController.Instance.ShowPanelByTag(selectedBuilding.currentData.uiPanelTag);
            }
        }
        else
        {
            UIController.Instance.HideAllPanels();
        }
    }
}
