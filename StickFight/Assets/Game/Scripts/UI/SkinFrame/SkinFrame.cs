using EnhancedUI.EnhancedScroller;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System.Collections;

public class SkinFrame : FrameBase, IEnhancedScrollerDelegate {
    [Header("Scroller")]
    [SerializeField] private EnhancedScroller myScroller;
    [SerializeField] private SkinFloorShow skinFloorPref;
    [SerializeField] private float hightFloor;
    [SerializeField] private SkinReview skinReview;
    [SerializeField] private NarbarManager narManager;
    [SerializeField] private SkinBtnBuy _getItemBtn;

    #region listSkin
    private List<SkinItemData> lstItemSkin = new List<SkinItemData>();
    private SkinItemData _skinUse = default;
    private List<SkinItemData> lstItemSkinUnLock = new List<SkinItemData>();
    private List<SkinItemData> lstItemSkinLock = new List<SkinItemData>();
    #endregion

    private ItemID cur_SkinID = default;
    private PlayerData player => DataManager.Instance.PlayerData;
    private void Awake() {
        narManager.CallbackChange += HalderNarBarChange;
    }

    public override void OnShow(Action onCompleted = null, bool instant = false) {
        base.OnShow(onCompleted, instant);
        //Set up skin 
        cur_SkinID = DataManager.Instance.PlayerData.SkinID;
        skinReview.Show(cur_SkinID);
        //Set up scroller
        myScroller.Delegate = this;
        GenderListSkin();
        myScroller.ReloadData();
        StartCoroutine(IERestartScroll());
    }

    IEnumerator IERestartScroll() {
        yield return null;
        myScroller.ScrollPosition = 0f;
    }



    #region Scroller 
    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex) {
        SkinFloorShow floor = scroller.GetCellView(skinFloorPref) as SkinFloorShow;
        int root = dataIndex * 3;
        floor.Show(GetElementAt(root), GetElementAt(root + 1), GetElementAt(root + 2), OnSelect);
        return floor;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex) {
        return hightFloor;
    }

    public int GetNumberOfCells(EnhancedScroller scroller) {
        int number = Mathf.CeilToInt(lstItemSkin.Count/3f);
        return number;
    }
    #endregion

    public SkinItemData GetElementAt(int index) {
        if(index >= lstItemSkin.Count) {
            return null;
        }
        return lstItemSkin.ElementAt(index);
    }

    public void OnSelect(SkinView skinview) {
        if(cur_SkinID == skinview.Model.ItemID) {
            return;
        }
        cur_SkinID = skinview.Model.ItemID;
        skinReview.Show(cur_SkinID);
    }

    public void GenderListSkin() {
        WayGetItem.PriceType type = WayGetItem.PriceType.NONE;
        int index = narManager.CurIndex;
        switch(index) {
            case 0:
                type = WayGetItem.PriceType.ITEMSTACK;
                break;
            case 1:
                type = WayGetItem.PriceType.LEVEL;
                break;
            case 2:
                type = WayGetItem.PriceType.SPECIAL;
                break;
            // case 3:
            //     type = WayGetItem.PriceType.PRESTIGE;
            //     break;
            default:
                type = WayGetItem.PriceType.NONE;
                break;
        }

        //Set up list
        _skinUse = null;
        lstItemSkinUnLock.Clear();
        lstItemSkinLock.Clear();
        ItemID skinPlayer = DataManager.Instance.PlayerData.SkinID;
        foreach(var item in DataManager.Instance.LstItem) {
            if(item is SkinItemData itemskin) {
                if(itemskin.WayGetItem.Type == type) {
                    if(itemskin.ItemID == skinPlayer) {
                        _skinUse = itemskin;
                    } else if(player.Enought(itemskin.ItemID)) {
                        lstItemSkinUnLock.Add(itemskin);
                    } else {
                        lstItemSkinLock.Add(itemskin);
                    }
                }
            }
        }
        //Gender listSkin
        lstItemSkin.Clear();
        if(_skinUse != null) {
            lstItemSkin.Add(_skinUse);
        }
        foreach(var skin in lstItemSkinUnLock) {
            lstItemSkin.Add(skin);
        }
        foreach(var skin in lstItemSkinLock) {
            lstItemSkin.Add(skin);
        }
    }

    private void HalderNarBarChange(int index) {
        GenderListSkin();
        myScroller.ReloadData();
        myScroller.ScrollPosition = 0f;
    }
}
