using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace BubbleZun.Effects
{
    public class FlashEffect : MonoBehaviour
    {
        public Image image;
        Color defaultColor;
        public float flashDuration = 0.1f;
        public float threshold = 0.3f;
        public Color flashColor;
        string flashTweenName = "FlashTween";
        void Awake()
        {
            if (image == null) image = GetComponent<Image>();
            defaultColor = image.color;
            flashTweenName += GetInstanceID();
        }
        public void Flash()
        {
            DOTween.Kill(flashTweenName);
            image.DOColor(flashColor, flashDuration * threshold).SetId(flashTweenName).OnComplete(() => {
                image.DOColor(defaultColor, flashDuration * (1 - threshold)).SetId(flashTweenName);
            });
        }
    }
}