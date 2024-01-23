using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemEnemy : EnemyBase
{
    private GolemAttack golemAttack => enemyAttack as GolemAttack;


    public override void GetDame(int dame, GameObject objMakeDame = null) {
        if(curStatus == EnemyStatus.DIE || curHeart <= 0) {
            return;
        }
        curHeart -= dame;
        Vector3 randomPos = new Vector3(Random.Range(-0.5f,0.5f),Random.Range(-0.5f,0.5f),0);
        SpawnerTextDame.Instance.Spawner(transform.position + randomPos, dame.ToString());
        if(curHeart <= 0) {
            Die(objMakeDame);
        } else {
            if(curStatus != EnemyStatus.ATTACK) {
                curStatus = EnemyStatus.GET_DAME;
                enemyAnim.SetAnimGetDame(() => {
                    SetStatus(EnemyStatus.IDLE);
                });
            }
            rg2D.velocity = Vector2.zero;
            enemyBar.UpdateHeart(PercentHeart);
            var point = Physics2D.ClosestPoint(objMakeDame.transform.position, col2D);
            if(particleBlood != null) {
                particleBlood.transform.position = point;
                particleBlood?.Play();
                particleBlood.GetComponent<AudioSource>().Play();
            }
        }

    }

    protected override void Die(GameObject objMakeDame = null) {
        base.Die(objMakeDame);
        golemAttack.GolemDie();
    }
}
