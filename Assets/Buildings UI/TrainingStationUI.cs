using System;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TrainingStationUI : MonoBehaviour
{
    [Inject] private StalkerUnitManager stalkerUnitManager;

    [Header("UI элементы")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button startButton;
    [SerializeField] private Button startRaidButton;
    [SerializeField] private TextMeshProUGUI availableUnitsText;

    private StalkerTrainingStation trainingStation;

    void Awake()
    {
        startButton.onClick.RemoveAllListeners();
        startRaidButton.onClick.RemoveAllListeners();

        stalkerUnitManager.OnStalkerChanged += UpdateUnitCounterText;
    }
    void OnDestroy()
    {
        stalkerUnitManager.OnStalkerChanged -= UpdateUnitCounterText;
    }

    private void UpdateUnitCounterText()
    {
        availableUnitsText.text = $"Количество юнитов: {stalkerUnitManager.stalkers.Count} / {stalkerUnitManager.maxAvailableStalkers}";
    }


    public void Initialize(StalkerTrainingStation station)
    {
        trainingStation = station;

        station.BindUI(this);

        startButton.onClick.AddListener(OnStartTrainingButtonClicked);
        // startRaidButton.onClick.AddListener(OnStartRaindButtonClicked); TODO!!!!
        statusText.text = "Готово к тренировке";
        availableUnitsText.text = $"Количество юнитов: {stalkerUnitManager.stalkers.Count} / {stalkerUnitManager.maxAvailableStalkers}";
    }

    private void OnStartTrainingButtonClicked()
    {
        trainingStation.StartTraining();
    }

    public void SetStatus(string text)
    {
        statusText.text = text;
    }
}
