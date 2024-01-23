using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barpercent : MonoBehaviour
{
    [SerializeField] private Transform curBar;
    [SerializeField] private float timeBar;
    private Tween tween;
    public void Init() {
        curBar.localScale = Vector3.one;
    }

    public void UpdateHeart(float percent) {
        tween.CheckKillTween();
        float time = Mathf.Abs(curBar.localScale.x - percent) * timeBar;
        tween = curBar.DOScaleX(percent, time);
    }

    private void OnDisable() {
        tween.CheckKillTween();
    }
}
