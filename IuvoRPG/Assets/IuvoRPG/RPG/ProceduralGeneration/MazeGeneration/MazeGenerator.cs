using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [Header("Maze Dimensions")]
    [SerializeField] private int mazeWidth = 10;
    [SerializeField] private int mazeDepth = 10;
    [SerializeField] private int mazeHeight = 1;
    [SerializeField] private float generationSpeed = 0.1f;

    [Header("Level Settings")]
    [SerializeField] private float trapCellChance = 0.1f;
    [SerializeField] private float treasureCellChance = 0.1f;
    [SerializeField] private float enemyCellChance = 0.1f;
    [SerializeField] private float powerUpCellChance = 0.1f;
    [SerializeField] private float healthCellChance = 0.1f;
    [SerializeField] private float ammoCellChance = 0.1f;
    [SerializeField] private float weaponCellChance = 0.1f;
    [SerializeField] private float armorCellChance = 0.1f;
    [SerializeField] private float keyCellChance = 0.1f;
    [SerializeField] private float doorCellChance = 0.1f;
    [SerializeField] private float breakableSurfaceChance = 0.1f;
    [SerializeField] private bool startOnBoundary = false;

    [Header("Cell Wall Settings")]
    [SerializeField][Range(0.15f, 1.0f)] float maxWallPercent = 0.45f;
    public bool enableTopDownView = false;

    [Header("Prefabs")]
    [SerializeField] GameObject mazeCell;
    [SerializeField] private float mazeWallHeight = 1.0f;
    private Vector3 mazePrefabScale = new Vector3(1, 1, 1);

    [Header("Boundary Cell Settings")]
    [SerializeField] Material boundaryCellColor;
    public bool changeBoundaryCellColor = false;

    [Header("Maze Stats")]
    public int totalCells;
    public int boundaryCellsCount;
    public int innerCells;
    public int unvisitedCells;
    public int startingNumberOfWalls;
    public int currentNumberOfWalls;
    public int maxNumberOfWalls;
    public List<MazeCell> boundaryCells = new List<MazeCell>();
    public List<MazeBarrier> boundaryWalls = new List<MazeBarrier>();
    public List<MazeCell> innerCellsList = new List<MazeCell>();
    public MazeCell[,,] mazeLevel;
    public Maze mazeParentObject;
    private MazeCell prevCell;
    private MazeCell currCell;

    [Header("Generation Statuses")]
    public bool isFinishedRefining = false;
    public bool wallCountMet = false;
    public bool isFinishedSolvingBlockages = false;
    public bool generationInProgress = false;
    public bool mazeIsFinished = false;
    [SerializeField] private float totalGenerationTime;
    [SerializeField] private float finalTime;

    private void Start()
    {
        Time.timeScale = generationSpeed;
        Initialize();
        StartMazeGeneration();
        if (enableTopDownView)
        {
            foreach (MazeCell cell in mazeLevel)
            {
                cell.DestroyBarrier(cell.Ceiling);
                if (cell.Ceiling.isBoundaryBarrier)
                {
                    cell.Ceiling.DestroyBoundaryBarrier();
                }
            }
        }
    }

    public void Initialize()
    {
        totalGenerationTime = 0;
        totalCells = (mazeWidth * mazeHeight * mazeDepth);

        if (mazeParentObject == null)
        {
            mazeParentObject = Maze.CreateMaze(totalCells, trapCellChance, treasureCellChance, enemyCellChance,
                powerUpCellChance, healthCellChance, ammoCellChance, weaponCellChance, armorCellChance,
                keyCellChance, doorCellChance, breakableSurfaceChance);

        }
        startingNumberOfWalls = 0;
        unvisitedCells = totalCells;
        mazePrefabScale.y = mazeWallHeight;
        mazeCell.transform.localScale = mazePrefabScale;
        MazeCell cell = mazeCell.GetComponent<MazeCell>();

        mazeLevel = new MazeCell[mazeWidth, mazeHeight, mazeDepth];
        for (int k = 0; k < mazeHeight; k++)
        {
            for (int i = 0; i < mazeWidth; i++)
            {
                for (int j = 0; j < mazeDepth; j++)
                {
                    Vector3 cellPosition = new Vector3(i * mazePrefabScale.x, k * mazePrefabScale.y, j * mazePrefabScale.z);
                    mazeLevel[i, k, j] = Instantiate(cell, cellPosition, Quaternion.identity);
                    mazeLevel[i, k, j].transform.parent = mazeParentObject.transform;
                    mazeParentObject.cells.Add(mazeLevel[i, k, j]);
                    startingNumberOfWalls += mazeLevel[i, k, j].startingNumberOfCellWalls;
                }
            }
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        }
        currentNumberOfWalls = startingNumberOfWalls;
        maxNumberOfWalls = (int)((float)currentNumberOfWalls * maxWallPercent);
        MarkBoundaryCells();
        mazeParentObject.boudaryCells = boundaryCells;
        maxNumberOfWalls += boundaryWalls.Count;
    }

    public void MarkBoundaryCells()
    {
        for (int i = 0; i < mazeWidth; i++)
        {
            for (int j = 0; j < mazeDepth; j++)
            {
                for (int k = 0; k < mazeHeight; k++)
                {
                    if (IsBoundaryCell(mazeLevel[i, k, j]))
                    {
                        if (mazeLevel[i, k, j].isBoundaryCell)
                        {
                            boundaryCells.Add(mazeLevel[i, k, j]);
                        }
                    }
                }
            }
        }
    }

    public bool IsBoundaryCell(MazeCell cell)
    {
        int x = (int)(cell.transform.position.x);
        int y = (int)(cell.transform.position.y);
        int z = (int)(cell.transform.position.z);
        bool isBoundary = false;
        if (x == 0 || x == (mazeWidth - 1) || y == 0 || y == (mazeHeight - 1) || z == 0 || z == (mazeDepth - 1))
        {
            isBoundary = true;
            MarkBoundaryWalls(cell, (x), (y), (z));
        }
        StoreNeighboringCells(cell);
        return isBoundary;
    }

    public void MarkBoundaryWalls(MazeCell cell, float x, float y, float z)
    {
        if (x <= 0)
        {
            cell.boundaryWalls.Add(cell.LeftWall);
            cell.LeftWall.GetComponentInChildren<MeshRenderer>().material = boundaryCellColor;
            cell.LeftWall.isBoundaryBarrier = true;
            boundaryWalls.Add(cell.LeftWall);
        }
        if (x >= (mazeWidth - 1))
        {
            cell.boundaryWalls.Add(cell.RightWall);
            cell.RightWall.GetComponentInChildren<MeshRenderer>().material = boundaryCellColor;
            cell.RightWall.isBoundaryBarrier = true;
            boundaryWalls.Add(cell.RightWall);
        }
        if (z <= 0)
        {
            cell.boundaryWalls.Add(cell.BackWall);
            cell.BackWall.GetComponentInChildren<MeshRenderer>().material = boundaryCellColor;
            cell.BackWall.isBoundaryBarrier = true;
            boundaryWalls.Add(cell.BackWall);
        }
        if (z >= (mazeDepth - 1))
        {
            cell.boundaryWalls.Add(cell.FrontWall);
            cell.FrontWall.GetComponentInChildren<MeshRenderer>().material = boundaryCellColor;
            cell.FrontWall.isBoundaryBarrier = true;
            boundaryWalls.Add(cell.FrontWall);
        }
        if (mazeHeight > 1)
        {
            if (y <= 0)
            {
                cell.boundaryWalls.Add(cell.Floor);
                cell.Floor.GetComponentInChildren<MeshRenderer>().material = boundaryCellColor;
                cell.Floor.isBoundaryBarrier = true;
                boundaryWalls.Add(cell.Floor);
            }
            if (y >= (mazeHeight - 1))
            {
                cell.boundaryWalls.Add(cell.Ceiling);
                cell.Ceiling.GetComponentInChildren<MeshRenderer>().material = boundaryCellColor;
                cell.Ceiling.isBoundaryBarrier = true;
                boundaryWalls.Add(cell.Ceiling);
            }
        }
        cell.isBoundaryCell = (cell.boundaryWalls.Count > 0);
    }

    private void Update()
    {
        if (mazeIsFinished)
        {
            finalTime = totalGenerationTime;
            Time.timeScale = 1;
            mazeParentObject.SignalToStart();
            Debug.Log("Final Generation Time: " + finalTime);
            Destroy(gameObject);
            return;
        }
        else
        {
            totalGenerationTime += Time.deltaTime;
        }

        if (generationInProgress)
        {
            StartCoroutine(GenerateStep());
            UpdateMazeStats();
        }
        else
        {
            if (!isFinishedRefining && !isFinishedSolvingBlockages)
            {
                if (!RefineMaze())
                {
                    UpdateMazeStats();
                    RefineMaze();
                    UpdateMazeStats();
                    ReduceBlockages();
                    UpdateMazeStats();
                }
            }
            else if (isFinishedRefining && !isFinishedSolvingBlockages)
            {
                UpdateMazeStats();
                ReduceBlockages();
                if (isFinishedSolvingBlockages && currentNumberOfWalls <= maxNumberOfWalls)
                {
                    mazeIsFinished = true;
                }
                else
                {
                    UpdateMazeStats();
                    ReduceBlockages();
                    UpdateMazeStats();

                }
            }
        }
        if (isFinishedSolvingBlockages && isFinishedRefining && wallCountMet)
        {
            mazeIsFinished = true;
        }
    }

    private void UpdateMazeStats()
    {
        unvisitedCells = 0;
        foreach (MazeCell cell in mazeLevel)
        {
            if (!cell.cellWasVisited)
            {
                unvisitedCells++;
            }
        }
        boundaryCellsCount = boundaryCells.Count;
        int wallCount = 0;
        foreach (MazeCell cell in mazeLevel)
        {
            wallCount += cell.remainingWalls.Count;
        }
        currentNumberOfWalls = wallCount;
    }

    private IEnumerator GenerateStep()
    {
        yield return new WaitForSeconds(0.05f);
        MazeCell nextCell = GetNextUnvisitedNeighbor(currCell);
        if (nextCell != null)
        {
            ClearWalls(prevCell, currCell);
            prevCell = currCell;
            currCell = nextCell;
            currCell.Visit();
            unvisitedCells -= 1;
        }
        else
        {
            foreach (MazeCell cell in mazeLevel)
            {
                if (cell.cellWasVisited == true && unvisitedCells == 0)
                {
                    innerCells = (mazeWidth - 2) * (mazeDepth - 2);
                    boundaryCellsCount = boundaryCells.Count;
                    if (cell.isBoundaryCell == false)
                    {
                        boundaryCells.Remove(cell);
                    }
                    generationInProgress = false;
                    yield return new WaitForSeconds(0.05f);
                }
                else { yield return null; }
            }
        }
    }

    public bool RefineMaze()
    {
        foreach (MazeCell cell in mazeLevel)
        {
            if (currentNumberOfWalls <= maxNumberOfWalls)
            {
                isFinishedRefining = true;
                Debug.Log("Maze has been refined");
                return isFinishedRefining;
            }
            if (cell.DeleteRandomRemainingWall(mazeHeight))
            {
                currentNumberOfWalls -= 1;
            }
        }
        int wallCount = 0;
        foreach (MazeCell cell in mazeLevel)
        {
            wallCount += cell.remainingWalls.Count;
        }
        if (wallCount <= maxNumberOfWalls)
        {
            isFinishedRefining = true;
            Debug.Log("Maze has been refined");
        }
        return isFinishedRefining;
    }

    public void ReduceBlockages()
    {
        if (!wallCountMet)
        {
            if (currentNumberOfWalls > maxNumberOfWalls)
            {
                int toRemove = currentNumberOfWalls - maxNumberOfWalls;
                for (int i = 0; i < toRemove; i++)
                {
                    int randomX = Random.Range(0, mazeWidth);
                    int randomY = Random.Range(0, mazeHeight);
                    int randomZ = Random.Range(0, mazeDepth);
                    MazeCell cell = mazeLevel[randomX, randomY, randomZ];
                    if (cell.remainingWalls.Count != 0)
                    {
                        cell.DeleteRandomRemainingWall(mazeHeight);
                    }
                    else
                    {
                        i--;
                    }
                }
            }
            else if (currentNumberOfWalls <= maxNumberOfWalls)
            {
                wallCountMet = true;
                Debug.Log("Wall Count met.");
            }
        }
        else
        {
            isFinishedSolvingBlockages = true;
        }
    }

    public void StoreNeighboringCells(MazeCell cell)
    {
        cell.neighboringCells.Clear();
        foreach (MazeCell neighbor in mazeLevel)
        {
            if (cell == neighbor) continue;
            Vector3 currPos = cell.transform.position;
            Vector3 neighborPos = neighbor.transform.position;
            if (currPos.x == neighborPos.x && neighborPos.z == currPos.z + 1)
            {
                cell.FrontNeighbor = neighbor;
                cell.neighboringCells.Add(neighbor);
            }
            else if (currPos.x == neighborPos.x && neighborPos.z == currPos.z - 1)
            {
                cell.BackNeighbor = neighbor;
                cell.neighboringCells.Add(neighbor);
            }
            else if (currPos.z == neighborPos.z && neighborPos.x == currPos.x + 1)
            {
                cell.RightNeighbor = neighbor;
                cell.neighboringCells.Add(neighbor);
            }
            else if (currPos.z == neighborPos.z && neighborPos.x == currPos.x - 1)
            {
                cell.LeftNeighbor = neighbor;
                cell.neighboringCells.Add(neighbor);
            }
        }
    }

    private void StartMazeGeneration()
    {
        prevCell = null;
        currCell = mazeLevel[0, 0, 0];
        generationInProgress = true;
        currCell.Visit();
        unvisitedCells -= 1;
        prevCell = currCell;
    }

    private MazeCell GetNextUnvisitedNeighbor(MazeCell currCell)
    {
        var unvistedCells = GetUnvisitedCells(currCell);
        int max = unvistedCells.Count();
        return unvistedCells.OrderBy(_ => Random.Range(1, max)).FirstOrDefault();
    }

    private IEnumerable<MazeCell> GetUnvisitedCells(MazeCell currCell)
    {
        int x = (int)(currCell.transform.position.x);
        int y = (int)(currCell.transform.position.y);
        int z = (int)(currCell.transform.position.z);
        if ((x + 1) < mazeWidth)
        {
            var cellToRight = mazeLevel[x + 1, y, z];
            if (cellToRight.cellWasVisited == false)
            {
                yield return cellToRight;
            }
        }
        if ((x - 1) >= 0)
        {
            var cellToLeft = mazeLevel[x - 1, y, z];
            if (cellToLeft.cellWasVisited == false)
            {
                yield return cellToLeft;
            }
        }
        if ((z + 1) < mazeDepth)
        {
            var cellToFront = mazeLevel[x, y, z + 1];
            if (cellToFront.cellWasVisited == false)
            {
                yield return cellToFront;
            }
        }
        if ((z - 1) >= 0)
        {
            var cellToBack = mazeLevel[x, y, z - 1];
            if (cellToBack.cellWasVisited == false)
            {
                yield return cellToBack;
            }
        }
        if (mazeHeight > 1)
        {
            if (y + 1 < mazeHeight)
            {
                var cellToUp = mazeLevel[x, y + 1, z];
                if (cellToUp.cellWasVisited == false)
                {
                    yield return cellToUp;
                }
            }
            if (y - 1 >= 0)
            {
                var cellToDown = mazeLevel[x, y - 1, z];
                if (cellToDown.cellWasVisited == false)
                {
                    yield return cellToDown;
                }
            }
        }
        int randWidth = Random.Range(0, mazeWidth);
        int randHeight = Random.Range(0, mazeHeight);
        int randDepth = Random.Range(0, mazeDepth);
        if (mazeLevel[randWidth, randHeight, randDepth].cellWasVisited == false)
        {
            var cellToVisit = mazeLevel[randWidth, randHeight, randDepth];
            yield return cellToVisit;
        }
    }

    private void ClearWalls(MazeCell prevCell, MazeCell currCell)
    {
        if (prevCell == null) { return; }
        int prevX = (int)(prevCell.transform.position.x);
        int prevY = (int)(prevCell.transform.position.y);
        int prevZ = (int)(prevCell.transform.position.z);
        int currX = (int)(currCell.transform.position.x);
        int currY = (int)(currCell.transform.position.y);
        int currZ = (int)(currCell.transform.position.z);
        if (prevX < currX && prevCell.hasRightWall && !prevCell.boundaryWalls.Contains(prevCell.RightWall))
        {
            prevCell.DestroyBarrier(prevCell.RightWall);
            if (!currCell.boundaryWalls.Contains(currCell.LeftWall))
            {
                currCell.DestroyBarrier(currCell.LeftWall);
            }
        }
        else if (prevX > currX && prevCell.hasLeftWall && !prevCell.boundaryWalls.Contains(prevCell.LeftWall))
        {
            prevCell.DestroyBarrier(prevCell.LeftWall);
            if (!currCell.boundaryWalls.Contains(currCell.RightWall))
            {
                currCell.DestroyBarrier(currCell.RightWall);
            }
        }
        else if (prevZ < currZ && prevCell.hasFrontWall && !prevCell.boundaryWalls.Contains(prevCell.FrontWall))
        {
            prevCell.DestroyBarrier(prevCell.FrontWall);
            if (!currCell.boundaryWalls.Contains(currCell.BackWall))
            {
                currCell.DestroyBarrier(currCell.BackWall);
            }
        }
        else if (prevZ > currZ && prevCell.hasBackWall && !prevCell.boundaryWalls.Contains(prevCell.BackWall))
        {
            prevCell.DestroyBarrier(prevCell.BackWall);
            if (!currCell.boundaryWalls.Contains(currCell.FrontWall))
            {
                currCell.DestroyBarrier(currCell.FrontWall);
            }
        }
        else if (prevY < currY && prevCell.hasCeiling && !prevCell.boundaryWalls.Contains(prevCell.Ceiling))
        {
            prevCell.DestroyBarrier(prevCell.Ceiling);
            if (!currCell.boundaryWalls.Contains(currCell.Floor))
            {
                currCell.DestroyBarrier(currCell.Floor);
            }
        }
        else if (prevY > currY && prevCell.hasFloor && !prevCell.boundaryWalls.Contains(prevCell.Floor))
        {
            prevCell.DestroyBarrier(prevCell.Floor);
            if (!currCell.boundaryWalls.Contains(currCell.Ceiling))
            {
                currCell.DestroyBarrier(currCell.Ceiling);
            }
        }
    }
}
