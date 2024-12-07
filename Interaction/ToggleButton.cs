using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace BubbleZun.Interaction{
    public class ToggleButton : MouseInteractable, IMouseClick
    {
        public bool status = false;
        public UnityEvent onTurnOn;
        public UnityEvent onTurnOff;
        MouseClickDetector mouseClickDetector;
        public UnityEvent onLMBClick = new UnityEvent();
        public UnityEvent onRMBClick = new UnityEvent();
        public UnityEvent OnLMBClick => onLMBClick;
        public UnityEvent OnRMBClick => onRMBClick;
        public override void GenerateComponents(){
            if (generateComponentsDone) return;
            base.GenerateComponents();
            InstallComponent(ref mouseClickDetector);
            mouseClickDetector.onLMBClick.AddListener(Toggle);
        }
        protected override void Start(){
            base.Start();
            if (status) onTurnOn.Invoke();
            else onTurnOff.Invoke();
        }
        void Toggle(){
            if (claimFocusOnInteraction) interactionObject?.ClaimFocus();
            status = !status;
            if (status) onTurnOn.Invoke();
            else onTurnOff.Invoke();
        }
    }
}
