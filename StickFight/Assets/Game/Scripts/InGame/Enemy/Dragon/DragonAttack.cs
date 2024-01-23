using DG.Tweening;
using Spine.Unity;
using System;
using UnityEngine;

public class DragonAttack : EnemyAttack {
    [SerializeField] public DragonAttack.Status CurAttackStatus;
    [SerializeField] private Rigidbody2D rb2D;
    [SerializeField] private BeamRayCast beamFace;
    [SerializeField] private DragonFire particleFire;
    [SerializeField, SpineAnimation] private string animMove,animAttack,animShock,animIdle;
    [Header("Customer")]
    [SerializeField] private float highPlus;
    [SerializeField] private float speedMove;
    [SerializeField] private float timeStun;
    private bool moveHight = false;

    private void Start() {
        particleFire.Init(enemyBase.curDame);
        particleFire.TurnOn(false);
    }

    public override void Attack(Action callback = null) {
        CurAttackStatus = Status.SETUPHIGH;
        enemyAnim.SetAnim(0, animMove, true);
        SetUpAttack();
    }

    private void SetUpAttack() {
        CurAttackStatus = Status.ATTACK;
        enemyAnim.SetAnim(0, animAttack, true);
        particleFire.TurnOn(true);
    }

    private void Update() {
        if(CurAttackStatus != Status.ATTACK) {
            return;
        }

        if(!CheckFaceCanMove()) {
            rb2D.velocity = new Vector2(0f, rb2D.velocity.y);
            SetUpStun();
            enemyBase.Flip();
        }
    }

    private void SetUpStun() {
        CurAttackStatus = Status.STUN;
        particleFire.TurnOn(false);
        enemyAnim.SetAnim(0, animShock, true);
        tween = DOVirtual.DelayedCall(timeStun, () => {
            SetUpAttack();
        });
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

    private void FixedUpdate() {
        if(CurAttackStatus == Status.ATTACK) {
            rb2D.velocity = new Vector2((int)enemyBase.Display.right.x * speedMove, rb2D.velocity.y);
        }
    }

    public enum Status {
        NONE,
        SETUPHIGH,
        ATTACK,
        STUN
    }
}
