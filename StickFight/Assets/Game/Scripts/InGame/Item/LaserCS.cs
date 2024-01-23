using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LaserCS : MonoBehaviour
{
    [SerializeField] private Animator animator,animatorTop;
    [SerializeField] private GameObject laser;
    [SerializeField] private float timeOn=1f,timeDelay=2f;
    [SerializeField] private ParticleSystem parTop,parBot;
    WaitForSeconds timeAnimOpen,timeAnimClose,timeAnimActive,timeAnimDelay;
    private void Start() {
        var clips = animator.runtimeAnimatorController.animationClips;
        timeAnimOpen = new WaitForSeconds(clips[1].length);
        timeAnimClose = new WaitForSeconds(clips[2].length);
        timeAnimActive = new WaitForSeconds(timeOn);
        timeAnimDelay = new WaitForSeconds(timeDelay);
        animator.SetBool("TurnOn", false);
        animatorTop.SetBool("TurnOn", false);
        laser.gameObject.SetActive(false);
        parTop.Stop();
        parBot.Stop();
        StartCoroutine(ActiveLaser());
    }

    IEnumerator ActiveLaser() {
        yield return timeAnimDelay;
        animator.SetBool("TurnOn",true);
        animatorTop.SetBool("TurnOn", true);
        yield return timeAnimOpen;
        laser.gameObject.SetActive(true);
        parTop.Play();
        parBot.Play();
        yield return timeAnimActive;
        laser.gameObject.SetActive(false);
        parTop.Stop();
        parBot.Stop();
        animator.SetBool("TurnOn", false);
        animatorTop.SetBool("TurnOn", false);
        yield return timeAnimClose;
        StartCoroutine(ActiveLaser());
    }

    private void OnDisable() {
        StopAllCoroutines();
    }
}
