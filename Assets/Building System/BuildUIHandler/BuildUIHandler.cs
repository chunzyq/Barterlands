using UnityEngine;
using UnityEngine.UI;
using System;

public class BuildUIHandler : MonoBehaviour
{
    [Header("Кнопки")]
    [SerializeField] private Button enterBuildModeBtn;
    [SerializeField] private Button closeListOfBuildingsBtn;
    
    [Header("UI панели")]
    [SerializeField] private GameObject buildingPanel;
    
    public event Action OnBuildModeEntered;
    public event Action OnBuildModeExited;
    public event Action<BuildingData> OnBuildingTypeSelected;

    private void Start()
    {
        enterBuildModeBtn.onClick.AddListener(() => OnBuildModeEntered?.Invoke());
        closeListOfBuildingsBtn.onClick.AddListener(() => OnBuildModeExited?.Invoke());
        
        var buildManager = GetComponent<BuildManager>();
        buildManager.OnBuildModeChanged += HandleBuildModeChanged;
    }
    
    private void HandleBuildModeChanged(bool isInBuildMode)
    {
        buildingPanel.SetActive(isInBuildMode);
    }
    
    public void SelectBuildingType(BuildingData data)
    {
        OnBuildingTypeSelected?.Invoke(data);
    }
}
