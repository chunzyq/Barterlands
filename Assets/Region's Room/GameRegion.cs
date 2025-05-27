using System;
using UnityEngine;
using Zenject;

public class GameRegion : MonoBehaviour
{
    [SerializeField] private GameObject blackCoverageObject;
    [SerializeField] private Material blackCoverageMat;
    [SerializeField] public RegionData regionData;

    public event Action<GameRegion, RegionState> OnStateChanged;

    public void ChangeState(RegionState newState)
    {
        RegionState oldState = newState;
        regionData.state = newState;

        UpdateVisual(newState);

        OnStateChanged?.Invoke(this, newState);
    }

    public void UpdateVisual(RegionState newState)
    {

        Renderer renderer = blackCoverageObject.GetComponent<Renderer>();
        renderer.material = blackCoverageMat;

        blackCoverageObject.SetActive(true);
    }
}
