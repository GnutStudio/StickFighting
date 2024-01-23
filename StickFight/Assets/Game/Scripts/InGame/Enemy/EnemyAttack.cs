using Spine;
using Spine.Unity;
using System;
using UnityEngine;
using DG.Tweening;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] protected EnemyAnim enemyAnim;
    [SerializeField, SpineEvent] protected string eventATK;
    [SerializeField] protected OverlapCircleAll circleAttackInfo;
    [SerializeField] protected EnemyBase enemyBase;
    [SerializeField] protected float timeDelayAttack= 1f;
    [SerializeField] protected AudioClip soundAttack;
    protected Tween tween;
    protected bool canAttack;
    public bool CanAttack => canAttack;
    protected virtual void Awake() 
    { 
        enemyAnim.Anim.AnimationState.Event += EventDamege;
        canAttack = true;
    }

    public virtual void Attack(Action callback = null) 
    {
        if(canAttack) 
        {
            canAttack = false;
            enemyBase.Rg2D.velocity = Vector2.zero;
            enemyAnim.SetAnimAttack(callback);
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

    protected virtual void EventDamege(TrackEntry trackEntry, Spine.Event e) 
    {
        if(e.Data.Name == eventATK && circleAttackInfo != null) 
        {
            SoundManager.Instance.PlaySound(soundAttack);
            Collider2D[] listCol = Physics2D.OverlapCircleAll(circleAttackInfo.transform.position, circleAttackInfo.lookRadius, circleAttackInfo.layerMask);
            foreach(var col in listCol) 
            {
                Player player = col.transform.parent.GetComponent<Player>();
                if(player != null) 
                {
                    Logs.Log("Detech player");
                    player.GetDame(enemyBase.curDame);
                }
            }
        }
    }

    private void OnDisable() {
        tween.CheckKillTween();
    }
}
