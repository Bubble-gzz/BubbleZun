using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleZun.Interaction;
using UnityEngine.Events;
using BubbleZun.Utils;
namespace BubbleZun.Interaction
{
public interface ITwoPhase{
    bool autoInitState{get; set;}
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
    public int defaultIndex = 0;
    public bool ignoreRepeatedSelect = true;
    protected override void Start()
    {
        base.Start();
        if (contentRoot == null) contentRoot = transform;
        foreach (Transform child in contentRoot)
        {
            ITwoPhase twoPhase = child.GetComponent<ITwoPhase>();
            if (twoPhase != null) twoPhases.Add(twoPhase);
            twoPhase.autoInitState = false;

            if (defaultMouseSelect)
            {
                MouseClickDetector clickDetector = child.GetComponent<MouseClickDetector>();
                if (clickDetector == null) clickDetector = child.gameObject.AddComponent<MouseClickDetector>();
                if (clickDetector != null)
                {
                    clickDetector.onLMBClick.AddListener(() => Select(twoPhases.IndexOf(twoPhase)));
                }
            }
            selected.Add(false);
        }
        Select(defaultIndex);
    }
    public void Select(int index)
    {
        Select(index, true);
    }
    void Select(int index, bool animated = true)
    {
        for (int i = 0; i < twoPhases.Count; i++)
        {
            if (i == index) continue;
            TurnOff(i, animated);
        }
        BDebug.Log("[" + gameObject.name + "] Select: " + index);
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
        if (index < 0 || index >= twoPhases.Count) {
            BDebug.Log("Index out of range");
            return;
        }
        if (ignoreRepeatedSelect && selected[index]) return;
        twoPhases[index].TurnOn(animated);
        selected[index] = true;
        onSelect.Invoke(index);
    }
    void TurnOff(int index, bool animated = true)
    {
        if (index < 0 || index >= twoPhases.Count) return;
        if (ignoreRepeatedSelect && !selected[index]) return;
        twoPhases[index].TurnOff(animated);
        selected[index] = false;
        onDeselect.Invoke(index);
    }
}
}
