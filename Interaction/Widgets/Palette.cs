using System.Collections;
using System.Collections.Generic;
using BubbleZun.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Palette : MonoBehaviour
{
    public ColorPicker colorPicker;
    public Transform contentRoot;
    List<Image> patchImage = new List<Image>();
    public UnityEvent onChangeColor;
    public int currentColorIndex;
    public Color currentColor => GetColor(currentColorIndex);
    void Awake()
    {
        if (contentRoot == null) contentRoot = transform;
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
        currentColorIndex = id;
        colorPicker.SetColor(currentColor);
    }
}