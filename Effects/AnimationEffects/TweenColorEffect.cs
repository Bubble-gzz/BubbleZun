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
        string tweenName = "TweenColor_";
        void Awake()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            if (image == null) image = GetComponent<Image>();
            if (textMeshProUGUI == null) textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            tweenName += gameObject.GetInstanceID();
        }
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
        public void TweenColor(int index)
        {
            Color targetColor = colors[index];
            float duration = durations[index];
            DOTween.Kill(tweenName);
            if (spriteRenderer != null)
            {
                spriteRenderer.DOColor(targetColor, duration).SetId(tweenName);
            }
            
            if (image != null)
            {
                image.DOColor(targetColor, duration).SetId(tweenName);
            }
            
            if (textMeshProUGUI != null)
            {
                textMeshProUGUI.DOColor(targetColor, duration).SetId(tweenName);
            }
        }
    }
}
