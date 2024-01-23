using STU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : BossBase
{
    private WarriorAttack warriorAttack => enemyAttack as WarriorAttack;
    public override void GetDame(int dame, GameObject objMakeDame = null) {
        if(curStatus == EnemyStatus.DIE) {
            return;
        }
        curHeart -= dame;
        EventDispatcher.Dispatch<EventKey.BossGetDame>(new EventKey.BossGetDame());
        if(curHeart <= 0) {
            Die(objMakeDame);
        } else {
            //curStatus = EnemyStatus.GET_DAME;
            if(!warriorAttack.Attacking) {
                rg2D.velocity = Vector2.zero;
                enemyAnim.SetAnimGetDame(() => {
                    enemyAnim.AnimIdle();
                });
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
