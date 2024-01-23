using STU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBoss : BossBase
{
    [SerializeField] private SpiderLandSp smallSpiderPref;
    [SerializeField] private int timeForSpawner = 2;
    private int timeGetAttack = 0;
    private List<SpiderLandSp> lstSmallSpider = new List<SpiderLandSp>();
    public override void GetDame(int dame, GameObject objMakeDame = null) {
        if(curStatus == EnemyStatus.DIE || curHeart <= 0) {
            return;
        }
        curHeart -= dame;
        SpawnerSmall();
        EventDispatcher.Dispatch<EventKey.BossGetDame>(new EventKey.BossGetDame());
        if(curHeart <= 0) {
            Die(objMakeDame);
        } else {
            //curStatus = EnemyStatus.GET_DAME;
            rg2D.velocity = Vector2.zero;
            enemyAnim.SetAnimGetDame(() => {
                SetStatus(EnemyStatus.IDLE);
            });
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
        ClearSmallSpider();
    }

    private void SpawnerSmall() {
        timeGetAttack++;
        if(timeGetAttack >= timeForSpawner ) {
            var smallSpider = smallSpiderPref.Spawn(InGameManager.Instance.LevelMap.transform);
            bool Boolean  = (Random.value > 0.5f);
            smallSpider.transform.position = transform.position + transform.right * (Boolean?7f:-7f);
            timeGetAttack = 0;
            lstSmallSpider.Add(smallSpider);
        }
    }

    private void ClearSmallSpider() {
        foreach(var spider in lstSmallSpider) {
            if(spider.curHeart > 0 && spider.gameObject.activeInHierarchy ) {
                spider.GetDame(spider.curHeart);
            }
        }
    }
}
