using UnityEngine;

public class MoveObjDame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.transform.parent.GetComponentInParent<Player>();
        if(player != null) {
            Debug.Log("get dame");
            int dame = Mathf.RoundToInt(player.OriginHeart * 0.2f);
            player.GetDameStun(dame);
        }
    }
}