using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance;

    [SerializeField] private GameObject[] buildingPanels;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void HideAllPanels()
    {
        foreach (var panel in buildingPanels)
        {
            panel.SetActive(false);
        }
    }

    public void ShowPanelByTag(string panelTag)
    {
        HideAllPanels();

        foreach (var panel in buildingPanels)
        {
            if (panel.CompareTag(panelTag))
            {
                panel.SetActive(true);
                return;
            }
        }
    }
}
