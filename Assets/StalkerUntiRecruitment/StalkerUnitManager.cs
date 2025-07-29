using UnityEngine;
using System.Collections.Generic;
using Barterlands.Logging;
using System;
using Zenject;

public class StalkerUnitManager : MonoBehaviour
{
    public List<StalkerData> stalkers = new List<StalkerData>();
    public List<StalkerData> stalkersReadyForRaid = new List<StalkerData>();
    public event Action OnStalkerChanged;
    private ILoggerService _logger;
    public int maxAvailableStalkers = 3;

    void Awake()
    {
        stalkers.Clear();
    }

    public void AddStalkers(StalkerData stalkerData)
    {
        stalkers.Add(stalkerData);

        _logger = new UnityLogger();

        OnStalkerChanged?.Invoke();
        _logger.Info($"Сталкер добавлен! Всего сталкеров: {stalkers.Count}");
    }

    public bool TryAddStalkerToList()
    {
        if (stalkers.Count == 0) return false;

        StalkerData stalkerToAdd = stalkers[0];

        stalkersReadyForRaid.Add(stalkerToAdd);

        stalkers.RemoveAt(0);

        return true;
    }

    public bool TryRemoveStalkerFromList()
    {
        if (stalkersReadyForRaid.Count == 0) return false;

        stalkers.Add(stalkersReadyForRaid[stalkersReadyForRaid.Count - 1]);
        stalkersReadyForRaid.RemoveAt(stalkersReadyForRaid.Count - 1);

        OnStalkerChanged?.Invoke();

        return true;
    }

    private void OnDestroy()
    {
        OnStalkerChanged = null;
    }
}
