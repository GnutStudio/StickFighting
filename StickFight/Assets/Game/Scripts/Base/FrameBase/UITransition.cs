using DG.Tweening;
using System;
using UnityEngine;

public abstract class UITransition : MonoBehaviour {
    [SerializeField, Range(0f, 10f)] private float delay = 0f;
    [SerializeField, Range(0f, 10f)] private float duration = 0.5f;
    [SerializeField] private Ease ease = Ease.Linear;

    public float Duration { get => duration; }
    public float Delay { get => delay; }
    public float TotalDuration { get => Duration + Delay; }
    public Ease Ease { get => ease; }
    public Tween Tween { get; protected set; }

    public void Stop() {
        Tween?.Kill(false);
    }
    public abstract void ResetState();
    public abstract void DoTransition(Action onCompleted);
}
