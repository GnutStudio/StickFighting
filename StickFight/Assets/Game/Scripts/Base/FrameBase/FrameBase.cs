using System;
using UnityEngine;

public class FrameBase : MonoBehaviour {
    [Header("[Events]")]
    [SerializeField] private Action<FrameBase> onShowed;
    [SerializeField] private Action<FrameBase> onHidden;

    [Header("[Animation]")]
    [SerializeField] protected UIAnimation showAnimation;

    [SerializeField] protected UIAnimation hideAnimation;

    private FrameManager uiManager;
    public void Init(FrameManager uiManager) {
        this.uiManager = uiManager;
    }

    public void Show(Action onCompleted = null) {
        gameObject.SetActive(true);
        OnShow();
        onCompleted?.Invoke();
    }

    public virtual void OnShow(Action onCompleted = null, bool instant = false) {
        this.gameObject.SetActive(true);

        if(hideAnimation) {
            hideAnimation.ResetState();
        }

        if(instant || !showAnimation) {
            onCompleted?.Invoke();
        } else {
            showAnimation.Play(onCompleted);
        }
    }

    public virtual void Hide(Action onCompleted = null, bool instant = false) {
        if(showAnimation) {
            showAnimation.ResetState();
        }

        if(instant || !hideAnimation) {
            this.gameObject.SetActive(false);
            onCompleted?.Invoke();
        } else {
            hideAnimation.Play(() => {
                this.gameObject.SetActive(false);
                onCompleted?.Invoke();
            });
        }
    }


}
