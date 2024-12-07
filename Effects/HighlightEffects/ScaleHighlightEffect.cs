using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleZun.Effects.HighlightEffects;
namespace BubbleZun.Effects.HighlightEffects
{
    public class ScaleHighlightEffect : MonoBehaviour, IHighlightEffect
    {
        public Vector3 highlightScale = new Vector3(1.1f, 1.1f, 1.1f);
        [SerializeField] private Transform targetTransform;
        private Vector3 originalScale;

        private void Awake()
        {
            if (targetTransform == null) targetTransform = transform;
            originalScale = targetTransform.localScale;
        }

        public void Highlight()
        {
            targetTransform.localScale = Vector3.Scale(originalScale, highlightScale);
        }

        public void Unhighlight()
        {
            targetTransform.localScale = originalScale;
        }
    }
}
