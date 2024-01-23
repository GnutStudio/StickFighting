using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WayGetItem
{
    public PriceType Type;
    public ItemStack ItemStackDetail;
    public int LevelDetail;
    public List<SpecialType> LstTypeDetail;
    public enum PriceType {
        NONE = 0,
        ITEMSTACK = 1,
        ADS = 2,
        SPIN = 3,
        LEVEL = 4,
        SPECIAL = 5,
        PRESTIGE = 6
    }

    public enum SpecialType {
        NONE = 0,
        DAILY = 1,
        GIFTS = 2,
        SHOP = 3,
        EVENT = 4,
        SPIN = 5,
    }
}
