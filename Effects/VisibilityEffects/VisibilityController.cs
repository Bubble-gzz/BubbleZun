using System;
using System.Collections;
using System.Collections.Generic;
using BubbleZun.Interaction;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using BubbleZun.Effects.AnimationEffects;
namespace BubbleZun.Effects.VisibilityEffects
{
    public class VisibilityController : MonoBehaviour
    {
        // Start is called before the first frame update
        public bool hideOnStart = false;
        public bool status = true;
        /*Tweening*/
        public bool tweenPos;

        public Vector2 startPos;
        public Vector2 endPos;
        public Ease tweenPosEase;
        TweenPositionEffect posEffect;

        public bool tweenAlpha;
        public Ease tweenAlphaEase;
        public TweenAlphaEffect alphaEffect;

        public float tweenDuration;


        /*Disabling*/
        public bool disableWhenHide;
        public Transform objectRoot;
        public InteractionObject interactionRoot;

        /*Callback*/  
        public UnityEvent onShow = new UnityEvent();
        public UnityEvent onHide = new UnityEvent();
        void Awake(){
            if (!disableWhenHide) {
                objectRoot = null;
                interactionRoot = null;
            }
            if (tweenPos) {
                posEffect = gameObject.GetComponent<TweenPositionEffect>();
                if (posEffect == null) posEffect = gameObject.AddComponent<TweenPositionEffect>();
                posEffect.positions = new List<Vector2>{startPos, endPos};
                posEffect.durations = new List<float>{tweenDuration, tweenDuration};
                posEffect.easings = new List<Ease>{tweenPosEase, tweenPosEase};
            }
            if (tweenAlpha) {
                alphaEffect = gameObject.GetComponent<TweenAlphaEffect>();
                if (alphaEffect == null) alphaEffect = gameObject.AddComponent<TweenAlphaEffect>();
                alphaEffect.alphas = new List<float>{0, 1};
                alphaEffect.durations = new List<float>{tweenDuration, tweenDuration};
            }
        }
        void Start(){
            if (hideOnStart) {
                status = false;
                if (tweenAlpha) alphaEffect.SetAlpha(0);
                if (tweenPos) posEffect.SetPos(0);
            }
            else {
                status = true;
                if (tweenAlpha) alphaEffect.SetAlpha(1);
                if (tweenPos) posEffect.SetPos(1);
            }
        }
        
        public void Show(){
            if (status) return;
            status = true;
            onShow.Invoke();
            if (objectRoot) objectRoot.gameObject.SetActive(true);
            if (interactionRoot) interactionRoot.Enable();
            if (tweenAlpha) alphaEffect.TweenAlpha(1);
            if (tweenPos) posEffect.TweenPos(1);
        }
        public void Hide(){
            if (!status) return;
            status = false;
            onHide.Invoke();
            if (objectRoot) objectRoot.gameObject.SetActive(false);
            if (interactionRoot) interactionRoot.Disable();
            if (tweenAlpha) alphaEffect.TweenAlpha(0);
            if (tweenPos) posEffect.TweenPos(0);
        }
    }
}