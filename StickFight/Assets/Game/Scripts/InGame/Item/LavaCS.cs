using UnityEngine;

public class LavaCS : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision) {
        Player player = collision.transform.GetComponent<Player>();
        if(player != null) {
            player.GetDame(player.curHeart);
        }
    }
}
