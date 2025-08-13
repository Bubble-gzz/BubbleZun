using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
namespace BubbleZun.Interaction{
    interface IMouseClick{
        public UnityEvent OnLMBClick { get; }
        public UnityEvent OnRMBClick { get; }
    }
    public class MouseClickDetector : MouseInteractable, IMouseClick
    {
        public UnityEvent onLMBClick = new UnityEvent();
        public UnityEvent onRMBClick = new UnityEvent();
        public UnityEvent OnLMBClick => onLMBClick;
        public UnityEvent OnRMBClick => onRMBClick;
        MouseDetector mouseDetector;
        public int priority = 0;
        public bool blockRaycast = false;
        InputEvent lmbEvent;
        InputEvent rmbEvent;
        protected override void Awake()
        {
            base.Awake();
            lmbEvent = new InputEvent(() => OnPointerClick(0), gameObject, priority, blockRaycast);
            rmbEvent = new InputEvent(() => OnPointerClick(1), gameObject, priority, blockRaycast);
            InputSystem.AddMouseClickEvent(0, lmbEvent);
            InputSystem.AddMouseClickEvent(1, rmbEvent);
        }
        protected override void Update()
        {
            base.Update();
            lmbEvent.SetPriority(priority);
            lmbEvent.stopPropagation = blockRaycast;
            rmbEvent.SetPriority(priority);
            rmbEvent.stopPropagation = blockRaycast;
        }
        public override void GenerateComponents(){
            if (generateComponentsDone) return;
            base.GenerateComponents();
            InstallComponent(ref mouseDetector);
        }
        public override void CopySettings(Interactable interactable){
            base.CopySettings(interactable);
            if (interactable is IMouseClick mouseClick){
                onLMBClick = mouseClick.OnLMBClick;
                onRMBClick = mouseClick.OnRMBClick;
            }
        }
        public void OnPointerClick(int button)
        {
            if (!IsInteractable()) return;
            if (button == 0) {
                onLMBClick.Invoke();
            }
            else if (button == 1) {
                onRMBClick.Invoke();
            }
            if (interactionObject != null) {
                if (claimFocusOnInteraction) interactionObject.ClaimFocus();
            }
        }
    }
}