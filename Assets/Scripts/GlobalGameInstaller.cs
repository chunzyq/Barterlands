using UnityEngine;
using Zenject;

public class GlobalGameInstaller : MonoInstaller
{

    [SerializeField] private StalkerUnitManager stalkerUnitManager;
    [SerializeField] private ResourseManager resourseManager;
    public override void InstallBindings()
    {
        Container.Bind<StalkerUnitManager>().FromComponentInNewPrefab(stalkerUnitManager).AsSingle().NonLazy();
        Container.Bind<ResourseManager>().FromComponentInNewPrefab(resourseManager).AsSingle().NonLazy();


    }
}
