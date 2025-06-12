using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleZun.Effects.HighlightEffects;
using UnityEngine.Events;
public class TwoPhaseSwitch : MonoBehaviour
{
    // Start is called before the first frame update
    bool isOn = false;
    public UnityEvent onTurnOn;
    public UnityEvent onTurnOff;
    public List<IHighlightEffect> animatedEffects;
    bool haveState = false;
    void Start()
    {
        if (!haveState)
        {
            TurnOff(false); //default state
        }
    }
    public void Toggle()
    {
        if (isOn) TurnOff();
        else TurnOn();
    }
    public void TurnOn(bool animated = true)
    {
        haveState = true;
        isOn = true;
        onTurnOn.Invoke();
        if (animated)
        {
            foreach (var effect in animatedEffects)
            {
                effect.Highlight();
            }
        }
    }
    public void TurnOff(bool animated = true)
    {
        haveState = true;
        isOn = false;
        onTurnOff.Invoke();
        if (animated)
        {
            foreach (var effect in animatedEffects)
            {
                effect.Unhighlight();
            }
        }
    }
}
