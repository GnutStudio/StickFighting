using STU;
using UnityEngine;

public class Dragon : BossBase
{
    private DragonAttack bossAttack => enemyAttack as DragonAttack;
    public override void GetDame(int dame, GameObject objMakeDame = null) {
        if(curStatus == EnemyStatus.DIE || curHeart <= 0) {
            return;
        }
        curHeart -= dame;
        EventDispatcher.Dispatch<EventKey.BossGetDame>(new EventKey.BossGetDame());
        
        if(curHeart <= 0) {
            Die(objMakeDame);
        } 
        else 
        {
            //curStatus = EnemyStatus.GET_DAME;
            rg2D.velocity = Vector2.zero;
            if(bossAttack.CurAttackStatus != DragonAttack.Status.ATTACK) 
            {
                enemyAnim.SetAnimGetDame(() => 
                {
                    enemyAnim.AnimIdle();
                });
            }
            var point = Physics2D.ClosestPoint(objMakeDame.transform.position, col2D);
            if(particleBlood != null) 
            {
                particleBlood.transform.position = point;
                particleBlood?.Play();
                particleBlood.GetComponent<AudioSource>().Play();
            }
        }
    }
}
