using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using DG.Tweening;
public class ReviveFrame : FrameBase
{
    [SerializeField] private Image imgCircle;
    [SerializeField] private TextMeshProUGUI txt_Count;
    [SerializeField] private Button btn_Revive;
    [Header("Customer")]
    [SerializeField] private int timeCount;
    private int curcount;
    private Tween tweenCircle;
    private Tween tweenCount;
    private void Awake() {
        btn_Revive.onClick.AddListener(ButtonRevive);
    }

    public override void OnShow(Action onCompleted = null, bool instant = false) {
        base.OnShow(onCompleted, instant);
        imgCircle.fillAmount = 1f;
        curcount = timeCount;
        txt_Count.transform.localScale = Vector3.one * 0.8f;
        StartCircle();
        StartCount(curcount);
    }

    private void StartCircle() {
        tweenCircle.CheckKillTween();
        tweenCircle = DOTween.To(() => 1f, (value) => { imgCircle.fillAmount = value; }, 0f, timeCount).SetEase(Ease.Linear).OnComplete(()=> {
            InGameManager.Instance.Lose();
            //AdsManager.Instance.ShowInterstitial();
            SceneManagerLoad.Instance.LoadSceneAsyn(SceneManagerLoad.SCENE_HOME);
        });
    }

    private void StartCount(int number) {
        tweenCount.CheckKillTween();
        txt_Count.text = number.ToString();
        tweenCount = txt_Count.transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutExpo).OnComplete(()=> {
            tweenCount = txt_Count.transform.DOScale(Vector3.one * 0.8f, 0.5f).SetEase(Ease.InExpo).OnComplete(() => {
                if(number > 0) {
                    StartCount(number-1);
                }
            });
        });
    }

    private void ButtonRevive() {
        InGameManager.Instance.Revived();
        Hide();
    }

    private void OnDisable() {
        tweenCircle.CheckKillTween();
        tweenCount.CheckKillTween();
    }

    [ContextMenu("TestCount")] 
    public void TestCount() {
        imgCircle.fillAmount = 1f;
        curcount = timeCount;
        txt_Count.transform.localScale = Vector3.one * 0.8f;
        StartCircle();
        StartCount(curcount);
    }
}
