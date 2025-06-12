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
    void Awake()
    {
        text.text = options[selectedIndex];
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
}
}