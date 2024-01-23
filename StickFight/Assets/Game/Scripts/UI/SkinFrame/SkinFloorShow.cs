using EnhancedUI.EnhancedScroller;
using System;
using UnityEngine;

public class SkinFloorShow : EnhancedScrollerCellView {

    [SerializeField] private SkinView skinView1;
    [SerializeField] private SkinView skinView2;
    [SerializeField] private SkinView skinView3;
    public void Show(SkinItemData itemData1, SkinItemData itemData2, SkinItemData itemData3,Action<SkinView> OnSelect) {
        skinView1.SetOnSelect(OnSelect);
        skinView1.Show(itemData1);

        skinView2.SetOnSelect(OnSelect);
        skinView2.Show(itemData2);

        skinView3.SetOnSelect(OnSelect);
        skinView3.Show(itemData3);
    }
    
}
