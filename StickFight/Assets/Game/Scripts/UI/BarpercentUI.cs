using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarpercentUI : MonoBehaviour
{
    [SerializeField] private Image curBar;
    [SerializeField] private float timeBar;
    private Tween tween;

    public void Show(float percent) {
        tween.CheckKillTween();
        float time = Mathf.Abs(curBar.fillAmount - percent) * timeBar;
        tween = curBar.DOFillAmount(percent, time);
    }

    private void OnDisable() {
        tween.CheckKillTween();
    }
}
