using Spine;
using UnityEngine;

public class EnemyCreeperAttack : EnemyAttack
{
    [SerializeField] private ParticleSystem effectPref;
    protected override void EventDamege(TrackEntry trackEntry, Spine.Event e) {
        // Play some sound if the event named "footstep" fired.
        if(e.Data.Name == eventATK && circleAttackInfo != null) {
            if(enemyBase.CurStatus == EnemyStatus.DIE) {
                return;
            }
            SoundManager.Instance.PlaySound(soundAttack);
            
            var effectnew = effectPref.Spawn();
            effectnew.transform.position = transform.position;
            effectnew.Play();
            
            Collider2D[] listCol = Physics2D.OverlapCircleAll(circleAttackInfo.transform.position, circleAttackInfo.lookRadius, circleAttackInfo.layerMask);
            foreach(var col in listCol) {
                Player player = col.transform.parent.GetComponent<Player>();
                if(player != null) {
                    //Debug.Log("Detech player");
                    player.GetDameStun(enemyBase.curDame);
                    //SoundManager.Instance.Vibrate();
                }
            }
        }
    }
}
