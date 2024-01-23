using UnityEngine;

public class EnemyCreeper : EnemyBase {
    public override void GetDame(int dame, GameObject objMakeDame = null) {
        if(curStatus == EnemyStatus.DIE || curHeart <= 0) {
            return;
        }
        curHeart -= dame;
        if(curHeart <= 0) {
            Die(objMakeDame);
        } else {
            rg2D.velocity = Vector2.zero;
            if(curStatus != EnemyStatus.ATTACK) {
                curStatus = EnemyStatus.ATTACK;
                enemyAttack.Attack(() => { DieByExploed(); });
            }
            enemyBar.UpdateHeart(curHeart / (float)originHeart);
            var point = Physics2D.ClosestPoint(objMakeDame.transform.position, col2D);
            if(particleBlood != null) {
                particleBlood.transform.position = point;
                particleBlood?.Play();
                particleBlood.GetComponent<AudioSource>().Play();
            }
        }
    }

    protected override bool SetStatus(EnemyStatus status) {
        if(curStatus == EnemyStatus.DIE || curStatus == status) {
            return false;
        }
        curStatus = status;
        if(curStatus == EnemyStatus.ATTACK) {
            rg2D.velocity = Vector2.zero;
            enemyAttack.Attack(() => { DieByExploed(); });
        } else if(curStatus == EnemyStatus.DETECH || curStatus == EnemyStatus.MOVE) {
            enemyAnim.SetAnimWalk();
        } else {
            enemyAnim.SetAnimWalk();
        }
        return true;
    }

    private void DieByExploed() {
        if(curStatus == EnemyStatus.DIE) {
            return;
        }
        //Debug.Log("DieByExploed");
        curStatus = EnemyStatus.DIE;
        transform.gameObject.SetActive(false);
        SpawnerCoin.Instance.Spawner(transform.position, 3);
        InGameManager.Instance.AddEnemyDie(this);
    }
}
