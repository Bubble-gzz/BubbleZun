using System.Collections;
using System.Collections.Generic;
using BubbleZun.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace BubbleZun.Interaction{
    public class MouseInteractable : Interactable
    {
        // Start is called before the first frame update
        public bool blockNonUIObject = true;
        public bool transparent = false;
        [HideInInspector] public bool useScreenSpace = false;

        public Collider2D detectArea;
        public List<GameObject> subObjects = new List<GameObject>();
        public UnityEvent onMouseEnter = new UnityEvent();
        public UnityEvent onMouseExit = new UnityEvent();
        protected override void Awake()
        {
            base.Awake();
            useScreenSpace = GetComponentInParent<Canvas>() != null;
            if (!useScreenSpace) blockNonUIObject = false;
            if (detectArea == null) detectArea = GetComponent<Collider2D>();
            if (!subObjects.Contains(gameObject)) subObjects.Add(gameObject);
        }
        bool IsInteractableLastFrame = false;
        protected override void Start()
        {
            base.Start();
            UpdateRaycastTarget();
        }
        protected virtual void Update()
        {
            if (IsInteractableLastFrame != IsInteractable())
            {
                UpdateRaycastTarget();
                IsInteractableLastFrame = IsInteractable();
            }
        }
        void UpdateRaycastTarget(){
            foreach (var obj in subObjects)
            {
                foreach (var graphic in obj.GetComponents<Graphic>())
                {
                    graphic.raycastTarget = IsInteractable();
                }
            }
        }
        public override void CopySettings(Interactable interactable){
            base.CopySettings(interactable);
            if (interactable is MouseInteractable mouseInteractable) {
                blockNonUIObject = mouseInteractable.blockNonUIObject;
                useScreenSpace = mouseInteractable.useScreenSpace;
                transparent = mouseInteractable.transparent;
                detectArea = mouseInteractable.detectArea;
                onMouseEnter = mouseInteractable.onMouseEnter;
                onMouseExit = mouseInteractable.onMouseExit;
            }
        }
    }
}