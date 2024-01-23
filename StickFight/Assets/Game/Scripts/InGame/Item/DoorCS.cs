using STU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;

public class DoorCS : MonoBehaviour
{
    [SerializeField] private Collider2D col2D;
    [SerializeField] private SpineBase spineDoor;
    [SerializeField,SpineAnimation] private string animClose,animOpen;
    [SerializeField] private ParticleSystem par;
    private Tween tweenMovePlayer;
    private void Start() {
        col2D.enabled = false;
        spineDoor.SetAnim(0,animClose,true);
    }

    private void OnEnable() {
        EventDispatcher.AddListener<EventKey.EnemyDie>(HalderEvent);
    }

    private void OnDisable() {
        EventDispatcher.RemoveListener<EventKey.EnemyDie>(HalderEvent);
        //tween.CheckKillTween();
        tweenMovePlayer.CheckKillTween();
    }

    private void HalderEvent(EventKey.EnemyDie evt) {
        if(InGameManager.Instance.KillAllEnemy) {
            col2D.enabled = true;
            spineDoor.SetAnim(0, animOpen, true);
            par.Play();
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.transform.parent.GetComponent<Player>();
        if(player != null) {
            tweenMovePlayer.CheckKillTween();
            tweenMovePlayer = player.transform.DOMoveX(transform.position.x, 1f).OnComplete(() => {
                InGameManager.Instance.FinishMap();
            });
            player.SetPlayerStatusCheckRank(EnumPlayerStatus.WIN);
        }
    }
}
