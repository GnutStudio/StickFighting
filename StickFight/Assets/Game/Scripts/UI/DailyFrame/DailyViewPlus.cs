using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DailyViewPlus : DailyView
{
    [SerializeField] private GameObject disPlaySkin;
    [SerializeField] private SkeletonGraphic skeletonGraphic;
    public override void Show(ItemStack itemStack) {
        if(itemStack.ItemID.GetDataByID() is SkinItemData skin) {
            if(DataManager.Instance.PlayerData.Enought(itemStack.ItemID)) {
                disPlaySkin.SetActive(false);
                itemStackV.gameObject.SetActive(true);
                this.reward = new ItemStack(ItemID.COIN, 300);
                itemStackV.Show(this.reward);
                this.status = GetStatus();
                Show(this.status);
            } else {
                disPlaySkin.SetActive(true);
                itemStackV.gameObject.SetActive(false);
                skeletonGraphic.Skeleton.SetSkin(skin.NameSpine);
                this.reward = itemStack;
                this.status = GetStatus();
                Show(this.status);
            }
        } else {
            this.reward = itemStack;
            itemStackV.Show(itemStack);
            this.status = GetStatus();
            Show(this.status);
        }
    }
}
