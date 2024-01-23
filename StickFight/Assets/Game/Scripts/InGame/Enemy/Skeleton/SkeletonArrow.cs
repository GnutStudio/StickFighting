using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Unity.Collections.LowLevel.Unsafe;

public class SkeletonArrow : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rg2D;
    [SerializeField] private ParticleSystem fire;
    private Tween tween;
    //Info
    private int dame;
    public void MoveTarget(int dame, Vector2 force) {
        fire.Play();
        this.dame = dame;
        rg2D.velocity = force;
        tween.CheckKillTween();
        tween = DOVirtual.DelayedCall(3f, () => {
            this.Recycle();
        });
    }

    private void Update() {
        Vector2 velocity = rg2D.velocity;
        float angle = Vector2.Angle(Vector2.right,rg2D.velocity);
        if(velocity.y >= 0 ) {
            transform.eulerAngles = new Vector3(0, 0, angle);
        } else {
            transform.eulerAngles = new Vector3(0, 0, -angle);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.transform.parent.GetComponent<Player>();
        if(player) {
            player.GetDame(dame);
        }
        this.Recycle();
    }

    private void OnDisable() {
        tween.CheckKillTween();
    }
}
