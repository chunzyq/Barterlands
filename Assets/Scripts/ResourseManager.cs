using System.Collections;
using UnityEngine;

public class ResourseManager : MonoBehaviour
{
    public static ResourseManager Instance;

    public int metalAmount = 30;
    public float productionInterval = 15f;
    public int metalPerFactory = 32;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

            if (UIController.Instance != null && UIController.Instance.mainInterfaceUI != null)
            {
                UIController.Instance.mainInterfaceUI.UpdateMetalText(metalAmount);
            }
        }
    }

    public bool SpendMetal(int cost)
    {
        if (metalAmount >= cost)
        {
            metalAmount -= cost;
            if (UIController.Instance != null && UIController.Instance.mainInterfaceUI != null)
            {
                UIController.Instance.mainInterfaceUI.UpdateMetalText(metalAmount);
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

        if (UIController.Instance != null && UIController.Instance.mainInterfaceUI != null)
        {
            UIController.Instance.mainInterfaceUI.UpdateMetalText(metalAmount);
        }
    }
}