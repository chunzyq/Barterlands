using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

public class BuildInputHandler : MonoBehaviour
{
    [SerializeField] private LayerMask buildingLayer;

    public event Action OnPlaceBuilding;
    public event Action OnCancelBuilding;
    public event Action<BuildingInstance> OnBuildingClicked;

    private BuildManager _buildManager;

    [Inject]
    private void Construct(BuildManager buildManager)
    {
        _buildManager = buildManager;
    }

    private void Update()
    {
        if (MenuController.Instance.isPaused == true) return;

        bool isOverUI = EventSystem.current.IsPointerOverGameObject();

        if (_buildManager.InBuildMode && !isOverUI)
        {
            if (Input.GetMouseButtonDown(0))
            {
                OnPlaceBuilding?.Invoke();
            }

            if (Input.GetMouseButtonDown(1))
            {
                OnCancelBuilding?.Invoke();
            }
        }

        if (Input.GetMouseButtonDown(0) && !isOverUI)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, buildingLayer))
            {
                var building = hit.collider.GetComponent<BuildingInstance>();
                if (building != null && !building.isPreview)
                {
                    OnBuildingClicked?.Invoke(building);
                }
            }
        }
    }
}
