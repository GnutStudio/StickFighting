using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using DG.Tweening;
using STU;

public class SceneManagerLoad : Singleton<SceneManagerLoad> {
    public const string SCENE_LOGO = "Logo";
    public const string SCENE_HOME = "Home";
    public const string SCENE_GAME = "Game";

    [SerializeField] private Image fade;
    [SerializeField] private float duration = 0.1f;
    [SerializeField] private float fadeDuration = 0.5f;

    private Coroutine loadingCoroutine;

    public bool IsLoading => loadingCoroutine != null;

    private void Start() {
        fade.gameObject.SetActive(false);
        Color color = Color.black;
        color.a = 0;
        fade.color = color;
    }

    public void LoadSceneAsyn(string sceneName, Action onFadeIn = null, Action onFadeOut = null) {
        if(IsLoading) {
            Debug.LogError("[SceneLoader] More than one scene loader was started");
            return;
        }
        fade.gameObject.SetActive(true);
        fade.DOColor(Color.black, 0.5f).OnComplete(() => {
            UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            StartCoroutine(LoadScene(sceneName));
        });

        //loadingCoroutine = StartCoroutine(DoLoadSceneAsyn(
        //    () => SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single)
        //    , duration
        //    , onFadeIn
        //    , onFadeOut));

    }

    IEnumerator LoadScene(string sceneName) {
        yield return new WaitUntil(() => string.Compare(sceneName, UnityEngine.SceneManagement.SceneManager.GetActiveScene().name) == 0);
        EventDispatcher.Dispatch<EventKey.SceneChange>(new EventKey.SceneChange(sceneName));
        TurnOffFade();
    }

    private void TurnOffFade() {
        fade.gameObject.SetActive(false);
        Color color = Color.black;
        color.a = 0;
        fade.color = color;
    }



    public void LoadSceneAsyn(int sceneIndex, Action onFadeIn = null, Action onFadeOut = null) {
        if(IsLoading) {
            Debug.LogError("[SceneLoader] More than one scene loader was started");
            return;
        }

        loadingCoroutine = StartCoroutine(DoLoadSceneAsyn(
            () => UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Single)
            , duration
            , onFadeIn
            , onFadeOut));
    }

    private IEnumerator DoLoadSceneAsyn(Func<AsyncOperation> loader, float duration, Action onFadeIn = null, Action onFadeOut = null) {
        SetFadeState(true);
        //SetBackgroundState(false);
        yield return StartCoroutine(DoFade(fade, 1, fadeDuration));
        //SetBackgroundState(true);
        onFadeIn?.Invoke();
        yield return StartCoroutine(DoFade(fade, 0, fadeDuration));
        AsyncOperation operation = loader?.Invoke();

        if(operation != null) {
            operation.allowSceneActivation = false;
            float elpased = 0f;

            while(!operation.isDone) {
                if(operation.progress >= 0.9f) { break; }
                elpased += Time.deltaTime;
                yield return null;
            }
            yield return new WaitForSeconds(duration - elpased);
        }
        Resources.UnloadUnusedAssets();
        GC.Collect();

        yield return StartCoroutine(DoFade(fade, 1, fadeDuration));

        if(operation != null) {
            operation.allowSceneActivation = true;
        }

        yield return null;

        //SetBackgroundState(false);
        onFadeOut?.Invoke();
        yield return StartCoroutine(DoFade(fade, 0, fadeDuration));
        SetFadeState(false);
        loadingCoroutine = null;
    }

    private IEnumerator DoFade(Graphic target, float endValue, float duration, Action onComplete = null) {
        float startValue = target.color.a;
        float elapsed = 0f;

        while(elapsed <= duration) {
            SetAlpha(target, Mathf.Lerp(startValue, endValue, elapsed / duration));
            elapsed += Time.deltaTime;
            yield return null;
        }

        SetAlpha(target, endValue);

        onComplete?.Invoke();
    }

    private void SetFadeState(bool enabled) {
        if(fade) {
            fade.enabled = enabled;
        }
    }

    private void SetAlpha(Graphic graphic, float alpha) {
        if(graphic) {
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }


}
