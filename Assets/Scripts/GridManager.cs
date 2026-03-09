using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static int width = 5;
    public static int height = 5;
    public static int left = 5;
    public GameObject cellPrefab;

    private Cell[,] cells;

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        cells = new Cell[width, height];

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 pos = new Vector3(x - width / 2, y - height / 2, 0);

                GameObject cellObj = Instantiate(cellPrefab, pos, Quaternion.identity, transform);

                Cell cell = cellObj.GetComponent<Cell>();
                cell.gridPosition = new Vector2Int(x, y);

                cells[x, y] = cell;
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        return cells[x, y];
    }
}