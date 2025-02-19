using UnityEngine;

public class GridFiller : MonoBehaviour
{
    public int gridCount = 20;        // Количество ячеек в одну сторону
    public float gridSize = 1.0f;       // Размер ячейки
    public Material lineMaterial;     // Материал для линий

    private GameObject gridParent;    // Родительский объект для всех линий

    void Start()
    {
        CreateGrid();
        // Изначально сетка выключена
        SetGridActive(false);
    }

    void CreateGrid()
    {
        gridParent = new GameObject("GridParent");

        float halfGrid = gridCount * gridSize / 2.0f;

        // Создание вертикальных линий
        for (int i = 0; i <= gridCount; i++)
        {
            float x = -halfGrid + i * gridSize;
            GameObject lineObj = new GameObject("VerticalLine_" + i);
            lineObj.transform.parent = gridParent.transform;
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.material = lineMaterial;
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.positionCount = 2;
            lr.SetPosition(0, new Vector3(x, 0, -halfGrid));
            lr.SetPosition(1, new Vector3(x, 0, halfGrid));
        }

        // Создание горизонтальных линий
        for (int j = 0; j <= gridCount; j++)
        {
            float z = -halfGrid + j * gridSize;
            GameObject lineObj = new GameObject("HorizontalLine_" + j);
            lineObj.transform.parent = gridParent.transform;
            LineRenderer lr = lineObj.AddComponent<LineRenderer>();
            lr.material = lineMaterial;
            lr.startWidth = 0.05f;
            lr.endWidth = 0.05f;
            lr.positionCount = 2;
            lr.SetPosition(0, new Vector3(-halfGrid, 0, z));
            lr.SetPosition(1, new Vector3(halfGrid, 0, z));
        }
    }

    // Метод для включения/выключения отображения сетки
    public void SetGridActive(bool active)
    {
        if (gridParent != null)
            gridParent.SetActive(active);
    }
}
