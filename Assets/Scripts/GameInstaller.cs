using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] SettlementManager settlementManager;
    [SerializeField] UIController uIController;
    [SerializeField] ResourseManager resourseManager;

    public override void InstallBindings()
    {
        Container.Bind<SettlementManager>().FromInstance(settlementManager).AsSingle();
        Container.Bind<UIController>().FromInstance(uIController).AsSingle();
        Container.Bind<ResourseManager>().FromInstance(resourseManager).AsSingle();
    }
}
