using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LaboratoryUI : MonoBehaviour
{
    public TextMeshProUGUI researchTimeText;
    public TextMeshProUGUI currentWorkersText;
    public TextMeshProUGUI currentEfficiencyText;
    public Button addWorkersButton;
    public Button removeWorkersButton;

    public float efficiencyPercent;
    public int researchTimeDependsOnWorkers;

    private LaboratorySettings currentSettings;

    public void UpdateUI(LaboratorySettings settings)
    {
        currentSettings = settings;
        UpdateAllUI();
    }
    void Awake()
    {
        addWorkersButton.onClick.AddListener(OnAddWorkersButtonClicked);
        removeWorkersButton.onClick.AddListener(OnRemoveWorkersButtonClicked);
    }
    private void OnAddWorkersButtonClicked()
    {
        if (currentSettings.currentLaboratoryWorkers < currentSettings.maxLaboratoryWorkers)
        {
            currentSettings.currentLaboratoryWorkers += 1;

            UpdateAllUI();
        }
        else
        {
            Debug.Log("Максимальное количество рабочих достигнуто.");
        }
    }
    public void OnRemoveWorkersButtonClicked()
    {
        if (currentSettings.currentLaboratoryWorkers > 0)
        {
            currentSettings.currentLaboratoryWorkers -= 1;

            UpdateAllUI();
        }
        else
        {
            Debug.Log("Минимальное количество рабочих достигнуто.");
        }
    }
    public void UpdateAllUI()
    {
        efficiencyPercent = currentSettings.currentLabEfficiency = (currentSettings.currentLaboratoryWorkers / (float)currentSettings.maxLaboratoryWorkers) * 100;
        researchTimeDependsOnWorkers = currentSettings.baseResearchTime - (currentSettings.currentLaboratoryWorkers * currentSettings.baseReduceResearchTimePerWorker);

        currentWorkersText.text = "Current Workers: " + currentSettings.currentLaboratoryWorkers.ToString() + "/" + currentSettings.maxLaboratoryWorkers;
        researchTimeText.text = "Research Time: " + researchTimeDependsOnWorkers.ToString() + " min";
        currentEfficiencyText.text = "Current Efficinecy: " + efficiencyPercent.ToString("F0") + "%";
    }
}
