using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class HouseUI : MonoBehaviour
{
    public TextMeshProUGUI peopleCountText;
    public Button increaseButton;
    public Button deleteBuildingButton;

    private HouseSettings currentSettings;
    public void UpdateUI(HouseSettings settings)
    {
        currentSettings = settings;
        peopleCountText.text = "People Count: " + settings.peopleCount.ToString();
    }

    void Awake()
    {
        increaseButton.onClick.AddListener(OnIncreaseButtonClicked);
        deleteBuildingButton.onClick.AddListener(OnDeleteBuildingButtonClicked);
    }

    private void OnDeleteBuildingButtonClicked()
    {
        UIController.Instance.DeleteCurrentBuilding();
    }

    private void OnIncreaseButtonClicked()
    {
        currentSettings.peopleCount += 10;
        peopleCountText.text = "People Count: " + currentSettings.peopleCount.ToString();
    }
}
