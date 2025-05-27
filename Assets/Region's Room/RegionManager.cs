using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class RegionManager : MonoBehaviour
{


    [Inject] GameRegion gameRegion;
    [SerializeField] private List<GameRegion> allRegions = new List<GameRegion>();

    void Start()
    {
        foreach (GameRegion region in allRegions)
        {
            region.OnStateChanged += OnRegionsStateChanged;
        }
    }

    private void OnRegionsStateChanged(GameRegion region, RegionState newState)
    {
        Debug.Log($"Регион {region.regionData.coordinates} изменил состояние на {newState}");
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.B))
        {
            gameRegion.ChangeState(RegionState.Locked);
        }
    }
}
