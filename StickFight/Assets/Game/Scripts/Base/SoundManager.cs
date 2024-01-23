using STU;
using System;
using System.Collections;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> {
    public const string SETTING_SAVE = "SETTING";
    [SerializeField] private AudioSource musicAudioSource;
    [SerializeField] private AudioSource soundAudioSource;
    [Header("Customer")]
    [SerializeField] private AudioClip audio_PressButton;
    [SerializeField] private AudioClip audio_CollectGold;
    // [SerializeField] private AudioClip audio_Attack1;
    // [SerializeField] private AudioClip audio_Attack2;
    // [SerializeField] private AudioClip audio_Attack3;
    // [SerializeField] private AudioClip audio_Slashing12;
    // [SerializeField] private AudioClip audio_Slashing3;

    private Coroutine fadeCoroutine;
    private SettingSave settingS;
    public SettingSave SettingS {
        get {
            if(settingS == null) {
                LoadData();
            }
            return settingS;
        }
    }

    public float MusicVolume {
        get {
            return Mathf.Clamp01(SettingS.MusicVolume);
        }

        set {
            float v = Mathf.Clamp01(value);
            SettingS.MusicVolume = v;
            if(musicAudioSource) musicAudioSource.volume = v;
        }
    }

    public float VolumeSound {
        get {
            return Mathf.Clamp01(SettingS.SoundVolume);
        }
        set {
            float v = Mathf.Clamp01(value);
            SettingS.SoundVolume = v;
            if(soundAudioSource) soundAudioSource.volume = v;
        }
    }

    public bool MusicEnabled {
        get {
            return SettingS.MusicEnabled;
        }

        set {
            bool curSTT = SettingS.MusicEnabled;
            if(curSTT == value) {
                return;
            }
            SettingS.MusicEnabled = value;
            if(value) PlayMusic(musicAudioSource.clip);
            else StopMusic();
        }
    }

    public bool SoundEnabled {
        get { return SettingS.SoundEnabled; }
        set { 
            SettingS.SoundEnabled = value;
            EventDispatcher.Dispatch<EventKey.SoundChange>(new EventKey.SoundChange(value));
        }
    }

    public bool VibrateEnabled {
        get { return SettingS.VibrateEnabled; }
        set { SettingS.VibrateEnabled = value; }
    }
    #region Music
    public void PlayMusic(AudioClip audioClip = null, float volumeScale = 0.5f, bool loop = true, float fadeDuration = 0.5f) {
        if(musicAudioSource == null || audioClip == null || !MusicEnabled) {
            if(musicAudioSource && audioClip) musicAudioSource.clip = audioClip;
            return;
        }

        bool isFade = fadeDuration > 0f;

        if(isFade) {
            Fade(musicAudioSource, MusicVolume, 0f, fadeDuration, () => {
                musicAudioSource.clip = audioClip;
                musicAudioSource.loop = loop;
                musicAudioSource.Play();

                Fade(musicAudioSource, 0f, MusicVolume * volumeScale);
            });
        } else {
            musicAudioSource.clip = audioClip;
            musicAudioSource.loop = loop;
            musicAudioSource.volume = MusicVolume * volumeScale;
        }
    }

    public void StopMusic(float fadeDuration = 1f, Action onComplete = null) {
        if(musicAudioSource == null) {
            onComplete?.Invoke();
            return;
        }

        bool isFade = fadeDuration > 0f;

        if(isFade) {
            Fade(musicAudioSource, 1f, 0f, fadeDuration, () => {
                musicAudioSource.Stop();
                onComplete?.Invoke();
            });
        } else {
            musicAudioSource.Stop();
            onComplete?.Invoke();
        }
    }
    #endregion
    #region Sound
    public void PlaySound(AudioClip audioClip, float volumeScale = 1f) {
        if(soundAudioSource == null || audioClip == null || !SoundEnabled)
            return;
        soundAudioSource.PlayOneShot(audioClip, volumeScale);
    }

    public void PlaySoundButton() {
        PlaySound(audio_PressButton);
    }

    public void PlaySoundGoldCollect()
    {
        PlaySound(audio_CollectGold);
    }

    // public void PlaySoundAttack1()
    // {
    //     PlaySound(audio_Slashing12);
    // }
    // public void PlaySoundAttack2()
    // {
    //     PlaySound(audio_Slashing12);
    // }
    // public void PlaySoundAttack3()
    // {
    //     PlaySound(audio_Slashing3);
    // }
    //
    // public void PlaySoundSlashing12()
    // {
    //     PlaySound(audio_Attack2);
    // }
    // public void PlaySoundSlashing3()
    // {
    //     PlaySound(audio_Attack3);
    // }
    #endregion
    #region Vibrate
    public void Vibrate() {
        if(VibrateEnabled) Handheld.Vibrate();
    }
    #endregion

    #region Helper
    private void Fade(AudioSource audio, float from, float to, float duration = 1, Action onCompleted = null) {
        if(fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        StartCoroutine(IEFadeAudioSound(audio, from, to, duration, onCompleted));
    }

    private IEnumerator IEFadeAudioSound(AudioSource audioSource, float from, float to, float duration = 1, Action onCompleted = null) {
        float elapsed = 0f;
        while(elapsed < duration) {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        audioSource.volume = to;
        if(onCompleted != null) onCompleted.Invoke();
    }
    #endregion

    private void OnApplicationQuit() {
        SaveData();
    }


    private void OnApplicationFocus(bool focus) {
        if(!focus) {
            SaveData();
        }
    }

    private void OnApplicationPause(bool pause) {
        if(pause) {
            SaveData();
        }
    }

    private void SaveData() {
        string jsSettingSave = JsonUtility.ToJson(SettingS);
        PlayerPrefs.SetString(SETTING_SAVE, jsSettingSave);
    }

    private void LoadData() {
        string jsSettingSave = PlayerPrefs.GetString(SETTING_SAVE);
        if(!string.IsNullOrEmpty(jsSettingSave)) {
            settingS = JsonUtility.FromJson<SettingSave>(jsSettingSave);
        } else {
            settingS = new SettingSave();
        }
    }
}

[System.Serializable]
public class SettingSave {
    public bool MusicEnabled;
    public bool SoundEnabled;
    public bool VibrateEnabled;
    public float MusicVolume;
    public float SoundVolume;
    public SettingSave() {
        MusicEnabled = true;
        SoundEnabled = true;
        VibrateEnabled = true;
        MusicVolume = 1f;
        SoundVolume = 1f;
    }
}
