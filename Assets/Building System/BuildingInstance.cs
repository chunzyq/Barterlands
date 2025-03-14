using UnityEngine;

public class BuildingInstance : MonoBehaviour
{
    public string InstanceID {get; private set;} // создаётся поле для уникального ID здания, у которого есть публичный доступ для ДОСТУПА, но приватный доступ для изменения

    public BuildingData buildingData; // доступ к дате зданий
    public FactorySettings factorySettings;
    public LaboratorySettings laboratorySettings;
    public HouseSettings houseSettings;

    private void Awake()
    {
        GenerateUniqueID();

        switch (buildingData.buildingType)
        {
            case BuildingType.Factory:
                factorySettings = new FactorySettings();
                break;
            case BuildingType.House:
                houseSettings = new HouseSettings();
                break;
            case BuildingType.Laboratory:
                laboratorySettings = new LaboratorySettings();
                break;
        }
    }
    private void GenerateUniqueID()
    {
        InstanceID = System.Guid.NewGuid().ToString(); // генерируется уникальный ID
        Debug.Log(InstanceID);
    }

    private void OnMouseDown()
    {
        UIController.Instance.OpenBuildingUI(this); // при нажатии на ЛКМ на здание - инфа о нём передаётся в UIController в функцию
        Debug.Log("Clicked");
    }
}
