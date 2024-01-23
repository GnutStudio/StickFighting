using Spine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpiderBossAttack : EnemyAttack
{
    [SerializeField] private BulletCS bulletPref;
    [SerializeField] private float speed = 5f;
    protected override void EventDamege(TrackEntry trackEntry, Spine.Event e) {
        if(e.Data.Name == eventATK && circleAttackInfo != null) {
            SoundManager.Instance.PlaySound(soundAttack);
            var bullet = bulletPref.Spawn(InGameManager.Instance.LevelMap.transform);
            bullet.transform.position = circleAttackInfo.transform.position;
            bullet.Fire(circleAttackInfo.transform.right,speed,enemyBase.curDame);
        }
    }

}
