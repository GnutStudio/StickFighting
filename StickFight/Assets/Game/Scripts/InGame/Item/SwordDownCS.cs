using UnityEngine;
using DG.Tweening;

public class SwordDownCS : MonoBehaviour {
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Rigidbody2D rg2D;
    [SerializeField] private int dame;
    [SerializeField] private ParticleSystem particlePref;
    [SerializeField] private SpriteRenderer render;
    private bool active;
    private Tween tween;
    private void Awake() {
        active = false;
        rg2D.bodyType = RigidbodyType2D.Static;
    }

    private void Update() {
        if(active) {
            return;
        }
        var hit = Physics2D.Raycast(transform.position,transform.up * -1f,10f,layerMask);
        if(hit) {
            Player player = hit.collider.transform.parent.GetComponent<Player>();
            if(player != null) {
                active = true;
                rg2D.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        
        CharacterBase character = collision.transform.parent.GetComponent<CharacterBase>();
        if(character) {
            character.GetDame(dame);
            tween.CheckKillTween();
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Ground") 
        {
            Debug.Log("touch the ground");
            tween.CheckKillTween();
            Color color = Color.white;
            tween = DOVirtual.DelayedCall(1f, () => {
                tween = DOTween.To(() => 1f, (value) => { 
                    color.a = value;
                    render.color = color;
                }, 0f, 0.5f).SetEase(Ease.Linear).OnComplete(() => {
                    gameObject.SetActive(false);
                });
            });
        }
    }

    private void OnDisable() {
        tween.CheckKillTween();
    }
}