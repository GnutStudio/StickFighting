using STU;
using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerHorizontal))]
[RequireComponent(typeof(PlayerVertical))]
[RequireComponent(typeof(PlayerAnim))]
[RequireComponent(typeof(PlayerAttack))]

public class Player : CharacterBase {
    [SerializeField] private List<SortStatus> lstSortStatus;
    [SerializeField] private PlayerAnim playerAnim;
    [SerializeField] private PlayerAttack playerAttack;
    [SerializeField] private PlayerVertical playerVertical;
    [SerializeField] private PlayerHorizontal playerHorizontal;
    [SerializeField] private Rigidbody2D rg2D;
    [Header("Effect")]
    [SerializeField] private ParticleSystem par_NewBlood;
    [SerializeField] private ParticleSystem par_GetWeapon;
    [SerializeField] private ParticleSystem par_Heal;
    [SerializeField] private ParticleSystem parSoundPref;
    [SerializeField] private AudioClip soundGetWeapon,soundHealing;
    [Header("Custom")]
    [SerializeField] private float timeProtect;
    private PlayerData playerData => DataManager.Instance.PlayerData;
    private ItemID skinID = ItemID.SKIN_00;
    private SortStatus curStatus;
    public SortStatus CurStatus => curStatus;
    public WeaponData Weapon = null;
    public Rigidbody2D RG2D => rg2D;
    public int Dame {
        get {
            int result = curDame;
            if(Weapon != null) {
                int bonus = Mathf.RoundToInt(curDame*Weapon.Dame/100f);
                result += bonus;
            }
            return result;
        }
    }
    private bool isProtect = false;
    private Tween tween;
    protected override void Awake() {
        base.Awake();
        curStatus = new SortStatus();
        skinID = playerData.SkinID;
    }

    public void SetUpPlayer() {
        Weapon = null;
        skinID = playerData.SkinID;
        var skinData = skinID.GetDataByID() as SkinItemData;
        playerAnim.SetSkin(skinData.NameSpine);
        SetUpHeartDame();
        SetPlayerStatus(EnumPlayerStatus.IDLE);
        isProtect = false;
        EventDispatcher.Dispatch<EventKey.PlayerChange>(new EventKey.PlayerChange());
    }

    private void SetUpHeartDame() {
        var levelPlayerInfo = RuleDameAndHeart.GetTotalDameHeartCoinByLevel(playerData.LevelPlayer);
        originHeart = levelPlayerInfo.Heart;
        originDame = levelPlayerInfo.Damage;
        curHeart = originHeart;
        curDame = originDame;
    }

    public override void GetDame(int dame, GameObject objMakeDame = null) {
        if(curStatus.TypeStatus == EnumPlayerStatus.DIE || curStatus.TypeStatus == EnumPlayerStatus.WIN) {
            return;
        }
        if(curHeart <= 0) {
            return;
        }
        curHeart -= dame;
        if(curHeart <= 0) {
            var par = parSoundPref.Spawn(InGameManager.Instance.LevelMap.transform);
            par.transform.position = transform.position;
            SetPlayerStatus(EnumPlayerStatus.DIE, () => {
                InGameManager.Instance.Die();
            });
        } else {
            SetPlayerStatus(EnumPlayerStatus.GETDAME, () => {
                SetPlayerStatus(EnumPlayerStatus.IDLE);
            });
            par_NewBlood.Play();
        }
        EventDispatcher.Dispatch<EventKey.PlayerChange>(new EventKey.PlayerChange());
    }

    public void GetDameStun(int dame, GameObject objMakeDame = null, bool fall = true) {
        if(curHeart <= 0 || isProtect) {
            return;
        }
        curHeart -= dame;
        if(curHeart <= 0) {
            SetPlayerStatus(EnumPlayerStatus.DIE, () => {
                FrameManager.Instance.Push<ReviveFrame>();
            });
        } else {
            if(fall) {
                SetPlayerStatus(EnumPlayerStatus.STUN, () => {
                    SetPlayerStatus(EnumPlayerStatus.IDLE);
                });
            } else {
                SetPlayerStatus(EnumPlayerStatus.GETDAME, () => {
                    SetPlayerStatus(EnumPlayerStatus.IDLE);
                });
            }
            isProtect = true;
            tween.CheckKillTween();
            tween = DOVirtual.DelayedCall(timeProtect, () => {
                isProtect = false;
            });
            playerAnim.Flash(timeProtect);
            par_NewBlood.Play();
        }
        EventDispatcher.Dispatch<EventKey.PlayerChange>(new EventKey.PlayerChange());
    }

