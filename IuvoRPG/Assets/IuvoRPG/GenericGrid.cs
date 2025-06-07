using UnityEngine;

public enum CenteringMode
{
    TransformPosition, // Default: use transform.position
    BoundsMin,         // Use the minimum bound (bottom/left/back)
    BoundsMax,         // Use the maximum bound (top/right/front)
    CustomPosition     // Use a custom Vector3
}

public class GenericGrid<T> where T : UnityEngine.Object
{
    public int gridWidth;
    public int gridLength;
    public Vector3 cellSize = Vector3.one;
    public Vector3 cellOffset = Vector3.zero;
    public Vector3 gridCenter = Vector3.zero;
    public T gridItem;

    public CenteringMode centeringMode = CenteringMode.TransformPosition;
    public Vector3 customCenter = Vector3.zero;

    public void GenerateGrid(Transform parent)
    {
        // setup grid size
        Vector3 totalGridSize = new Vector3(
            gridWidth * cellSize.x + (gridWidth - 1) * cellOffset.x,
            cellSize.y,
            gridLength * cellSize.z + (gridLength - 1) * cellOffset.z
        );

        // setup centering logic
        Vector3 center = gridCenter;
        switch (centeringMode)
        {
            case CenteringMode.BoundsMin:
                center = gridCenter - new Vector3(totalGridSize.x, 0, totalGridSize.z) * 0.5f + new Vector3(totalGridSize.x, 0, totalGridSize.z) * 0.5f;
                break;
            case CenteringMode.BoundsMax:
                center = gridCenter + new Vector3(totalGridSize.x, 0, totalGridSize.z) * 0.5f;
                break;
            case CenteringMode.CustomPosition:
                center = customCenter;
                break;
            case CenteringMode.TransformPosition:
            default:
                center = gridCenter;
                break;
        }


        Vector3 bottomLeft = center - new Vector3(totalGridSize.x, 0, totalGridSize.z) * 0.5f;

        for (int x = 0; x < gridWidth; x++)
        {
            for (int z = 0; z < gridLength; z++)
            {
                Vector3 position = bottomLeft + new Vector3(
                    x * (cellSize.x + cellOffset.x),
                    0,
                    z * (cellSize.z + cellOffset.z)
                );
                var obj = Object.Instantiate(gridItem, position, Quaternion.identity, parent);
                if (obj is GameObject go)
                {
                    go.transform.localScale = cellSize;
                }
            }
        }


    }
}