using System;
using Unity.VisualScripting;
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
       // if (regionData.state == newState) return;

        if (newState == RegionState.Chosen)
        {
              
            blackCoverageObject.SetActive(false);            
        }

    }

    public void UnlockRegion(GameRegion region)
    {
        ChangeState(RegionState.Chosen);
    }

    public void LockRegion(GameRegion region)
    {
        ChangeState(RegionState.Locked);
    }

    private void MakeAvaliable(GameRegion region)
    {
        ChangeState(RegionState.AvailableForChoise);
    }
}
