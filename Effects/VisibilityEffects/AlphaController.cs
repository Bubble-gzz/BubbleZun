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
        Image image;
        TextMeshProUGUI textMeshProUGUI;
        float alpha = 1f;
        List<AlphaController> parentControllers = new List<AlphaController>();
        void Awake(){
            spriteRenderer = GetComponent<SpriteRenderer>();
            image = GetComponent<Image>();
            textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        }

        void Start()
        {
            FetchAlphaControllers();
        }

        // Update is called once per frame
        public void FetchAlphaControllers()
        {
            Debug.Log(Time.time + " FetchAlphaControllers");
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
            DOTween.To(() => alpha, x => alpha = x, targetAlpha, duration);
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
            if (spriteRenderer != null) spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, totalAlpha);
            if (image != null) image.color = new Color(image.color.r, image.color.g, image.color.b, totalAlpha);
            if (textMeshProUGUI != null) textMeshProUGUI.color = new Color(textMeshProUGUI.color.r, textMeshProUGUI.color.g, textMeshProUGUI.color.b, totalAlpha);
        }
    }
}