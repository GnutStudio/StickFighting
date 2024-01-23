using STU;
using UnityEngine;

public class WitchCS : BossBase {
    [SerializeField] private GameObject land;
    private WitchAttack witchATK => enemyAttack as WitchAttack;

    protected override void Start() {
        base.Start();
        land.SetActive(false);
    }

    public override void GetDame(int dame, GameObject objMakeDame = null) {
        if(curStatus == EnemyStatus.DIE || curHeart <= 0) {
            return;
        }
        curHeart -= dame;
        EventDispatcher.Dispatch<EventKey.BossGetDame>(new EventKey.BossGetDame());
        if(curHeart <= 0) {
            land.SetActive(true);
            Die(objMakeDame);
        } else {
            if(witchATK.IsAttacking && !witchATK.IsStun) {
                //Dont do anything;
            } else {
                rg2D.velocity = Vector2.zero;
                enemyAnim.SetAnimGetDame(() => {
                    enemyAnim.AnimIdle();
                });
            }
            particleBlood?.Play();
        }
    }
}