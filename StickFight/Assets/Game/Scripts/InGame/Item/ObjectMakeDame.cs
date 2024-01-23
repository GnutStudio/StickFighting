using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMakeDame : MonoBehaviour
{
    [SerializeField] private int damage;
    
    public void SetDame(int dame) {
        this.damage = dame;
        Debug.Log("Set dame");
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.transform.parent.GetComponent<Player>();
        if(player != null) {
            Debug.Log("get dame");
            player.GetDameStun(damage);
        }
    }
}
