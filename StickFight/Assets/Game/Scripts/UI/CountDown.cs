using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System;

public class CountDown : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI txt_Text;
    private Action callBack;
    private Tween tween;
    public void Show(TimeSpan time, Action callBack = null) {
        tween.CheckKillTween();
        int second = (int)time.TotalSeconds;
        tween = DOTween.To(() => second,
            (value) => {
                txt_Text.CheckSetString(value.GetHoursBySeconds());
            },
            0,
            second).SetEase(Ease.Linear).OnComplete(() => {
                callBack?.Invoke();
            });
    }

    private void OnDisable() {
        tween.CheckKillTween();
    }
}
