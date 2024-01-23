using UnityEngine;

public class SpikeDieCS : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.transform.parent.GetComponent<Player>();
        if(player != null) {
            player.GetDameStun(player.curHeart, fall: false);
        }
    }
}
