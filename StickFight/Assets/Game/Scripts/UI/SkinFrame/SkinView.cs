using STU;
using System;
using UnityEngine;
using UnityEngine.UI;

public class SkinView : MonoBehaviour {
    [Header("Data")]
    [SerializeField] private SkinItemData model;
    [Space]
    [SerializeField] private Button btn_Select;
    [Header("Displaye")]
    [SerializeField] private GameObject main;
    [SerializeField] private Image icon;
    [SerializeField] public DisplayObjects bgDisplay; // 0.Using, 1.Lock, 2.Unlock
    [SerializeField] public Image iconLock;
    [SerializeField] private SkinBtnBuy _getItemBtn;

    public SkinItemData Model => model;
    private Action<SkinView> _actionSelect;

    private void Awake() {
        btn_Select.onClick.AddListener(OnSelect);
    }

    private void OnEnable() {
        EventDispatcher.AddListener<EventKey.IteamChange>(HalderItemChange);
        EventDispatcher.AddListener<EventKey.EventSkinChange>(HalderSkinChange);
    }

    private void OnDisable() {
        EventDispatcher.RemoveListener<EventKey.IteamChange>(HalderItemChange);
        EventDispatcher.RemoveListener<EventKey.EventSkinChange>(HalderSkinChange);
    }

    private void HalderItemChange(EventKey.IteamChange evt) {
        if(evt.itemID == Model.ItemID) {
            Show(Model);
        }
    }

    private void HalderSkinChange(EventKey.EventSkinChange evt) {
        if(Model.ItemID == evt.IDAfter || Model.ItemID == evt.IDBefor) {
            Show(Model);
        }
    }

    public void Show(SkinItemData skinData) {
        this.model = skinData;
        if(skinData == null) {
            main.SetActive(false);
            return;
        }
        main.SetActive(true);
        icon.sprite = model.Icon;
        if(model.ItemID == DataManager.Instance.PlayerData.SkinID) {
            SetUpUsing();
        } else if(DataManager.Instance.PlayerData.Enought(model.ItemID)) {
            SetUpUnLock();
        } else {
            SetUpLock();
        }
    }

    public void SetUpUsing() {
        bgDisplay.Active(0);
        iconLock.gameObject.SetActive(false);
    }

    public void SetUpUnLock() {
        bgDisplay.Active(2);
        iconLock.gameObject.SetActive(false);
    }

    private void SetUpLock() {
        bgDisplay.Active(1);
        iconLock.gameObject.SetActive(true);
    }

    public void SetOnSelect(Action<SkinView> onSelect) {
        this._actionSelect = onSelect;
    }

    private void OnSelect() {
        _actionSelect?.Invoke(this);
    }
}
