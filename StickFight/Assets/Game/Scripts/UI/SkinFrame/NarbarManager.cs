using System;
using System.Collections.Generic;
using UnityEngine;

public class NarbarManager : MonoBehaviour
{
    [SerializeField] private List<Barbase> lstBarbase;
    [SerializeField] private int curIndex;
    public Action<int> CallbackChange;
    public int CurIndex => curIndex;
    private void Awake() {
        foreach(var bar in lstBarbase) {
            bar.Init(this);
        }
    }

    private void Start() {
        OnSelect(curIndex);
        foreach(var bar in lstBarbase) {
            bar.Select(bar.Index == curIndex);
        }
    }

    public void OnSelect(int index) {
        if(curIndex == index) {
            return;
        }
        curIndex = index;
        foreach(var bar in lstBarbase) {
            bar.Select(bar.Index == index);
        }
        CallbackChange?.Invoke(curIndex);
    }
}
