using DG.Tweening;
using System;
using UnityEngine;

[AddComponentMenu("Puppy/Engine/UI/Transitions/Anchor Position")]
public class UIAnchorPosition : UITransition {
    [SerializeField] private RectTransform target;
    [SerializeField] private Vector2 from;
    [SerializeField] private Vector2 to;
    [SerializeField] private bool snapping = false;

    private void Reset() {
        target = transform as RectTransform;
    }

    public override void ResetState() {
        if(target) {
            target.anchoredPosition = from;
        }
        Tween?.Kill();
    }

    public override void DoTransition(Action onCompleted) {
        ResetState();
        if(target) {
            Tween = target.DOAnchorPos(to, Duration, snapping)
                          .SetEase(Ease)
                          .SetDelay(Delay)
                          .OnComplete(() => onCompleted?.Invoke());
        }
    }

    private void CopyStartState() {
        from = target.anchoredPosition;
    }
    private void CopyFinishState() {
        to = target.anchoredPosition;
    }
}