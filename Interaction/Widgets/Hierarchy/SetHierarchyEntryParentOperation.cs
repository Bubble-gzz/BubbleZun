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
    public SetHierarchyEntryParentOperation(HierarchyEntry entry, HierarchyEntry newParent) : base("Set Hierarchy Entry Parent")
    {
        this.entry = entry;
        this.oldParent = entry.parent;
        this.newParent = newParent;
    }
    public override void Undo(bool mute = false)
    {
        entry.hierarchy.ChangeEntryParent(entry, oldParent);
        Hierarchy.onUndoSetEntryParentOperation.Invoke();
    }
    public override void Redo(bool mute = false)
    {
        entry.hierarchy.ChangeEntryParent(entry, newParent);
        Hierarchy.onRedoSetEntryParentOperation.Invoke();
    }
}
