using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("Puppy/Engine/UI/Transitions/Scale")]
public class UIScale : UITransition {
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 from;
    [SerializeField] private Vector3 to;

    private void Reset() {
        target = transform;
    }

    public override void ResetState() {
        if(target) {
            target.localScale = from;
        }
        Tween?.Kill();
    }

    public override void DoTransition(Action onCompleted) {
        ResetState();
        if(target) {
            Tween = target.DOScale(to, Duration)
                          .SetUpdate(true)
                          .SetEase(Ease)
                          .SetDelay(Delay)
                          .OnComplete(() => onCompleted?.Invoke());
        }
    }

    private void CopyStartState() {
        from = target.localScale;
    }
    private void CopyFinishState() {
        to = target.localScale;
    }
}
