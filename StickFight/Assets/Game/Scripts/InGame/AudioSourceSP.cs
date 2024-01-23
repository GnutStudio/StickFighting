using STU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceSP : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    private void Awake() {
        if(audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
    }

    private void OnEnable() {
        EventDispatcher.AddListener<EventKey.SoundChange>(HalderSoundChange);
        if(audioSource != null) {
            audioSource.enabled = SoundManager.Instance.SoundEnabled;
        }
    }

    private void OnDisable() {
        EventDispatcher.RemoveListener<EventKey.SoundChange>(HalderSoundChange);
    }

    private void HalderSoundChange(EventKey.SoundChange evt) {
        if(audioSource!=null) {
            audioSource.enabled = SoundManager.Instance.SoundEnabled;
        }
    }

    private void OnValidate() {
        if(audioSource == null) {
            audioSource = GetComponent<AudioSource>();
        }
    }
}
