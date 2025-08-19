using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
namespace BubbleZun.Interaction.Widgets
{
public class RouletteSelector : MonoBehaviour
{
    public static string mixedOption = "-";
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
        if (selectedIndex == -1) selectedIndex = 0;
        else {
            selectedIndex--;
            if (selectedIndex < 0) selectedIndex = options.Count - 1;
        }
        UpdateSelectedItem();
    }
    public void TurnRight()
    {
        if (selectedIndex == -1) selectedIndex = 0;
        else {
            selectedIndex++;
            if (selectedIndex >= options.Count) selectedIndex = 0;
        }
        UpdateSelectedItem();
    }

    void UpdateSelectedItem(bool mute = false)
    {
        if (selectedIndex == -1) {
            text.text = mixedOption;
            if (!mute) onSelect.Invoke(mixedOption);
        }
        else {
            text.text = options[selectedIndex];
            if (!mute) onSelect.Invoke(options[selectedIndex]);
        }
    }
    public void AddOption(string option)
    {
        options.Add(option);
    }
    public void Select(int index, bool mute = false)
    {
        if ((index < 0 || index >= options.Count) && index != -1) return;
        selectedIndex = index;
        UpdateSelectedItem(mute);
    }
}
}