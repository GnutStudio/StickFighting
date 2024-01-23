using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject effect;
    [SerializeField] private ParticleSystem partUpRead;
    [SerializeField] private AudioClip sound;

    private void Start() {
        fire.SetActive(false);
        effect.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(!fire.activeInHierarchy) {
            Player player = collision.transform.parent.GetComponent<Player>();
            if(player!=null) {
                fire.SetActive(true);
                effect.SetActive(true);
                partUpRead.Play();
                SoundManager.Instance.PlaySound(sound);
                InGameManager.Instance.PositionRevive = transform.position;
            }
        }
    }
}
