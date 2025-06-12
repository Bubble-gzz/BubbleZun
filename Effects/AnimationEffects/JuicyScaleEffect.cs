using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace BubbleZun.Effects.AnimationEffects
{
    public class JuicyScaleEffect : MonoBehaviour
    {
        // Start is called before the first frame update
        public Vector2 popupScale = new Vector2(1.2f, 1.2f);
        public Vector2 normalScale = new Vector2(1f, 1f);
        public Vector2 squeezeScale = new Vector2(0.8f, 0.8f);
        public Vector2 blastScale = new Vector2(1.2f, 1.2f);
        public float playSpeed = 1f;
        public void PopUp()
        {
            DOTween.Kill(transform);
            Vector3 targetScale = ScaleToVector3(popupScale);
            transform.DOScale(targetScale, 0.1f / playSpeed).SetEase(Ease.Linear);
        }
        public void PopBack()
        {
            DOTween.Kill(transform);
            Vector3 targetScale = ScaleToVector3(normalScale);
            transform.DOScale(targetScale, 0.1f / playSpeed).SetEase(Ease.Linear);
        }
        public void Squeeze()
        {
            DOTween.Kill(transform);
            Vector3 targetScale = ScaleToVector3(squeezeScale);
            transform.localScale = targetScale;
            transform.DOScale(ScaleToVector3(normalScale), 0.7f / playSpeed).SetEase(Ease.OutElastic);
        }
        public void BlastOut()
        {
            DOTween.Kill(transform);
            Vector3 targetScale = ScaleToVector3(blastScale);
            transform.localScale = targetScale;
            transform.DOScale(ScaleToVector3(normalScale), 0.1f / playSpeed).SetEase(Ease.OutSine);
        }
        Vector3 ScaleToVector3(Vector2 scale)
        {
            Vector3 res = scale;
            res.z = transform.localScale.z;
            return res;
        }
    }
}