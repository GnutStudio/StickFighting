using STU;
using System.Collections.Generic;

[System.Serializable]
public class PlayerData {
    public int LevelPlayer = 0;
#if !UNLOCK_ALL
    public int LevelMap = 0;
    public List<ItemStack> eventory = new List<ItemStack>(){ new ItemStack(ItemID.SKIN_00, 0),new ItemStack(ItemID.LUCKYWEEL,3),new ItemStack(ItemID.ITEMBOW,3),new ItemStack(ItemID.ITEMSWORD,3), new ItemStack(ItemID.LIFE,3)};
#else
    public int LevelMap = 30;
    public List<ItemStack> eventory = new List<ItemStack>(){ new ItemStack(ItemID.SKIN_00, 0),new ItemStack(ItemID.LUCKYWEEL,3),new ItemStack(ItemID.ITEMBOW,3),new ItemStack(ItemID.ITEMSWORD,3), new ItemStack(ItemID.LIFE,3), new ItemStack(ItemID.COIN,999999), new ItemStack(ItemID.UNLOCKSKINS,1)};
#endif
    public SpinSave SpinSave = new SpinSave();
    public DailySave DailySave = new DailySave();
    public GiftsSave GiftsSave = new GiftsSave();
    public ItemID SkinID = ItemID.SKIN_00;

    public void AddItem(List<ItemStack> lstItemStack) {
        foreach(var itemStack in lstItemStack) {
            AddItem(itemStack);
        }
    }

    public void AddItem(IEnumerable<ItemStack> itemStacks) {
        foreach(var itemStack in itemStacks) {
            AddItem(itemStack);
        }
    }

    public void AddItem(ItemStack itemStack) {
        ItemStack result = GetItemSaveByItemId(itemStack.ItemID);
        result.Add(itemStack.Amount);
        EventDispatcher.Dispatch<EventKey.IteamChange>(new EventKey.IteamChange(itemStack.ItemID, result.Amount, itemStack.Amount));
    }

    public bool RemoveItem(ItemStack itemStack) {
        ItemStack result = GetItemSaveByItemId(itemStack.ItemID);
        if(result.Amount < itemStack.Amount) {
            return false;
        }
        result.Add(-itemStack.Amount);
        EventDispatcher.Dispatch<EventKey.IteamChange>(new EventKey.IteamChange(itemStack.ItemID, result.Amount, itemStack.Amount));
        return true;
    }

    public ItemStack GetItemSaveByItemId(ItemID itemID) {
        ItemStack result = eventory.Find(x => x.ItemID == itemID);
        if(result == null) {
            result = new ItemStack(itemID, 0);
            eventory.Add(result);
        }
        return result;
    }

    public bool GetLevelPass(int level) {
        int levelMax = DataManager.Instance.LevelMapMax;
        if(LevelMap >= levelMax) {
            LevelMap = levelMax;
            return false;
        }

        if(level + 1 > LevelMap) {
            LevelMap = level + 1;
            return true;
        }

        return false;
    }

    public bool Enought(ItemID itemID, int amout = 1) {
        ItemStack item = eventory.Find(x=>x.ItemID == itemID);
        if(item == null) {
            if(itemID.GetDataByID() is SkinItemData skinData) {
                if(Enought(ItemID.UNLOCKSKINS)) {
                    return true;
                }
            }
            return false;
        }
        return item.Amount >= amout;
    }

    public void Update() {
        DailySave.GetUpDate();
    }

    public void SetSkin(ItemID itemID) {
        var itemData = itemID.GetDataByID();
        if(itemData as SkinItemData) {
            ItemID idBefor = SkinID;
            ItemID idAfter = itemID;
            SkinID = itemID;
            EventDispatcher.Dispatch<EventKey.EventSkinChange>(new EventKey.EventSkinChange(idBefor,idAfter));
        }
    }
}
