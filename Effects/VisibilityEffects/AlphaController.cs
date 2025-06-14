using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
namespace BubbleZun.Effects.VisibilityEffects{
    public class AlphaController : MonoBehaviour
    {
        // Start is called before the first frame update
        SpriteRenderer spriteRenderer;
        CanvasGroup canvasGroup;
        [SerializeField] float alpha = 1f;
        List<AlphaController> parentControllers = new List<AlphaController>();
        string tweenName = "alpha";
        void Awake(){
            spriteRenderer = GetComponent<SpriteRenderer>();
            canvasGroup = GetComponent<CanvasGroup>();
            if (spriteRenderer == null && canvasGroup == null)
            {
                canvasGroup = gameObject.AddComponent<CanvasGroup>();
            }
            tweenName = "alpha" + GetInstanceID();
        }

        void Start()
        {
            FetchAlphaControllers();
        }

        // Update is called once per frame
        public void FetchAlphaControllers()
        {
            parentControllers.Clear();
            Transform current = transform.parent;
            while (current != null)
            {
                if (current.TryGetComponent<AlphaController>(out var controller))
                {
                    parentControllers.Add(controller);
                }
                current = current.parent;
            }
        }
        public void TweenAlpha(float targetAlpha, float duration){
            //Debug.Log("Time.time: " + Time.time + "TweenAlpha:[" + targetAlpha + " , " + duration + "]");
            DOTween.Kill(tweenName);
            DOTween.To(() => alpha, x => alpha = x, targetAlpha, duration).SetId(tweenName);
        }
        public void SetAlpha(float targetAlpha){
            alpha = targetAlpha;
        }
        public float GetTotalAlpha(){
            float totalAlpha = alpha;
            foreach (var controller in parentControllers){
                totalAlpha *= controller.alpha;
            }
            return totalAlpha;
        }
        void Update()
        {
            float totalAlpha = GetTotalAlpha();
            if (canvasGroup != null) {
                canvasGroup.alpha = totalAlpha;
                return;
            }
            if (spriteRenderer != null) spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, totalAlpha);
        }
    }
}