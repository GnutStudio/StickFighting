using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class GolemStone : MonoBehaviour {
    [SerializeField] private ObjectMakeDame stone;
    [SerializeField] private float hight;
    private Tween tween;
    private void OnEnable() {
        stone.transform.localPosition = Vector2.down * hight;
        stone.gameObject.SetActive(false);
    }

    public void Active(int dame) {
        //stone.SetDame(dame);
        stone.gameObject.SetActive(true);
        tween.CheckKillTween();
        tween = stone.transform.DOLocalMoveY(0, 0.5f).SetEase(Ease.OutExpo).OnComplete(() => {
            this.Recycle();
        });
    }

    private void OnDisable() {
        tween.CheckKillTween();
    }

}
