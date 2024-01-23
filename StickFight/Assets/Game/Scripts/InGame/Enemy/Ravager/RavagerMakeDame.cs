using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RavagerMakeDame : MonoBehaviour
{
    [SerializeField] private RavagerAttack ravagerAttack;
    [SerializeField] private RavagerBoss ranvagerBoss;
    private void OnTriggerStay2D(Collider2D collision) {
        if(ravagerAttack.AttackType == RavagerAttack.RavagerAttackType.MOVE || ravagerAttack.AttackType == RavagerAttack.RavagerAttackType.ATTACK) {
            Player player = collision.transform.parent.GetComponent<Player>();
            if(player != null) {
                player.GetDameStun(ranvagerBoss.curDame);
            }
        }
    }
}
