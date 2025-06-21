using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RegionUIHandler : MonoBehaviour
{
    [SerializeField] private ChooseRegionForGrow regionSelector;

    private List<GameRegion> currentCandidates = new List<GameRegion>();

    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button buttonOption1;
    [SerializeField] private Button buttonOption2;

    void Start()
    {
        menuPanel.SetActive(false);
    }

    public void OpenGrowthMenu(List<GameRegion> candidates)
    {
        currentCandidates = candidates ?? new List<GameRegion>();

        if (candidates == null || candidates.Count == 0)
        {
            menuPanel.SetActive(false);
            return;
        }

        buttonOption1.onClick.RemoveAllListeners();
        buttonOption2.onClick.RemoveAllListeners();

        if (currentCandidates.Count == 1)
        {
            GameRegion r0 = currentCandidates[0];
            SetButton(buttonOption1, r0);

            buttonOption2.gameObject.SetActive(false);

            menuPanel.SetActive(true);
            return;
        }

        GameRegion region1 = currentCandidates[0];
        GameRegion region2 = currentCandidates[1];

        buttonOption1.gameObject.SetActive(true);
        buttonOption2.gameObject.SetActive(true);

        SetButton(buttonOption1, region1);
        SetButton(buttonOption2, region2);

        menuPanel.SetActive(true);
    }

    private void SetButton(Button btn, GameRegion region)
    {
        TextMeshProUGUI txt = btn.GetComponentInChildren<TextMeshProUGUI>();
        if (txt != null)
        {
            txt.text = region.regionData.type.ToString();
        }

        btn.onClick.AddListener(() =>
        {
            regionSelector.ApplyGrowth(new List<GameRegion> { region });
            menuPanel.SetActive(false);
        });
    }
}
