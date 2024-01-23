using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
using STU;

public class ItemListener : MonoBehaviour {
    [SerializeField] private ItemID itemID;
    [SerializeField] private TextMeshProUGUI txt_Amount;
    [SerializeField] private Button btn_Shop;
    private ItemStack itemSave => DataManager.Instance.PlayerData.GetItemSaveByItemId(itemID);
    private int cur_int;
    private Tween tween;

    private void Awake() {
        if(btn_Shop!=null) {
            btn_Shop.onClick.AddListener(OpenShop);
        }
    }
    private void OnEnable() {
        EventDispatcher.AddListener<EventKey.IteamChange>(UpdateAmount);
    }

    private void OnDisable() {
        EventDispatcher.RemoveListener<EventKey.IteamChange>(UpdateAmount);
    }

    private void Start() {
        this.cur_int = itemSave.Amount;
        txt_Amount.text = this.cur_int.ToString();
    }
    
    public void UpdateAmount(EventKey.IteamChange evt) {
        tween.CheckKillTween(true);
        int amout = itemSave.Amount;
        tween = DOTween.To(() => cur_int,
            (value) => {
                txt_Amount.text = value.ToString();
            },
            amout,
            0.5f
            ).OnComplete(() => {
                this.cur_int = amout;
                txt_Amount.text = cur_int.ToString();
            });
    }

    private void OpenShop() {
        //var frame = FrameManager.Instance.GetFrameTop();
        //if(!(frame is GameFrame) && !(frame is HomeFrame)) {
        //    FrameManager.Instance.GetFrameTop().Hide();
        //}                
        FrameManager.Instance.Push<ShopFrame>();
    }
}
