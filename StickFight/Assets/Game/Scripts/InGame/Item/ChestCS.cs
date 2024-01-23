using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class ChestCS : MonoBehaviour
{
    [SerializeField] private SpineBase spine;
    [SerializeField,SpineAnimation] private string animIdle,animOpen,animOpenIdle;
    [SerializeField] private Transform point;
    [SerializeField] private AudioClip soundMusic;
    [SerializeField] private ParticleSystem par;

    public bool open;
    private void Start() {
        spine.SetAnim(0, animIdle, true);
        open = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(open) {
            return;
        }
        Player player = collision.transform.parent.GetComponent<Player>();
        if(player != null) {
            // par.Play();
            //StartCoroutine(playPar());

            open = true;
            SoundManager.Instance.PlaySound(soundMusic);
            par.Play();
            spine.SetAnim(0,animOpen,false,()=> {
                SpawnerCoin.Instance.SpawnerII(point.position,9);
                spine.SetAnim(0, animOpenIdle, true);
            });
        }
    }

    //IEnumerator playPar()
    //{
    //    var playPar = par.Spawn();
    //    playPar.transform.position = transform.position;
    //    playPar.Play();
    //    yield return new WaitForSeconds(1.5f);
    //    blink.SetActive(false);
    //    loot.SetActive(false);
    //}
}
