using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using STU;

public class HomeFrame : FrameBase {
    [SerializeField] private Button btn_Play;
    [SerializeField] private Button btn_Spin,btn_Skin,btn_Daily, btn_Levels, btn_Shop,btn_LinkGame,btn_RemoveAds,btn_Gifts;
    [SerializeField] private TextMeshProUGUI txt_Level;
    [SerializeField] private AudioClip musicMenu;
    [SerializeField] private GameObject obj_Restore;
    [SerializeField] private Button btn_Restore;
    private void Awake() {
        btn_Play.onClick.AddListener(StartGame);
        btn_Spin.onClick.AddListener(() => { FrameManager.Instance.Push<SpinFrame>(); });
        btn_Skin.onClick.AddListener(() => { TextNotify.Instance.Show("Coming soon"); }); // FrameManager.Instance.Push<SkinFrame>();
        btn_Daily.onClick.AddListener(() => { FrameManager.Instance.Push<DailyFrame>(); });
        btn_Levels.onClick.AddListener(() => { FrameManager.Instance.Push<LevelSelectFrame>(); });
        btn_Shop.onClick.AddListener(() => { TextNotify.Instance.Show("Coming soon"); }); //FrameManager.Instance.Push<ShopFrame>();
        btn_LinkGame.onClick.AddListener(() => { Application.OpenURL(@"https://apps.apple.com/us/app/red-blue-mighty-adventure/id1598253974"); });
        btn_RemoveAds.onClick.AddListener(BuyRemoveAds);
        btn_Gifts.onClick.AddListener(() => { FrameManager.Instance.Push<GiftsFrame>(); });
        btn_Restore.onClick.AddListener(() => {
            IAPManager.Instance.RestorePurchases();
        });
    }

    private void OnEnable() {
        EventDispatcher.AddListener<EventKey.IteamChange>(HalderEventItemChange);
    }

    private void OnDisable() {
        EventDispatcher.RemoveListener<EventKey.IteamChange>(HalderEventItemChange);
    }

    private void HalderEventItemChange(EventKey.IteamChange evt) {
        if(evt.itemID == ItemID.REMOVEADS && evt.curAmount > 0) {
            btn_RemoveAds.transform.parent.gameObject.SetActive(false);
        }
    }

    public override void OnShow(Action onCompleted = null, bool instant = false) {
        base.OnShow(onCompleted, instant);
        txt_Level.text = $"LEVEL {DataManager.Instance.PlayerData.LevelMap + 1}";
        SoundManager.Instance.PlayMusic(musicMenu);
        btn_RemoveAds.transform.parent.gameObject.SetActive(ItemID.REMOVEADS.GetSaveByID().Amount < 1);
#if NO_ADS
        btn_RemoveAds.transform.parent.gameObject.SetActive(false);
#endif
        ShowDailyReward();
        //AdsManager.Instance.ShowBanner(AdsManager.BannerAdPosition.Top);
        obj_Restore.SetActive(false);
#if IAP
#if(UNITY_IPHONE || UNITY_IOS)
        obj_Restore.SetActive(true);
#endif
#endif
    }

    private void StartGame() {
        //if(Application.internetReachability == NetworkReachability.NotReachable) {
        //    FrameManager.Instance.Push<ConectFrame>();
        //    return;
        //}
        GameManager.Instance.CurLevel = DataManager.Instance.PlayerData.LevelMap;
        SceneManagerLoad.Instance.LoadSceneAsyn(SceneManagerLoad.SCENE_GAME);
    }

    private void BuyRemoveAds() {
        IAPManager.Instance.BuyItem("com.metagame.removeads", (value) => {
            if(value) {
                var reward = new ItemStack(ItemID.REMOVEADS, 1);
                CollectionController.Instance.GetItemStack(reward, Vector2.zero, () => {
                    DataManager.Instance.PlayerData.AddItem(new ItemStack(ItemID.REMOVEADS, 1));
                });
            }
        });
    }

    private void ShowDailyReward() {
        if(GameManager.Instance.ShowDaily == false) {
            GameManager.Instance.ShowDaily = true;
            FrameManager.Instance.Push<DailyFrame>();
        }
    }
}
