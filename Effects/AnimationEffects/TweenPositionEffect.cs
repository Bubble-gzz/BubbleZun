using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace BubbleZun.Effects.AnimationEffects
{
    public class TweenPositionEffect : MonoBehaviour
    {
        bool useScreenSpace = false;
        public List<Vector2> positions = new List<Vector2>();
        public List<float> durations = new List<float>();
        public List<Ease> easings = new List<Ease>();
        
        private RectTransform rectTransform;
        string tweenName = "TweenPosition_";
        void Awake()
        {
            Debug.Log("TweenPosEffet Awake, Parent: " + transform.parent.name);
            useScreenSpace = GetComponentInParent<Canvas>() != null;
            rectTransform = GetComponent<RectTransform>();
            tweenName += gameObject.GetInstanceID();
        }
        
        public void TweenPos(int index)
        {
            DOTween.Kill(tweenName);
            if (useScreenSpace)
            {
                // UI对象使用DOAnchorPos
                rectTransform.DOAnchorPos(positions[index], durations[index])
                    .SetEase(easings[index]).SetId(tweenName);
            }
            else
            {
                // 非UI对象使用DOMove
                Vector3 newPos = new Vector3(positions[index].x, positions[index].y, transform.position.z);
                transform.DOMove(newPos, durations[index])
                    .SetEase(easings[index]).SetId(tweenName);
            }
        }
        
        public void SetPos(int index)
        {
            DOTween.Kill(tweenName);
            if (useScreenSpace)
            {
                //Debug.Log("SetPos (screen space) for " + gameObject.name + ": " + positions[index] + " " + rectTransform.anchoredPosition);
                rectTransform.anchoredPosition = positions[index];
               // Debug.Log("anchoredPosition after set: " + rectTransform.anchoredPosition);
          
            }
            else
            {
               //Debug.Log("SetPos (world space) for " + gameObject.name + ": " + positions[index]);
                Vector3 newPos = new Vector3(positions[index].x, positions[index].y, transform.position.z);
                transform.position = newPos;
                //Debug.Log("anchoredPosition after set: " + rectTransform.anchoredPosition);
            }
        }
    }
}