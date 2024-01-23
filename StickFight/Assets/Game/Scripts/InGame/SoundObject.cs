using STU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : MonoBehaviour
{
    [SerializeField] private AudioSource audioSourece;
    private void OnEnable() {
        EventDispatcher.AddListener<EventKey.SoundChange>(HalderEventSoundChange);
    }

    private void OnDisable() {
        EventDispatcher.RemoveListener<EventKey.SoundChange>(HalderEventSoundChange);
    }
    private void HalderEventSoundChange(EventKey.SoundChange evt) {
        audioSourece.enabled = evt.Enable;
    }
    private void Start() {
        audioSourece.enabled = SoundManager.Instance.SoundEnabled;
    }
}
