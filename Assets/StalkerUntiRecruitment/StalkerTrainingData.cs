using UnityEngine;

[CreateAssetMenu(fileName = "StalkerUnit", menuName = "Stalker/StalkerTrainingData")]
public class StalkerTrainingData : ScriptableObject
{
    public int humanCost = 1;
    public int metalCost = 200;
    public float trainingTimeInSeconds = 60f;
}

