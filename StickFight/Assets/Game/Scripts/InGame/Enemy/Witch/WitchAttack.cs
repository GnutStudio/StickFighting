using DG.Tweening;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WitchAttack : EnemyAttack {
    [Header("CustomerAttack")]
    [SerializeField] private int turnAttack;
    [SerializeField] private float timeAttack = 2,timeAffterAttack = 2f;
    [SerializeField] private LaserWitch laser;
    [Header("CheckSpace")]
    [SerializeField] private List<BeamRayCast> lstCheckSpace;
    [Header("PointTele")]
    [SerializeField] private List<Transform> lstPointTele;
    [Header("PointAttackII")]
    [SerializeField] private List<Transform> lstPointAttackType2;
    [SerializeField] private BulletCS bulletPref;
    [SerializeField] private float speedBullet = 5f;
    [Header("Spine")]
    [SerializeField,SpineAnimation] private string pre_Blink,blink,pre_Attack,attack;
    private Action callback;
    private int curTurnAttack;
    private bool isAttacking;
    private bool isStun;
    private Transform transformTarget;
    public bool IsAttacking => isAttacking;
    public bool IsStun => isStun;
    private Tween tweenAttack;
    public override void Attack(Action callback = null) {
        //base.Attack(callback);
        this.callback = callback;
        curTurnAttack = turnAttack;
        DoAttack();
    }

    private void DoAttack() {
        curTurnAttack--;
        if(curTurnAttack >= 0) {
            isAttacking = true;
            isStun = false;
            if(curTurnAttack == 0) {
                transformTarget = lstPointTele[0];
                TeleToTarget(AttackTypeII);
            } else {
                transformTarget = FindPlayer();
                TeleToTarget(AttackTypeI);
            }
        } else {
            isStun = true;
            tween.CheckKillTween();
            tween = DOVirtual.DelayedCall(timeDelayAttack, () => {
                callback?.Invoke();
                isAttacking = false;
            });
        }
    }

    private void TeleToTarget(Action callback) {
        enemyAnim.SetAnim(0, pre_Blink, false, () => {
            transform.position = transformTarget.position;
            if(transformTarget.position.x>=lstPointTele[0].position.x) {
                enemyBase.Flip(DirHorizontal.RIGHT);
            } else {
                enemyBase.Flip(DirHorizontal.LEFT);
            }
            enemyAnim.SetAnim(0, blink, false, () => {
                callback?.Invoke();
            });
        });
    }

    private void AttackTypeI() {
        enemyAnim.SetAnim(0, pre_Attack, false, () => {
            laser.TurnOn(laser.transform.right * -1f, enemyBase.curDame);
            enemyAnim.SetAnimByTime(0, attack, timeAttack, () => {
                laser.TurnOff();
                enemyAnim.AnimIdle();
                tweenAttack.CheckKillTween();
                tweenAttack = DOVirtual.DelayedCall(timeAffterAttack, () => {    
                    DoAttack();
                });
            });
        });
    }

    private void AttackTypeII() {
        enemyAnim.SetAnim(0, pre_Attack, false, () => {
            enemyAnim.SetAnim(0, attack, false, () => {
                foreach(var point in lstPointAttackType2) {
                    var bullet = bulletPref.Spawn(InGameManager.Instance.LevelMap.transform);
                    bullet.transform.position = point.position;
                    bullet.Fire(Vector2.down, speedBullet, enemyBase.curDame);
                }
                DoAttack();
            });
        });
    }

    private Transform FindPlayer() {
        foreach(var col in lstCheckSpace[0].ArrayCollider2D) {
            if(col != null && col.transform.parent.GetComponent<Player>() != null) {
                if(col.transform.position.x >= lstPointTele[0].position.x) {
                    return lstPointTele[1];
                } else {
                    return lstPointTele[2];
                }
            }
        }

        foreach(var col in lstCheckSpace[1].ArrayCollider2D) {
            if(col != null && col.transform.parent.GetComponent<Player>() != null) {
                if(col.transform.position.x >= lstPointTele[0].position.x) {
                    return lstPointTele[3];
                } else {
                    return lstPointTele[4];
                }
            }
        }

        foreach(var col in lstCheckSpace[2].ArrayCollider2D) {
            if(col != null && col.transform.parent.GetComponent<Player>() != null) {
                if(col.transform.position.x >= lstPointTele[0].position.x) {
                    return lstPointTele[5];
                } else {
                    return lstPointTele[6];
                }
            }
        }

        return lstPointTele[1];
    }

    private void OnDisable() {
        tweenAttack.CheckKillTween();
    }
}
