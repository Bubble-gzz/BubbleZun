using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;
using BubbleZun.Effects.HighlightEffects;
namespace BubbleZun.Interaction{
    public class MouseDetector : MouseInteractable, IPointerEnterHandler, IPointerExitHandler
    {

        public bool isMouseOver = false;

        public List<IHighlightEffect> highlightEffects = new List<IHighlightEffect>();
        RectTransform rectTransform;
        protected override void Start(){
            base.Start();
            List<IHighlightEffect> effects = GetComponents<IHighlightEffect>().ToList();
            foreach (var effect in effects)
                if (!highlightEffects.Contains(effect)) highlightEffects.Add(effect);
            if (useScreenSpace) rectTransform = GetComponent<RectTransform>();
        }

        // Update is called once per frame
        bool isMouseOverLastFrame = false;

        protected override void Update()
        {
            base.Update();
            //if (name == "SettingIcon") Debug.Log("[Time: " + Time.time + "] currentFocusedObject: " + InteractionSystem.Instance.currentFocusedObject);
            if (!IsAreaActive()) isMouseOver = false;
            else {
                if (transparent) TransparentAreaCheck();
            }
            StatusCheck();
        }
        void StatusCheck()
        {
            if (isMouseOver != isMouseOverLastFrame)
            {
                SetHighLight(isMouseOver);
                if (isMouseOver && IsInteractable()) {
                    onMouseEnter.Invoke();
                    if (blockNonUIObject) InteractionSystem.AddBlockingUIElement(this);
                    if (claimFocusOnInteraction) interactionObject.ClaimFocus();
                }
                else {
                    onMouseExit.Invoke();
                    if (blockNonUIObject) InteractionSystem.RemoveBlockingUIElement(this);
                }
                isMouseOverLastFrame = isMouseOver;
            }
        }
        void SetHighLight(bool isHighLight)
        {
            foreach (var effect in highlightEffects)
            {
                if (isHighLight) effect.Highlight();
                else effect.Unhighlight();
            }
        }
        void UpdateStatus(bool isMouseOver)
        {
            if (!IsAreaActive()) return;
            this.isMouseOver = isMouseOver;
        }
        bool IsAreaActive()
        {
            if (!useScreenSpace && InteractionSystem.isBlockedByUI) return false;
            return IsInteractable();
        }

        void TransparentAreaCheck()
        {
            if (!useScreenSpace)
            {
                if (detectArea == null) {
                    Debug.LogError("Detect area is not set for " + name);
                    return;
                }
                Vector2 mousePosition = Input.mousePosition;
                if (!useScreenSpace) mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
                UpdateStatus(detectArea.OverlapPoint(mousePosition));
            }
            else 
            {
                Vector2 localPoint;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    rectTransform,
                    Input.mousePosition,
                    null,
                    out localPoint
                );
                UpdateStatus(rectTransform.rect.Contains(localPoint));
            }
        }
        public void OnMouseEnter() { if (transparent) return; UpdateStatus(true); }
        public void OnMouseExit() { if (transparent) return; UpdateStatus(false); }
        public void OnPointerEnter(PointerEventData eventData) { if (transparent) return; UpdateStatus(true); }
        public void OnPointerExit(PointerEventData eventData) { if (transparent) return; UpdateStatus(false); }
    }
}
