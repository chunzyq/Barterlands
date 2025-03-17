using UnityEngine;

[System.Serializable]
public class LaboratorySettings
{
    public int maxLaboratoryWorkers = 6;
    public int currentLaboratoryWorkers = 0;
    public float currentLabEfficiency = 0f;
    public int baseReduceResearchTimePerWorker = 5;

    public int baseResearchTime = 60;
}
