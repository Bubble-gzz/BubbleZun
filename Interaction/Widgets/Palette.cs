using System.Collections;
using System.Collections.Generic;
using BubbleZun.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Palette : MonoBehaviour
{
    public GroupSelector groupSelector;
    public ColorPicker colorPicker;
    public Transform contentRoot;
    List<Image> patchImage = new List<Image>();
    public UnityEvent onChangeColor;
    public int currentColorIndex => groupSelector.currentIndex;
    public Color currentColor => GetColor(currentColorIndex);
    void Awake()
    {
        if (groupSelector == null) groupSelector = GetComponentInChildren<GroupSelector>();
        if (groupSelector == null) groupSelector = gameObject.AddComponent<GroupSelector>();
        if (contentRoot == null) contentRoot = transform;
        groupSelector.defaultMouseSelect = true;
        groupSelector.contentRoot = contentRoot;
        groupSelector.onSelect.AddListener(OnSelectSwatch);
        foreach (Transform child in contentRoot)
        {
            Image image = child.GetComponent<Image>();
            if (image != null) patchImage.Add(image);

        }
    }
    void Start()
    {
        colorPicker.SetColor(currentColor);
    }
    void ChangeColor(int id, Color newColor)
    {
        patchImage[id].color = newColor;
        onChangeColor.Invoke();
    }
    public Color GetColor(int id)
    {
        return patchImage[id].color;
    }
    public void EditCurrentColor(Color color)
    {
        ChangeColor(currentColorIndex, color);
    }
    public void InitColor(int id, Color color)
    {
        patchImage[id].color = color;
    }
    public void OnSelectSwatch(int id)
    {
        colorPicker.SetColor(currentColor);
    }
}