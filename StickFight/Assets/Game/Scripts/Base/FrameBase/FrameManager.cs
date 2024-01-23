using STU;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FrameManager : Singleton<FrameManager> {
    [SerializeField] private FrameBase firstFrame;
    [SerializeField] private List<FrameBase> lstFrameCollectes;
    private List<FrameBase> lstFrameReleases;

    private void Awake() {
        lstFrameReleases = new List<FrameBase>();
        if(firstFrame != null) {
            FrameBase instance = Instantiate(firstFrame, transform);
            lstFrameReleases.Add(instance);
            instance.Init(this);
            instance.Show();
        }
    }

    public F Push<F>(Action<F> pushOption = null, Action onCompleted = null) where F : FrameBase {
        F frame = GetFrame<F>();
        if(frame) {
            pushOption?.Invoke(frame);
            frame.Show(onCompleted);
        }
        frame.transform.SetAsLastSibling();
        return frame;
    }

    public F GetFrame<F>() where F : FrameBase {
        foreach(FrameBase frame in lstFrameReleases) {
            if(frame is F instance) {
                return instance;
            }
        }

        foreach(FrameBase frame in lstFrameCollectes) {
            if(frame is F target) {
                F instance = Instantiate(target, transform);
                instance.Init(this);
                lstFrameReleases.Add(instance);
                return instance;
            }
        }
        return null;
    }

    public FrameBase GetFrameTop(){
        return transform.GetComponentsInChildren<FrameBase>(false).Last<FrameBase>();
    }
}
