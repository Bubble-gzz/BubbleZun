using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleZun.Utils;
using BubbleZun.Interaction;
using UnityEngine.Events;
public class SetHierarchyEntryParentOperation : Operation
{
    HierarchyEntry entry;
    HierarchyEntry newParent;
    HierarchyEntry oldParent;
    public SetHierarchyEntryParentOperation(HierarchyEntry entry, HierarchyEntry newParent, bool mute = false) : base("Set Hierarchy Entry Parent", mute)
    {
        this.entry = entry;
        this.oldParent = entry.parent;
        this.newParent = newParent;
    }
    public override void Undo()
    {
        entry.hierarchy.ChangeEntryParent(entry, oldParent);
        Hierarchy.onUndoMoveEntryOperation.Invoke();
    }
    public override void Redo()
    {
        entry.hierarchy.ChangeEntryParent(entry, newParent);
        Hierarchy.onRedoMoveEntryOperation.Invoke();
    }
}
