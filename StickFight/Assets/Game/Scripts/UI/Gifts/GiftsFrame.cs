using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiftsFrame : FrameBase {
    [SerializeField]public List<GiftData> lstGiftData;
    [SerializeField]public List<GiftsView> lstGiftView;
    private GiftsSave giftsSave => DataManager.Instance.PlayerData.GiftsSave;
    public override void OnShow(Action onCompleted = null, bool instant = false) {
        base.OnShow(onCompleted, instant);
        if(giftsSave.GetAllSkin() == true) {
            giftsSave.Update(lstGiftData);
        }
        Gender();
    }

    private void Gender() {
        int i = 0;
        foreach(var giftS in giftsSave.lstGift) {
            var giftD = GetGiftDataByID(giftS.IDSkin);
            lstGiftView[i].Show(giftS,giftD);
            i++;
        }
    }

    private GiftData GetGiftDataByID(ItemID id) {
        return lstGiftData.Find(x => x.IDSkin == id);
    }
}

[System.Serializable]
public class GiftData {
    public ItemID IDSkin;
    public int TurnAds;
}
