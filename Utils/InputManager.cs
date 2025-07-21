using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using BubbleZun.Utils;

public class InputManager : Singleton<InputManager>
{
    public UnityEvent OnLMBDown = new UnityEvent();
    Dictionary<KeyCode, UnityEvent> keyEvents = new Dictionary<KeyCode, UnityEvent>();
    bool IsEditingInputField()
    {
        GameObject current = EventSystem.current.currentSelectedGameObject;
        if (current == null) return false;
        return current.GetComponent<InputField>() != null || current.GetComponent<TMP_InputField>() != null;
    }
    void Start()
    {
        
    }
    bool IsHoldingCtrlOrShift()
    {
        return Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) || Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }
    void Update()
    {
        if (!IsEditingInputField() && !IsHoldingCtrlOrShift()) {
            foreach (var keyEvent in keyEvents) {
                if (Input.GetKeyDown(keyEvent.Key)) {
                    keyEvent.Value.Invoke();
                }
            }
        }
    }
    public void AddListener(KeyCode key, UnityAction callback)
    {
        if (!keyEvents.ContainsKey(key)) {
            keyEvents[key] = new UnityEvent();
        }
        keyEvents[key].RemoveListener(callback);
        keyEvents[key].AddListener(callback);
    }
    public void RemoveListener(KeyCode key, UnityAction callback)
    {
        if (keyEvents.ContainsKey(key)) {
            keyEvents[key].RemoveListener(callback);
        }
    }
}
