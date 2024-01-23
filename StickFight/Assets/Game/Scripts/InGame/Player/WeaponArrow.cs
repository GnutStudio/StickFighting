using UnityEngine;

public class WeaponArrow : MonoBehaviour {
    [SerializeField] private Rigidbody2D rg2D;
    [Header("SetupInfo")]
    [SerializeField] private Vector2 direction;
    [SerializeField] private float speed;
    [SerializeField] private int dame;
    [SerializeField] private ParticleSystem particlePref;
    public void Action(Vector2 direction, float speed, int dame) {
        this.direction = direction;
        this.speed = speed;
        this.dame = dame;
        transform.right = direction;
    }

    private void FixedUpdate() {
        rg2D.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        EnemyBase enemy = collision.transform.GetComponentInParent<EnemyBase>();
        if(enemy) 
        {
            enemy.GetDame(dame, gameObject);
        }
        if (!enemy)
        {
            this.Recycle();
        }

        if(particlePref != null) 
        {
            var par = particlePref.Spawn();
            par.transform.position = transform.position;
        }
        if(enemy==null) {
            this.Recycle();
        }
    }
}

