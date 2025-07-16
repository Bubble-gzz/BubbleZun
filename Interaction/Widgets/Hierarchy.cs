using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using BubbleZun.Effects.AnimationEffects;
namespace BubbleZun.Interaction
{
public class Hierarchy : MonoBehaviour
{
    // Start is called before the first frame update
    RectTransform rt;
    public RectTransform content;
    public float indent = 20;
    public float spacing = 35;
    public float leftPadding = 10, rightPadding = 10;
    public float topPadding = 10, bottomPadding = 10;
    public float entryHeight = 30;
    float contentHeight;
    [HideInInspector] public List<HierarchyEntry> entries = new List<HierarchyEntry>();
    [HideInInspector] public HierarchyEntry root;
    public GameObject entryPrefab;
    [HideInInspector] public List<HierarchyEntry> currentSelectedEntries = new List<HierarchyEntry>();
    [HideInInspector] public HierarchyEntry currentSelectedEntry => currentSelectedEntries.Count == 1 ? currentSelectedEntries[0] : null;
    public UnityEvent onHierarchyChanged = new UnityEvent();
    void Awake()
    {
        rt = GetComponent<RectTransform>();
        if (content == null) content = GetComponent<RectTransform>();
    }
    void Start()
    {
        /*
        CreateEntry(null, "Root", null);
        CreateEntry(root, "Loop1", null);
        CreateEntry(root, "Loop2", null);
*/
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
        contentHeight = 0;
        UpdateHierarchy(root, null, 0, true);
        contentHeight += topPadding + bottomPadding;
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, contentHeight);
        onHierarchyChanged.Invoke();
    }
    void UpdateHierarchy(HierarchyEntry entry, HierarchyEntry parent,int depth, bool show)
    {
        entry.depth = depth;
        entry.parent = parent;
        entries.Add(entry);
        entry.SetVisibility(show);
        entry.UpdateUI();

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
        contentHeight = Mathf.Max(contentHeight, - entry.y);
        foreach (HierarchyEntry child in entry.children) {
            UpdateHierarchy(child, entry, depth + 1, show & entry.expanded);
        }
    }
    public HierarchyEntry CreateEntry(HierarchyEntry parent, string id, object bindObject)
    {
        HierarchyEntry newEntry = Instantiate(entryPrefab, content).GetComponent<HierarchyEntry>();
        newEntry.id = id;
        newEntry.hierarchy = this;
        newEntry.bindObject = bindObject;
        newEntry.expanded = false;
        //Debug.Log("CreateEntry: " + newEntry.id);

        RectTransform rt = newEntry.GetComponent<RectTransform>();

        if (parent != null) {
            rt.anchoredPosition = parent.GetComponent<RectTransform>().anchoredPosition;
        }

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
}
}