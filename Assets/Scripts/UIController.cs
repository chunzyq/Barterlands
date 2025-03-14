using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class UIController : MonoBehaviour
{
    public static UIController Instance;
    private BuildingData buildingData;

    public GameObject industialUIPrefab;
    public GameObject scientificUIPrefab;
    public GameObject generalUIPrefab;
    private GameObject activeBuildingUI;

    public Dictionary<string, FactorySettings> allFactorySettings = new Dictionary<string, FactorySettings>();

    private void Awake()
    {
        Instance = this;
    }

    public void OpenBuildingUI(BuildingInstance building)
    {
        if (activeBuildingUI != null)
        {
            Destroy(activeBuildingUI);
        }

        GameObject prefabToInstantiate = null;

        switch (building.buildingData.buildingType)
        {
            case BuildingType.Factory:
                prefabToInstantiate = industialUIPrefab;
                break;
            case BuildingType.Laboratory:
                prefabToInstantiate = scientificUIPrefab;
                break;
            case BuildingType.House:
                prefabToInstantiate = generalUIPrefab;
                break;
        }

        if (prefabToInstantiate != null)
        {
            activeBuildingUI = Instantiate(prefabToInstantiate, transform);

            if (building.buildingData.buildingType == BuildingType.Factory)
            {
                FactoryUI ui = activeBuildingUI.GetComponent<FactoryUI>();
                ui.UpdateUI(building.factorySettings);
            }
            else if (building.buildingData.buildingType == BuildingType.Laboratory)
            {
                LaboratoryUI ui = activeBuildingUI.GetComponent<LaboratoryUI>();
                ui.UpdateUI(building.laboratorySettings);
            }
            else if (building.buildingData.buildingType == BuildingType.House)
            {
                HouseUI ui = activeBuildingUI.GetComponent<HouseUI>();
                ui.UpdateUI(building.houseSettings);
            }
        }
    }
}
