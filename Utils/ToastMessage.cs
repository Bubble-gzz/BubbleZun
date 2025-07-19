using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;  
using BubbleZun.Effects.AnimationEffects;
namespace BubbleZun.Utils{
    public class ToastMessage : MonoBehaviour
    {
        public enum Type
        {
            OK,
            WARNING,
            ERROR
        }

        [SerializeField] Color color_OK;
        [SerializeField] Color color_WARNING;
        [SerializeField] Color color_ERROR;
        [SerializeField] float padding = 30f;
        [SerializeField] float minWidth = 100f;

        public Type type;
        [SerializeField] Image background;
        [SerializeField] TMP_Text message;
        public TweenPositionEffect tweenPosEffect;
        public TweenAlphaEffect tweenAlphaEffect;
        RectTransform rt;
        public float duration = 1.5f;
        void Awake()
        {
            rt = GetComponent<RectTransform>();
            rt.anchorMin = new Vector2(0.5f, 0f);
            rt.anchorMax = new Vector2(0.5f, 0f);
            if (tweenPosEffect == null) tweenPosEffect = GetComponent<TweenPositionEffect>();
            if (tweenAlphaEffect == null) tweenAlphaEffect = GetComponent<TweenAlphaEffect>();
        }
        void Start()
        {
            tweenPosEffect.SetPos(0);
            tweenAlphaEffect.SetAlpha(0);
            //Debug.Log("Awake, rt.anchoredPosition: " + rt.anchoredPosition);
            PopUp(duration);
        }

        float timeDelta = 0;
        void Update()
        {
            timeDelta += Time.deltaTime;
            if (timeDelta > 0.2f)
            {
                timeDelta = 0;
                //Debug.Log("Update, rt.anchoredPosition: " + rt.anchoredPosition);
            }
        }

        public void SetType(Type type)
        {
            this.type = type;
            background.color = type switch
            {
                Type.OK => color_OK,
                Type.WARNING => color_WARNING,
                Type.ERROR => color_ERROR,
                _ => background.color
            };
        }

        public void SetMessage(string message)
        {
            this.message.text = message;
            float width = Mathf.Max(this.message.preferredWidth + padding, minWidth);
            rt.sizeDelta = new Vector2(width, rt.sizeDelta.y);
        }

        public void PopUp(float duration = 1.5f)
        {
            StartCoroutine(PopUpCoroutine(duration));
        }
        bool fadeOut = false;
        public void FadeOut(bool interupt = false)
        {
            if (fadeOut) return;
            fadeOut = true;
            StartCoroutine(FadeOutCoroutine(interupt));
        }
        IEnumerator FadeOutCoroutine(bool interupt)
        {
            tweenAlphaEffect.TweenAlpha(2);
            if (interupt) tweenPosEffect.TweenPos(3);
            else tweenPosEffect.TweenPos(2);
            yield return new WaitForSeconds(2f);
            Destroy(gameObject);
        }
        IEnumerator PopUpCoroutine(float duration)
        {
            tweenPosEffect.TweenPos(1);
            tweenAlphaEffect.TweenAlpha(1);
            yield return new WaitForSeconds(duration);
            FadeOut();
        }
    }

}
