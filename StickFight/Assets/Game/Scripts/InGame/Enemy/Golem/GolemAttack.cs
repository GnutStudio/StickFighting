using DG.Tweening;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemAttack : EnemyAttack
{
    [SerializeField] private GolemStone golemStone;
    private void Start() {
        golemStone.gameObject.SetActive(false);
    }

    public override void Attack(Action callback = null) {
        if(canAttack) {
            canAttack = false;
            enemyBase.Rg2D.velocity = Vector2.zero;
            //animmation.AnimationState.SetAnimation(trackIndex, animationName, loop);
            enemyAnim.SetAnimAttack(()=> {
                callback?.Invoke();
                golemStone.gameObject.SetActive(false);
            });
            golemStone.gameObject.SetActive(true);
            Vector3 position = InGameManager.Instance.Player.transform.position;
            position.y = transform.position.y;
            golemStone.transform.position = position;

            tween.CheckKillTween();
            tween = DOVirtual.DelayedCall(timeDelayAttack, () => {
                canAttack = true;
            });
        } else {
            callback?.Invoke();
        }
    }

    protected override void EventDamege(TrackEntry trackEntry, Spine.Event e) {
        if(e.Data.Name == eventATK && golemStone != null) {
            SoundManager.Instance.PlaySound(soundAttack);
            golemStone.Active(enemyBase.curDame);
        }
    }

    public void GolemDie() {
        golemStone.gameObject.SetActive(false);
    }
}
