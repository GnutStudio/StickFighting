using STU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireworkManager : Singleton<FireworkManager> {
    [SerializeField] private RectTransform rect;
    [SerializeField] private List<ParticleSystem> lstPar;
    [SerializeField] private float delayTime = 0.2f;
    [SerializeField] private Vector2 delta;
    [SerializeField] private int turnFireWork = 10;
    [SerializeField] private AudioClip soundFireWork;
    [SerializeField] private AudioClip musicWin;
    private Coroutine corutine;
    public void FireWork() {
        if(corutine != null) {
            StopCoroutine(corutine);
        }
        corutine = StartCoroutine(IEFireWork());
    }

    IEnumerator IEFireWork() {
        SoundManager.Instance.PlaySound(soundFireWork);
        SoundManager.Instance.PlayMusic();
        for(int i = 0; i < turnFireWork; i++) {
            int randomIndex = Random.Range(0,lstPar.Count);
            var par = lstPar[randomIndex].Spawn();
            par.transform.GetComponent<RectTransform>().SetParent(rect);
            float xRandom = Random.Range(-delta.x,delta.x);
            float yRandom = Random.Range(-delta.y,delta.y);
            Vector2 deltaRandom = new Vector2(xRandom,yRandom);
            par.transform.position = rect.position + (Vector3)deltaRandom;
            yield return new WaitForSeconds(delayTime);
        }
        yield return null;
    }

    [ContextMenu("TestFireWork")]
    public void TestFireWork() {
        if(corutine != null) {
            StopCoroutine(corutine);
        }
        corutine = StartCoroutine(IEFireWork());
    }

    private void OnDisable() {
        if(corutine != null) {
            StopCoroutine(corutine);
        }
    }
}
