using DG.Tweening;
using STU;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextNotify : Singleton<TextNotify> {
    public static string ADS_FAIL = "Ads appear error";
    [SerializeField] private TextMeshProUGUI txt_Notify;
    [SerializeField] private float hightMore;
    [SerializeField] private float timeDisapear;
    Tween tween;
    private void Start() {
        txt_Notify.gameObject.SetActive(false);
    }

    public void Show(string text) {
        txt_Notify.transform.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
        txt_Notify.gameObject.SetActive(true);
        txt_Notify.text = text;
        if(tween != null && tween.IsPlaying() == true) {
            tween.Kill();
        }
        tween = txt_Notify.transform.DOLocalMoveY(hightMore, timeDisapear).OnComplete(() => {
            txt_Notify.gameObject.SetActive(false);
        });
    }

    public void ShowFailAds() {
        Show("Can't get the reward");
    }

    [ContextMenu("ShowDemo")]
    public void ShowDemo() {
        Show("Show Demo");
    }

    private void OnDisable() {
        tween.CheckKillTween();
    }
    //public void ShowNotEnough(string addStart = "Coin") {
    //    Show($"{addStart} " + LocalizationManager.GetTranslation("not enough"));
    //}
}
