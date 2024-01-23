using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EnableAutoRecycle : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    private void Awake() {
        if(source==null) {
            source = GetComponent<AudioSource>();
        }
    }
    private void OnEnable() {
        if(source!=null) {
            source.enabled = SoundManager.Instance.SoundEnabled;
        }
    }

    private void OnParticleSystemStopped() {
        gameObject.Recycle();
    }
}
