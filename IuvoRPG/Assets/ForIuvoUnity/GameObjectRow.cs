using UnityEngine;

public class GameObjectRow : MonoBehaviour
{
    [Header("Row Settings")]
    public int rowLength;
    public GameObject rowItem;

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
        var row = new GenericRow<GameObject>
        {
            rowLength = this.rowLength,
            cellSize = this.cellSize,
            cellOffset = keepConnected ? new Vector3(-cellSize.x + 1, 0, 0) : this.cellOffset,
            rowCenter = transform.position,
            rowItem = this.rowItem,
            centeringMode = this.centeringMode,
            customCenter = this.customCenter
        };

        row.GenerateRow(transform);
    }
}