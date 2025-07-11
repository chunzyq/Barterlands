using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Barterlands.Logging;
using UnityEngine.PlayerLoop;

public class FactoryUI : MonoBehaviour
{
    [Inject] UIController uIController;
    [Inject] SettlementManager settlementManager;
    public TextMeshProUGUI productionRateText;
    public TextMeshProUGUI currentWorkersText;
    public TextMeshProUGUI currentEfficiencyText;
    public Button addWorkersButton;
    public Button removeWorkersButton;
    public Button deleteBuildingButton;

    public float efficiencyPercent;
    public int productionRate;

    private FactorySettings currentFactorySettings;
    private ILoggerService _logger;

    public void UpdateUI(FactorySettings settings)
    {
        currentFactorySettings = settings;
        UpdateAllUI();

    }

    void Awake()
    {
        addWorkersButton.onClick.AddListener(OnAddWorkersButtonClicked);
        removeWorkersButton.onClick.AddListener(OnRemoveWorkersButtonClicked);
        deleteBuildingButton.onClick.AddListener(OnDeleteBuildingButtonClicked);

        _logger = new UnityLogger();
    }

    private void OnDeleteBuildingButtonClicked()
    {
        uIController.DeleteCurrentBuilding();
    }

    public void OnAddWorkersButtonClicked()
    {
        if (currentFactorySettings.currentFactoryWorkers < currentFactorySettings.maxFactoryWorkers && settlementManager.freeWorkers > 0)
        {
            currentFactorySettings.currentFactoryWorkers += 1;

            UpdateAllUI();
        }
        else
        {
            _logger.Warning("Максимальное количество рабочих достигнуто.");
        }
    }
    public void OnRemoveWorkersButtonClicked()
    {
        if (currentFactorySettings.currentFactoryWorkers > 0)
        {
            currentFactorySettings.currentFactoryWorkers -= 1;

            UpdateAllUI();
            // UIController.Instance.mainInterfaceUI.UpdateIntefaceFactoryUI();
        }
        else
        {
            _logger.Warning("Минимальное количество рабочих достигнуто.");
        }
    }
    public void UpdateAllUI()
    {

        efficiencyPercent = currentFactorySettings.currentFacEfficiency = (currentFactorySettings.currentFactoryWorkers / (float)currentFactorySettings.maxFactoryWorkers) * 100;
        productionRate = currentFactorySettings.currentFactoryWorkers * currentFactorySettings.baseProductionRatePerWorker;

        currentWorkersText.text = "Current Workers: " + currentFactorySettings.currentFactoryWorkers.ToString() + "/" + currentFactorySettings.maxFactoryWorkers;
        currentEfficiencyText.text = "Current Efficiency: " + efficiencyPercent.ToString("F0") + "%";
        productionRateText.text = "Production Rate: " + productionRate.ToString() + "/h";

        if (currentFactorySettings.currentFactoryWorkers == currentFactorySettings.maxFactoryWorkers)
        {
            addWorkersButton.gameObject.SetActive(false);
        }
        else
        {
            addWorkersButton.gameObject.SetActive(true);
        }

        if (currentFactorySettings.currentFactoryWorkers == 0)
        {
            removeWorkersButton.gameObject.SetActive(false);
        }
        else
        {
            removeWorkersButton.gameObject.SetActive(true);
        }
    }
}