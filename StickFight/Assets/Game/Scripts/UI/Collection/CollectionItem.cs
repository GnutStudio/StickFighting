using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class CollectionItem : MonoBehaviour {
    [SerializeField] private Image icon;
    [SerializeField] private float speed;
    [SerializeField] private AudioClip m_sound;
    [SerializeField] private ParticleSystem parPref;
    private Tween tween;
    public void Show(Sprite icon) {
        this.icon.sprite = icon;
    }

    public void Move(Vector2 startPos, Vector2 endPos, Action callback = null, float timeDelay = 0) {
        transform.position = startPos;
        float time = Vector2.Distance(startPos,endPos)/speed;
        tween = DOVirtual.DelayedCall(timeDelay, () => {
            tween = transform.DOMove(endPos, time).SetEase(Ease.Linear).OnComplete(() => {
                SoundManager.Instance.PlaySound(m_sound);
                gameObject.Recycle();
                var par = parPref.Spawn();
                par.transform.GetComponent<RectTransform>().SetParent(CollectionController.Instance.DefaulTarget);
                par.transform.position = endPos;
                callback?.Invoke();
            });
        });
    }

    public void Move(Vector2 root, Vector2 startPos, Vector2 endPos, Action callback = null, float timeDelay = 0) {
        transform.position = root;
        tween = transform.DOMove(startPos, 0.5f).OnComplete(() => {
            float time = Vector2.Distance(startPos,endPos)/speed;
            tween = DOVirtual.DelayedCall(timeDelay, () => {
                tween = transform.DOMove(endPos, time).SetEase(Ease.Linear).OnComplete(() => {
                    SoundManager.Instance.PlaySound(m_sound);
                    gameObject.Recycle();
                    var par = parPref.Spawn();
                    par.transform.GetComponent<RectTransform>().SetParent(CollectionController.Instance.DefaulTarget);
                    par.transform.position = endPos;
                    callback?.Invoke();
                });
            });
        });
    }

    private void OnDisable() {
        tween.CheckKillTween();
    }
}
