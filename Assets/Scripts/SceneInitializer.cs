using UnityEngine;
using Zenject;

public class SceneInitializer : MonoBehaviour
{
    [Inject] private ResourseManager _resourceManager;
    [Inject] private UIController _uiController;
    [Inject] private SettlementManager _settlementManager;

    private void Start()
    {
        _resourceManager.InitializeDependecies(_uiController, _settlementManager);
    }
}
