using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BubbleZun.Effects.AnimationEffects;
using BubbleZun.Utils;
namespace BubbleZun.Interaction
{
public class Hierarchy : MonoBehaviour
{
    // Start is called before the first frame update
    public static UndoSystem undoSystem;
    public static UnityEvent onRedoMoveEntryOperation = new UnityEvent();
    public static UnityEvent onUndoMoveEntryOperation = new UnityEvent();
    public RectTransform rt;
    public RectTransform content;
    public float indent = 20;
    public float spacing = 35;
    public float leftPadding = 10, rightPadding = 10;
    public float topPadding = 10, bottomPadding = 10;
    public float entryHeight = 30;
    public float viewportHeight = 800;
    float contentHeight;
    [HideInInspector] public List<HierarchyEntry> entries = new List<HierarchyEntry>();
    [HideInInspector] public HierarchyEntry root;
    public GameObject entryPrefab;
    ObjectPool entryRendererPool;
    [HideInInspector] public List<HierarchyEntry> currentSelectedEntries = new List<HierarchyEntry>();
    [HideInInspector] public HierarchyEntry currentSelectedEntry => currentSelectedEntries.Count == 1 ? currentSelectedEntries[0] : null;
    public UnityEvent onHierarchyChanged = new UnityEvent();
    void Awake()
    {
        rt = GetComponent<RectTransform>();
        if (content == null) content = GetComponent<RectTransform>();
        entryRendererPool = new ObjectPool(entryPrefab, 0, content);
    }
    void Update()
    {
        RefreshUI();
    }
    HierarchyEntry lastEntry;
    public void UpdateHierarchy()
    {
        entries.Clear();
        lastEntry = null;
        contentHeight = 0;
        UpdateHierarchy(root, null, 0, true);
        contentHeight += topPadding + bottomPadding;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, contentHeight);
        onHierarchyChanged.Invoke();
        entries.Sort((a, b) => a.y.CompareTo(b.y)); //待优化
        RefreshUI();
    }
    void UpdateHierarchy(HierarchyEntry entry, HierarchyEntry parent,int depth, bool show)
    {
        entry.depth = depth;
        entry.parent = parent;
        entries.Add(entry);
        entry.visible = show;

        if (show)
        {
            if (lastEntry != null) lastEntry.next = entry;
            entry.prev = lastEntry;
            entry.next = null;
            entry.y = lastEntry == null ? 0 : lastEntry.y - spacing;
            lastEntry = entry;
        }
        else {
            entry.y = parent.y;
        }

        entry.renderer?.UpdateUI(true);

        contentHeight = Mathf.Max(contentHeight, - entry.y);
        foreach (HierarchyEntry child in entry.children) {
            UpdateHierarchy(child, entry, depth + 1, show & entry.expanded);
        }
    }
    public HierarchyEntry CreateEntry(HierarchyEntry parent, string id, object bindObject, bool updateHierarchy = true)
    {
        HierarchyEntry newEntry = new HierarchyEntry();
        newEntry.id = id;
        newEntry.hierarchy = this;
        newEntry.bindObject = bindObject;
        newEntry.expanded = true;
        newEntry.y = parent == null ? 0 : parent.y;

        if (parent == null) root = newEntry;
        else {
            parent.children.Add(newEntry);
            BDebug.Log("Add child: " + newEntry.id + " to " + parent.id);
            parent.expanded = true;
        }

        newEntry.parent = parent;
        if (updateHierarchy) UpdateHierarchy();
        return newEntry;
    }
    public void AddSelectedEntry(HierarchyEntry entry)
    {
        if (currentSelectedEntries.Contains(entry)) return;
        currentSelectedEntries.Add(entry);
    }
    public void RemoveSelectedEntry(HierarchyEntry entry)
    {
        currentSelectedEntries.Remove(entry);
    }
    public void ClearSelectedEntries()
    {
        currentSelectedEntries.Clear();
    }
    public HierarchyEntry currentDraggingEntry;
    public void ChangeEntryParent(HierarchyEntry entry, HierarchyEntry newParent)
    {
        if (newParent == null) return;
        if (entry.parent == newParent) return;
        if (newParent.IsChildOf(entry)) return;
        entry.parent.children.Remove(entry);
        newParent.children.Add(entry);
        entry.parent = newParent;
        newParent.expanded = true;
        UpdateHierarchy();
    }
    public void MoveEntryAfter(HierarchyEntry entry, HierarchyEntry newPrev)
    {
        if (newPrev == null) return;
        if (newPrev.IsChildOf(entry)) return;
        HierarchyEntry nextEntry = newPrev.next;
        HierarchyEntry newParent;
        
        entry.parent.children.Remove(entry);

        if (nextEntry != null && nextEntry.IsChildOf(newPrev))
        {
            newParent = newPrev;
            newParent.children.Insert(0, entry);
        }
        else {
            newParent = newPrev.parent;
            for (int i = 0; i < newParent.children.Count; i++) {
                if (newParent.children[i] == newPrev) {
                    newParent.children.Insert(i + 1, entry);
                    break;
                }
            }
        }
        entry.parent = newParent;

        UpdateHierarchy();
    }
    int visibleRangeL, visibleRangeR;
    void RefreshUI()
    {
        visibleRangeL = entries.Count - 1; visibleRangeR = 0;
        for (int i = 0; i < entries.Count; i++)
        {
            HierarchyEntry entry = entries[i];
            if (InViewPort(entry)) 
            {
                visibleRangeL = Mathf.Min(visibleRangeL, i);
                visibleRangeR = Mathf.Max(visibleRangeR, i);
                if (entries[i].renderer == null && entry.visible) {
                    HierarchyEntryRenderer entryRenderer = entryRendererPool.GetObject(content).GetComponent<HierarchyEntryRenderer>();
                    entryRenderer.entry = entries[i];
                    entries[i].renderer = entryRenderer;
                    entryRenderer.Init();
                }
            }
            else {
                if (entries[i].renderer != null) {
                    entries[i].renderer.Recycle();
                    entries[i].renderer = null;
                }
            }
        }
    }
    virtual public bool InViewPort(HierarchyEntry entry)
    {
        float y = entry.y + transform.localPosition.y;
        return y > - viewportHeight - entryHeight && y < entryHeight;
    }
}
}