using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using STU;

public class BossBarHeart : MonoBehaviour {
    [SerializeField] private RectTransform rect;
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private BarpercentUI barPercentUI;
    private Tween tweenMove;
    private BossBase boss;
    private void OnEnable() {
        EventDispatcher.AddListener<EventKey.EventSetupNewGame>(HalderEventSetupNewGame);
        EventDispatcher.AddListener<EventKey.BossArea>(HalderEventEnterBossArea);
        EventDispatcher.AddListener<EventKey.BossGetDame>(HalderEventBossGetDame);
    }

    private void OnDisable() {
        EventDispatcher.RemoveListener<EventKey.EventSetupNewGame>(HalderEventSetupNewGame);
        EventDispatcher.RemoveListener<EventKey.BossArea>(HalderEventEnterBossArea);
        EventDispatcher.RemoveListener<EventKey.BossGetDame>(HalderEventBossGetDame);
        tweenMove.CheckKillTween();
    }

    private void HalderEventSetupNewGame(EventKey.EventSetupNewGame evt) {
        StartActive(false);
    }

    private void HalderEventEnterBossArea(EventKey.BossArea evt) {
        StartActive(evt.Enter);
    }

    private void HalderEventBossGetDame(EventKey.BossGetDame evt) {
        barPercentUI.Show(boss.PercentHeart);
    }

    public void StartActive(bool active) {
        if(active) {
            boss = InGameManager.Instance.LevelMap.transform.GetComponentInChildren<BossBase>();
            barPercentUI.Show(boss.PercentHeart);
            tweenMove.CheckKillTween();
            tweenMove = rect.DOAnchorPos(targetPosition, 2f).SetEase(Ease.OutBack);
        } else {
            rect.anchoredPosition = new Vector3(0, -targetPosition.y, 0);
        }
    }
}
