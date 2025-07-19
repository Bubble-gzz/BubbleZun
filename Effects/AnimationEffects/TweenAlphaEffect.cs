using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using BubbleZun.Effects.VisibilityEffects;
using System.Linq;
namespace BubbleZun.Effects.AnimationEffects
{
    public class TweenAlphaEffect : MonoBehaviour
    {
        // Start is called before the first frame update
        public List<float> alphas = new List<float>();
        public List<float> durations = new List<float>();
        AlphaController alphaController;
        CanvasGroup canvasGroup;
        string tweenName = "TweenAlpha_";
        void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
            tweenName += gameObject.GetInstanceID();
        }
        void CheckAlphaController(){
            if (alphaController == null)
            {
                alphaController = GetComponent<AlphaController>();
                if (alphaController == null) {
                    alphaController = gameObject.AddComponent<AlphaController>();
                    alphaController.FetchAlphaControllers();
                    List<AlphaController> childAlphaControllers = GetComponentsInChildren<AlphaController>().ToList();
                    foreach (var childAlphaController in childAlphaControllers)
                    {
                        childAlphaController.FetchAlphaControllers();
                    }
                }
            }
        }
        public void TweenAlpha(int index){
            //CheckAlphaController();
            DOTween.Kill(tweenName);
            if (canvasGroup != null)
            {
                canvasGroup.DOFade(alphas[index], durations[index]).SetId(tweenName);
            }
            else
            {
                alphaController.TweenAlpha(alphas[index], durations[index]);    
            }
        }
        public void SetAlpha(int index){
            DOTween.Kill(tweenName);
            //CheckAlphaController();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = alphas[index];
            }
            else
            {
                alphaController.SetAlpha(alphas[index]);
            }
        }
    }
}