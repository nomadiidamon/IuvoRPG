using System;
using System.Collections.Generic;
using UnityEngine;

public class BreakableColumn : MonoBehaviour
{
    public MazeCell _my_ParentCell;
    public BreakableBarrier _my_ParentBarrier;
    public List<BreakableBit> _my_ChildBits = new List<BreakableBit>();
    public BreakableChunk _my_ParentChunck;
    public BreakableSegment _my_ParentSegment;

}
