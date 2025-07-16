using UnityEngine;

[CreateAssetMenu(fileName = "StalkerUnit", menuName = "Stalker/StalkerTrainingData")]
public class StalkerTrainingData : ScriptableObject
{
    [Header("Ресурсы")]
    public int humanCost = 1;
    public int metalCost = 200;

    [Header("Тренировка")]
    public float trainingTimeInSeconds = 60f;
}

