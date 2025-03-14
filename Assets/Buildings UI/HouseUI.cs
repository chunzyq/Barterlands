using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HouseUI : MonoBehaviour
{
    public TextMeshProUGUI peopleCountText;
    public Button increaseButton;

    private HouseSettings currentSettings;
    public void UpdateUI(HouseSettings settings)
    {
        currentSettings = settings;
        peopleCountText.text = "People Count: " + settings.peopleCount.ToString();
    }

    void Awake()
    {
        increaseButton.onClick.AddListener(OnIncreaseButtonClicked);
    }

    private void OnIncreaseButtonClicked()
    {
        currentSettings.peopleCount += 10;
        peopleCountText.text = "People Count: " + currentSettings.peopleCount.ToString();
    }
}
