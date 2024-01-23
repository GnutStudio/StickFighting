using DG.Tweening;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnemyEndermanAttack : EnemyAttack
{
    [SerializeField] private LaserBase laser;
    private Vector3 pointTarget;

    protected override void Awake()
    {
        base.Awake();
        laser.TurnOff();
    }

    public override void Attack(Action callback = null)
    {
        if (canAttack)
        {
            pointTarget = InGameManager.Instance.Player.transform.position;
            pointTarget.y = transform.position.y;
            canAttack = false;
            enemyBase.Rg2D.velocity = Vector2.zero;
            enemyAnim.SetAnimAttack(() => {
                laser.TurnOff();
                callback?.Invoke();
            });
            tween.CheckKillTween();
            tween = DOVirtual.DelayedCall(timeDelayAttack, () =>
            {
                canAttack = true;
            });
        }
        else
        {
            callback?.Invoke();
        }
    }

    protected override void EventDamege(TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == eventATK)
        {
            SoundManager.Instance.PlaySound(soundAttack);
            Vector3 direction = (pointTarget - laser.transform.position).normalized;
            laser.TurnOn(direction,enemyBase.curDame);
            //var bullet = bulletPref.Spawn(InGameManager.Instance.LevelMap);
            //bullet.transform.position = circleAttackInfo.transform.position;
            //Vector2 direction = (pointTarget - circleAttackInfo.transform.position).normalized;
            //bullet(direction, enemyBase.da);
        }
    }
}
