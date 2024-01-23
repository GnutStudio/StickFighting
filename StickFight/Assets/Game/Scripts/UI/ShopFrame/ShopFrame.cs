using System;
using System.Collections.Generic;
using UnityEngine;

public class ShopFrame : FrameBase
{
    [SerializeField] private NarbarManager narManager;
    [SerializeField] private DisplayObjects displayShop; // 0.Pack, 1.Coin, 2,Skin 
    private void Awake() {
        narManager.CallbackChange += HalderNarBarChange;
    }

    public override void OnShow(Action onCompleted = null, bool instant = false)
    {
        base.OnShow(onCompleted, instant);
    }

    private void HalderNarBarChange(int index) {
        displayShop.Active(index);
    }
}
