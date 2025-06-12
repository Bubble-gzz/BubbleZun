using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace BubbleZun.Effects.AnimationEffects
{
    public class SineFloatEffect : MonoBehaviour
    {
        bool useScreenSpace = false;
        private RectTransform rectTransform;
        [SerializeField] float amplitude = 10f;
        [SerializeField] float speed = 1f;
        [SerializeField] float offset = 0f;
        void Awake()
        {
            useScreenSpace = GetComponentInParent<Canvas>() != null;
            if (useScreenSpace)
            {
                rectTransform = GetComponent<RectTransform>();
            }
        }
        void Update()
        {
            if (useScreenSpace)
            {
                rectTransform.anchoredPosition = new Vector2(0, Mathf.Sin(Time.time * speed + offset) * amplitude);
            }
            else
            {
                transform.position = new Vector3(0, Mathf.Sin(Time.time * speed + offset) * amplitude, transform.position.z);
            }
        }

    }
}