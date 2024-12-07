using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using BubbleZun.Effects.FocusEffects;
namespace BubbleZun.Interaction{
    public class InteractionObject : MonoBehaviour
    {
        public enum InteractionState
        {
            Inactive,
            Active,
            Focused,
            Exclusive
        }

        [SerializeField] private InteractionState currentState = InteractionState.Active;
        [SerializeField] private InteractionObject _parent;
        [SerializeField] private InteractionObject _exclusiveRoot;

        public InteractionObject Parent
        {
            get => GetParent();
            set => _parent = value;
        }

        public InteractionObject ExclusiveRoot
        {
            get
            {
                if (_exclusiveRoot == null)
                {
                    _exclusiveRoot = FindTopmostInteractionObject();
                }
                return _exclusiveRoot;
            }
            set => _exclusiveRoot = value;
        }
        List<IFocusEffect> focusEffects = new List<IFocusEffect>();

        private void Awake()
        {
            if (_exclusiveRoot == null)
            {
                _exclusiveRoot = FindTopmostInteractionObject();
            }
            focusEffects = GetComponents<IFocusEffect>().ToList();
        }
        void Start(){
            SystemOperationOnly_LoseFocus();
        }
        private InteractionObject FindTopmostInteractionObject()
        {
            InteractionObject topmost = this;
            Transform current = transform.parent;
            while (current != null)
            {
                var parentInteractionObject = current.GetComponent<InteractionObject>();
                if (parentInteractionObject != null)
                {
                    topmost = parentInteractionObject;
                }
                current = current.parent;
            }

            return topmost;
        }

        public UnityEvent onFocusGained = new UnityEvent();
        public UnityEvent onFocusLost = new UnityEvent();
        public bool IsInteractable()
        {
            if (InteractionSystem.IsBlocked(this)) return false;
            return currentState != InteractionState.Inactive;
        }
        public bool IsFocused()
        {
            return currentState == InteractionState.Focused || currentState == InteractionState.Exclusive;
        }
        public bool IsExclusive()
        {
            return currentState == InteractionState.Exclusive;
        }
        public InteractionObject GetParent()
        {
            if (_parent == null) _parent = transform.parent?.GetComponentInParent<InteractionObject>();
            return _parent;
        }
        public void SystemOperationOnly_GainFocus()
        {
            Debug.Log(this.name + " Gaining focus");
            onFocusGained.Invoke();
            currentState = InteractionState.Focused;
            foreach (var effect in focusEffects)
            {
                effect.Focus();
            }
        }
        public void SystemOperationOnly_LoseFocus()
        {
            onFocusLost.Invoke();
            currentState = InteractionState.Active;
            foreach (var effect in focusEffects)
            {
                effect.Unfocus();
            }
        }
        public void ClaimFocus()
        {
            Debug.Log(this.name + " Claiming focus");
            InteractionSystem.SetFocusedObject(this);
        }
        public void ReleaseFocus()
        {
            if (!IsFocused()) return;
            InteractionSystem.ReleaseFocusedObject(this);
        }
        public void SetExclusive()
        {
            if (currentState != InteractionState.Focused) return; 
            currentState = InteractionState.Exclusive;
        }
        public void QuitExclusive()
        {
            if (currentState != InteractionState.Exclusive) return;
            currentState = InteractionState.Focused;
        }
        public void Disable(){
            ReleaseFocus();
            currentState = InteractionState.Inactive;
        }
        public void Enable(){
            currentState = InteractionState.Active;
        }
    }
}
