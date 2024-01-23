using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Barbase : MonoBehaviour
{
    [SerializeField] private int index;
    [SerializeField] private Button btn_Bar;
    [SerializeField] private GameObject objActive;
    [SerializeField] private NarbarManager manager;
    public int Index => index;
    private void Awake() {
        btn_Bar.onClick.AddListener(OnSelect);
    }

    public void Init(NarbarManager manager) {
        this.manager = manager;
    }

    public void OnSelect() {
        manager.OnSelect(index);
    }

    public void Select(bool select) {
        objActive.SetActive(select);
    }
}
