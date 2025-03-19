using System.Collections;
using UnityEngine;

public class ResourseManager : MonoBehaviour
{
    public static ResourseManager Instance;

    public int metalAmount = 0;
    public float productionInterval = 15f;
    public int metalPerFactory = 5;

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

            int factoryCount = 0;

            foreach (BuildingInstance building in BuildingInstance.allBuildingsInstance)
            {
                if (building.buildingData.buildingType == BuildingType.Factory)
                {
                    factoryCount++;
                }
            }

            metalAmount += factoryCount * metalPerFactory;

            if (UIController.Instance != null && UIController.Instance.mainInterfaceUI != null)
            {
                UIController.Instance.mainInterfaceUI.UpdateMetalText(metalAmount);
            }
        }
    }
}
