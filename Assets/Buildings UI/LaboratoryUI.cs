using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LaboratoryUI : MonoBehaviour
{
    public TextMeshProUGUI researchTime;
    public TextMeshProUGUI currentWorkers;
    public TextMeshProUGUI currentEfficiencyText;
    public Button increaseResearchTime;
    public Button addWorkersButton;

    private LaboratorySettings currentSettings;

    public void UpdateUI(LaboratorySettings settings)
    {
        currentSettings = settings;
        currentEfficiencyText.text = "Current Efficinecy: " + settings.currentLabEfficiency.ToString("F0") + "%";
        currentWorkers.text = "Current Workers: " + settings.currentLaboratoryWorkers.ToString();
        researchTime.text = "Research Time: " + settings.researchTime.ToString();
    }
    void Awake()
    {
        increaseResearchTime.onClick.AddListener(OnIncreaseButtonClicked);
        addWorkersButton.onClick.AddListener(OnAddWorkersButtonClicked);
    }
    private void OnIncreaseButtonClicked()
    {
        currentSettings.researchTime += 10;
        researchTime.text = "Research Time: " + currentSettings.researchTime.ToString();
    }
    private void OnAddWorkersButtonClicked()
    {
        if (currentSettings.currentLaboratoryWorkers < currentSettings.maxLaboratoryWorkers)
        {
            currentSettings.currentLaboratoryWorkers += 1;
            float efficiencyPercent = currentSettings.currentLabEfficiency = (currentSettings.currentLaboratoryWorkers / (float)currentSettings.maxLaboratoryWorkers) * 100; 
            currentWorkers.text = "Current Workers: " + currentSettings.currentLaboratoryWorkers.ToString();
            currentEfficiencyText.text = "Current Efficinecy: " + efficiencyPercent.ToString("F0") + "%";
        }
        else
        {
            Debug.Log("Максимальное количество рабочих достигнуто.");
            addWorkersButton.interactable = false;
        }
    }
}
