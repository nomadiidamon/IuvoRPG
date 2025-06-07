using UnityEngine;

public class MazeBarrier : MonoBehaviour
{
    public bool isBoundaryBarrier = false;
    public bool isBreakableBarrier = false;
    public bool isTrulyBreakableBarrier = false;
    public MazeCell myParentCell;
    public BoxCollider barrierCollider;
    public MeshRenderer barrierRenderer;

    private void Awake()
    {
        if (myParentCell == null)
        {
            myParentCell = GetComponentInParent<MazeCell>();
        }
        if (barrierCollider == null)
        {
            barrierCollider = GetComponent<BoxCollider>();
        }
        if (barrierRenderer == null)
        {
            barrierRenderer = GetComponent<MeshRenderer>();
        }
    }

    public void DestroyBarrier()
    {
        if (isBreakableBarrier) return;
        if (!isBoundaryBarrier)
        {
            myParentCell.remainingWalls.Remove(this);
            myParentCell.destroyedWalls.Add(this);
            gameObject.SetActive(false);
        }
    }

    public void DestroyBoundaryBarrier()
    {
        if (!isBoundaryBarrier) return;
        myParentCell.remainingWalls.Remove(this);
        myParentCell.destroyedWalls.Add(this);
        gameObject.SetActive(false);
    }

    public void RestoreBarrier()
    {
        if (gameObject.activeSelf) return;
        gameObject.SetActive(true);
        myParentCell.remainingWalls.Add(this);
        myParentCell.destroyedWalls.Remove(this);
    }

    public void DestroyBreakableBarrier()
    {
        if (!isBreakableBarrier) return;
        myParentCell.remainingWalls.Remove(this);
        myParentCell.destroyedWalls.Add(this);
        gameObject.SetActive(false);
    }
}
