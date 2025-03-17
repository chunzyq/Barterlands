using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FactorySettings
{
    public int maxFactoryWorkers = 4;
    public int currentFactoryWorkers = 0;
    public float currentFacEfficiency = 0f;
    public int baseProductionRatePerWorker = 2;

    public int productionRate
    {
        get {return currentFactoryWorkers * baseProductionRatePerWorker;}
    }
}
