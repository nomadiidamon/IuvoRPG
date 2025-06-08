using System.Collections.Generic;
using UnityEngine;

public class BreakableBarrier : MonoBehaviour
{
    public List<BreakableBit> _my_ChildBits = new List<BreakableBit>();
    public List<BreakableColumn> _my_ChildColumns = new List<BreakableColumn>();
    public List<BreakableChunk> _my_ChildChunks = new List<BreakableChunk>();
    public List<BreakableSegment> _my_ChildSegments = new List<BreakableSegment>();

    public BreakableBarrier _my_ParentBarrier;
    public MazeCell _my_ParentCell;
}
