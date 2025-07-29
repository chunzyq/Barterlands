using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using Barterlands.Logging;

public class RaidMenuHandler : MonoBehaviour
{
    [Inject] private StalkerUnitManager stalkerUnitManager;
    [Inject] private RaidManager raidProcess;
    [SerializeField] private TextMeshProUGUI availableUnits;
    [SerializeField] private TextMeshProUGUI stalkersReadyForRaid;
    [SerializeField] private Button plusOneUnit;
    [SerializeField] private Button minusOneUnit;
    [SerializeField] private Button startRaid;

    public TextMeshProUGUI raidStatus;
    private ILoggerService _logger;


    private void Awake()
    {
        plusOneUnit.onClick.AddListener(AddStalkerToRaid);
        minusOneUnit.onClick.AddListener(RemoveStalkerFromRaid);
        startRaid.onClick.AddListener(raidProcess.StartRaid);

        availableUnits.text = $"Количество юнитов: {stalkerUnitManager.stalkers.Count}";
        stalkersReadyForRaid.text = $"В рейд идут {stalkerUnitManager.stalkersReadyForRaid.Count} сталкер(а)";

        stalkerUnitManager.OnStalkerChanged += UpdateTextForAvailableUnits;

        _logger = new UnityLogger();
    }

    private void AddStalkerToRaid()
    {
        bool success = stalkerUnitManager.TryAddStalkerToList();

        if (!success)
        {
            _logger.Warning("Не осталось сталкеров для добавления!");
        }

        UpdateTextForReadyStalkers();
        UpdateTextForAvailableUnits();
    }

    private void RemoveStalkerFromRaid()
    {
        bool success = stalkerUnitManager.TryRemoveStalkerFromList();

        if (!success)
        {
            _logger.Warning("Нечего удалять!");
        }

        UpdateTextForReadyStalkers();
        UpdateTextForAvailableUnits();
    }

    private void UpdateTextForAvailableUnits()
    {
        availableUnits.text = $"Количество юнитов: {stalkerUnitManager.stalkers.Count}";
    }

    private void UpdateTextForReadyStalkers()
    {
        stalkersReadyForRaid.text = $"В рейд идут {stalkerUnitManager.stalkersReadyForRaid.Count} сталкер(а)";
    }
}
