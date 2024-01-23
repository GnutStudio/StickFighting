using UnityEngine;

public class DragonFire : MonoBehaviour
{
    [SerializeField] private ParticleSystem render;
    private int dame;
    public void Init(int dame) {
        this.dame = dame;
    }

    public void TurnOn(bool turnOn) {
        if(turnOn) {
            render.Play();
        } else {
            render.Stop();
        }
    }

    private void OnParticleCollision(GameObject other) {
        Player player = other.transform.GetComponent<Player>();
        dame = Mathf.RoundToInt(player.OriginHeart * 0.2f);
        if(player) {
            player.GetDameStun(dame,fall:false);
        }
    }
}
