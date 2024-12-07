using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;
namespace BubbleZun.Effects.AnimationEffects
{
    public class TweenScaleEffect : MonoBehaviour
    {
        public List<Vector3> scales;
        public List<Ease> easeTypes;
        public List<float> durations = new List<float> { 0.5f };
        public void PopOut()
        {
            TweenScale(0);
        }
        public void PopBack()
        {
            TweenScale(1);
        }
        public void TweenScale(int index)
        {
            transform.DOScale(scales[index], durations[index]).SetEase(easeTypes[index]);
        }
    }
}
