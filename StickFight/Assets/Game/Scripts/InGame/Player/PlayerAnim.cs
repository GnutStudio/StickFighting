using Spine;
using Spine.Unity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : SpineBase {
    [Header("Player")]
    [SerializeField] private Player player;
    [SerializeField] private PlayerMovement playerMovement;
    [Header("NameAnime")]
    [SerializeField, SpineAnimation] private string animRun;
    [SerializeField, SpineAnimation] private string animJump,animJumpFall,animJumpBefor;
    [SerializeField, SpineAnimation] private string animClimb;
    [SerializeField, SpineAnimation] private string animDash;
    [SerializeField, SpineAnimation] private string animDie;
    [SerializeField, SpineAnimation] private string animGetDame;
    [SerializeField, SpineAnimation] private string animWin;
    [SerializeField, SpineAnimation] private string animStun;
    [Header("NameAnime Attack")]
    [SerializeField, SpineAnimation] private List<string> lstAttackNone;
    [SerializeField, SpineAnimation] private List<string> lstAttackWeapon;
    [SerializeField, SpineAnimation] private string longAttack;
    [SerializeField, SpineAnimation] private string climbAttack,climbAttackWeapon;
    [Header("Sound")]
    [SerializeField] private List<AudioClip> lstSoundATHand;
    [SerializeField] private List<AudioClip> lstSoundATSword;
    [SerializeField] private AudioClip longAttackSound;
    [SerializeField] private AudioClip DashSound;
    [SerializeField] private AudioClip ClimbSound;
    [SerializeField] private AudioClip JumpSound;
    [SerializeField] private AudioClip talkingDamage;
    [SerializeField] private AudioClip audioRun;
    [SerializeField] private AudioClip audioWin;
    [Header("Event")]
    [SerializeField,SpineEvent] private string evtStep;
    protected override void Awake() {
        //animmation.AnimationState.Complete += HandleEventComplete;
        animmation.AnimationState.Event += HalderEvent;
    }

    private void HalderEvent(TrackEntry trackEntry, Spine.Event e) {
        if(e.Data.Name == evtStep) {
            SoundManager.Instance.PlaySound(audioRun);
        }
    }

    //protected override void HandleEventComplete(TrackEntry trackEntry) {
    //    base.HandleEventComplete(trackEntry);
    //    if(trackEntry.Animation.Name == animRun) {
    //        SoundManager.Instance.PlaySound(audioRun);
    //    }
    //}

    public void HalderAnim(EnumPlayerStatus typeAnim, Action callback = null) {
        switch(typeAnim) {
            case EnumPlayerStatus.IDLE:
                SetAnim(0, animIdle, true, callback);
                break;
            case EnumPlayerStatus.RUN:
                SetAnim(0, animRun, true);
                break;
            case EnumPlayerStatus.DASH:
                SetAnim(0, animDash, true, callback);
                SoundManager.Instance.PlaySound(DashSound);
                break;
            case EnumPlayerStatus.JUMP:
                SetAnim(0, animJump, true, callback);
                break;
            case EnumPlayerStatus.JUMPFALL:
                SetAnim(0, animJumpFall, true, callback);
                break;
            case EnumPlayerStatus.ATTACK1:
                HalderAnimAttack(EnumPlayerStatus.ATTACK1, callback);
                break;
            case EnumPlayerStatus.ATTACK2:
                HalderAnimAttack(EnumPlayerStatus.ATTACK2, callback);
                break;
            case EnumPlayerStatus.ATTACK3:
                HalderAnimAttack(EnumPlayerStatus.ATTACK3, callback);
                break;
            case EnumPlayerStatus.CLIMB:
                SetAnim(0, animClimb, false, callback);
                SoundManager.Instance.PlaySound(ClimbSound);
                break;
            case EnumPlayerStatus.DIE:
                SetAnim(0, animDie, false, callback);
                SoundManager.Instance.PlaySound(talkingDamage);
                break;
            case EnumPlayerStatus.GETDAME:
                SetAnim(0, animGetDame, false, callback);
                SoundManager.Instance.PlaySound(talkingDamage);
                break;
            case EnumPlayerStatus.WIN:
                SetAnim(0, animWin, false, callback);
                SoundManager.Instance.PlaySound(audioWin);
                break;
            case EnumPlayerStatus.STUN:
                SetAnim(0, animStun, false, callback);
                SoundManager.Instance.PlaySound(talkingDamage);
                break;
            case EnumPlayerStatus.JUMPBEFOR:
                SetAnim(0, animJumpBefor, false, callback);
                SoundManager.Instance.PlaySound(JumpSound);
                break;
            default:
                SetAnim(0, animIdle, true, callback);
                break;
        }
    }

    public string GetStringAnimByWeapon(WeaponData data, int index) {
        if(data == null) {
            return lstAttackNone[index];
        } else if(data.TypeWeapon == TypeWeapon.SORT) {
            return lstAttackWeapon[index];
        } else if(data.TypeWeapon == TypeWeapon.LONG) {
            return longAttack;
        } else {
            return lstAttackNone[index];
        }
    }

    public AudioClip GetAudioClipByWeaponIndex(WeaponData data, int index)
    {
        if(data == null) 
        {
            return lstSoundATHand[index];
        } 
        else if(data.TypeWeapon == TypeWeapon.SORT)
        {
            return lstSoundATSword[index];
        } 
        else if(data.TypeWeapon == TypeWeapon.LONG) 
        {
            return longAttackSound;
        } 
        else 
        {
            return lstSoundATHand[index];
        }
    }

    public void HalderAnimAttack(EnumPlayerStatus enumPlayerStatus, Action callback) {
        if(enumPlayerStatus != EnumPlayerStatus.ATTACK1 && enumPlayerStatus != EnumPlayerStatus.ATTACK2 && enumPlayerStatus != EnumPlayerStatus.ATTACK3) {
            return;
        }

        if(playerMovement.PlayerTourch == PlayerTourch.WALL) {
            if(player.Weapon == null || player.Weapon.TypeWeapon == TypeWeapon.LONG) {
                SetAnim(0, climbAttack, false, callback);
                SoundManager.Instance.PlaySound(lstSoundATHand[1]);
            } else {
                SetAnim(0, climbAttackWeapon, false, callback);
                SoundManager.Instance.PlaySound(lstSoundATSword[2]);
            }
        } else {
            if(enumPlayerStatus == EnumPlayerStatus.ATTACK1) 
            {
                SetAnim(0, GetStringAnimByWeapon(player.Weapon, 0), false, callback);
                SoundManager.Instance.PlaySound(GetAudioClipByWeaponIndex(player.Weapon, 0));
            } 
            else if(enumPlayerStatus == EnumPlayerStatus.ATTACK2) 
            {
                SetAnim(0, GetStringAnimByWeapon(player.Weapon, 1), false, callback);
                SoundManager.Instance.PlaySound(GetAudioClipByWeaponIndex(player.Weapon, 1));
            } 
            else 
            {
                SetAnim(0, GetStringAnimByWeapon(player.Weapon, 2), false, callback);
                SoundManager.Instance.PlaySound(GetAudioClipByWeaponIndex(player.Weapon, 2));
            }
        }
    }

    public void SetSkin(params string[] lstSkin) {
        Skin skin = new Skin("Skin");
        foreach(string nameSkin in lstSkin) {
            skin.AddSkin(Anim.Skeleton.Data.FindSkin(nameSkin));
        }
        Anim.Skeleton.SetSkin(skin);
        Anim.Skeleton.SetSlotsToSetupPose();
    }

    public void AddSkin(string skinAdd) {
        Skin skin = Anim.Skeleton.Skin;
        skin.AddSkin(Anim.Skeleton.Data.FindSkin(skinAdd));
        Anim.Skeleton.SetSkin(skin);
        Anim.Skeleton.SetSlotsToSetupPose();
        //Anim.Skeleton.Skin.AddSkin(Anim.Skeleton.Data.FindSkin(skinAdd));
    }
}




