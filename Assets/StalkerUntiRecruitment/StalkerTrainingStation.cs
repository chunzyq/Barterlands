using System.Collections;
using UnityEngine;
using Zenject;

public class StalkerTrainingStation : MonoBehaviour
{
    [Header("Data & UI")]
    [SerializeField] private StalkerTrainingData trainingData;
    [SerializeField] private TrainingStationUI trainingStationUI;

    private ResourseManager resourseManager;
    private SettlementManager settlementManager;

    private bool _isTraining;

    void Awake()
    {
        resourseManager = FindAnyObjectByType<ResourseManager>();
        settlementManager = FindAnyObjectByType<SettlementManager>();
    }

    public void BindUI(TrainingStationUI ui)
    {
        trainingStationUI = ui;
    }

    public void StartTraining()
    {
        if (_isTraining)
        {
            Debug.Log("Лох");
            return;
        }

        bool paid = resourseManager.SpendMetal(trainingData.metalCost);
        bool staffed = settlementManager.freeWorkers >= trainingData.humanCost;

        if (paid && staffed)
        {
            settlementManager.ModifyPopulation(trainingData.humanCost);

            trainingStationUI.SetStatus("Тренировка идёт...");
            _isTraining = true;

            StartCoroutine(TrainingCoroutine());
        }
        else
        {
            trainingStationUI.SetStatus("Недостаточно материалов и/или персонала");
        }
    }

    private IEnumerator TrainingCoroutine()
    {
        float remainingTime = trainingData.trainingTimeInSeconds;
        
        while (remainingTime > 0)
        {
            trainingStationUI.SetStatus($"Тренировка идёт... Осталось: {remainingTime:F0} сек.");
            remainingTime -= 1f;
            yield return new WaitForSeconds(1f);
        }

        _isTraining = false;
        trainingStationUI.SetStatus("Тренировка завершена! Сталкер готов.");
    }
}