    public bool SetPlayerStatusCheckRank(EnumPlayerStatus typeAnim, Action callback = null) {
        SortStatus sortStatus = lstSortStatus.Find(x=>x.TypeStatus == typeAnim);
        if(sortStatus == null) {
            return false;
        }
        if(sortStatus.TypeStatus != curStatus.TypeStatus && sortStatus.Rank >= curStatus.Rank) {
            SetPlayerStatus(typeAnim, callback);
            return true;
        }
        return false;
    }

    private void SetPlayerStatus(EnumPlayerStatus status, Action callback = null) {
        SortStatus sortstatus = lstSortStatus.Find(x=> x.TypeStatus == status);
        curStatus.Set(sortstatus);
        //set up 
        if(curStatus.TypeStatus == EnumPlayerStatus.DASH) {
            playerAttack.SetUpNoneAttack();
            playerVertical.SetUpNoVertical();
        } else if(curStatus.TypeStatus == EnumPlayerStatus.GETDAME || curStatus.TypeStatus == EnumPlayerStatus.DIE || curStatus.TypeStatus == EnumPlayerStatus.WIN || curStatus.TypeStatus == EnumPlayerStatus.STUN) {
            playerAttack.SetUpNoneAttack();
            playerVertical.SetUpNoVertical();
            playerHorizontal.SetUpNoMove();
            rg2D.velocity = Vector2.zero;
        }
        playerAnim.HalderAnim(curStatus.TypeStatus, callback);
    }

    public void SetIdleCheckStatus(List<EnumPlayerStatus> lstStatus, Action callback = null) {
        if(curStatus.TypeStatus == EnumPlayerStatus.IDLE) {
            return;
        }
        bool canSet = false;
        foreach(var status in lstStatus) {
            if(status == curStatus.TypeStatus) {
                canSet = true;
                break;
            }
        }

        if(canSet) {
            SetPlayerStatus(EnumPlayerStatus.IDLE, callback);
        }
    }

    public void SetAnimCheckStatus(EnumPlayerStatus typeStatus, List<EnumPlayerStatus> lstStatus, Action callback = null) {
        if(curStatus.TypeStatus == typeStatus) {
            return;
        }
        bool canSet = false;
        foreach(var status in lstStatus) {
            if(status == curStatus.TypeStatus) {
                canSet = true;
                break;
            }
        }

        if(canSet) {
            SetPlayerStatus(typeStatus, callback);
        }
    }

    public void Healing(float percent) {
        int heart = Mathf.RoundToInt(originHeart*percent);
        curHeart += heart;
        curHeart = Mathf.Min(curHeart, originHeart);
        par_Heal.Play();
        SoundManager.Instance.PlaySound(soundHealing);
        EventDispatcher.Dispatch<EventKey.PlayerChange>(new EventKey.PlayerChange());
    }

    public void SetWeapon(WeaponData data) {
        this.Weapon = data;
        playerAnim.AddSkin(Weapon.NameSkin);
        par_GetWeapon.Play();
        SoundManager.Instance.PlaySound(soundGetWeapon);
        EventDispatcher.Dispatch<EventKey.PlayerChange>(new EventKey.PlayerChange());
    }
}

[System.Serializable]
public class SortStatus {
    public EnumPlayerStatus TypeStatus;
    public int Rank;

    public SortStatus() {
        this.TypeStatus = EnumPlayerStatus.IDLE;
        this.Rank = 0;
    }

    public SortStatus(EnumPlayerStatus typeAnim, int rank) {
        this.TypeStatus = typeAnim;
        this.Rank = rank;
    }

    public void Set(EnumPlayerStatus typeAnim, int rank) {
        this.TypeStatus = typeAnim;
        this.Rank = rank;
    }

    public void Set(SortStatus sortAnime) {
        this.TypeStatus = sortAnime.TypeStatus;
        this.Rank = sortAnime.Rank;
    }

    public void Defaul() {
        this.TypeStatus = EnumPlayerStatus.IDLE;
        this.Rank = 0;
    }
}

public enum EnumPlayerStatus {
    NONE,
    IDLE,
    RUN,
    JUMP,
    JUMPFALL,
    DASH,
    ATTACK1,
    ATTACK2,
    ATTACK3,
    CLIMB,
    DIE,
    GETDAME,
    WIN,
    STUN,
    JUMPBEFOR,
}
