using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerAttack : MonoBehaviour {
    public static float TIME_DELAY_KEY = 0.5f;
    [SerializeField,SpineEvent] private string eventATK;
    [SerializeField] private Player player;
    [SerializeField] private PlayerAnim playerAnim;
    [SerializeField] private PlayerMovement playerMovemenet;
    [SerializeField] private OverlapCircleAll circleAttackHand,circleAttackSort,circleAttackHandWall,circleAttackSortWall;
    [SerializeField] private Transform positionArrow;
    [Header("ListAnim")]
    [SerializeField] private List<EnumPlayerStatus> lstStatusIdle;
    public TurnAttack TurnAttack;
    private bool inputAttack;
    public bool IsAttacking {
        get {
            bool isAttacking = false;
            foreach(var status in lstStatusIdle) {
                if(status == player.CurStatus.TypeStatus) {
                    isAttacking = true;
                    break;
                }
            }
            return isAttacking;
        }
    }
    private void Awake() {
        TurnAttack = new TurnAttack();
        playerAnim.Anim.AnimationState.Event += EventDamage;
    }

    private void Update() {
        if(player.CurStatus.TypeStatus == EnumPlayerStatus.DIE || player.CurStatus.TypeStatus == EnumPlayerStatus.WIN) {
            return;
        }
        //Input
        if(Input.GetKeyDown(KeyCode.C) || CrossPlatformInputManager.GetButtonDown("Attack")) {
            inputAttack = true;
        }
        //Halder Input 
        if(inputAttack) {
            inputAttack = false;
            if(TurnAttack.input >= 3) {
                return;
            }
            if(TurnAttack.input == 0) {
                if(player.Weapon!= null && player.Weapon.TypeWeapon == TypeWeapon.LONG ) {
                    player.SetPlayerStatusCheckRank(EnumPlayerStatus.ATTACK3, () => { SetUpNoneAttack(); });
                } else {
                    if(player.SetPlayerStatusCheckRank(EnumPlayerStatus.ATTACK1, DoAttack)) {
                        TurnAttack.input++;
                    }
                }
            } else if(player.CurStatus.TypeStatus == EnumPlayerStatus.ATTACK1 || player.CurStatus.TypeStatus == EnumPlayerStatus.ATTACK2 || player.CurStatus.TypeStatus == EnumPlayerStatus.ATTACK3) {
                TurnAttack.input++;
            }
        }
    }

    private void DoAttack() {
        if(player.CurStatus.TypeStatus == EnumPlayerStatus.ATTACK1 && TurnAttack.input > 1) {
            if(!player.SetPlayerStatusCheckRank(EnumPlayerStatus.ATTACK2, DoAttack)) {
                SetUpNoneAttack();
            }
            return;
        } else if(player.CurStatus.TypeStatus == EnumPlayerStatus.ATTACK2 && TurnAttack.input > 2) {
            player.SetPlayerStatusCheckRank(EnumPlayerStatus.ATTACK3, () => { SetUpNoneAttack(); });
            return;
        } else {
            SetUpNoneAttack();
            return;
        }
    }

    public void SetUpNoneAttack() {
        TurnAttack.Defaul();
        player.SetIdleCheckStatus(lstStatusIdle);
    }

    void EventDamage(TrackEntry trackEntry, Spine.Event e) {
        // Play some sound if the event named "footstep" fired.
        if(e.Data.Name == eventATK) {
            if(playerMovemenet.PlayerTourch == PlayerTourch.WALL) 
            {
                if(player.Weapon == null) 
                {
                    HalderEventDameByOverlapCircleAll(circleAttackHandWall,player.curDame);
                } 
                else if(player.Weapon.TypeWeapon == TypeWeapon.SORT) 
                {
                    HalderEventDameByOverlapCircleAll(circleAttackSortWall,player.Dame);
                } 
                else if(player.Weapon.TypeWeapon == TypeWeapon.LONG) 
                {
                    HalderEventDameByOverlapCircleAll(circleAttackHandWall,player.curDame);
                }
            } 
            else 
            {
                if(player.Weapon == null) 
                {
                    HalderEventDameByOverlapCircleAll(circleAttackHand,player.curDame);
                } 
                else if(player.Weapon.TypeWeapon == TypeWeapon.SORT) 
                {
                    HalderEventDameByOverlapCircleAll(circleAttackSort,player.Dame);
                }
                else if(player.Weapon.TypeWeapon == TypeWeapon.LONG) 
                {
                    HalderEventAttackLong(player.Weapon);
                }
            }
        }
    }

    private void HalderEventDameByOverlapCircleAll(OverlapCircleAll infoAttack,int dame) {
        Collider2D[] listCol = Physics2D.OverlapCircleAll(infoAttack.transform.position, infoAttack.lookRadius, infoAttack.layerMask);
        foreach(var col in listCol) {
            EnemyBase enemyBase = col.transform.GetComponentInParent<EnemyBase>();
            if(enemyBase != null) {
                enemyBase.GetDame(dame, gameObject);
                try 
                {
                    var point = Physics2D.ClosestPoint(infoAttack.transform.position,col);
                    SpawnerEffect.Instance.SpawnerEffectDame(point);
                } 
                catch 
                {
                    Debug.Log("NUll Closest");
                    SpawnerEffect.Instance.SpawnerEffectDame(infoAttack.transform.position);
                } 
            }
        }
    }

    private void HalderEventAttackLong(WeaponData data) {
        var arrow = data.ArrowPref.Spawn(InGameManager.Instance.LevelMap);
        arrow.transform.position = positionArrow.position;
        arrow.Action(positionArrow.right,data.Speed,player.Dame);
    }
}

[System.Serializable]
public class TurnAttack {
    public int input = 0;
    //public float timeCountDown = 0f;
    //public EnumPlayerAttack curAttack = EnumPlayerAttack.NONE;

    public void Defaul() {
        this.input = 0;
        //this.timeCountDown = 0f;
        //this.curAttack = EnumPlayerAttack.NONE;
    }
}


//public enum EnumPlayerAttack {
//    NONE = 0,
//    ATTACK1 = 1,
//    ATTACK2 = 2,
//    ATTACK3 = 3
//}
