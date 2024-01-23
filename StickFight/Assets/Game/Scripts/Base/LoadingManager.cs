using DG.Tweening;
using STU;
using UnityEngine;
using UnityEngine.UI;

public class LoadingManager : Singleton<LoadingManager> {
    //[SerializeField] private Image cur_;
    [SerializeField] private float time = 7;
    private Tween tween;
    private bool LoadFinal;
    private void OnEnable() {
        EventDispatcher.AddListener<EventKey.LoadFinal>(LoadHome);
    }

    private void OnDisable() {
        EventDispatcher.RemoveListener<EventKey.LoadFinal>(LoadHome);
        tween.CheckKillTween();
    }


    private void Start() {
        LoadFinal = false;
        tween = DOVirtual.DelayedCall(time, () => {
            LoadFinal = true;
            if(GameManager.Instance.State == GameManager.States.Started) {
                SceneManagerLoad.Instance.LoadSceneAsyn(SceneManagerLoad.SCENE_HOME);
            } else {
                Debug.Log("Load too long");
            }
        });
    }

    private void LoadHome(EventKey.LoadFinal evt) {
        if(LoadFinal == true) {
            SceneManagerLoad.Instance.LoadSceneAsyn(SceneManagerLoad.SCENE_HOME);
        }
    }
}

