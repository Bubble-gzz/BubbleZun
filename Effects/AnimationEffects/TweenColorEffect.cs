using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using BubbleZun.Effects.VisibilityEffects;
namespace BubbleZun.Effects.AnimationEffects
{
    public class TweenColorEffect : MonoBehaviour
    {
        // Start is called before the first frame update
        public List<Color> colors;
        public List<float> durations = new List<float> { 0.2f };
        SpriteRenderer spriteRenderer;
        Image image;
        TextMeshProUGUI textMeshProUGUI;
        AlphaController alphaController;
        void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            if (image == null) image = GetComponent<Image>();
            if (textMeshProUGUI == null) textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            alphaController = GetComponent<AlphaController>();
        }
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        void CheckAlphaController(){
            if (alphaController == null) alphaController = GetComponent<AlphaController>();
        }
        public void TweenColor(int index)
        {
            Color targetColor = colors[index];
            float duration = durations[index];
            CheckAlphaController();
            if (alphaController != null) targetColor.a = 0;
            // 只处理RGB，保持Alpha不变
            Color rgbOnly = targetColor;

            if (spriteRenderer != null)
            {
                spriteRenderer.DOBlendableColor(rgbOnly, duration);
            }
            
            if (image != null)
            {
                image.DOBlendableColor(rgbOnly, duration);
            }
            
            if (textMeshProUGUI != null)
            {
                textMeshProUGUI.DOBlendableColor(rgbOnly, duration);
            }
        }
    }
}
