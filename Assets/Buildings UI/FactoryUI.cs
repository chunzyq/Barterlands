using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FactoryUI : MonoBehaviour
{
    public TextMeshProUGUI productionRateText;
    public Button increaseButton;

    private FactorySettings currentSettings;

    public void UpdateUI(FactorySettings settings)
    {
        currentSettings = settings;
        productionRateText.text = "Production Rate: " + settings.productionRate.ToString();
    }

    void Awake()
    {
        increaseButton.onClick.AddListener(OnIncreaseButtonClicked);
    }

    private void OnIncreaseButtonClicked()
    {
        currentSettings.productionRate += 10;
        productionRateText.text = "Production Rate: " + currentSettings.productionRate.ToString();
    }
}
