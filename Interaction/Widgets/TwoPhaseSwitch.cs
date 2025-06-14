using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleZun.Effects.HighlightEffects;
using UnityEngine.Events;
using BubbleZun.Interaction;
namespace BubbleZun.Interaction
{
public class TwoPhaseSwitch : Interactable, ITwoPhase
{
    // Start is called before the first frame update
    bool isOn = false;
    public UnityEvent onTurnOn = new UnityEvent();
    public UnityEvent onTurnOff = new UnityEvent();
    public List<IHighlightEffect> animatedEffects = new List<IHighlightEffect>();
    bool haveState = false;
    protected override void Start()
    {
        base.Start();
        if (!haveState)
        {
            TurnOff(false); //default state
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
        if (interactionObject != null && !interactionObject.IsInteractable()) return;
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
        if (interactionObject != null && !interactionObject.IsInteractable()) return;
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
    public void ReplayCurrentState(bool animated = true)
    {
        if (isOn) TurnOn(animated);
        else TurnOff(animated);
    }
}
}