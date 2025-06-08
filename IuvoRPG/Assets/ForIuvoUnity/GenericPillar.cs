using UnityEngine;

public class GenericPillar<T> where T : UnityEngine.Object
{
    public int pillarHeight;
    public Vector3 cellSize = Vector3.one;
    public Vector3 cellOffset = Vector3.zero;
    public Vector3 pillarCenter = Vector3.zero;
    public T pillarItem;

    public CenteringMode centeringMode = CenteringMode.TransformPosition;
    public Vector3 customCenter = Vector3.zero;

    public void GeneratePillar(Transform parent)
    {
        float totalHeight = pillarHeight * cellSize.y + (pillarHeight - 1) * cellOffset.y;

        Vector3 center = pillarCenter;
        switch (centeringMode)
        {
            case CenteringMode.BoundsMin:
                center = pillarCenter - new Vector3(0, totalHeight, 0) + new Vector3(0, totalHeight * 0.5f, 0) * 0.5f;
                break;
            case CenteringMode.BoundsMax:
                center = pillarCenter + new Vector3(0, totalHeight, 0) + new Vector3(0, totalHeight * 0.5f, 0) * 0.5f;
                break;
            case CenteringMode.CustomPosition:
                center = customCenter;
                break;
            case CenteringMode.TransformPosition:
            default:
                center = pillarCenter;
                break;
        }

        Vector3 bottom = center - new Vector3(0, totalHeight, 0) * 0.5f;

        for (int y = 0; y < pillarHeight; y++)
        {
            Vector3 position = bottom + new Vector3(
                0,
                y * (cellSize.y + cellOffset.y),
                0
            );
            var obj = Object.Instantiate(pillarItem, position, Quaternion.identity, parent);
            if (obj is GameObject go)
            {
                go.transform.localScale = cellSize;
            }
        }
    }
}