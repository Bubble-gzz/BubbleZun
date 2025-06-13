using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleZun.Interaction;
using UnityEngine.Events;
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
    public Transform contentRoot;
    public int currentIndex{get; private set;}
    public UnityEvent<int> onSelect = new UnityEvent<int>();
    public bool mouseSelect = true;
    protected override void Start()
    {
        base.Start();
        if (contentRoot == null) contentRoot = transform;
        foreach (Transform child in contentRoot)
        {
            ITwoPhase twoPhase = child.GetComponent<ITwoPhase>();
            if (twoPhase != null) twoPhases.Add(twoPhase);

            if (mouseSelect)
            {
                MouseClickDetector clickDetector = child.GetComponent<MouseClickDetector>();
                if (clickDetector != null)
                {
                    clickDetector.onLMBClick.AddListener(() => Select(twoPhases.IndexOf(twoPhase)));
                }
            }
        }
        for (int i = 1; i < twoPhases.Count; i++)
        {
            twoPhases[i].TurnOff(false);
        }
        twoPhases[0].TurnOn(false);
        currentIndex = 0;
    }

    public void Select(int index, bool animated = true)
    {
        if (interactionObject != null && !interactionObject.IsInteractable()) return;
        if (index < 0 || index >= twoPhases.Count) return;
        if (index == currentIndex) return;
        twoPhases[currentIndex].TurnOff(animated);
        twoPhases[index].TurnOn(animated);
        currentIndex = index;
        onSelect.Invoke(index);
    }
}
}
