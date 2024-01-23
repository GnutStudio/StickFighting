using UnityEngine;

public class FireRayCS : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rg2D;
    [SerializeField] private ParticleSystem explosion;

    private Vector2 direction;
    private float speed;
    private int dame;
    
    public void Fire(int dame) {
        this.dame = dame;
    }
    private void OnTriggerEnter2D(Collider2D collision) 
    {
        Player player = collision.transform.parent.GetComponent<Player>();
        if(player!=null) {
            player.GetDame(dame);
        }
        var exp = explosion.Spawn(InGameManager.Instance.transform);
        exp.transform.position = player.transform.position;
    }
}