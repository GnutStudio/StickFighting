using STU;
using System;
using UnityEngine;
using UnityEngine.UI;

public class GameFrame : FrameBase
{
    [SerializeField] private LifeAndHeart lifeAndHeart;
    [SerializeField] private Button btn_Setting;
    [SerializeField] private BossBarHeart bossBarHeart;
    [SerializeField] private RoomSetting roomSetting;

    public BossBarHeart BossBarHeart => bossBarHeart;
    public RoomSetting RoomSetting => roomSetting;
    private void Awake() {
        btn_Setting.onClick.AddListener(() => {
            FrameManager.Instance.Push<SettingFrame>();
        });   
    }

    private void OnEnable() {
        EventDispatcher.AddListener<EventKey.EventSetupNewGame>(HalderEventSetupNewGame);
    }

    private void OnDisable() {
        EventDispatcher.RemoveListener<EventKey.EventSetupNewGame>(HalderEventSetupNewGame);
    }

    private void HalderEventSetupNewGame(EventKey.EventSetupNewGame evt) {
        bossBarHeart.StartActive(false);
        roomSetting.StartActive(true);
    }
}
