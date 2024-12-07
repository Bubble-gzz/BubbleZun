using UnityEngine;
using System.Collections.Generic;
namespace BubbleZun.Interaction{
    public class Interactable : MonoBehaviour
    {
        public bool bindInteractionObject = true;

        public InteractionObject interactionObject;
        public bool claimFocusOnInteraction = true;
        protected virtual void Awake()
        {
            if (bindInteractionObject) {
                if (interactionObject == null) interactionObject = GetComponent<InteractionObject>();
                if (interactionObject == null) interactionObject = gameObject.AddComponent<InteractionObject>();
            }
            else {
                interactionObject = null;
                claimFocusOnInteraction = false;
            }
        }
        protected virtual void Start(){
            GenerateComponents();
        }
        protected bool generateComponentsDone = false;
        public virtual void GenerateComponents(){
            if (generateComponentsDone) return;
            generateComponentsDone = true;
        }
        virtual public void CopySettings(Interactable interactable)
        {
            interactionObject = interactable.interactionObject;
        }
        protected bool IsInteractable()
        {
            if (!bindInteractionObject) return true;
            bool interactable = true;
            InteractionObject current = interactionObject;
            while (current != null)
            {
                if (!current.IsInteractable()) {
                    interactable = false;
                    break;
                }
                current = current.GetParent();
            }
            return interactable;
        }
        protected T InstallComponent<T>(ref T component) where T : Interactable
        {
            if (component == null) {
                component = gameObject.GetComponent<T>();
                if (component == null) component = gameObject.AddComponent<T>();
            }
            component.claimFocusOnInteraction = false;
            component.CopySettings(this);
            component.GenerateComponents();
            return component;
        }
    }
}
