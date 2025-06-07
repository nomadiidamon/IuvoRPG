using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class Maze : MonoBehaviour
{
    public int totalCells;
    public MazeCell startingCell;
    public MazeCell endingCell;
    public bool alterStartAndEndColor = true;
    public Material startingCellColor ;
    public Material endingCellColor;
    public Material breakableBarrierColor;
    public Material trapCellColor;
    public float trapCellChance = 0.1f;
    public float treasureCellChance = 0.1f;
    public float enemyCellChance = 0.1f;
    public float powerUpCellChance = 0.1f;
    public float healthCellChance = 0.1f;
    public float ammoCellChance = 0.1f;
    public float weaponCellChance = 0.1f;
    public float armorCellChance = 0.1f;
    public float keyCellChance = 0.1f;
    public float doorCellChance = 0.1f;
    public float breakableSurfaceChance = 0.1f;
    public bool startOnBoundary = true;
    public NavMeshSurface navMeshSurface;

    public List<MazeCell> cells = new List<MazeCell>();
    public List<MazeCell> boudaryCells = new List<MazeCell>();
    public List<MazeCell> innerCells = new List<MazeCell>();
    public List<MazeCell> isolatedCells = new List<MazeCell>();
    public List<MazeCell> specialCells = new List<MazeCell>();
    public List<MazeBarrier> breakableBarriers = new List<MazeBarrier>();
    public List<MazeCell> trapCells = new List<MazeCell>();

    public bool levelIsReady = false;
    public bool start = false;
    public bool hideTopFloor = false;

    public static Maze CreateMaze(int totalCells, float trapCellChance = 0.1f, float treasureCellChance = 0.1f, float enemyChance = 0.1f,
        float powerUpChance = 0.1f, float healthCellChance = 0.1f, float ammoCellChance = 0.1f, float weaponCellChance = 0.1f,
        float armorCellChance = 0.1f, float keyCellChance = 0.1f, float doorCellChance = 0.1f, float breakableSurfaceChance = 0.1f, Material startCell = null, Material endCell = null, bool startOnBoundary = true)
    {
        GameObject mazeGameObject = new GameObject("Maze");
        Maze maze = mazeGameObject.AddComponent<Maze>();
        maze.totalCells = totalCells;
        maze.trapCellChance = trapCellChance;
        maze.treasureCellChance = treasureCellChance;
        maze.enemyCellChance = enemyChance;
        maze.powerUpCellChance = powerUpChance;
        maze.healthCellChance = healthCellChance;
        maze.ammoCellChance = ammoCellChance;
        maze.weaponCellChance = weaponCellChance;
        maze.armorCellChance = armorCellChance;
        maze.keyCellChance = keyCellChance;
        maze.doorCellChance = doorCellChance;
        maze.breakableSurfaceChance = breakableSurfaceChance;
        maze.startingCellColor = startCell;
        maze.endingCellColor = endCell;
        maze.startOnBoundary = startOnBoundary;
        return maze;
    }

    private void Update()
    {
        if (!start)
        {
            return;
        }
        else
        {
            if (!levelIsReady)
            {
                InitializeSpecialCells();

                foreach (var cell in cells)
                {
                    if (cell.cellCollider != null)
                    {
                        cell.cellCollider.isTrigger = true;
                    }
                }

                levelIsReady = true;
                //InitializeNavMesh();
                //navMeshSurface.enabled = true;
                //navMeshSurface.BuildNavMesh();
            }
        }
        if (hideTopFloor)
        {
            foreach (MazeCell cell in cells)
            {
                if (cell.transform.position.y > 0)
                {
                    cell.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            foreach (MazeCell cell in cells)
            {
                if (cell.transform.position.y > 0)
                {
                    cell.gameObject.SetActive(true);
                }
            }
        }
    }

    public bool SignalToStart()
    {
        start = true;
        return start;
    }

    public void InitializeSpecialCells()
    {
        specialCells.Clear();
        InitializeIsolatedCells();
        PickStartAndEndCells();
        InitializeInnerCells();
        InitializeTrapCells();
        InitializeBreakableBarriers();
    }
    public void PickStartAndEndCells()
    {
        startingCell = GetRandomCell(startOnBoundary);
        endingCell = GetRandomCell(startOnBoundary);
        while (endingCell == startingCell)
        {
            endingCell = GetRandomCell(startOnBoundary);
        }
        List<MazeCell> neighbors = new List<MazeCell>();
        foreach (MazeCell cell in startingCell.neighboringCells)
        {
            foreach (MazeCell neighbor in cell.neighboringCells)
            {
                if (neighbor != startingCell)
                {
                    neighbors.Add(neighbor);
                }
            }
        }
        if (neighbors.Contains(endingCell))
        {
            PickStartAndEndCells();
        }

        if (alterStartAndEndColor)
        {
            foreach (MazeBarrier wall in startingCell.remainingWalls)
            {
                wall.GetComponentInChildren<MeshRenderer>().material = startingCellColor;
            }
            foreach (MazeBarrier wall in endingCell.remainingWalls)
            {
                wall.GetComponentInChildren<MeshRenderer>().material = endingCellColor;
            }
        }
        specialCells.Add(startingCell);
        specialCells.Add(endingCell);
    }

    public void InitializeInnerCells()
    {
        if (cells.Count == 0)
        {
            return;
        }
        else
        {
            innerCells.Clear();
            foreach (MazeCell cell in cells)
            {
                if (!cell.isBoundaryCell)
                {
                    innerCells.Add(cell);
                }
            }
        }
    }

    public void InitializeTrapCells()
    {
        trapCells.Clear();
        foreach (MazeCell cell in cells)
        {
            if (cell == startingCell || cell == endingCell) { continue; }
            if (Random.value < trapCellChance)
            {
                trapCells.Add(cell);
                cell.isTrapCell = true;
                specialCells.Add(cell);
                foreach (MazeBarrier wall in cell.sharerdWalls)
                {
                    wall.GetComponentInChildren<MeshRenderer>().material = trapCellColor;
                }
            }
        }
    }

    public void InitializeBreakableBarriers()
    {
        breakableBarriers.Clear();
        foreach (MazeCell cell in cells)
        {
            if (cell == startingCell || cell == endingCell) { continue; }
            if (cell.isTrapCell) { continue; }
            if (Random.value < breakableSurfaceChance)
            {
                int randomBarrier = Random.Range(0, cell.remainingWalls.Count);
                MazeBarrier barrier = cell.remainingWalls[randomBarrier];
                if (barrier.isBoundaryBarrier)
                {
                    continue;
                }
                else
                {
                    barrier.isBreakableBarrier = true;
                    barrier.GetComponentInChildren<MeshRenderer>().material = breakableBarrierColor;
                    breakableBarriers.Add(barrier);
                    specialCells.Add(cell);
                }
            }

        }
    }

    public void InitializeIsolatedCells()
    {
        isolatedCells.Clear();
        foreach (MazeCell cell in cells)
        {
            cell.sharerdWalls.Clear();

            bool frontWall = false;
            if (cell.FrontWall.gameObject.activeSelf || (cell.FrontNeighbor != null && cell.FrontNeighbor.BackWall.gameObject.activeSelf))
            {
                frontWall = true;
                if (cell.FrontWall.gameObject.activeSelf) cell.sharerdWalls.Add(cell.FrontWall);
                if (cell.FrontNeighbor != null && cell.FrontNeighbor.BackWall.gameObject.activeSelf) cell.sharerdWalls.Add(cell.FrontNeighbor.BackWall);
            }
            bool backWall = false;
            if (cell.BackWall.gameObject.activeSelf || (cell.BackNeighbor != null && cell.BackNeighbor.FrontWall.gameObject.activeSelf))
            {
                backWall = true;
                if (cell.BackWall.gameObject.activeSelf) cell.sharerdWalls.Add(cell.BackWall);
                if (cell.BackNeighbor != null && cell.BackNeighbor.FrontWall.gameObject.activeSelf) cell.sharerdWalls.Add(cell.BackNeighbor.FrontWall);
            }


            bool leftWall = false;
            if (cell.LeftWall.gameObject.activeSelf || (cell.LeftNeighbor != null && cell.LeftNeighbor.RightWall.gameObject.activeSelf))
            {
                leftWall = true;
                if (cell.LeftWall.gameObject.activeSelf) cell.sharerdWalls.Add(cell.LeftWall);
                if (cell.LeftNeighbor != null && cell.LeftNeighbor.RightWall.gameObject.activeSelf) cell.sharerdWalls.Add(cell.LeftNeighbor.RightWall);
            }
            bool rightWall = false;
            if (cell.RightWall.gameObject.activeSelf || (cell.RightNeighbor != null && cell.RightNeighbor.LeftWall.gameObject.activeSelf))
            {
                rightWall = true;
                if (cell.RightWall.gameObject.activeSelf) cell.sharerdWalls.Add(cell.RightWall);
                if (cell.RightNeighbor != null && cell.RightNeighbor.LeftWall.gameObject.activeSelf) cell.sharerdWalls.Add(cell.RightNeighbor.LeftWall);
            }

            if (frontWall && backWall && leftWall && rightWall)
            {
                cell.isIsolatedCell = true;
                isolatedCells.Add(cell);
                specialCells.Add(cell);
            }
        }
        Debug.Log("Isolated Cells: " + isolatedCells.Count);

        foreach (MazeCell cell in isolatedCells)
        {
            int randomBarrier = Random.Range(0, cell.sharerdWalls.Count);
            MazeBarrier barrier = cell.sharerdWalls[randomBarrier];
            barrier.isBreakableBarrier = true;
            barrier.GetComponentInChildren<MeshRenderer>().material = breakableBarrierColor;
            breakableBarriers.Add(barrier);
        }
    }

    public void InitializeNavMesh()
    {
        NavMeshSurface[] surfaces = FindObjectsByType<NavMeshSurface>(FindObjectsSortMode.None);

        foreach (NavMeshSurface surface in surfaces)
        {
            surface.UpdateNavMesh(surface.navMeshData);
        }
    }

    public MazeCell GetRandomCell(bool isBoundaryCell)
    {
        if (isBoundaryCell)
        {
            if (boudaryCells.Count == 0)
            {
                return cells[Random.Range(0, cells.Count)];
            }
            return boudaryCells[Random.Range(0, boudaryCells.Count)];
        }
        else
        {
            return cells[Random.Range(0, cells.Count)];
        }
    }
}

