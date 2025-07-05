using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
namespace BubbleZun.Interaction.Widgets
{
public class RouletteSelector : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] List<string> options;
    public UnityEvent<string> onSelect;
    int selectedIndex = 0;
    public int defaultIndex = 0;
    void Awake()
    {
        if (defaultIndex >= 0 && defaultIndex < options.Count)
            Select(defaultIndex);
    }
    public void TurnLeft()
    {
        selectedIndex--;
        if (selectedIndex < 0) selectedIndex = options.Count - 1;
        UpdateSelectedItem();
    }
    public void TurnRight()
    {
        selectedIndex++;
        if (selectedIndex >= options.Count) selectedIndex = 0;
        UpdateSelectedItem();
    }
    void UpdateSelectedItem()
    {
        text.text = options[selectedIndex];
        onSelect.Invoke(options[selectedIndex]);
    }
    public void AddOption(string option)
    {
        options.Add(option);
    }
    public void Select(int index)
    {
        if (index < 0 || index >= options.Count) return;
        selectedIndex = index;
        UpdateSelectedItem();
    }
}
}