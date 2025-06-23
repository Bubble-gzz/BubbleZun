using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BubbleZun.Effects.AnimationEffects;
using UnityEngine.Events;
using TMPro;
namespace BubbleZun.Interaction
{
public class HierarchyEntry : MonoBehaviour
{
    // Start is called before the first frame update
    public Hierarchy hierarchy;
    public object bindObject;
    public int depth;
    public float y;
    public float _y => rectTransform.localPosition.y;
    public bool expanded;
    public bool show{get; private set;}
    public HierarchyEntry parent;
    public HierarchyEntry prev, next;
    public List<HierarchyEntry> children = new List<HierarchyEntry>();
    public bool hasChildren => children.Count > 0;
    public bool isRoot => hierarchy?.root == this;
    InteractionObject interactionObject;

    TweenAlphaEffect alphaEffect;
    [SerializeField] TweenRotationEffect expandIcon;
    [SerializeField] TweenAlphaEffect expandIconAlpha;

    public string id{
        get => text.text;
        set => text.text = value;
    }
    public TMP_Text text;

    void Awake()
    {
        if (selectState == null) selectState = GetComponent<ITwoPhase>();
        rectTransform = GetComponent<RectTransform>();
        if (alphaEffect == null) alphaEffect = GetComponent<TweenAlphaEffect>();
    }
    void Start()
    {
        if (expandIcon != null) expandIcon.SetRotation(expanded ? 1 : 0);        
    }

    // Update is called once per frame
    RectTransform rectTransform;
    bool lastShowState = false;
    void Update()
    {
        Vector2 targetPos = new Vector2(rectTransform.anchoredPosition.x, y);
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPos, Time.deltaTime * 10);
        if (lastShowState != show)
        {
            if (interactionObject == null) interactionObject = GetComponent<InteractionObject>();
            if (interactionObject != null) {
                if (show) interactionObject.Enable();
                else interactionObject.Disable();
            }
            lastShowState = show;
        }
        if (isDragging) {
            if (Input.GetMouseButtonUp(0)) {
                isDragging = false;
                hierarchy.currentDraggingEntry = null;
            }
        }
    }
    public void SetVisibility(bool visible)
    {
        if (visible == show) return;
        show = visible;
        if (alphaEffect != null) alphaEffect.TweenAlpha(visible ? 1 : 0);
    }

    bool selected = false;
    ITwoPhase selectState;
    public void Select(bool exluded = true)
    {
        if (exluded)
        {
            if (selected) return;
            selected = true;
            TurnOn();
            HierarchyEntry lastSelectedEntry = hierarchy.currentSelectedEntry;
            if (lastSelectedEntry != null) {
                lastSelectedEntry.TurnOff();
                lastSelectedEntry.selected = false;
            }
            hierarchy.ClearSelectedEntries();
            hierarchy.AddSelectedEntry(this);
        }
        else {
            selected = !selected;
            if (selected) {
                TurnOn();
                hierarchy.AddSelectedEntry(this);
            }
            else {
                TurnOff();
                hierarchy.RemoveSelectedEntry(this);
            }
        }
    }
    public void TurnOff()
    {
        selectState.TurnOff();
    }
    public void TurnOn()
    {
        selectState.TurnOn();
    }
    public void ToggleExpand()
    {
        expanded = !expanded;
        if (expandIcon != null)
        {
            expandIcon.TweenRotation(expanded ? 1 : 0);
        }
        hierarchy.UpdateHierarchy();
    }
    public void UpdateUI()
    {
        if (expandIconAlpha != null) 
        {
            expandIconAlpha.TweenAlpha(children.Count > 0 ? 1 : 0);
        }
        if (expandIcon != null)
        {
            expandIcon.TweenRotation(expanded ? 1 : 0);
        }
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        DOVirtual.Float(rt.offsetMin.x, hierarchy.leftPadding + hierarchy.indent * depth, 0.2f, (x) => rt.offsetMin = new Vector2(x, rt.offsetMin.y));
        DOVirtual.Float(rt.offsetMax.x, -hierarchy.rightPadding, 0.2f, (x) => rt.offsetMax = new Vector2(x, rt.offsetMax.y));
        DOVirtual.Float(rt.sizeDelta.y, hierarchy.entryHeight, 0.2f, (y) => rt.sizeDelta = new Vector2(rt.sizeDelta.x, y));
    }
    bool isDragging = false;
    public void DragStart()
    {
        hierarchy.currentDraggingEntry = this;
        isDragging = true;
    }
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
        hierarchy.UpdateHierarchy();
        //Debug.Log("Destroy HierarchyEntry: " + id);
        Destroy(gameObject);
    }
}
}