using UnityEngine;
using Zenject;

public class BuildSelectionHandler : MonoBehaviour
{
    [Inject] private UIController _uiController;
    
    [SerializeField] private LayerMask buildingLayer;
    
    private BuildingInstance _currentSelection;
    private Outline _hoveredOutline;
    
    private void Update()
    {
        HandleMouseHover();
    }
    
    public void SelectBuilding(BuildingInstance building)
    {
        if (_currentSelection != null)
        {
            _currentSelection.Deselect();
        }
        
        _currentSelection = building;
        building.isSelected = true;
        
        var outline = building.GetComponent<Outline>();
        if (outline != null) outline.enabled = true;
        
        _uiController.OpenBuildingUI(building);
    }
    
    public void DeselectCurrent()
    {
        if (_currentSelection != null)
        {
            _currentSelection.Deselect();
            _currentSelection = null;
        }
        
        _uiController.CloseBuildingUI();
    }
    
    private void HandleMouseHover()
    {
        if (MenuController.Instance.isPaused) return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, buildingLayer))
        {
            var building = hit.collider.GetComponent<BuildingInstance>();
            var outline = hit.collider.GetComponent<Outline>();
            
            if (outline != null && building != null && !building.isSelected && !building.isPreview)
            {
                if (_hoveredOutline != null && _hoveredOutline != outline)
                {
                    _hoveredOutline.enabled = false;
                }
                
                outline.enabled = true;
                _hoveredOutline = outline;
            }
        }
        else if (_hoveredOutline != null)
        {
            var building = _hoveredOutline.GetComponent<BuildingInstance>();
            if (building != null && !building.isSelected)
            {
                _hoveredOutline.enabled = false;
            }
            _hoveredOutline = null;
        }
    }
}
