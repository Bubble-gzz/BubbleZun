using System.Collections;
using System.Collections.Generic;
using BubbleZun.Effects.AnimationEffects;
using UnityEngine;
using BubbleZun.Effects;
using BubbleZun.Utils;
using TMPro;
using DG.Tweening;
public class CursorTipPanel : MonoBehaviour
{
    bool showing = false;
    public TweenPositionEffect tweenPos;
    public TweenAlphaEffect tweenAlpha;
    public FlashEffect flashEffect;
    public TMP_Text tipText;
    RectTransform rt;
    void Awake()
    {
        rt = GetComponent<RectTransform>();
        if (tipText == null) tipText = GetComponentInChildren<TMP_Text>();

        if (tweenPos == null) tweenPos = GetComponent<TweenPositionEffect>();
        if (tweenAlpha == null) tweenAlpha = GetComponent<TweenAlphaEffect>();
        if (flashEffect == null) flashEffect = GetComponent<FlashEffect>();
        tweenPos.SetPos(0);
        tweenAlpha.SetAlpha(0);
    }
    public void Update()
    {
        if (showing) {
            float targetWidth = Mathf.Max(tipText.preferredWidth + 20f, 100f);
            rt.sizeDelta = Vector2.Lerp(rt.sizeDelta, new Vector2(targetWidth, rt.sizeDelta.y), 20f * Time.deltaTime);
        }
    }
    public void OnShowTip()
    {
        if (showing == false)
        {
            BDebug.Log("OnShowTip");
            tweenPos.TweenPos(1);
            tweenAlpha.TweenAlpha(1);
            showing = true;
        }
        else {
            flashEffect.Flash();
        }
    }
    public void OnHideTip()
    {
        if (showing == false) return;
        BDebug.Log("OnHideTip");
        showing = false;
        tweenPos.TweenPos(0);
        tweenAlpha.TweenAlpha(0);
    }
}
