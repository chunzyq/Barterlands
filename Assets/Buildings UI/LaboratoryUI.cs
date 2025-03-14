using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LaboratoryUI : MonoBehaviour
{
    public TextMeshProUGUI researchTime;
    public Button increaseResearchTime;

    private LaboratorySettings currentSettings;

    public void UpdateUI(LaboratorySettings settings)
    {
        currentSettings = settings;
        researchTime.text = "Research Time: " + settings.researchTime.ToString();
    }
    void Awake()
    {
        increaseResearchTime.onClick.AddListener(OnIncreaseButtonClicked);
    }
    private void OnIncreaseButtonClicked()
    {
        currentSettings.researchTime += 10;
        researchTime.text = "Research Time: " + currentSettings.researchTime.ToString();
    }
}
