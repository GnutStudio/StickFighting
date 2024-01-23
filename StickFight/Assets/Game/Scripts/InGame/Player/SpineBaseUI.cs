using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineBaseUI : MonoBehaviour
{
    [SerializeField] private SkeletonGraphic animmation;
    [SerializeField] private Action evtComplate;
    public SkeletonGraphic Anim => animmation;
    private void Awake() {
        animmation.AnimationState.Complete += HandleEventComplete;
    }

    private void HandleEventComplete(TrackEntry trackEntry) {
        evtComplate?.Invoke();
    }

    public void SetAnim(int trackIndex, string animationName, bool loop, Action callBack = null) {
        if(animmation == null) {
            callBack?.Invoke();
            return;
        }
        animmation.AnimationState.SetAnimation(trackIndex, animationName, loop);
        evtComplate = callBack;
    }
}
