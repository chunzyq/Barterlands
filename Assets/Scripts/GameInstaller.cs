using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] SettlementManager settlementManager;
    [SerializeField] UIController uIController;
    [SerializeField] ResourseManager resourseManager;
    [SerializeField] GameObject _factoryPrefab;
    [SerializeField] GameObject _laboratoryPrefab;
    [SerializeField] InterfaceUI interfaceUI;

    public override void InstallBindings()
    {
        Container.Bind<SettlementManager>().FromInstance(settlementManager).AsSingle();
        Container.Bind<UIController>().FromInstance(uIController).AsSingle();
        Container.Bind<ResourseManager>().FromInstance(resourseManager).AsSingle();
        Container.Bind<InterfaceUI>().FromComponentInHierarchy().AsSingle();
        // Container.Bind<IBuildStrategy>().To<FactoryBuildingStategy>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesTo<FactoryBuildingStategy>().AsSingle().WithArguments(_factoryPrefab);
        Container.BindInterfacesTo<LaboratoryBuildingStrategy>().AsSingle().WithArguments(_laboratoryPrefab);

    }
}
