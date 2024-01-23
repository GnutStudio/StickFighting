using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SpineBase : MonoBehaviour {
    [SerializeField] protected SkeletonAnimation animmation;
    //[SerializeField] private Action evtComplate;
    [SerializeField, SpineAnimation] protected string animIdle;
    public SkeletonAnimation Anim => animmation;
    private Tween tween;
    private Tween tweenFade;
    protected virtual void Awake() {
        //animmation.AnimationState.Complete += HandleEventComplete;
    }

    //protected virtual void HandleEventComplete(TrackEntry trackEntry) {
    //    evtComplate?.Invoke();
    //}

    public void SetAnim(int trackIndex, string animationName, bool loop, Action callBack = null) {
        if(animmation == null || string.IsNullOrEmpty(animationName)) {
            callBack?.Invoke();
            return;
        }
        TrackEntry track = animmation.AnimationState.SetAnimation(trackIndex, animationName, loop);
        track.Complete += (value)=> { callBack?.Invoke(); };
        //evtComplate = callBack;
    }

    public void SetAnimByTime(int trackIndex, string animationName, float timeDelay, Action callBack) {
        if(animmation == null) {
            tween.CheckKillTween();
            tween = DOVirtual.DelayedCall(timeDelay, () => {
                callBack?.Invoke();
            });
            return;
        }
        animmation.AnimationState.SetAnimation(trackIndex, animationName, true);
        tween.CheckKillTween();
        tween = DOVirtual.DelayedCall(timeDelay, () => {
            callBack?.Invoke(); 
        });
    }

    public void AnimIdle(int trackIndex = 0, bool loop = true, Action callBack = null) {
        SetAnim(trackIndex, animIdle, loop, callBack);
    }

    public float SetTimeAnim(int trackIndex, string animationName, bool loop) {
        SetAnim(trackIndex, animationName, loop);
        return animmation.AnimationState.GetCurrent(0).TrackTime;
    }

    public void Fade(bool appear, float timeFade = 1, Action callback = null) {
        tweenFade.CheckKillTween();
        if(appear) {
            tweenFade = DOTween.To(() => 0f, (value) => { Anim.skeleton.A = value; }, 1f, timeFade).OnComplete(() => {
                callback?.Invoke();
            });
        } else {
            tweenFade = DOTween.To(() => 1f, (value) => { Anim.skeleton.A = value; }, 0f, timeFade).OnComplete(() => {
                callback?.Invoke();
            });
        }
    }

    public void Flash(float timeFlash,Action callback = null) {
        Anim.skeleton.A = 0.5f;
        tweenFade.CheckKillTween();
        tweenFade = DOVirtual.DelayedCall(timeFlash, () => {
            Anim.skeleton.A = 1f;
            callback?.Invoke();
        });
    }

    private void OnDisable() {
        tween.CheckKillTween();
        tweenFade.CheckKillTween();
        Anim.skeleton.A = 1f;
    }

    public void SetOrder(int order) {
        Anim.GetComponent<MeshRenderer>().sortingOrder = order;
    }
}
