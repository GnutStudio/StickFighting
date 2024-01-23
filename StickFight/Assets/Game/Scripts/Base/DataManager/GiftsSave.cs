using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GiftsSave {
    public string str_Day;
    public List<GiftS> lstGift;
    public DateTime dateStart => DateTime.Parse(str_Day);

    public GiftsSave() {
        str_Day = DateTime.Today.AddDays(-1).ToString();
        lstGift = new List<GiftS>();
    }

    public bool GetAllSkin() {
        bool result = true;
        foreach(var save in lstGift) {
            if(save.GetSkin == false) {
                return false;
            }
        }
        return result;
    }

    public void Update(List<GiftData> lstGiftData) {
        str_Day = DateTime.Today.ToString();
        if(lstGift == null) {
            lstGift = new List<GiftS>();
        }
        lstGift.Clear();
        foreach(var data in lstGiftData) {
            lstGift.Add(new GiftS(data.IDSkin));
        }
    }

    public GiftS GetGiftSByItemID(ItemID id) {
        return lstGift.Find(x => x.IDSkin == id);
    }
}

[System.Serializable]
public class GiftS {
    public ItemID IDSkin;
    public int TurnWatch;
    public bool GetSkin;

    public GiftS(ItemID itemID) {
        this.IDSkin = itemID;
        this.TurnWatch = 0;
        this.GetSkin = false;
    }
}
