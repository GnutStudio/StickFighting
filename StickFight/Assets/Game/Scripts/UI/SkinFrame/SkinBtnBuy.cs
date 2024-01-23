using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SkinBtnBuy : MonoBehaviour {
    [SerializeField] private DisplayObjects disPlayBtn; //0.Use , 1.Price, 2.Lock
    [SerializeField] private Button btnUse,btnBuy;
    [SerializeField] private ItemStackView itemStackView;
    [SerializeField] private TextMeshProUGUI txtDetailLock;
    private SkinItemData dataSkin;
    private PlayerData player => DataManager.Instance.PlayerData;

    private void Start() {
        btnUse.onClick.AddListener(BtnUse);
        btnBuy.onClick.AddListener(BtnBuySkin);
    }

    public void Show(SkinItemData itemData) {
        this.dataSkin = itemData;
        SetUpShow();
    }

    private void SetUpShow() {
        if(player.SkinID == dataSkin.ItemID) {
            disPlayBtn.ActiveAll(false);
        } else if(player.Enought(dataSkin.ItemID)) {
            disPlayBtn.Active(0);
        } else {
            if(dataSkin.WayGetItem.Type == WayGetItem.PriceType.ITEMSTACK) {
                disPlayBtn.Active(1);
                itemStackView.Show(dataSkin.WayGetItem.ItemStackDetail);
            } else {
                disPlayBtn.Active(2);
                if(dataSkin.WayGetItem.Type == WayGetItem.PriceType.LEVEL) {
                    if(player.LevelMap > dataSkin.WayGetItem.LevelDetail) {
                        player.AddItem(new ItemStack(dataSkin.ItemID, 1));
                        SetUpShow();
                        return;
                    } else {
                        txtDetailLock.text = $"UnLock at Level {dataSkin.WayGetItem.LevelDetail}";
                    }
                } else if(dataSkin.WayGetItem.Type == WayGetItem.PriceType.SPECIAL) {
                    string result = "Get at: \n";
                    foreach(var type in dataSkin.WayGetItem.LstTypeDetail) {
                        result += $"{type}, ";
                    }
                    result = result.Remove(result.Length - 2, 2);
                    txtDetailLock.text = result;
                } else if(dataSkin.WayGetItem.Type == WayGetItem.PriceType.PRESTIGE) {
                    txtDetailLock.text = $"PRESTIGE";
                } else {
                    txtDetailLock.text = $"LOCK";
                }
            }
        }
    }

    private void BtnUse() {
        player.SetSkin(dataSkin.ItemID);
        SetUpShow();
    }

    private void BtnBuySkin() {
        if(player.RemoveItem(dataSkin.WayGetItem.ItemStackDetail)) {
            player.AddItem(new ItemStack(dataSkin.ItemID, 1));
            disPlayBtn.Active(1);
            SetUpShow();
        } else {
            TextNotify.Instance.Show("Not Enough");
        }
    }

}
