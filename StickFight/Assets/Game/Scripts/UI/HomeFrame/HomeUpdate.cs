using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using STU;

public class HomeUpdate : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI txt_Level,txt_Hp,txt_Damage,txt_Coin;
    [SerializeField] private Button btn_UpByCoin,btn_UpByAds;
    private PlayerData playerData => DataManager.Instance.PlayerData;
    private LevelInfo levelPlayerInfo;
    private void Awake() {
        btn_UpByCoin.onClick.AddListener(HalderUpdateByCoin);
        btn_UpByAds.onClick.AddListener(HalderUpdateByAds);
    }

    private void OnEnable() {
        Show();
        EventDispatcher.AddListener<EventKey.IteamChange>(HalderItemChanger);
    }

    private void OnDisable() {
        EventDispatcher.RemoveListener<EventKey.IteamChange>(HalderItemChanger);
    }

    private void HalderItemChanger(EventKey.IteamChange evt) {
        if(evt.itemID == ItemID.COIN) {
            Show();
        }
    }


    private void Start() {
        Show();
    }

    private void Show() {
        levelPlayerInfo = RuleDameAndHeart.GetTotalDameHeartCoinByLevel(playerData.LevelPlayer);
        txt_Level.text = (playerData.LevelPlayer + 1).ToString();
        txt_Hp.text = levelPlayerInfo.Heart.ToString();
        txt_Damage.text = levelPlayerInfo.Damage.ToString();
        txt_Coin.text = levelPlayerInfo.Coin.ToString();

        bool enoughCoin = playerData.Enought(ItemID.COIN,levelPlayerInfo.Coin);
        btn_UpByCoin.gameObject.SetActive(enoughCoin);
        btn_UpByAds.gameObject.SetActive(!enoughCoin);

    }


    private void HalderUpdateByCoin() {
        if(playerData.RemoveItem(new ItemStack(ItemID.COIN, levelPlayerInfo.Coin))) {
            playerData.LevelPlayer++;
            Show();
            EventDispatcher.Dispatch<EventKey.EventUpdatePower>(new EventKey.EventUpdatePower());
        }
    }

    private void HalderUpdateByAds() {
        playerData.LevelPlayer++;
        Show();
        EventDispatcher.Dispatch<EventKey.EventUpdatePower>(new EventKey.EventUpdatePower()); 
    }
}
