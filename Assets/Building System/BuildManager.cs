using System;
using UnityEngine;
using Zenject;

public class BuildManager : MonoBehaviour
{

    [Inject] private BuildInputHandler _inputHandler;
    [Inject] private BuildPreviewHandler _previewHandler;
    [Inject] private BuildingPlacementHandler _placementHandler;
    [Inject] private BuildUIHandler _uiHandler;
    [Inject] private BuildSelectionHandler _selectionHandler;

    public bool InBuildMode { get; private set; }
    public BuildingData CurrentBuildingData { get; private set; }

    public event Action<bool> OnBuildModeChanged;
    public event Action<BuildingData> OnBuildingSelected;


    void Start()
    {
        _inputHandler.OnPlaceBuilding += HandlePlaceBuilding;
        _inputHandler.OnCancelBuilding += ExitBuildMode;
        _inputHandler.OnBuildingClicked += HandleBuildingClicked;
        _inputHandler.OnEmptyClick += HandleDeselectBuildingMenu;

        _uiHandler.OnBuildingTypeSelected += SelectBuilding;
        _uiHandler.OnBuildModeEntered += EnterBuildMode;
        _uiHandler.OnBuildModeExited += ExitBuildMode;

        _selectionHandler.OnBuildingDeselect += HandleDeselectBuildingMenu;

        _placementHandler.OnBuildingPlaced += ExitBuildMode;
    }

    private void OnDestroy()
    {
        _inputHandler.OnPlaceBuilding -= HandlePlaceBuilding;
        _inputHandler.OnCancelBuilding -= ExitBuildMode;
        _inputHandler.OnBuildingClicked -= HandleBuildingClicked;
        _inputHandler.OnEmptyClick -= HandleDeselectBuildingMenu;

        _uiHandler.OnBuildingTypeSelected -= SelectBuilding;
        _uiHandler.OnBuildModeEntered -= EnterBuildMode;
        _uiHandler.OnBuildModeExited -= ExitBuildMode;

        _selectionHandler.OnBuildingDeselect -= HandleDeselectBuildingMenu;

        _placementHandler.OnBuildingPlaced -= ExitBuildMode;

    }

    public void EnterBuildMode()
    {
        InBuildMode = true;
        OnBuildModeChanged?.Invoke(true);
    }

    public void ExitBuildMode()
    {
        InBuildMode = false;
        CurrentBuildingData = null;
        OnBuildModeChanged?.Invoke(false);
        _previewHandler.DestroyPreview();
    }

    public void SelectBuilding(BuildingData data)
    {
        CurrentBuildingData = data;
        OnBuildingSelected?.Invoke(data);

        if (!InBuildMode)
        {
            EnterBuildMode();
        }

        _previewHandler.CreatePreview(data);
    }

    private void HandlePlaceBuilding()
    {
        if (!InBuildMode || CurrentBuildingData == null) return;

        var position = _previewHandler.GetPreviewPosition();
        var rotation = _previewHandler.GetPreviewRotation();

        if (_placementHandler.TryPlaceBuilding(CurrentBuildingData, position, rotation))
        {
            ExitBuildMode();
        }
    }

    private void HandleBuildingClicked(BuildingInstance building)
    {
        if (!InBuildMode)
        {
            _selectionHandler.SelectBuilding(building);
        }
    }

    private void HandleDeselectBuildingMenu()
    {
        if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            _selectionHandler.DeselectCurrent();
        }
    }
}
