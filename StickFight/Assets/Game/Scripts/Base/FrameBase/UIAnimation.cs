using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimation : MonoBehaviour {

    private class UITransitionComparer : IComparer<UITransition> {
        public int Compare(UITransition x, UITransition y) {
            return y.TotalDuration.CompareTo(x.TotalDuration);
        }
    }

    [Header("[Transitions]")]
    [SerializeField] private UITransition[] transitions;
    private static readonly UITransitionComparer comparer = new UITransitionComparer();

    private void Reset() {
        //transitions = GetComponents<UITransition>();
    }

    private void OnValidate() {
        Reset();
    }

    public void Initialize() {
        System.Array.Sort(transitions, comparer);
    }

    public void ResetState() {
        foreach(UITransition transition in transitions) {
            transition.Stop();
            transition.ResetState();
        }
    }

    public void Stop() {
        foreach(UITransition transition in transitions) {
            transition.Stop();
        }
    }

    public void Play() {
        Play(null);
    }

    public void Play(System.Action onCompleted) {
        Stop();

        if(transitions.Length <= 0) {
            onCompleted?.Invoke();
        } else {
            transitions[0].DoTransition(onCompleted);
            for(int i = 1; i < transitions.Length; i++) {
                transitions[i].DoTransition(null);
            }
        }
    }
}
