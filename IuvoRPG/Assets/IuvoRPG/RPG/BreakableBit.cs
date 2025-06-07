using UnityEngine;

// smallest component of the Breakable objects
public class BreakableBit : MonoBehaviour
{
    public MeshRenderer rend;
    public BoxCollider coll;
    public BreakableColumn _my_ParentColumn;
    public BreakableChunk _my_ParentChunck;
    public BreakableSegment _my_ParentSegment;
    public BreakableBarrier _my_ParentBarrier;
    public MazeCell _my_ParentCell;

    public bool isBreakableBit = true;
    public bool isBreakingNow = false;

    public Color _my_primaryColor;
    public Color _my_secondaryColor;     
    public Color _my_tertiaryColor;
}
