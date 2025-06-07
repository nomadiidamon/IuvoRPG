using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GenericRow<T> where T : UnityEngine.Object
{
    public int rowLength;
    public Vector3 cellSize = Vector3.one;
    public Vector3 cellOffset = Vector3.zero;
    public Vector3 rowCenter = Vector3.zero;
    public T rowItem;

    public CenteringMode centeringMode = CenteringMode.TransformPosition;
    public Vector3 customCenter = Vector3.zero;

    public void GenerateRow(Transform parent)
    {
        float totalWidth = rowLength * cellSize.x + (rowLength - 1) * cellOffset.x;
        Vector3 center = rowCenter;
        switch (centeringMode)
        {
            case CenteringMode.BoundsMin:
                center = rowCenter - new Vector3(totalWidth, 0, 0) + new Vector3(totalWidth, 0, 0) * 0.5f;
                break;
            case CenteringMode.BoundsMax:
                center = rowCenter + new Vector3(totalWidth, 0, 0) * 0.5f + new Vector3(cellSize.x, 0, 0);
                break;
            case CenteringMode.CustomPosition:
                center = customCenter;
                break;
            case CenteringMode.TransformPosition:
            default:
                center = rowCenter;
                break;
        }

        Vector3 left = center - new Vector3(totalWidth, 0, 0) * 0.5f;

        for (int x = 0; x < rowLength; x++)
        {
            Vector3 position = left + new Vector3(
                x * (cellSize.x + cellOffset.x),
                0,
                0
            );
            var obj = Object.Instantiate(rowItem, position, Quaternion.identity, parent);
            if (obj is GameObject go)
            {
                go.transform.localScale = cellSize;
            }
        }
    }
}