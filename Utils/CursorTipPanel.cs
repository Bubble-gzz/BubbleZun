using System.Collections;
using System.Collections.Generic;
using BubbleZun.Effects.AnimationEffects;
using UnityEngine;
using BubbleZun.Effects;
using BubbleZun.Utils;
public class CursorTipPanel : MonoBehaviour
{
    bool showing = false;
    public TweenPositionEffect tweenPos;
    public TweenAlphaEffect tweenAlpha;
    public FlashEffect flashEffect;
    void Awake()
    {
        if (tweenPos == null) tweenPos = GetComponent<TweenPositionEffect>();
        if (tweenAlpha == null) tweenAlpha = GetComponent<TweenAlphaEffect>();
        if (flashEffect == null) flashEffect = GetComponent<FlashEffect>();
        tweenPos.SetPos(0);
        tweenAlpha.SetAlpha(0);
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
