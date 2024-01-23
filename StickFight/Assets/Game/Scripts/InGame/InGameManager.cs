using Com.LuisPedroFonseca.ProCamera2D;
using STU;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameManager : Singleton<InGameManager> {
    [SerializeField] private Player player;
    [SerializeField] private AudioClip musicInGame;
    public Player Player => player;
    [Header("Edit")]
    public bool Edit;
    [Header("GetByCode")]
    public LevelMap LevelMap;
    public int EnemyKilled;
    public Vector3 PositionRevive;
    public int CoinInGame = 0;
    public bool KillAllEnemy => EnemyKilled >= LevelMap.NumberEnemy;
    private DateTime startPlayGame;
    private void Start() {
        SetupNewGame();
        SoundManager.Instance.PlayMusic(musicInGame);
    }

    public void SetupNewGame() {
        PositionRevive = Vector3.zero;
        EnemyKilled = 0;
        CoinInGame = 0;
        startPlayGame = DateTime.Now;
        player.transform.position = PositionRevive;
        player.SetUpPlayer();
        if(LevelMap != null && !Edit) {
            Destroy(LevelMap.gameObject);
        }
        SetUpMap();
        EventDispatcher.Dispatch<EventKey.EventSetupNewGame>(new EventKey.EventSetupNewGame());
        //AdsManager.Instance.ShowInterstitial();
    }

    private void SetUpMap() {
        if(Edit) {
            return;
        }
        var mapPref = DataManager.Instance.GetlevelMapByLevel(GameManager.Instance.CurLevel);
        LevelMap = Instantiate(mapPref, transform);
        LevelMap.transform.localPosition = Vector3.zero;
    }

    public void AddEnemyDie(EnemyBase enemy) {
        EnemyKilled += 1;
        EventDispatcher.Dispatch<EventKey.EnemyDie>(new EventKey.EnemyDie(enemy));
    }

    public void FinishMap() {
        DataManager.Instance.PlayerData.GetLevelPass(LevelMap.Level);
        var WinFrame =  FrameManager.Instance.Push<WinFrame>(onCompleted:()=> {
            ItemStack rewardInGame = new ItemStack(ItemID.COIN, InGameManager.Instance.CoinInGame);
            CollectionController.Instance.GetItemStack(rewardInGame, new Vector2(Screen.width/2,Screen.height/2), () => {
                DataManager.Instance.PlayerData.AddItem(rewardInGame);
            });
        });
        TimeSpan timePlay = DateTime.Now-startPlayGame;
        //GameFirebase.LogEvent(GameEvent.Create(LevelMap.Level.ToString()).Add("Result", "Win").Add("Duration", timePlay.Seconds.ToString()));
    }

    public void Revived() {
        player.transform.position = PositionRevive;
        player.SetUpPlayer();
    }

    public void Die() {
        TimeSpan timePlay = DateTime.Now-startPlayGame;
        ///GameFirebase.LogEvent(GameEvent.Create(LevelMap.Level.ToString()).Add("Result", "Die").Add("Duration", timePlay.Seconds.ToString()));
        if(DataManager.Instance.PlayerData.RemoveItem(new ItemStack(ItemID.LIFE, 1))) {
            InGameManager.Instance.Revived();
        } else {
            FrameManager.Instance.Push<ReviveFrame>();
        }
    }

    public void Lose() {
        TimeSpan timePlay = DateTime.Now-startPlayGame;
        //GameFirebase.LogEvent(GameEvent.Create(LevelMap.Level.ToString()).Add("Result", "Lose").Add("Duration", timePlay.Seconds.ToString()));
    }
}
