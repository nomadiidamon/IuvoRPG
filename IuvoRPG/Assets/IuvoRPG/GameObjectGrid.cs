using UnityEngine;

public class GameObjectGrid : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridWidth;
    public int gridLength;
    public GameObject gridItem;

    [Header("Cell Settings")]
    public Vector3 cellSize = Vector3.one;
    public Vector3 cellOffset = Vector3.zero;

    [Tooltip("Enabling will override offset, preferring size, and may cause object overlap if used incorrectly")]
    public bool keepConnected = false;


    [Header("Centering")]
    public CenteringMode centeringMode = CenteringMode.TransformPosition;
    public Vector3 customCenter = Vector3.zero;


    void Start()
    {
        var grid = new GenericGrid<GameObject>
        {
            gridWidth = this.gridWidth,
            gridLength = this.gridLength,
            cellSize = this.cellSize,
            cellOffset = keepConnected ? new Vector3(-cellSize.x + 1, 0, -cellSize.z + 1) : this.cellOffset,
            gridCenter = transform.position,
            gridItem = this.gridItem,
            centeringMode = this.centeringMode,
            customCenter = this.customCenter
        };

        grid.GenerateGrid(transform);
    }

}
