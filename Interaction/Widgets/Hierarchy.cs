using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace BubbleZun.Interaction
{
public class Hierarchy : MonoBehaviour
{
    // Start is called before the first frame update
    public float indent;
    public float spacing;
    public List<HierarchyEntry> entries = new List<HierarchyEntry>();
    public HierarchyEntry root;
    public GameObject entryPrefab;
    public List<HierarchyEntry> currentSelectedEntries = new List<HierarchyEntry>();
    public HierarchyEntry currentSelectedEntry => currentSelectedEntries.Count == 1 ? currentSelectedEntries[0] : null;
    public UnityEvent onHierarchyChanged = new UnityEvent();
    void Start()
    {
        CreateEntry(null, "Root", null);
        CreateEntry(root, "Loop1", null);
        CreateEntry(root, "Loop2", null);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    HierarchyEntry lastEntry;
    public void UpdateHierarchy()
    {
        entries.Clear();
        lastEntry = null;
        UpdateHierarchy(root, null, 0, true);
        onHierarchyChanged.Invoke();
    }
    void UpdateHierarchy(HierarchyEntry entry, HierarchyEntry parent,int depth, bool show)
    {
        entry.depth = depth;
        entry.parent = parent;
        entries.Add(entry);
        entry.show = show;
        if (show)
        {

            entry.prev = lastEntry;
            entry.y = lastEntry == null ? 0 : lastEntry.y - spacing;
            lastEntry = entry;
        }
        else {
            entry.y = parent.y;
        }

        foreach (HierarchyEntry child in entry.children) {
            UpdateHierarchy(child, entry, depth + 1, show & entry.expanded);
        }
    }
    public HierarchyEntry CreateEntry(HierarchyEntry parent, string id, object bindObject)
    {
        HierarchyEntry newEntry = Instantiate(entryPrefab, transform).GetComponent<HierarchyEntry>();
        newEntry.id = id;
        newEntry.hierarchy = this;
        newEntry.bindObject = bindObject;
        newEntry.expanded = false;
        Debug.Log("CreateEntry: " + newEntry.id);

        RectTransform rectTransform = newEntry.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 1);
        rectTransform.anchorMax = new Vector2(1, 1);

        if (parent == null) root = newEntry;
        else {
            parent.children.Add(newEntry);
            parent.expanded = true;
        }

        newEntry.parent = parent;
        UpdateHierarchy();
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
    public void AddTestEntry()
    {
        if (currentSelectedEntry == null) return;
        CreateEntry(currentSelectedEntry, "Test" + entries.Count, null);
        UpdateHierarchy();
    }
}
}