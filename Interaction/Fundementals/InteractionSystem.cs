using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleZun.Utils;
namespace BubbleZun.Interaction{
    public class InteractionSystem : Singleton<InteractionSystem>
    {
        InteractionObject currentFocusedObject;
        public static bool isBlockedByUI => Instance.blockingUIElements.Count > 0;
        List<Object> blockingUIElements = new List<Object>();
        public static void AddBlockingUIElement(Object uiElement){
            if (!Instance.blockingUIElements.Contains(uiElement)) Instance.blockingUIElements.Add(uiElement);
        }
        public static void RemoveBlockingUIElement(Object uiElement){
            if (Instance.blockingUIElements.Contains(uiElement)) Instance.blockingUIElements.Remove(uiElement);
        }
        public static void SetFocusedObject(InteractionObject newObject){
            InteractionObject _u, u; _u = u = newObject;
            InteractionObject _v, v; _v = v = Instance.currentFocusedObject;

            while (_u != null && !_u.IsFocused()) _u = _u.GetParent();
            while (u != _u) { u.SystemOperationOnly_GainFocus(); u = u.GetParent(); }
            if (_v != null)
            {
                while (_v != null && _v != _u) _v = _v.GetParent();
                while (v != _v) { v.SystemOperationOnly_LoseFocus(); v = v.GetParent(); }
            }
            Instance.currentFocusedObject = newObject;
        }
        public static void ReleaseFocusedObject(InteractionObject objToRelease){
            InteractionObject u = Instance.currentFocusedObject;
            while (u != null)
            {
                u.SystemOperationOnly_LoseFocus();
                if (u == objToRelease) break;
                u = u.GetParent();
            }
            Instance.currentFocusedObject = u.GetParent();
        }
        public static bool IsBlocked(InteractionObject interactionObject){
            if (interactionObject.IsFocused()) return false;
            InteractionObject currentNode = Instance.currentFocusedObject;
            while (currentNode != null)
            {
                if (IsBlockedByNode(interactionObject, currentNode)) return true;
                currentNode = currentNode.GetParent();
            }
            return false;
        }
        static bool IsBlockedByNode(InteractionObject node, InteractionObject focusedNode){
            if (!focusedNode.IsExclusive()) return false;
            while (node != null)
            {
                if (node == focusedNode) return false;
                if (node == focusedNode.ExclusiveRoot) return true;
                node = node.GetParent();
            }
            return false;
        }
    }
}