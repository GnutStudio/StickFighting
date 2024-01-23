using Spine;
using Spine.Unity;
using STU;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSpinUI : MonoBehaviour {
    [SerializeField] private Button btn_Player;
    [SerializeField] private SkeletonGraphic skeleton;
    [SerializeField, SpineAnimation] private string animIdle;
    [SerializeField, SpineAnimation] private List<string> lstAnim;
    [SerializeField] private ParticleSystem parUpPower;
    [SerializeField, SpineAnimation] private string animWin;
    [SerializeField] private AudioClip soundUpdate;
    [SerializeField] private Action evtComplate;
    private Coroutine coroutine;
    private WaitForSeconds wTimeDelay;
    private void Awake() {
        skeleton.AnimationState.Complete += HandleEventComplete;
        btn_Player.onClick.AddListener(HalderOnSelect);
        wTimeDelay = new WaitForSeconds(6f);
    }

    private void OnEnable() {
        EventDispatcher.AddListener<EventKey.EventSkinChange>(EventSkinChange);
        EventDispatcher.AddListener<EventKey.EventUpdatePower>(EventUpdatePower);
        Show();
        if(lstAnim != null && lstAnim.Count > 0) {
            coroutine = StartCoroutine(IEAutoAnimPlayer());
        }
    }

    private void OnDisable() {
        EventDispatcher.RemoveListener<EventKey.EventSkinChange>(EventSkinChange);
        EventDispatcher.RemoveListener<EventKey.EventUpdatePower>(EventUpdatePower);
        if(coroutine != null) {
            StopCoroutine(coroutine);
        }
    }

    private void HandleEventComplete(TrackEntry trackEntry) {
        evtComplate?.Invoke();
        evtComplate = null;
    }

    private void HalderOnSelect() {
        if(lstAnim == null || lstAnim.Count == 0) {
            return;
        }
        int indexRandom = UnityEngine.Random.Range(0,lstAnim.Count);
        skeleton.AnimationState.SetAnimation(0, lstAnim[indexRandom], false);
        evtComplate = () => { skeleton.AnimationState.SetAnimation(0, animIdle, true); };
    }

    private void Show() {
        var skinCur = DataManager.Instance.PlayerData.SkinID.GetDataByID() as SkinItemData;
        skeleton.Skeleton.SetSkin(skeleton.SkeletonData.FindSkin(skinCur.NameSpine));
        skeleton.AnimationState.SetAnimation(0, animIdle, true);
    }

    private void EventSkinChange(EventKey.EventSkinChange evt) {
        Show();
    }

    private void EventUpdatePower(EventKey.EventUpdatePower evt) {
        if(parUpPower != null) {
            parUpPower.Play();
        }
        if(string.IsNullOrEmpty(animWin) == false) {
            skeleton.AnimationState.SetAnimation(0, animWin, false);
        }
        SoundManager.Instance.PlaySound(soundUpdate);
    }

    IEnumerator IEAutoAnimPlayer() {
        do {
            HalderOnSelect();
            yield return wTimeDelay;
        } while(true);
    }
}
