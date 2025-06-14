using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleZun.Effects.HighlightEffects;
using DG.Tweening;
namespace BubbleZun.Effects.HighlightEffects
{
    public class ScaleHighlightEffect : MonoBehaviour, IHighlightEffect
    {
        public Vector3 highlightScale = new Vector3(1.1f, 1.1f, 1.1f);
        [SerializeField] private Transform targetTransform;
        private Vector3 originalScale;
        string tweenName = "ScaleHighlightEffect";
        private void Awake()
        {
            if (targetTransform == null) targetTransform = transform;
            originalScale = targetTransform.localScale;
            tweenName = tweenName + "_" + gameObject.GetInstanceID();
        }

        public void Highlight()
        {
            DOTween.Kill(tweenName);
            targetTransform.DOScale(Vector3.Scale(originalScale, highlightScale * 1.05f), 0.2f).SetEase(Ease.OutBack).SetId(tweenName);
            targetTransform.DOScale(Vector3.Scale(originalScale, highlightScale), 0.1f).SetEase(Ease.Linear).SetDelay(0.2f).SetId(tweenName);
        }

        public void Unhighlight()
        {
            DOTween.Kill(tweenName);
            targetTransform.DOScale(originalScale, 0.2f).SetEase(Ease.OutBack).SetId(tweenName);
        }
    }
}
