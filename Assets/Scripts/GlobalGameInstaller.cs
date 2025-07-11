using UnityEngine;
using Zenject;

public class GlobalGameInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<ResourseManager>().FromComponentInNewPrefabResource("Resource Manager").AsSingle();
    }
}
