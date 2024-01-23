using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GiftsView : MonoBehaviour
{
    [SerializeField] private SpineBaseUI spinbase;
    [SerializeField] private DisplayObjects objBtnDisplay; //0. GetSkin, 1.AdsGetSkin;
    [SerializeField] private Button btn_Get,btn_Ads;
    [SerializeField] private TextMeshProUGUI txt_Precent;
    private GiftS giftS;
    private GiftData giftD;
    private SkinItemData skindata;
    private GiftsSave giftSave => DataManager.Instance.PlayerData.GiftsSave;
    private void Awake() {
        btn_Get.onClick.AddListener(GetSkin);
        btn_Ads.onClick.AddListener(WatchAds);
    }
    public void Show(GiftS giftS,GiftData giftD) {
        this.giftS = giftS;
        this.giftD = giftD;
        skindata = DataManager.Instance.GetItemDataByID(giftD.IDSkin) as SkinItemData;
        Gender();
    }

    private void Gender() {
        spinbase.Anim.Skeleton.SetSkin(skindata.NameSpine);
        if(giftS.GetSkin) {
            objBtnDisplay.ActiveAll(false);
        } else {
            if(giftS.TurnWatch >= giftD.TurnAds) {
                objBtnDisplay.Active(0);
            } else {
                objBtnDisplay.Active(1);
            }
        }
        txt_Precent.text = $"{giftS.TurnWatch} /{giftD.TurnAds}";
    }

    private void WatchAds() {
        var giftGet = giftSave.GetGiftSByItemID(giftD.IDSkin);
        giftGet.TurnWatch++;
        this.giftS = giftGet;
        Gender();
    }

    private void GetSkin() {
        var giftGet = giftSave.GetGiftSByItemID(giftD.IDSkin);
        giftGet.GetSkin = true;
        this.giftS = giftGet;
        Gender();
        ItemStack item = new ItemStack(giftD.IDSkin,1);
        CollectionController.Instance.GetItemStack(item,Vector2.zero,()=> {
            DataManager.Instance.PlayerData.AddItem(item);
        });
    }
}
