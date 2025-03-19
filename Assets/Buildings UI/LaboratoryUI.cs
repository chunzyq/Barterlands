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

    private LaboratorySettings currentLaboratorySettings;

    public void UpdateUI(LaboratorySettings settings)
    {
        currentLaboratorySettings = settings;
        UpdateAllUI();
    }
    void Awake()
    {
        addWorkersButton.onClick.AddListener(OnAddWorkersButtonClicked);
        removeWorkersButton.onClick.AddListener(OnRemoveWorkersButtonClicked);
    }
    private void OnAddWorkersButtonClicked()
    {
        if (currentLaboratorySettings.currentLaboratoryWorkers < currentLaboratorySettings.maxLaboratoryWorkers)
        {
            currentLaboratorySettings.currentLaboratoryWorkers += 1;

            UpdateAllUI();
            UIController.Instance.mainInterfaceUI.UpdateIntefaceLaboratoryUI();
        }
        else
        {
            Debug.Log("Максимальное количество рабочих достигнуто.");
        }
    }
    public void OnRemoveWorkersButtonClicked()
    {
        if (currentLaboratorySettings.currentLaboratoryWorkers > 0)
        {
            currentLaboratorySettings.currentLaboratoryWorkers -= 1;

            UpdateAllUI();
            UIController.Instance.mainInterfaceUI.UpdateIntefaceLaboratoryUI();
        }
        else
        {
            Debug.Log("Минимальное количество рабочих достигнуто.");
        }
    }
    public void UpdateAllUI()
    {
        efficiencyPercent = currentLaboratorySettings.currentLabEfficiency = (currentLaboratorySettings.currentLaboratoryWorkers / (float)currentLaboratorySettings.maxLaboratoryWorkers) * 100;
        researchTimeDependsOnWorkers = currentLaboratorySettings.baseResearchTime - (currentLaboratorySettings.currentLaboratoryWorkers * currentLaboratorySettings.baseReduceResearchTimePerWorker);

        currentWorkersText.text = "Current Workers: " + currentLaboratorySettings.currentLaboratoryWorkers.ToString() + "/" + currentLaboratorySettings.maxLaboratoryWorkers;
        researchTimeText.text = "Research Time: " + researchTimeDependsOnWorkers.ToString() + " min";
        currentEfficiencyText.text = "Current Efficinecy: " + efficiencyPercent.ToString("F0") + "%";
    }
}
