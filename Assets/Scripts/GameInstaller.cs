using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] UIController uIController;
    [SerializeField] GameObject _factoryPrefab;
    [SerializeField] GameObject _laboratoryPrefab;
    [SerializeField] InterfaceUI interfaceUI;
    [SerializeField] BuildUIHandler buildUIHandler;
    [SerializeField] BuildManager buildManager;
    [SerializeField] BuildPreviewHandler previewHandler;
    [SerializeField] BuildingPlacementHandler placementHandler;
    [SerializeField] BuildingPlacementValidator placementValidator;
    [SerializeField] BuildSelectionHandler selectionHandler;
    [SerializeField] private BuildManager buildManagerPrefab;
    [SerializeField] private Material validPlacementMaterial;
    [SerializeField] private Material invalidPlacementMaterial;
    [SerializeField] GameRegion gameRegion;


    public override void InstallBindings()
    {
        Container.Bind<SettlementManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<UIController>().FromComponentInHierarchy().AsSingle();
        Container.Bind<InterfaceUI>().FromComponentInHierarchy().AsSingle();
        Container.BindInterfacesTo<FactoryBuildingStategy>().AsSingle().WithArguments(_factoryPrefab);
        Container.BindInterfacesTo<LaboratoryBuildingStrategy>().AsSingle().WithArguments(_laboratoryPrefab);

        Container.Bind<BuildManager>().FromInstance(buildManager).AsSingle();
        Container.Bind<BuildInputHandler>().FromComponentInHierarchy().AsSingle();
        Container.Bind<BuildPreviewHandler>().FromInstance(previewHandler).AsSingle();
        Container.Bind<BuildingPlacementHandler>().FromInstance(placementHandler).AsSingle();
        Container.Bind<BuildUIHandler>().FromInstance(buildUIHandler).AsSingle();
        Container.Bind<BuildSelectionHandler>().FromInstance(selectionHandler).AsSingle();
        Container.Bind<BuildingPlacementValidator>().FromInstance(placementValidator).AsSingle();

        Container.Bind<UniversalBuildingStrategy>().FromNewComponentOnNewGameObject().AsSingle();

        Container.Bind<GameRegion>().FromInstance(gameRegion).AsSingle();

        Container.Bind<Material>().WithId("validPlacement").FromInstance(validPlacementMaterial);
        Container.Bind<Material>().WithId("invalidPlacement").FromInstance(invalidPlacementMaterial);

        Container.Bind<UnitSelectionUIManager>().FromComponentInHierarchy().AsSingle();


    }
}
