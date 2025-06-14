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
        string tweenName = "TweenColor_";
        void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            if (image == null) image = GetComponent<Image>();
            if (textMeshProUGUI == null) textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            alphaController = GetComponent<AlphaController>();
            tweenName += gameObject.GetInstanceID();
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
//            if (alphaController != null) targetColor.a = 0;
            //Debug.Log("["+Time.time+"] TweenColor: " + index + " in " + duration + "s");
            // 只处理RGB，保持Alpha不变
            Color rgbOnly = targetColor;
            DOTween.Kill(tweenName);
            if (spriteRenderer != null)
            {
                spriteRenderer.DOColor(rgbOnly, duration).SetId(tweenName);
            }
            
            if (image != null)
            {
                image.DOColor(rgbOnly, duration).SetId(tweenName);
            }
            
            if (textMeshProUGUI != null)
            {
                textMeshProUGUI.DOColor(rgbOnly, duration).SetId(tweenName);
            }
        }
    }
}
