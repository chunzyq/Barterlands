using UnityEngine;
using System.Collections.Generic;
using Barterlands.Logging;
using System;

public class StalkerUnitManager : MonoBehaviour
{
    public List<StalkerData> stalkers = new List<StalkerData>();
    public event Action OnStalkerChanged;

    private ILoggerService _logger;

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

    private void OnDestroy()
    {
        OnStalkerChanged = null;
    }
}
