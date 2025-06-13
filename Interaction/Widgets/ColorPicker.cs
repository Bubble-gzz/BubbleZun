using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using BubbleZun.Interaction;
namespace BubbleZun.Interaction
{
public class ColorPicker : Interactable, IDragHandler, IPointerDownHandler
{
    [SerializeField] Image SVPanel;
    [SerializeField] Image HueSlider; 
    [SerializeField] Image Output;
    [SerializeField] RectTransform SVHandle;
    [SerializeField] RectTransform HueHandle;
    public UnityEvent<Color> onColorChange;

    Vector2 SVPos;
    float HuePos;

    private float hue = 0;
    private float saturation = 1;
    private float value = 1;

    protected override void Start()
    {
        base.Start();
        UpdateSVPanel();
        UpdateHueSlider();
        UpdateColor();
    }

    void UpdateSVPanel(bool passive = false)
    {
        Texture2D texture = new Texture2D((int)SVPanel.rectTransform.rect.width, (int)SVPanel.rectTransform.rect.height);
        Color hueColor = Color.HSVToRGB(hue, 1, 1);

        for (int y = 0; y < texture.height; y++)
        {
            for (int x = 0; x < texture.width; x++)
            {
                float s = (float)x / texture.width;
                float v = (float)y / texture.height;
                Color color = Color.HSVToRGB(hue, s, v);
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        SVPanel.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        if (passive) {
            SVHandle.localPosition = new Vector2((saturation - 0.5f) * SVPanel.rectTransform.rect.width, (value - 0.5f) * SVPanel.rectTransform.rect.height);
        }
    }

    void UpdateHueSlider(bool passive = false)
    {
        Texture2D texture = new Texture2D((int)HueSlider.rectTransform.rect.width, (int)HueSlider.rectTransform.rect.height);
        
        for (int y = 0; y < texture.height; y++)
        {
            float h = (float)y / texture.height;
            Color color = Color.HSVToRGB(h, 1, 1);
            for (int x = 0; x < texture.width; x++)
            {
                texture.SetPixel(x, y, color);
            }
        }
        texture.Apply();
        HueSlider.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        if (passive) {
            HueHandle.localPosition = new Vector2(HueHandle.localPosition.x, (hue - 0.5f) * HueSlider.rectTransform.rect.height);
        }
    }

    void UpdateColor()
    {
        if (interactionObject != null && !interactionObject.IsInteractable()) return;
        Color newColor = Color.HSVToRGB(hue, saturation, value);
        Output.color = newColor;
        onColorChange?.Invoke(newColor);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (interactionObject != null && !interactionObject.IsInteractable()) return;
        Vector2 localPos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(SVPanel.rectTransform, eventData.position, eventData.pressEventCamera, out localPos))
        {
            if (eventData.pointerCurrentRaycast.gameObject == SVPanel.gameObject)
            {
                float halfWidth = SVPanel.rectTransform.rect.width * 0.5f;
                float halfHeight = SVPanel.rectTransform.rect.height * 0.5f;
                
                SVPos = new Vector2(
                    Mathf.Clamp(localPos.x, -halfWidth, halfWidth),
                    Mathf.Clamp(localPos.y, -halfHeight, halfHeight)
                );
                SVHandle.localPosition = SVPos;
                
                saturation = (SVPos.x + halfWidth) / SVPanel.rectTransform.rect.width;
                value = (SVPos.y + halfHeight) / SVPanel.rectTransform.rect.height;
                UpdateColor();
            }
        }
        
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(HueSlider.rectTransform, eventData.position, eventData.pressEventCamera, out localPos))
        {
            if (eventData.pointerCurrentRaycast.gameObject == HueSlider.gameObject)
            {
                float halfHeight = HueSlider.rectTransform.rect.height * 0.5f;
                HuePos = Mathf.Clamp(localPos.y, -halfHeight, halfHeight);
                HueHandle.localPosition = new Vector2(HueHandle.localPosition.x, HuePos);
                
                hue = (HuePos + halfHeight) / HueSlider.rectTransform.rect.height;
                UpdateSVPanel();
                UpdateColor();
            }
        }
    }
    public void SetColor(Color color)
    {
        Color.RGBToHSV(color, out hue, out saturation, out value);
        UpdateSVPanel(true);
        UpdateHueSlider(true);
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (interactionObject != null && !interactionObject.IsInteractable()) return;
        OnDrag(eventData);
    }
}
}