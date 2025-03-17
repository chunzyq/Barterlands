using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactoryUI : MonoBehaviour
{
    public TextMeshProUGUI productionRateText;
    public TextMeshProUGUI currentWorkersText;
    public TextMeshProUGUI currentEfficiencyText;
    public Button addWorkersButton;
    public Button removeWorkersButton;

    public float efficiencyPercent;
    public int productionRate;

    private FactorySettings currentSettings;

    public void UpdateUI(FactorySettings settings)
    {
        currentSettings = settings;
        UpdateAllUI();
        // currentEfficiencyText.text = "Current Efficiency: " + settings.currentFacEfficiency.ToString("F0") + "%";
        // currentWorkersText.text = "Current Workers: " + settings.currentFactoryWorkers.ToString() + "/" + settings.maxFactoryWorkers;
        // productionRateText.text = "Production Rate: " + settings.productionRate.ToString() + "/h";

    }

    void Awake()
    {
        addWorkersButton.onClick.AddListener(OnAddWorkersButtonClicked);
        removeWorkersButton.onClick.AddListener(OnRemoveWorkersButtonClicked);
    }
    public void OnAddWorkersButtonClicked()
    {
        if (currentSettings.currentFactoryWorkers < currentSettings.maxFactoryWorkers)
        {
            currentSettings.currentFactoryWorkers += 1;

            UpdateAllUI();

            // efficiencyPercent = currentSettings.currentFacEfficiency = (currentSettings.currentFactoryWorkers / (float)currentSettings.maxFactoryWorkers) * 100;
            // productionRate = currentSettings.currentFactoryWorkers * currentSettings.baseProductionRatePerWorker;

            // currentWorkersText.text = "Current Workers: " + currentSettings.currentFactoryWorkers.ToString() + "/" + currentSettings.maxFactoryWorkers;
            // currentEfficiencyText.text = "Current Efficiency: " + efficiencyPercent.ToString("F0") + "%";
            // productionRateText.text = "Production Rate: " + productionRate.ToString() + "/h";
        }
        else
        {
            Debug.Log("Максимальное количество рабочих достигнуто.");
        }
    }
    public void OnRemoveWorkersButtonClicked()
    {
        if (currentSettings.currentFactoryWorkers > 0)
        {
            currentSettings.currentFactoryWorkers -= 1;

            UpdateAllUI();
        

            // efficiencyPercent = currentSettings.currentFacEfficiency = (currentSettings.currentFactoryWorkers / (float)currentSettings.maxFactoryWorkers) * 100;
            // productionRate = currentSettings.currentFactoryWorkers * currentSettings.baseProductionRatePerWorker;

            // currentWorkersText.text = "Current Workers: " + currentSettings.currentFactoryWorkers.ToString() + "/" + currentSettings.maxFactoryWorkers;
            // currentEfficiencyText.text = "Current Efficiency: " + efficiencyPercent.ToString("F0") + "%";
            // productionRateText.text = "Production Rate: " + productionRate.ToString() + "/h";
        }
        else
        {
            Debug.Log("Минимальное количество рабочих достигнуто.");
        }
    }
    public void UpdateAllUI()
    {
        efficiencyPercent = currentSettings.currentFacEfficiency = (currentSettings.currentFactoryWorkers / (float)currentSettings.maxFactoryWorkers) * 100;
        productionRate = currentSettings.currentFactoryWorkers * currentSettings.baseProductionRatePerWorker;

        currentWorkersText.text = "Current Workers: " + currentSettings.currentFactoryWorkers.ToString() + "/" + currentSettings.maxFactoryWorkers;
        currentEfficiencyText.text = "Current Efficiency: " + efficiencyPercent.ToString("F0") + "%";
        productionRateText.text = "Production Rate: " + productionRate.ToString() + "/h";
    }
}
