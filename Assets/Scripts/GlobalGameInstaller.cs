using UnityEngine;
using Zenject;

public class GlobalGameInstaller : MonoInstaller
{

    [SerializeField] private StalkerUnitManager stalkerUnitManager;
    public override void InstallBindings()
    {
        Container.Bind<StalkerUnitManager>().FromInstance(stalkerUnitManager).AsSingle();
    }
}
