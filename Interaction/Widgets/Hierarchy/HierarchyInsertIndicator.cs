using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleZun.Effects.AnimationEffects;
using DG.Tweening;
using BubbleZun.Utils;
namespace BubbleZun.Interaction {
public class HierarchyInsertIndicator : MonoBehaviour
{   
    UndoSystem undoSystem => Hierarchy.undoSystem;
    public Hierarchy hierarchy;
    public HierarchyEntry InsertEntry;
    public bool InsertAfter;
    public float y;
    public TweenAlphaEffect alphaEffect;
    RectTransform rectTransform;
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        alphaEffect = GetComponent<TweenAlphaEffect>();
    }
    // Start is called before the first frame update
    void Start()
    {
        alphaEffect.SetAlpha(0);
    }
    void Update()
    {
        if (hierarchy.currentDraggingEntry != null) UpdatePosition();
        UpdateUI();
        if (Input.GetMouseButtonUp(0) && hierarchy.currentDraggingEntry != null) {
            if (InsertAfter) {
                undoSystem?.Register(new MoveHierarchyEntryAfterOperation(hierarchy.currentDraggingEntry, InsertEntry));
                hierarchy.MoveEntryAfter(hierarchy.currentDraggingEntry, InsertEntry);
            } else {
                undoSystem?.Register(new SetHierarchyEntryParentOperation(hierarchy.currentDraggingEntry, InsertEntry));
                hierarchy.ChangeEntryParent(hierarchy.currentDraggingEntry, InsertEntry);
            }
        }
    }

    // Update is called once per frame
    bool lastShowState = false;
    bool show = false;
    void UpdatePosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(hierarchy.content, Input.mousePosition, null, out Vector2 localPoint);
        float insertPos = localPoint.y;
        List<HierarchyEntry> entries = hierarchy.entries;
        for (int i = 0; i < entries.Count; i++) {
            //BDebug.Log("insertPos" + insertPos);
            HierarchyEntry entry = entries[i];
            //string debugLog = "entry._y: " + entry._y + " insertPos: " + insertPos + " spacing: " + hierarchy.spacing;
            //if (entry.next != null) debugLog += " next._y: " + entry.next._y;
            //Debug.Log(debugLog);
            if (Mathf.Abs(entry.y - insertPos) < hierarchy.spacing * 0.3f) {
                InsertEntry = entry;
                InsertAfter = false;
                y = entry.y;
                break;
            }
            if (entry.y > insertPos && (entry.next == null || insertPos > entry.next.y + hierarchy.spacing * 0.3f)) {
                InsertEntry = entry;
                InsertAfter = true;
                y = entry.y - hierarchy.spacing * 0.5f;
                break;
            }
        }
    }
    void UpdateUI()
    {
        show = hierarchy.currentDraggingEntry != null;
        if (show != lastShowState) {
            if (show) {
                y = hierarchy.currentDraggingEntry.y;
                rectTransform.anchoredPosition = new Vector2(rectTransform.anchoredPosition.x, y);
            }
            if (alphaEffect != null) {
                alphaEffect.TweenAlpha(show ? 1 : 0);
            }
            lastShowState = show;
        }
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, new Vector2(rectTransform.anchoredPosition.x, y), Time.deltaTime * 10);
    }
}
}