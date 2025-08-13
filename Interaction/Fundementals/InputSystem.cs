using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using JetBrains.Annotations;
using UnityEngine.EventSystems;
using System.Linq;
using BubbleZun.Utils;
namespace BubbleZun.Interaction{
    public class InputSystem : Singleton<InputSystem>
    {
        // Start is called before the first frame update
        static public InputSystem instance;
        Dictionary<KeyCode, List<InputEvent>> keyEvents = new Dictionary<KeyCode, List<InputEvent>>();
        List<InputEvent>[] MouseClickEvents = new List<InputEvent>[2]{new List<InputEvent>(), new List<InputEvent>()};

        // Update is called once per frame
        void Update()
        {
            foreach (var (key, evts) in keyEvents)
            {
                if (!Input.GetKeyDown(key)) continue;
                foreach (var evt in evts)
                {
                    if (!evt.active) continue;
                    evt.action();
                    if (evt.stopPropagation) break;
                }
            }
            if (Input.GetMouseButtonDown(0)) OnMouseClick(0);
            if (Input.GetMouseButtonDown(1)) OnMouseClick(1);
        }
        public void OnMouseClick(int button){
            List<RaycastResult> results = new List<RaycastResult>();
            PointerEventData eventData = new PointerEventData(EventSystem.current){
                position = Input.mousePosition
            };
            EventSystem.current.RaycastAll(eventData, results);
            string target = "";
            foreach (var result in results)
            {
                target += result.gameObject.name + " ";
            }
            Debug.Log("raycast result: " + target);
            List<InputEvent> events = new List<InputEvent>(MouseClickEvents[button]);
            foreach (var evt in events)
            {
                if (!evt.active || !results.Any(r => r.gameObject == evt.target)) continue;
                evt.action();
                if (evt.stopPropagation) break;
            }
        }
        static public void AddMouseClickEvent(int button, InputEvent evt){
            Instance._addMouseClickEvent(button, evt);
        }
        static public void RemoveMouseClickEvent(int button, InputEvent evt){
            Instance._removeMouseClickEvent(button, evt);
        }
        public void _addMouseClickEvent(int button, InputEvent evt){
            if (MouseClickEvents[button].Contains(evt)) return;
            MouseClickEvents[button].Add(evt);
            MouseClickEvents[button].Sort((a, b) => a.priority.CompareTo(b.priority));
        }
        public void _removeMouseClickEvent(int button, InputEvent evt){
            if (MouseClickEvents[button].Contains(evt)) return;
            MouseClickEvents[button].Remove(evt);
        }
        static public void AddKeyEvent(KeyCode key, InputEvent evt){
            Instance._addKeyEvent(key, evt);
        }
        static public void RemoveKeyEvent(KeyCode key, InputEvent evt){
            Instance._removeKeyEvent(key, evt);
        }
        public void _addKeyEvent(KeyCode key, InputEvent evt){
            if (!keyEvents.ContainsKey(key)) keyEvents[key] = new List<InputEvent>();
            if (keyEvents[key].Contains(evt)) return;
            keyEvents[key].Add(evt);
            keyEvents[key].Sort((a, b) => a.priority.CompareTo(b.priority));
        }
        public void _removeKeyEvent(KeyCode key, InputEvent evt){
            if (!keyEvents.ContainsKey(key)) return;
            keyEvents[key].Remove(evt);
        }
    }
    public class InputEvent{
        public int priority;
        public Action action;
        public bool stopPropagation;
        public bool active = true;
        public GameObject target;
        public InputEvent(Action action, GameObject target = null, int priority = 0, bool stopPropagation = false){
            this.priority = priority;
            this.action = action;
            this.stopPropagation = stopPropagation;
            this.target = target;
        }
        public void Enable() => active = true;
        public void Disable() => active = false;
        public void SetPriority(int priority) => this.priority = priority;
    }
}