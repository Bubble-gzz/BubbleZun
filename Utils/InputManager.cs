using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputManager : Singleton<InputManager>
{
    public UnityEvent OnLMBDown = new UnityEvent();
    void Start()
    {
        
    }
}
