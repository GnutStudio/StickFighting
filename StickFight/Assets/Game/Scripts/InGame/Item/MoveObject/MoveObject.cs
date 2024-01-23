using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class MoveObject : MonoBehaviour {
    [SerializeField] private GameObject obj;
    [SerializeField] private Transform pointEnd;
    [SerializeField] private bool showEnd;
    [SerializeField] private float speed;
    [SerializeField] private float timeDelayStart,timeDelayLoop;
    private Sequence sequence;
    private Tween tween;

    private void OnValidate() {
        if(showEnd) {
            obj.transform.position = pointEnd.position;
        } else {
            obj.transform.position = transform.position;
        }
    }

    private void Start() {
        obj.transform.position = transform.position;
        tween.CheckKillTween();
        tween = DOVirtual.DelayedCall(timeDelayStart, () => {
            float distance = Vector2.Distance(transform.position,pointEnd.position);
            float timeMove = distance / speed;
            sequence = DOTween.Sequence();
            sequence.Append(obj.transform.DOMove(pointEnd.position, timeMove).SetEase(Ease.Linear));
            sequence.AppendInterval(timeDelayLoop);
            sequence.Append(obj.transform.DOMove(transform.position, timeMove).SetEase(Ease.Linear));
            sequence.AppendInterval(timeDelayLoop);
            sequence.SetLoops(-1);
        });
    }


    private void OnDisable() {
        sequence.Kill();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(transform.position, pointEnd.position);
    }
}
