using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleZun.Interaction;
using UnityEngine.Events;
using BubbleZun.Utils;
namespace BubbleZun.Interaction
{
public interface ITwoPhase{
    void TurnOn(bool animated = true);
    void TurnOff(bool animated = true);
}
public class GroupSelector : Interactable
{
    // Start is called before the first frame update
    [SerializeField] List<ITwoPhase> twoPhases = new List<ITwoPhase>();
    List<bool> selected = new List<bool>();
    public Transform contentRoot;
    public int currentIndex{get; private set;}
    public UnityEvent<int> onSelect = new UnityEvent<int>();
    public UnityEvent<int> onDeselect = new UnityEvent<int>();
    public bool defaultMouseSelect = true;
    protected override void Start()
    {
        base.Start();
        if (contentRoot == null) contentRoot = transform;
        foreach (Transform child in contentRoot)
        {
            ITwoPhase twoPhase = child.GetComponent<ITwoPhase>();
            if (twoPhase != null) twoPhases.Add(twoPhase);

            if (defaultMouseSelect)
            {
                MouseClickDetector clickDetector = child.GetComponent<MouseClickDetector>();
                if (clickDetector != null)
                {
                    clickDetector.onLMBClick.AddListener(() => Select(twoPhases.IndexOf(twoPhase)));
                }
            }
            selected.Add(false);
        }
        for (int i = 1; i < twoPhases.Count; i++)
        {
            twoPhases[i].TurnOff(false);
        }
        twoPhases[0].TurnOn(false);
        selected[0] = true;
        currentIndex = 0;
    }
    public void Select(int index)
    {
        Select(index, true);
    }
    void Select(int index, bool animated = true)
    {
        BDebug.Log("Select: " + index);
        if (interactionObject != null && !interactionObject.IsInteractable()) {
            BDebug.Log("Not interactable");
            return;
        }
        if (index < 0 || index >= twoPhases.Count) {
            BDebug.Log("Index out of range");
            return;
        }
        for (int i = 0; i < twoPhases.Count; i++)
        {
            if (i == index) continue;
            if (selected[i]) TurnOff(i, animated);
        }
        TurnOn(index, animated);
        currentIndex = index;
    }
    public void Toggle(int index)
    {
        if (selected[index]) TurnOff(index);
        else TurnOn(index);
    }
    public void TurnOn(int index)
    {
        TurnOn(index, true);
    }
    public void TurnOff(int index)
    {
        TurnOff(index, true);
    }
    void TurnOn(int index, bool animated = true)
    {
        BDebug.Log("TurnOn: " + index);
        if (index < 0 || index >= twoPhases.Count) return;
        twoPhases[index].TurnOn(animated);
        selected[index] = true;
        onSelect.Invoke(index);
    }
    void TurnOff(int index, bool animated = true)
    {
        BDebug.Log("TurnOff: " + index);
        if (index < 0 || index >= twoPhases.Count) return;
        twoPhases[index].TurnOff(animated);
        selected[index] = false;
        onDeselect.Invoke(index);
    }
}
}
