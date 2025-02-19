using System.Collections;
using System.Transactions;
using Unity.VisualScripting;
using UnityEngine;

public class BuildingPlacementManager : MonoBehaviour
{
    public GameObject previewObjectPrefab;
    public LayerMask groundLayer;
    public LayerMask buildingLayer;

    private GameObject previewObject;
    public bool isBuildingMode;
    public static BuildingPlacementManager instance;
    public GridFiller gridManager;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            isBuildingMode = !isBuildingMode;
            ToggleBuildingMode();
            if (gridManager != null)
                gridManager.SetGridActive(isBuildingMode);
        }

        if (isBuildingMode)
        {
            UpdatePreviewObject();

            if (Input.GetMouseButtonDown(0))
            {
                PlaceBuilding();
            }
        }
        if (Input.GetKeyDown(KeyCode.X) && !isBuildingMode)
        {
            DeleteBuilding();
        }
    }
    void ToggleBuildingMode()
    {
        if (isBuildingMode)
        {
            previewObject = Instantiate(previewObjectPrefab);
            previewObject.GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            Destroy(previewObject);
        }
    }
    void UpdatePreviewObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, groundLayer))
        {
            Vector3 snappedPosition = new Vector3(Mathf.Round(hit.point.x), hit.point.y, Mathf.Round(hit.point.z)); // почитать
            previewObject.transform.position = snappedPosition;

            if (CanBuildAtPosition(snappedPosition))
            {
                previewObject.GetComponent<Renderer>().material.color = Color.green;
            }
            else
            {
                previewObject.GetComponent<Renderer>().material.color = Color.red;
            }
        }
    }
    bool CanBuildAtPosition(Vector3 position)
    {
        // todo
        return true;
    }

    void PlaceBuilding()
    {
        Vector3 position = previewObject.transform.position;

        Instantiate(previewObjectPrefab, position, Quaternion.identity); // почитать про это
        Destroy(previewObject);

        isBuildingMode = false;
    }

    void DeleteBuilding()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, buildingLayer))
        {
            Destroy(hit.collider.gameObject);
        }
    }
}
