using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderLand : EnemyBase {
    [SerializeField] private SpiderLandSp smallSpiderPref;
    [SerializeField] private float x,y;
    [SerializeField] private int amount;
    protected override void Die(GameObject objMakeDame = null) {
        curStatus = EnemyStatus.DIE;
        enemyAnim.SetAnimDie();
        tween = DOVirtual.DelayedCall(2f, () => {
            transform.gameObject.SetActive(false);
        });
        SpawnerCoin.Instance.Spawner(transform.position, 3);
        InGameManager.Instance.AddEnemyDie(this);
        for(int i = 0; i < amount; i++) {
            var smallSpider = smallSpiderPref.Spawn(InGameManager.Instance.LevelMap.transform,false);
            
            float xRandom = Random.Range(-x,x);
            if(xRandom <= 0) {
                smallSpider.Flip(DirHorizontal.LEFT);
            } else {
                smallSpider.Flip(DirHorizontal.RIGHT);
            }
            smallSpider.transform.position = transform.position + new Vector3(xRandom,2f,0);
            //smallSpider.MakeEnemyDelay();
            //smallSpider.Rg2D.AddForce(new Vector2(xRandom, y), ForceMode2D.Impulse);
        }
    }

    [ContextMenu("TestDie")]
    public void TestDie() {
        Die();
    }
}
