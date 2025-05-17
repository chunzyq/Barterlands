using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Zenject;

public class ResourceMenuController : MonoBehaviour
{
    [Inject] ResourseManager resourseManager;
    [Inject] SettlementManager settlementManager;

    [SerializeField] private TextMeshProUGUI metalAmount;
    [SerializeField] private TextMeshProUGUI freeWorkers;
    [SerializeField] private TextMeshProUGUI assignedWorkers;

    void Update()
    {
        Dictionary<ResourceType, int> prod = settlementManager.GetTotalHourlyProduction();

        metalAmount.text = $"Metal: {resourseManager.metalAmount.ToString()} ({GetRate(prod, ResourceType.Metal)}/h)";
        freeWorkers.text = "Free People: " + settlementManager.FreeWorkers.ToString();
        assignedWorkers.text = "Assigned Workers: " + settlementManager.TotalAssignedWorkers.ToString();
    }

    private int GetRate(Dictionary<ResourceType, int> d, ResourceType type)
    {
        return d.TryGetValue(type, out var v) ? v : 0;
    }
}
