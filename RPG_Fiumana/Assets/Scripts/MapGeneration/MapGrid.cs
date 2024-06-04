using Unity.VisualScripting;
using UnityEngine;

public class MapGrid : MonoBehaviour
{
    public GameObject cubePrefab;
    public int rows;
    public int columns;
    public float cellSize;


    private void Awake()
    {
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        for (int x = 0; x < rows; x++) {
            for (int z = 0; z < columns; z++) {
                // Calcola la posizione per ogni cubo nella griglia
                Vector3 position = new Vector3(x * cellSize, 0, z * cellSize);
                // Crea un nuovo cubo
                GameObject instantiated = Instantiate(cubePrefab, position, Quaternion.identity, transform);
                Cell cell = instantiated.GetComponent<Cell>();
                cell.tileSpecs.coords = new Vector2Int(x, z);
                cell.GenerateVisual(instantiated.transform.position, instantiated.transform);
            }
        }
    }

}
