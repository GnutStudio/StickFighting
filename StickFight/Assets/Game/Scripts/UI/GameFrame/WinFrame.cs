using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Spine.Unity;

public class WinFrame : FrameBase {
    [SerializeField] private ItemStackView itemCoinView;
    [SerializeField] private Button btn_Chest,btn_Home,btn_Replay,Btn_NextLevel;
    [SerializeField] private SpineBaseUI spineChest;
    [SerializeField,SpineAnimation] private string Idle,Open,Open_Idle;
    private PlayerData playerData => DataManager.Instance.PlayerData;
    private void Awake() {
        btn_Chest.onClick.AddListener(HalderOpenChest);
        btn_Home.onClick.AddListener(HalderHome);
        btn_Replay.onClick.AddListener(HalderReplay);
        Btn_NextLevel.onClick.AddListener(HalderNextLevel);
    }
    public override void OnShow(Action onCompleted = null, bool instant = false) {
        base.OnShow(onCompleted, instant);
        //GetReward
        FireworkManager.Instance.FireWork();
        ItemStack rewardInGame = new ItemStack(ItemID.COIN, InGameManager.Instance.CoinInGame);
        itemCoinView.Show(rewardInGame);
        btn_Chest.gameObject.SetActive(true);
        spineChest.SetAnim(0, Idle, true);
        //AdsManager.Instance.ShowInterstitial();
    }

    private void HalderOpenChest() {
        btn_Chest.gameObject.SetActive(false);
        spineChest.SetAnim(0, Open, false, () => {
            spineChest.SetAnim(0, Open_Idle, true);
            CollectionController.Instance.GetItemStack(itemCoinView.Model, Camera.main.WorldToScreenPoint(btn_Chest.transform.position), () => {
                DataManager.Instance.PlayerData.AddItem(itemCoinView.Model);
            });
        });
    }

    private void HalderHome() {
        //AdsManager.Instance.ShowInterstitial();
        SceneManagerLoad.Instance.LoadSceneAsyn(SceneManagerLoad.SCENE_HOME);
    }

    private void HalderReplay() {
        GameManager.Instance.CurLevel = InGameManager.Instance.LevelMap.Level;
        InGameManager.Instance.SetupNewGame();
        Hide();
    }

    private void HalderNextLevel() {
        //if(Application.internetReachability == NetworkReachability.NotReachable) {
        //    FrameManager.Instance.Push<ConectFrame>();
        //    return;
        //}
        int levelNext = InGameManager.Instance.LevelMap.Level+1;
        if(levelNext>DataManager.Instance.LevelMapMax) {
            levelNext = DataManager.Instance.LevelMapMax;
        }
        GameManager.Instance.CurLevel = levelNext;
        InGameManager.Instance.SetupNewGame();
        Hide();
    }
}
