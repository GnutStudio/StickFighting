using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class IAPManager {
    private void RestorePackGame(string productId) {
        if("com.metagame.removeads".Equals(productId)) {
            DataManager.Instance.PlayerData.AddItem(new ItemStack(ItemID.REMOVEADS, 1));
        }
    }
}
