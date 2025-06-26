using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace BubbleZun.Effects.AnimationEffects{
    public class TweenRectTransformEffect : MonoBehaviour
    {
        public List<Vector2> sizes = new List<Vector2>();
        public List<float> durations = new List<float>();
        public List<Ease> easings = new List<Ease>();
        
        private RectTransform rt;
        string tweenName = "TweenRectTransform_";
        void Awake()
        {
            rt = GetComponent<RectTransform>();
            tweenName += gameObject.GetInstanceID();
        }
        
        public void TweenSize(int index)
        {
            DOTween.Kill(tweenName);
            rt.DOSizeDelta(sizes[index], durations[index])
                .SetEase(easings[index]).SetId(tweenName);
        }
        
        public void SetSize(int index)
        {
            DOTween.Kill(tweenName);
            rt.sizeDelta = sizes[index];
        }
    }
}