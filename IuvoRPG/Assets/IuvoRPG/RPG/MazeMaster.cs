using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class MazeMaster : MonoBehaviour
{
    public static MazeMaster Instance;

    public Maze level;
    public GameObject levelParent;
    [SerializeField] public MazeTester tester;
    [SerializeField] public MazeGenerator mazeMaker;
    [SerializeField] public MazeBarrier breakableBarrier;
    public bool GOOD = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {
        LevelReady();
    }

    private bool LevelReady()
    {
        if (GOOD)
        {
            return true;
        }
        if (mazeMaker == null)
        {
            mazeMaker = null;
            level = FindAnyObjectByType<Maze>();
            tester.startingPos = level.startingCell.transform.position;
            tester.goalPos = level.endingCell.transform.position;
            GOOD = true;
            if (breakableBarrier != null)
            {
                ReplaceTemporaryBarriers();
            }
            level.transform.parent = levelParent.transform;
            NavMeshSurface surf = levelParent.AddComponent<NavMeshSurface>();
            if (surf != null)
            {
                if (surf.navMeshData != null)
                {
                    surf.UpdateNavMesh(surf.navMeshData);
                }

            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ReplaceTemporaryBarriers()
    {
        for (int i = 0; i < level.breakableBarriers.Count; i++)
        {
            MazeBarrier toReplace = Instantiate(breakableBarrier);
            toReplace.transform.position = level.breakableBarriers[i].transform.position;
            toReplace.myParentCell = level.breakableBarriers[i].myParentCell;
            toReplace.transform.parent = toReplace.myParentCell.transform;
            toReplace.myParentCell.remainingWalls.Add(toReplace);

            // Copy the properties of the original transform to the new transform
            toReplace.transform.localPosition = level.breakableBarriers[i].transform.localPosition;
            toReplace.transform.localRotation = level.breakableBarriers[i].transform.localRotation;
            toReplace.transform.localScale = level.breakableBarriers[i].transform.localScale;
        }
    }


}
