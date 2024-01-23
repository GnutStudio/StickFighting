using DG.Tweening;
using UnityEngine;

public class EnemyHeartBar : MonoBehaviour {
    private static float TIME_BAR = 0.5f;
    [SerializeField] private GameObject display;
    [SerializeField] private Transform curHeart;
    private Tween tween;
    public void Init() {
        display.SetActive(false);
        curHeart.localScale = Vector3.one;
    }

    public void UpdateHeart(float percent) {
        tween.CheckKillTween();
        display.SetActive(true);
        float time = Mathf.Abs(curHeart.localScale.x - percent) * TIME_BAR;
        tween = curHeart.DOScaleX(percent, time).OnComplete(() => {
            display.SetActive(false);
        });
    }

    private void OnDisable() {
        tween.CheckKillTween();
    }
}
