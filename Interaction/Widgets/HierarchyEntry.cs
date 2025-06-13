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
    public bool expanded;
    public bool show;
    public HierarchyEntry parent;
    public HierarchyEntry prev;
    public List<HierarchyEntry> children = new List<HierarchyEntry>();
    public bool hasChildren => children.Count > 0;

    public UnityEvent<bool> onVisibilityChanged = new UnityEvent<bool>();
    
    public string id{
        get => text.text;
        set => text.text = value;
    }
    public TMP_Text text;

    void Awake()
    {
        if (selectState == null) selectState = GetComponent<ITwoPhase>();
        rectTransform = GetComponent<RectTransform>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    RectTransform rectTransform;
    void Update()
    {
        Vector2 targetPos = new Vector2(hierarchy.indent * depth, y);
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPos, Time.deltaTime * 10);
    }
    public void SetVisibility(bool visible)
    {
        if (visible == show) return;
        show = visible;
        onVisibilityChanged.Invoke(visible);
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

}
}