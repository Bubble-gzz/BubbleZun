using System.Collections;
using System.Collections.Generic;
using BubbleZun.Interaction;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Palette : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] GameObject patchPrefab;
    [SerializeField] Transform swatchPanel;
    [SerializeField] ColorPicker colorPicker;
    [SerializeField] int row, col;
    int currentId;
    List<Color> colors = new List<Color>();
    List<Image> patchImage = new List<Image>();
    List<TwoPhaseSwitch> patchStatus = new List<TwoPhaseSwitch>();
    public UnityEvent<int> onSelectColor;
    void Start()
    {
        GenerateSwatch();
        BindColorPicker();
    }
    void GenerateSwatch()
    {
        currentId = 0;
        for (int i = 0; i < row; i++)
            for (int j = 0; j < col; j++)
            {
                GameObject patchObj = Instantiate(patchPrefab, swatchPanel);
                MouseClickDetector patch = patchObj.GetComponent<MouseClickDetector>();
                int id = i * col + j; // Calculate unique ID for each patch
                patch.onLMBClick.AddListener(() => SelectColor(id)); // Use lambda to capture the ID
                patchStatus.Add(patchObj.GetComponent<TwoPhaseSwitch>());
                patchImage.Add(patchObj.GetComponent<Image>());
            }
    }
    void BindColorPicker()
    {
        colorPicker.onColorChange.AddListener(EditCurrentColor);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void ChangeColor(int id, Color newColor)
    {
        colors[id] = newColor;
        patchImage[id].color = newColor;
    }
    public Color GetColor(int id)
    {
        return colors[id];
    }
    void SelectColor(int id)
    {
        if (id == currentId) return;
        patchStatus[currentId]?.TurnOff(false);
        patchStatus[id]?.TurnOn(true);
        currentId = id;
        onSelectColor.Invoke(id);
    }
    public void EditCurrentColor(Color color)
    {
        ChangeColor(currentId, color);
    }
}