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
    [SerializeField] BuildUIHandler buildUIHandler;
    [SerializeField] BuildManager buildManager;
    [SerializeField] BuildInputHandler buildInputHandler;
    [SerializeField] BuildPreviewHandler previewHandler;
    [SerializeField] BuildingPlacementHandler placementHandler;
    [SerializeField] BuildingPlacementValidator placementValidator;
    [SerializeField] BuildSelectionHandler selectionHandler;
    [SerializeField] private BuildManager buildManagerPrefab;
    [SerializeField] private Material validPlacementMaterial;
    [SerializeField] private Material invalidPlacementMaterial;

    public override void InstallBindings()
    {
        Container.Bind<SettlementManager>().FromInstance(settlementManager).AsSingle();
        Container.Bind<UIController>().FromInstance(uIController).AsSingle();
        Container.Bind<ResourseManager>().FromInstance(resourseManager).AsSingle();
        Container.Bind<InterfaceUI>().FromComponentInHierarchy().AsSingle();
        // Container.Bind<IBuildStrategy>().To<FactoryBuildingStategy>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesTo<FactoryBuildingStategy>().AsSingle().WithArguments(_factoryPrefab);
        Container.BindInterfacesTo<LaboratoryBuildingStrategy>().AsSingle().WithArguments(_laboratoryPrefab);

        // Container.Bind<BuildManager>().FromComponentInNewPrefab(buildManagerPrefab).AsSingle();
        // Container.Bind<BuildInputHandler>().FromNewComponentOnNewGameObject().AsSingle();
        // Container.Bind<BuildPreviewHandler>().FromNewComponentOnNewGameObject().AsSingle();
        // Container.Bind<BuildingPlacementHandler>().FromNewComponentOnNewGameObject().AsSingle();
        // Container.Bind<BuildUIHandler>().FromInstance(buildUIHandler).AsSingle();
        // Container.Bind<BuildSelectionHandler>().FromNewComponentOnNewGameObject().AsSingle();
        // Container.Bind<BuildingPlacementValidator>().FromNewComponentOnNewGameObject().AsSingle();

        Container.Bind<BuildManager>().FromInstance(buildManager).AsSingle();
        Container.Bind<BuildInputHandler>().FromInstance(buildInputHandler).AsSingle();
        Container.Bind<BuildPreviewHandler>().FromInstance(previewHandler).AsSingle();
        Container.Bind<BuildingPlacementHandler>().FromInstance(placementHandler).AsSingle();
        Container.Bind<BuildUIHandler>().FromInstance(buildUIHandler).AsSingle();
        Container.Bind<BuildSelectionHandler>().FromInstance(selectionHandler).AsSingle();
        Container.Bind<BuildingPlacementValidator>().FromInstance(placementValidator).AsSingle();

        Container.Bind<UniversalBuildingStrategy>().FromNewComponentOnNewGameObject().AsSingle();

        Container.Bind<Material>().WithId("validPlacement").FromInstance(validPlacementMaterial);
        Container.Bind<Material>().WithId("invalidPlacement").FromInstance(invalidPlacementMaterial);

    }
}
