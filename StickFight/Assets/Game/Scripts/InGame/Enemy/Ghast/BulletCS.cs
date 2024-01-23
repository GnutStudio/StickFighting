using UnityEngine;

public class BulletCS : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rg2D;
    [SerializeField] private ParticleSystem explosion;

    private Vector2 direction;
    private float speed;
    private int dame;
    
    public void Fire(Vector2 direction, float speed, int dame) {
        this.direction = direction;
        this.speed = speed;
        this.dame = dame;
        transform.right = direction;
    }

    private void FixedUpdate() {
        rg2D.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.transform.parent.GetComponent<BossBase>()) {
            return;
        }

        if(collision.GetComponent<BulletCS>()) {
            return;
        }

        Player player = collision.transform.parent.GetComponent<Player>();
        if(player!=null)
        {
            dame = Mathf.RoundToInt(player.OriginHeart * 0.15f);
            player.GetDame(dame);
        }
        this.Recycle();
        var exp = explosion.Spawn(InGameManager.Instance.transform);
        exp.transform.position = transform.position;
    }
}
