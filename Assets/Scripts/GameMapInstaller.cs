using UnityEngine;
using Zenject;

public class GameMapInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<UnitSelectionUIManager>().FromComponentInHierarchy().AsSingle(); //test
    }
}
