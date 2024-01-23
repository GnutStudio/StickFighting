using UnityEngine;

[System.Serializable]
public class ItemStack {
    [SerializeField] private ItemID itemID;
    [SerializeField] private int amount;

    public ItemID ItemID => itemID;
    public int Amount => amount;

    public ItemData ItemData {
        get {
            if(DataManager.Instance == null) {
                return null;
            }
            return DataManager.Instance.GetItemDataByID(itemID);
        }
    }

    public ItemStack(ItemID itemID, int amount) {
        this.itemID = itemID;
        this.amount = amount;
    }

    public void Add(int amout) {
        this.amount += amout;
    }

    public static ItemStack operator *(ItemStack a, int b) {
        int amoutN = a.amount * b;
        ItemStack c = new ItemStack(a.itemID,amoutN);
        return c;
    }

    public ItemData Data {
        get {
            return DataManager.Instance.GetItemDataByID(itemID);
        }
    }
}
