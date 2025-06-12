using System.Collections;
using System.Collections.Generic;
using BubbleZun.Effects.HighlightEffects;
using UnityEngine;
using UnityEngine.UI;
public class ImageHighlightEffect : MonoBehaviour, IHighlightEffect
{
    [SerializeField] Image highlightImg, originalImg;
    void Awake()
    {
        highlightImg.enabled = false;
        originalImg.enabled = true;
    }
    public void Highlight()
    {
        highlightImg.enabled = true;
        originalImg.enabled = false;
    }
    public void Unhighlight()
    {
        highlightImg.enabled = false;
        originalImg.enabled = true;
    }
}
