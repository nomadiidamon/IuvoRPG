using System.Collections.Generic;
using UnityEngine;

public class MazeCell : MonoBehaviour
{
    [SerializeField] public MazeBarrier LeftWall;
    public bool hasLeftWall { get; set; }

    [SerializeField] public MazeBarrier RightWall;
    public bool hasRightWall { get; private set; }

    [SerializeField] public MazeBarrier FrontWall;
    public bool hasFrontWall { get; private set; }

    [SerializeField] public MazeBarrier BackWall;
    public bool hasBackWall { get; private set; }

    [SerializeField] public MazeBarrier Ceiling;
    public bool hasCeiling { get; private set; }

    [SerializeField] public MazeBarrier Floor;
    public bool hasFloor { get; private set; }

    /// Marks the cell as visited
    [SerializeField] public GameObject UnvisitedCell;
    [SerializeField] private Transform unvistedCellSize;
    public bool cellWasVisited { get; private set; }
    public void Visit()
    {
        cellWasVisited = true; UnvisitedCell.SetActive(false);
    }

    public MazeCell LeftNeighbor;
    public MazeCell RightNeighbor;
    public MazeCell FrontNeighbor;
    public MazeCell BackNeighbor;
    public MazeCell CeilingNeighbor;
    public MazeCell FloorNeighbor;

    [Header("Maze Cell Information")]
    public int startingNumberOfCellWalls;
    public bool isBoundaryCell = false;
    public bool isIsolatedCell = false;
    public bool isTrapCell = false;
    public List<MazeBarrier> boundaryWalls = new List<MazeBarrier>();
    public List<MazeBarrier> remainingWalls = new List<MazeBarrier>();
    public List<MazeBarrier> destroyedWalls = new List<MazeBarrier>();
    public List<MazeCell> neighboringCells = new List<MazeCell>();
    public List<MazeBarrier> sharerdWalls = new List<MazeBarrier>();
    public BoxCollider cellCollider;
    private Vector3 unvisitedCellScale;
    public int sharedWallCount = 0;

    private void Awake()
    {
        cellCollider = GetComponent<BoxCollider>();
        unvisitedCellScale = unvistedCellSize.lossyScale;
        cellCollider.size = new Vector3(unvisitedCellScale.x, 1, unvisitedCellScale.z);
        cellWasVisited = false;
        boundaryWalls.Clear();
        remainingWalls.Clear();
        startingNumberOfCellWalls = 0;
        if (LeftWall != null)
        {
            hasLeftWall = true;
            remainingWalls.Add(LeftWall);
            startingNumberOfCellWalls += 1;
            LeftWall.myParentCell = this;
        }
        if (RightWall != null)
        {
            hasRightWall = true;
            remainingWalls.Add(RightWall);
            startingNumberOfCellWalls += 1;
            RightWall.myParentCell = this;
        }
        if (FrontWall != null)
        {
            hasFrontWall = true;
            remainingWalls.Add(FrontWall);
            startingNumberOfCellWalls += 1;
            FrontWall.myParentCell = this;
        }
        if (BackWall != null)
        {
            hasBackWall = true;
            remainingWalls.Add(BackWall);
            startingNumberOfCellWalls += 1;
            BackWall.myParentCell = this;
        }
        if (Ceiling != null)
        {
            hasCeiling = true;
            Ceiling.myParentCell = this;
        }
        if (Floor != null)
        {
            hasFloor = true;
            Floor.myParentCell = this;
        }
    }

    public void DestroyBarrier(MazeBarrier barrier)
    {
        barrier.DestroyBarrier();
    }

    public void RestoreBarrier(MazeBarrier barrier)
    {
        barrier.RestoreBarrier();
    }

    public bool DeleteRandomRemainingWall(int mazeHeight)
    {
        if (remainingWalls.Count > 0)
        {
            int randomIndex = Random.Range(0, remainingWalls.Count);
            MazeBarrier barrier = remainingWalls[randomIndex];
            if (!barrier.isBoundaryBarrier)
            {
                if (barrier == LeftWall) DestroyBarrier(LeftWall);
                else if (barrier == RightWall) DestroyBarrier(RightWall);
                else if (barrier == FrontWall) DestroyBarrier(FrontWall);
                else if (barrier == BackWall) DestroyBarrier(BackWall);
                else if (mazeHeight > 1)
                {
                    if (barrier == Ceiling) DestroyBarrier(Ceiling);
                    else if (barrier == Floor) DestroyBarrier(Floor);
                }
            }
            return true;
        }
        else return false;
    }

    public bool RestoreRandomWall(bool topDownView)
    {
        if (destroyedWalls.Count > 0)
        {
            int randomIndex = Random.Range(0, destroyedWalls.Count);

            MazeBarrier wall = destroyedWalls[randomIndex];
            if (wall == LeftWall) RestoreBarrier(LeftWall);
            else if (wall == RightWall) RestoreBarrier(RightWall);
            else if (wall == FrontWall) RestoreBarrier(FrontWall);
            else if (wall == BackWall) RestoreBarrier(BackWall);
            else if (wall == Ceiling) RestoreBarrier(Ceiling);
            else if (wall == Floor) RestoreBarrier(Floor);
            return true;
        }
        return false;
    }
}
