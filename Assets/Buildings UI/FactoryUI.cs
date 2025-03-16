using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactoryUI : MonoBehaviour
{
    public TextMeshProUGUI productionRateText;
    public TextMeshProUGUI currentWorkersText;
    public TextMeshProUGUI currentEfficiencyText;
    public Button increaseButton;
    public Button addWorkersButton;

    private FactorySettings currentSettings;

    public void UpdateUI(FactorySettings settings)
    {
        currentSettings = settings;
        currentEfficiencyText.text = "Current Efficiency: " + settings.currentFacEfficiency.ToString("F0") + "%";
        currentWorkersText.text = "Current Workers: " + settings.currentFactoryWorkers.ToString();
        productionRateText.text = "Production Rate: " + settings.productionRate.ToString();

    }

    void Awake()
    {
        increaseButton.onClick.AddListener(OnIncreaseButtonClicked);
        addWorkersButton.onClick.AddListener(OnAddWorkersButtonClicked);
    }

    private void OnIncreaseButtonClicked()
    {
        currentSettings.productionRate += 10;
        productionRateText.text = "Production Rate: " + currentSettings.productionRate.ToString();
    }
    public void OnAddWorkersButtonClicked()
    {
        if (currentSettings.currentFactoryWorkers < currentSettings.maxFactoryWorkers)
        {
            currentSettings.currentFactoryWorkers += 1;
            float efficiencyPercent = currentSettings.currentFacEfficiency = (currentSettings.currentFactoryWorkers / (float)currentSettings.maxFactoryWorkers) * 100;
            currentWorkersText.text = "Current Workers: " + currentSettings.currentFactoryWorkers.ToString();
            currentEfficiencyText.text = "Current Efficiency: " + efficiencyPercent.ToString("F0") + "%";
        }
        else
        {
            Debug.Log("Максимальное количество рабочих достигнуто.");
            addWorkersButton.interactable = false;
        }
    }
}
