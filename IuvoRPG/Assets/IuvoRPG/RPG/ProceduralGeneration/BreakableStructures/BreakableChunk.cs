using System.Collections.Generic;
using UnityEngine;

public class BreakableChunk : MonoBehaviour
{
    public List<BreakableBit> _my_ChildBits = new List<BreakableBit>();
    public List<BreakableColumn> _my_ChildColumns = new List<BreakableColumn>();
    public BreakableSegment _my_ParentSegment;
    public BreakableBarrier _my_ParentBarrier;
    public MazeCell _my_ParentCell;
}
