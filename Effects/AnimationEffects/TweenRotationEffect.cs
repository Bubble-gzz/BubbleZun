using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace BubbleZun.Effects.AnimationEffects{
public class TweenRotationEffect : MonoBehaviour
{
        public List<Vector3> rotations = new List<Vector3>();
        public List<float> durations = new List<float>();
        public List<Ease> easings = new List<Ease>();
        
        private RectTransform rectTransform;
        string tweenName = "TweenRotation_";
        void Awake()
        {
            tweenName += gameObject.GetInstanceID();
        }
        
        public void TweenRotation(int index)
        {
            DOTween.Kill(tweenName);
            transform.DORotate(rotations[index], durations[index])
                    .SetEase(easings[index]).SetId(tweenName);
        }
        
        public void SetRotation(int index)
        {
            DOTween.Kill(tweenName);
            transform.rotation = Quaternion.Euler(rotations[index]);
        }
}
}