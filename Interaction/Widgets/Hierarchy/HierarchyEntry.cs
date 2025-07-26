using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BubbleZun.Effects.AnimationEffects;
using UnityEngine.Events;
using TMPro;
using BubbleZun.Utils;
namespace BubbleZun.Interaction
{
public class HierarchyEntry
{
    // Start is called before the first frame update
    public Hierarchy hierarchy;
    public object bindObject;
    public int depth;
    public float y;
    public bool visible;
    public bool expanded;
    public HierarchyEntry parent;
    public HierarchyEntry prev, next;
    public string id;
    public List<HierarchyEntry> children = new List<HierarchyEntry>();
    public bool hasChildren => children.Count > 0;
    public bool isRoot => hierarchy?.root == this;
    public HierarchyEntryRenderer renderer;
 
    public bool IsChildOf(HierarchyEntry entry)
    {
        HierarchyEntry parent = this;
        while (parent != null) {
            if (parent == entry) return true;
            parent = parent.parent;
        }
        return false;
    }
    public void Delete()
    {
        //Debug.Log("Delete HierarchyEntry: " + id);
        List<HierarchyEntry> children = new List<HierarchyEntry>(this.children);
        foreach (var child in children) child.Delete();
        if (parent != null) parent.children.Remove(this);
        if (hierarchy.root == this) hierarchy.root = null;
        parent = null;
        renderer.Recycle();
        renderer = null;
    }
    public bool selected;
    public void Select(bool multiSelect = false)
    {
        if (!multiSelect)
        {
            if (selected) return;
            selected = true;
            renderer?.TurnOn();
            HierarchyEntry lastSelectedEntry = hierarchy.currentSelectedEntry;
            if (lastSelectedEntry != null) {
                lastSelectedEntry.renderer?.TurnOff();
                lastSelectedEntry.selected = false;
            }
            hierarchy.ClearSelectedEntries();
            hierarchy.AddSelectedEntry(this);
        }
        else {
            selected = !selected;
            if (selected) {
                renderer?.TurnOn();
                hierarchy.AddSelectedEntry(this);
            }
            else {
                renderer?.TurnOff();
                hierarchy.RemoveSelectedEntry(this);
            }
        }
    }
}
}