using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LaboratoryUI : MonoBehaviour
{
    [Inject] UIController uIController;
    [Inject] SettlementManager settlementManager;
    public TextMeshProUGUI researchTimeText;
    public TextMeshProUGUI currentWorkersText;
    public TextMeshProUGUI currentEfficiencyText;
    public Button addWorkersButton;
    public Button removeWorkersButton;
    public Button deleteBuildingButton;

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
        deleteBuildingButton.onClick.AddListener(OnDeleteBuildingButtonClicked);
    }

    private void OnDeleteBuildingButtonClicked()
    {
        uIController.DeleteCurrentBuilding();
    }

    private void OnAddWorkersButtonClicked()
    {
        if (currentLaboratorySettings.currentLaboratoryWorkers < currentLaboratorySettings.maxLaboratoryWorkers && settlementManager.freeWorkers > 0)
        {
            currentLaboratorySettings.currentLaboratoryWorkers += 1;

            UpdateAllUI();
            uIController.mainInterfaceUI.UpdateIntefaceLaboratoryUI();
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
            uIController.mainInterfaceUI.UpdateIntefaceLaboratoryUI();
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

        if (currentLaboratorySettings.currentLaboratoryWorkers == currentLaboratorySettings.maxLaboratoryWorkers)
        {
            addWorkersButton.gameObject.SetActive(false);
        }
        else
        {
            addWorkersButton.gameObject.SetActive(true);
        }

        if (currentLaboratorySettings.currentLaboratoryWorkers == 0)
        {
            removeWorkersButton.gameObject.SetActive(false);
        }
        else
        {
            removeWorkersButton.gameObject.SetActive(true);
        }
    }
}