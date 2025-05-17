using System.Collections;
using UnityEngine;
using Zenject;

public class ResourseManager : MonoBehaviour
{
    [Inject] UIController uIController;

    public int metalAmount = 30;
    public float productionInterval = 15f;
    public int metalPerFactory = 32;

    private void Start()
    {
        StartCoroutine(ProduceResources());
    }
    private IEnumerator ProduceResources()
    {
        while (true)
        {
            yield return new WaitForSeconds(productionInterval);

            int totalProduction = 0;

            foreach (BuildingInstance building in BuildingInstance.allBuildingsInstance)
            {
                if (building.buildingData.buildingType == BuildingType.Factory)
                {
                    float efficiencyFactor = building.factorySettings.currentFacEfficiency / 100f;
                    totalProduction += Mathf.RoundToInt(metalPerFactory * efficiencyFactor);

                }
            }

            metalAmount += totalProduction;

            if (uIController.mainInterfaceUI != null)
            {
                uIController.mainInterfaceUI.UpdateMetalText(metalAmount);
            }
        }
    }

    public bool SpendMetal(int cost)
    {
        if (metalAmount >= cost)
        {
            metalAmount -= cost;
            if (uIController.mainInterfaceUI != null)
            {
                uIController.mainInterfaceUI.UpdateMetalText(metalAmount);
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    public void RefundMetal(int amount)
    {
        metalAmount += amount;

        if (uIController.mainInterfaceUI != null)
        {
            uIController.mainInterfaceUI.UpdateMetalText(metalAmount);
        }
    }
}