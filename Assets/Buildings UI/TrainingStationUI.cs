using System;
using NUnit.Framework.Constraints;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TrainingStationUI : MonoBehaviour
{
    [Header("UI элементы")]
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private Button startButton;

    private StalkerTrainingStation trainingStation;

    void Awake()
    {
        startButton.onClick.RemoveAllListeners();
    }

    public void Initialize(StalkerTrainingStation station)
    {
        trainingStation = station;

        station.BindUI(this);

        startButton.onClick.AddListener(OnButtonClicked);
        statusText.text = "Готово к тренировке";
    }

    private void OnButtonClicked()
    {
        trainingStation.StartTraining();
    }

    public void SetStatus(string text)
    {
        statusText.text = text;
    }
}
