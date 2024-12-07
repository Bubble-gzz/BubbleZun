using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace BubbleZun.Effects.AnimationEffects
{
    public class JuicyScaleEffect : MonoBehaviour
    {
        // Start is called before the first frame update
        public Vector3 popupScale = new Vector3(1.2f, 1.2f, 1.2f);
        public Vector3 normalScale = new Vector3(1f, 1f, 1f);
        public Vector3 squeezeScale = new Vector3(0.8f, 0.8f, 0.8f);
        public float playSpeed = 1f;
        public void PopUp()
        {
            DOTween.Kill(transform);
            transform.DOScale(popupScale, 0.1f / playSpeed).SetEase(Ease.Linear);
        }
        public void PopBack()
        {
            DOTween.Kill(transform);
            transform.DOScale(normalScale, 0.1f / playSpeed).SetEase(Ease.Linear);
        }
        public void Squeeze()
        {
            DOTween.Kill(transform);
            transform.localScale = squeezeScale;
            transform.DOScale(popupScale, 0.7f / playSpeed).SetEase(Ease.OutElastic);
        }
    }
}