using UnityEngine;
using UnityEngine.Events;
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

        public override void GenerateComponents(){
            if (generateComponentsDone) return;
            base.GenerateComponents();
            InstallComponent(ref mouseDetector);
        }
        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            if (!IsInteractable()) return;
            if (!mouseDetector.isMouseOver) return;
            if (Input.GetMouseButtonDown(0)) {
                onLMBClick.Invoke();
                if (interactionObject != null) {
                    if (claimFocusOnInteraction) interactionObject.ClaimFocus();
                }
            }
            if (Input.GetMouseButtonDown(1)) {
                onRMBClick.Invoke();
                if (interactionObject != null) {
                    if (claimFocusOnInteraction) interactionObject.ClaimFocus();
                }
            }
        }
        public override void CopySettings(Interactable interactable){
            base.CopySettings(interactable);
            if (interactable is IMouseClick mouseClick){
                onLMBClick = mouseClick.OnLMBClick;
                onRMBClick = mouseClick.OnRMBClick;
            }
        }
    }
}