using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingFrame : FrameBase
{
    [SerializeField] private Button btn_Close,btn_Home,btn_Replay;
    [SerializeField] private Toggle toggleMusic,toggleSound,toggleVibrate;
    private void Awake() {
        btn_Close.onClick.AddListener(()=> {
            Hide();
        });
        toggleMusic.onValueChanged.AddListener(HalderValueMusicChange);
        toggleSound.onValueChanged.AddListener(HalderValueSoundChange);
        toggleVibrate.onValueChanged.AddListener(HalderValueVibrateChange);
        btn_Home.onClick.AddListener(()=> {
            Hide();
            ///AdsManager.Instance.ShowInterstitial();
            SceneManagerLoad.Instance.LoadSceneAsyn(SceneManagerLoad.SCENE_HOME);
        });
        btn_Replay.onClick.AddListener(()=> {
            Hide();
            InGameManager.Instance.SetupNewGame();
        });
    }
    public override void OnShow(Action onCompleted = null, bool instant = false) {
        base.OnShow(onCompleted, instant);
        Time.timeScale = 0f;
        toggleMusic.isOn = SoundManager.Instance.MusicEnabled;
        toggleSound.isOn = SoundManager.Instance.SoundEnabled;
        toggleVibrate.isOn = SoundManager.Instance.VibrateEnabled;
    }

    public override void Hide(Action onCompleted = null, bool instant = false) {
        base.Hide(onCompleted, instant);
        Time.timeScale = 1f;
    }

    private void HalderValueMusicChange(bool value) {
        SoundManager.Instance.MusicEnabled = value;
    }

    private void HalderValueSoundChange(bool value) {
        SoundManager.Instance.SoundEnabled = value;
    }

    private void HalderValueVibrateChange(bool value) {
        SoundManager.Instance.VibrateEnabled = value;
    }
}
