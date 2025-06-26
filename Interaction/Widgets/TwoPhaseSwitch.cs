using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleZun.Effects.HighlightEffects;
using UnityEngine.Events;
using BubbleZun.Interaction;
using BubbleZun.Utils;
namespace BubbleZun.Interaction
{
public class TwoPhaseSwitch : Interactable, ITwoPhase
{
    // Start is called before the first frame update
    public bool isOn = false;
    public UnityEvent onTurnOn = new UnityEvent();
    public UnityEvent onTurnOff = new UnityEvent();
    public List<IHighlightEffect> animatedEffects = new List<IHighlightEffect>();
    public bool ignoreRepeatedTrigger = true;
    public bool autoInitState{get; set;} = true;
    protected override void Start()
    {
        base.Start();
        if (autoInitState)
        {
            if (isOn) {
                isOn = false;
                TurnOn(false);
            }
            else {
                isOn = true;
                TurnOff(false);
            }
        }
    }
    public void Toggle()
    {
        if (interactionObject != null && !interactionObject.IsInteractable()) return;
        if (isOn) TurnOff();
        else TurnOn();
    }
    public void TurnOn(bool animated = true)
    {
        //if (interactionObject != null && !interactionObject.IsInteractable()) return;
        if (ignoreRepeatedTrigger && isOn) return;
        isOn = true;
        onTurnOn.Invoke();
        BDebug.Log("[" + gameObject.name + "] TurnOn");
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
        //if (interactionObject != null && !interactionObject.IsInteractable()) return;
        if (ignoreRepeatedTrigger && !isOn) return;
        isOn = false;
        onTurnOff.Invoke();
        BDebug.Log("[" + gameObject.name + "] TurnOff");
        if (animated)
        {
            foreach (var effect in animatedEffects)
            {
                effect.Unhighlight();
            }
        }
    }
    public void ReplayCurrentState(bool animated = true)
    {
        if (isOn) TurnOn(animated);
        else TurnOff(animated);
    }
}
}