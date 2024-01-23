using Spine;
using System;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;

public class GhastAttack : EnemyAttack
{
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private BeamRayCast beamFace;
    [SerializeField, SpineAnimation] private string animMove,animAttack,animShock,animIdle;
    [Header("Customer")]
    [SerializeField] private float highPlus;
    [SerializeField] private float speedMove;
    [SerializeField] private int timeAttack;
    [SerializeField] private float timeStun;
    [SerializeField] private BulletCS ghastBulletPref;
    [SerializeField] private Transform pointDown,pointMouth;
    [SerializeField] private float speedBullet;
    private Tween tweenGhast;
    public GhastAttack.Status CurAttackStatus;
    private int curTimeAttack;
    private Action callback;
    private bool moveHight = false;
    
    public override void Attack(Action callback = null) {
        this.callback = callback;
        CurAttackStatus = Status.SETUPHIGH;
        enemyAnim.SetAnim(0,animIdle,true);
        if(moveHight == false) {
            tweenGhast = transform.DOMove(transform.position + Vector3.up * highPlus, 1f).SetEase(Ease.Linear).OnComplete(() => {
                moveHight = true;
                SetUpMove();
            });
        } else {
            SetUpMove();
        }
        
    }

    private void SpawnerFireDown() {
        var bullet = ghastBulletPref.Spawn(InGameManager.Instance.LevelMap.transform);
        bullet.transform.position = pointDown.position;
        bullet.Fire(Vector2.down, speedBullet, enemyBase.curDame);
    }

    private void Update() {
        if(enemyBase.CurStatus != EnemyStatus.ATTACK) {
            return;
        }

        if(CurAttackStatus != Status.MOVE) { 
            return;
        }

        if(!CheckFaceCanMove()) {
            rb2D.velocity = new Vector2(0f, rb2D.velocity.y);
            enemyBase.Flip();
            curTimeAttack = timeAttack;
            AttackFire();
        }
    }

    private void SetUpMove() {
        CurAttackStatus = Status.MOVE;
        enemyAnim.SetAnim(0, animMove, true, () => {
            SpawnerFireDown();
        });
    }

    private void AttackFire() {
        if(curTimeAttack>0) {
            CurAttackStatus = Status.ATTACK;
            enemyAnim.SetAnim(0, animAttack, true, () => {
                curTimeAttack--;
                AttackFire();
            });
        } else {
            CurAttackStatus = Status.STUN;
            enemyAnim.SetAnim(0, animShock,true);
            tweenGhast = DOVirtual.DelayedCall(timeStun,()=> {
                //SetUpMove();
                  callback?.Invoke();
            });
        }
        
    }

    protected override void EventDamege(TrackEntry trackEntry, Spine.Event e) {
        if(e.Data.Name == eventATK) {
            SpawnerFireMouth();
        }
    }

    private void SpawnerFireMouth() {
        var bullet = ghastBulletPref.Spawn(InGameManager.Instance.LevelMap.transform);
        bullet.transform.position = pointMouth.position;
        Player player = InGameManager.Instance.Player;
        Vector2 direction = (player.transform.position-pointMouth.position).normalized;
        bullet.Fire(direction, speedBullet, enemyBase.curDame);
    }

    private void FixedUpdate() {
        if(CurAttackStatus == Status.MOVE) {
            rb2D.velocity = new Vector2((int)enemyBase.Display.right.x * speedMove, rb2D.velocity.y);
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

    private void OnDisable() {
        tweenGhast.CheckKillTween();
    }

    public enum Status {
        NONE,
        SETUPHIGH,
        MOVE,
        ATTACK,
        STUN
    }
}
