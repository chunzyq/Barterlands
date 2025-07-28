using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Zenject;
using System;

public class UnitSelectionUIManager : MonoBehaviour
{
    [Header("UI ссылки")]
    [SerializeField] private Transform selectedUnitsContainer;
    [SerializeField] private GameObject addUnitButtonPrefab;
    [SerializeField] private GameObject unitButtonPrefab;
    [SerializeField] private GameObject unitSelectionPanel;

    private Button addUnitButton;
    private StalkerUnitManager unitManager;
    public int maxAvailableUnits = 2;
    private List<StalkerData> selected = new List<StalkerData>();

    [Inject]
    public void Construct(StalkerUnitManager manager)
    {
        unitManager = manager;
    }

    private void Awake()
    {
        var addGO = Instantiate(addUnitButtonPrefab, selectedUnitsContainer);
        addUnitButton = addGO.GetComponent<Button>();
        addUnitButton.onClick.AddListener(ToggleSelectionPanel);
        unitSelectionPanel.SetActive(false);

        unitManager.OnStalkerChanged += PopulateSelectionPanel;
    }

    private void OnDisable()
    {
        if (unitManager != null)
        {
            unitManager.OnStalkerChanged -= PopulateSelectionPanel;
        }
    }

    private void ToggleSelectionPanel()
    {
        bool isActive = unitSelectionPanel.activeSelf;
        if (!isActive) PopulateSelectionPanel();
        unitSelectionPanel.SetActive(!isActive);
    }

    private void PopulateSelectionPanel()
    {
        var content = unitSelectionPanel.transform.Find("Content");
        foreach (Transform child in content) Destroy(child.gameObject);

        foreach (var stalker in unitManager.stalkers)
        {
            var go = Instantiate(unitButtonPrefab, content);
            var ui = go.GetComponent<UnitButtonUI>();
            ui.Initialize(stalker);
            ui.OnUnitClicked += OnStalkerSelected;
        }
    }

    private void OnStalkerSelected(StalkerData stalker)
    {
        if (selected.Contains(stalker)) return;
        selected.Add(stalker);

        var go = Instantiate(unitButtonPrefab, selectedUnitsContainer);
        var ui = go.GetComponent<UnitButtonUI>();
        ui.Initialize(stalker);
        ui.OnUnitClicked += _ => RemoveSelected(stalker, go);

        addUnitButton.transform.SetAsLastSibling();

        unitSelectionPanel.SetActive(false);
    }

    private void RemoveSelected(StalkerData stalker, GameObject go)
    {
        selected.Remove(stalker);
        Destroy(go);
    }

    public bool HasSelectedUnits => selected.Count > 0;
}
