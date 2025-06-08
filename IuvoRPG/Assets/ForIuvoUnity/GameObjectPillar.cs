using UnityEngine;

public class GameObjectPillar : MonoBehaviour
{
    [Header("Pillar Settings")]
    public int pillarHeight;
    public GameObject pillarItem;

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
        var pillar = new GenericPillar<GameObject>
        {
            pillarHeight = this.pillarHeight,
            cellSize = this.cellSize,
            cellOffset = keepConnected ? new Vector3(0, -cellSize.y + 1, 0) : this.cellOffset,
            pillarCenter = transform.position,
            pillarItem = this.pillarItem,
            centeringMode = this.centeringMode,
            customCenter = this.customCenter
        };

        pillar.GeneratePillar(transform);
    }
}