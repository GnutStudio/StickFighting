using System;
using Spine.Unity;
using UnityEngine;

public class SkinReview : MonoBehaviour
{
    [SerializeField] private SkeletonGraphic graphic;
    [SerializeField] private SkinBtnBuy skinBtnBuy;
    private SkinItemData skinData;

    public void Show(ItemID itemID) {
        skinData = DataManager.Instance.GetItemDataByID(itemID) as SkinItemData;
        graphic.Skeleton.SetSkin(skinData.NameSpine);
        skinBtnBuy.Show(skinData);
    }
}
