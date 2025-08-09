using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleZun.Utils;
using BubbleZun.Interaction;
public class MoveHierarchyEntryAfterOperation : Operation
{
    HierarchyEntry entry;
    HierarchyEntry oldPrev;
    HierarchyEntry newPrev;
    public MoveHierarchyEntryAfterOperation(HierarchyEntry entry, HierarchyEntry newPrev, bool mute = false) : base("Move Hierarchy Entry After", mute)
    {
        this.entry = entry;
        this.oldPrev = entry.prev;
        this.newPrev = newPrev;
    }
    public override void Undo()
    {
        entry.hierarchy.MoveEntryAfter(entry, oldPrev);
        Hierarchy.onUndoMoveEntryOperation.Invoke();
    }
    public override void Redo()
    {
        entry.hierarchy.MoveEntryAfter(entry, newPrev);
        Hierarchy.onRedoMoveEntryOperation.Invoke();
    }
}
