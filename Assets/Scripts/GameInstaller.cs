using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private GameObject _factoryPrefab;
    [SerializeField] private GameObject _laboratoryPrefab;
    [SerializeField] private GameRegion gameRegion;
    [SerializeField] private Material validPlacementMaterial;
    [SerializeField] private Material invalidPlacementMaterial;


    public override void InstallBindings()
    {
        // Менеджеры и другие важные штуки, которые отвечают за бОльшую часть логики
        Container.Bind<SettlementManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<UIController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<InterfaceUI>().FromComponentInHierarchy().AsSingle();
        Container.Bind<StalkerUnitManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<ResourseManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<BuildManager>().FromComponentInHierarchy().AsSingle();

        // Стратегии строительства разных типов зданий TODO
        Container.BindInterfacesTo<FactoryBuildingStategy>().AsSingle().WithArguments(_factoryPrefab);
        Container.BindInterfacesTo<LaboratoryBuildingStrategy>().AsSingle().WithArguments(_laboratoryPrefab);
        Container.Bind<UniversalBuildingStrategy>().FromNewComponentOnNewGameObject().AsSingle();

        // Компоненты, которые отвечают за строительство
        Container.Bind<BuildInputHandler>().FromComponentInHierarchy().AsSingle();
        Container.Bind<BuildPreviewHandler>().FromComponentInHierarchy().AsSingle();
        Container.Bind<BuildingPlacementHandler>().FromComponentInHierarchy().AsSingle();
        Container.Bind<BuildUIHandler>().FromComponentInHierarchy().AsSingle();
        Container.Bind<BuildSelectionHandler>().FromComponentInHierarchy().AsSingle();
        Container.Bind<BuildingPlacementValidator>().FromComponentInHierarchy().AsSingle();

        // Игровой регион
        Container.Bind<GameRegion>().FromInstance(gameRegion).AsSingle();

        // Материалы
        Container.Bind<Material>().WithId("validPlacement").FromInstance(validPlacementMaterial);
        Container.Bind<Material>().WithId("invalidPlacement").FromInstance(invalidPlacementMaterial);
    }
}
