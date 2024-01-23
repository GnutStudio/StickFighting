using STU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhastBoss : BossBase
{
    private GhastAttack gastAttack => enemyAttack as GhastAttack;

    protected override void Update() 
    {
        if(curStatus == EnemyStatus.IDLE || curStatus == EnemyStatus.MOVE || curStatus == EnemyStatus.NONE || curStatus == EnemyStatus.DETECH || curStatus == EnemyStatus.GET_DAME) 
        {
            afterUpdateStatus = EnemyStatus.MOVE;
            SetupStatus();
            SetStatus(afterUpdateStatus);
        }
    }
    public override void GetDame(int dame, GameObject objMakeDame = null) 
    {
        if(curStatus == EnemyStatus.DIE || curHeart <= 0) 
        {
            return;
        }
        curHeart -= dame;
        EventDispatcher.Dispatch<EventKey.BossGetDame>(new EventKey.BossGetDame());
        if(curHeart <= 0) 
        {
            rg2D.velocity = Vector3.zero;
            Die(objMakeDame);
        } 
        else 
        {
            rg2D.velocity = Vector2.zero;
            if(gastAttack.CurAttackStatus != GhastAttack.Status.MOVE && gastAttack.CurAttackStatus != GhastAttack.Status.ATTACK) 
            {
                enemyAnim.SetAnimGetDame();
            }
            var point = Physics2D.ClosestPoint(objMakeDame.transform.position, col2D);
            if(particleBlood != null) {
                particleBlood.transform.position = point;
                particleBlood?.Play();
                particleBlood.GetComponent<AudioSource>().Play();
            }
        }
    }
}
