using UnityEngine;
using DG.Tweening;
public class SpikeCS : MonoBehaviour {
    [SerializeField] private BoxCollider2D  boxCollider;
    [SerializeField] private float timeUp = 0.5f,timeDown = 1f,timeDelay = 2f;
    [SerializeField] private bool showStart;
    private Sequence mySequence;
    private int dame;

    private void Start() {
        float yStart = -boxCollider.size.y;
        transform.localPosition = new Vector3(0, yStart, 0);
        mySequence = DOTween.Sequence();
        mySequence.Append(transform.DOLocalMoveY(0, timeUp).SetEase(Ease.OutExpo));
        mySequence.Append(transform.DOLocalMoveY(yStart, timeDown).SetEase(Ease.Linear));
        mySequence.AppendInterval(timeDelay);
        mySequence.SetLoops(-1);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Player player = collision.transform.parent.GetComponent<Player>();
        dame = Mathf.RoundToInt(player.OriginHeart * 0.2f);
        if(player != null) {
            player.GetDameStun( dame, fall:false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Player player = collision.transform.parent.GetComponent<Player>();
        dame = Mathf.RoundToInt(player.OriginHeart * 0.2f);
        if(player != null) {
            player.GetDameStun( dame, fall:false);
        }
    }

    private void OnDisable() {
        mySequence.Kill();
    }

    private void OnValidate() {
        if(showStart) {
            float yStart = -boxCollider.size.y;
            transform.localPosition = new Vector3(0, yStart, 0);
        } else {
            transform.localPosition = Vector3.zero;
        }
    }
}
