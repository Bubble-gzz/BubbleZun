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
public class HierarchyEntryRenderer : MonoBehaviour, IObjectPoolable
{
    // Start is called before the first frame update
    public ObjectPool pool{get; set;}
    Hierarchy hierarchy => entry?.hierarchy;
    public HierarchyEntry entry;
    int depth => entry?.depth ?? 0;
    bool expanded{get => entry?.expanded ?? false; set{if (entry != null) entry.expanded = value;}}
    bool hasChildren => entry?.hasChildren ?? false;
    public float y => entry?.y ?? 0;

    public bool show{get => entry?.visible ?? false; set{if (entry != null) entry.visible = value;}}
    InteractionObject interactionObject;

    TweenAlphaEffect alphaEffect;
    [SerializeField] TweenRotationEffect expandIcon;
    [SerializeField] TweenAlphaEffect expandIconAlpha;


    public TMP_Text text;

    void Awake()
    {
        if (selectState == null) selectState = GetComponent<ITwoPhase>();
        rectTransform = GetComponent<RectTransform>();
        if (alphaEffect == null) alphaEffect = GetComponent<TweenAlphaEffect>();
    }
    public void Init()
    {
        RectTransform parentRT = entry?.parent?.renderer?.rectTransform;
        if (parentRT != null) rectTransform.anchoredPosition = parentRT.anchoredPosition;
        UpdateUI(false);
        UpdateVisibility(true);
    }

    // Update is called once per frame
    RectTransform rectTransform;
    bool lastShowState = false;
    void Update()
    {
        Vector2 targetPos = new Vector2(rectTransform.anchoredPosition.x, y);
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPos, Time.deltaTime * 10);
        UpdateVisibility();
        UpdateDragState();
        text.text = entry.id;
    }
    public void UpdateVisibility(bool force = false)
    {
        if (!force && lastShowState == show) return;
        lastShowState = show;
        if (alphaEffect != null) alphaEffect.TweenAlpha(show ? 1 : 0);
        if (interactionObject == null) interactionObject = GetComponent<InteractionObject>();
        if (interactionObject != null) {
            if (show) interactionObject.Enable();
            else interactionObject.Disable();
        }
    }
    ITwoPhase selectState;
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
    public void UpdateUI(bool animated = true)
    {
        if (expandIconAlpha != null) 
        {
            int f = hasChildren ? 1 : 0;
            //BDebug.Log("UpdateUI: " + entry.id + " " + f);
            if (animated) expandIconAlpha.TweenAlpha(f);
            else expandIconAlpha.SetAlpha(f);
        }
        if (expandIcon != null)
        {
            int r = expanded ? 1 : 0;
            if (animated) expandIcon.TweenRotation(r);
            else expandIcon.SetRotation(r);
        }
        RectTransform rt = GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 1);
        rt.anchorMax = new Vector2(1, 1);
        if (animated) {
            DOVirtual.Float(rt.offsetMin.x, hierarchy.leftPadding + hierarchy.indent * depth, 0.2f, (x) => rt.offsetMin = new Vector2(x, rt.offsetMin.y));
            DOVirtual.Float(rt.offsetMax.x, -hierarchy.rightPadding, 0.2f, (x) => rt.offsetMax = new Vector2(x, rt.offsetMax.y));
            DOVirtual.Float(rt.sizeDelta.y, hierarchy.entryHeight, 0.2f, (y) => rt.sizeDelta = new Vector2(rt.sizeDelta.x, y));
        }
        else {
            rt.offsetMin = new Vector2(hierarchy.leftPadding + hierarchy.indent * depth, rt.offsetMin.y);
            rt.offsetMax = new Vector2(-hierarchy.rightPadding, rt.offsetMax.y);
            rt.sizeDelta = new Vector2(rt.sizeDelta.x, hierarchy.entryHeight);
        }
        if (!animated) {
            Vector2 targetPos = new Vector2(rectTransform.anchoredPosition.x, y);
            rectTransform.anchoredPosition = targetPos;
        }
       
        if (entry?.selected ?? false) TurnOn();
        else TurnOff();
    }
    bool isDragging = false;
    bool beforeDragStart = false;
    float beforeDragCountdown = 0;
    public void OnClick()
    {
        if (!isDragging) {
            beforeDragStart = true;
            beforeDragCountdown = 0.2f;
        }
    }
    void UpdateDragState()
    {
        if (beforeDragStart)
        {
            if (Input.GetMouseButtonUp(0))
            {
                beforeDragStart = false;
            }
            else {
                beforeDragCountdown -= Time.deltaTime;
                if (beforeDragCountdown <= 0) {
                    beforeDragStart = false;
                    DragStart();
                }
            }
        }
        if (isDragging) {
            if (Input.GetMouseButtonUp(0)) {
                isDragging = false;
                hierarchy.currentDraggingEntry = null;
            }
        }    
    }
    void DragStart()
    {
        hierarchy.currentDraggingEntry = entry;
        isDragging = true;
    }
    public void Recycle()
    {
        StartCoroutine(RecycleCoroutine());
    }
    IEnumerator RecycleCoroutine()
    {
        yield return new WaitForSeconds(0.2f);
        pool.Recycle(gameObject);
    }
}
}