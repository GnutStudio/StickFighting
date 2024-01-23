using DG.Tweening;
using Spine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorAttack : EnemyAttack {
    [SerializeField] private float angleStart;
    [SerializeField] private BeamRayCast beamFace,beamDownward;
    [SerializeField] private Vector2 force;
    [Header("CustomerFire")]
    [SerializeField] private int timeFire;
    [SerializeField] private float timeDelay;
    [SerializeField] private BulletCS bulletPref;
    [SerializeField] private float speed;
    private Action callback;
    private bool attacking = false;
    public bool Attacking => attacking;
    private WaitForSeconds waitTimeDelay;
    private Coroutine coroutine;
    protected override void Awake() {
        base.Awake();
        waitTimeDelay = new WaitForSeconds(timeDelay);
        //gravityScaleArrow = -Physics2D.gravity.y;
    }

    private void Update() {
        if(!CheckFaceCanMove()) {
            Vector2 curVelocity = enemyBase.Rg2D.velocity;
            curVelocity.y = 0;
            enemyBase.Rg2D.velocity = curVelocity;
        }
    }

    private bool CheckFaceCanMove() {
        Collider2D collider2D = null;
        foreach(var col in beamFace.ArrayCollider2D) {
            if(col != null) {
                collider2D = col;
                break;
            }
        }
        return collider2D == null;
    }
    public override void Attack(Action callback = null) {
        if(canAttack) {
            this.callback = callback;
            canAttack = false;
            enemyBase.Rg2D.velocity = Vector2.zero;
            Vector2 forceNew = enemyBase.dirFace == DirHorizontal.RIGHT?force:new Vector2(-force.x,force.y);
            enemyBase.Rg2D.velocity = forceNew;
            enemyAnim.SetAnimAttack();
            attacking = true;
        } else {
            callback?.Invoke();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(!attacking) {
            return;
        }
        if(CheckDownWardCanMove()) {
            coroutine = StartCoroutine(SpawnerBullet());
        }
    }

    IEnumerator SpawnerBullet() 
    {
        for(int i = 0; i < timeFire; i++ ) 
        {
            BulletCS bullet = bulletPref.Spawn(InGameManager.Instance.LevelMap.transform);
            bullet.transform.position = transform.position;
            bullet.Fire(Vector2.right,speed,enemyBase.curDame);

            bullet = bulletPref.Spawn(InGameManager.Instance.LevelMap.transform);
            bullet.transform.position = transform.position;
            bullet.Fire(Vector2.left,speed,enemyBase.curDame);
            yield return waitTimeDelay;
        }
        attacking = false;
        callback?.Invoke();
        tween.CheckKillTween();
        tween = DOVirtual.DelayedCall(timeDelayAttack, () => {
            canAttack = true;
        });
    }

    protected bool CheckDownWardCanMove() {
        Collider2D collider2D = null;
        foreach(var col in beamDownward.ArrayCollider2D) {
            if(col != null) {
                collider2D = col;
                break;
            }
        }
        return collider2D != null;
    }

    protected override void EventDamege(TrackEntry trackEntry, Spine.Event e) {
        if(e.Data.Name == eventATK && circleAttackInfo != null) {
            SoundManager.Instance.PlaySound(soundAttack);
            Collider2D[] listCol = Physics2D.OverlapCircleAll(circleAttackInfo.transform.position, circleAttackInfo.lookRadius, circleAttackInfo.layerMask);
            foreach(var col in listCol) {
                Player player = col.transform.parent.GetComponent<Player>();
                if(player != null) {
                    Debug.Log("Detech player");
                    player.GetDameStun(enemyBase.curDame);
                }
            }
        }
    }

    private void OnDisable() {
        if(coroutine != null) {
            StopCoroutine(coroutine);
        }
    }
}
