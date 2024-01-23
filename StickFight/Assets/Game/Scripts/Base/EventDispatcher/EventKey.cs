using STU;

public static class EventKey {
    public struct LoadFinal : IEventArgs {

    }

    public struct SceneChange : IEventArgs {
        public string nameScene;

        public SceneChange(string nameScene) {
            this.nameScene = nameScene;
        }
    }

    public struct IteamChange : IEventArgs {
        public ItemID itemID;
        public int curAmount;
        public int changeAmount;

        public IteamChange(ItemID itemID, int curAmount, int changeAmount) {
            this.itemID = itemID;
            this.curAmount = curAmount;
            this.changeAmount = changeAmount;
        }
    }

    public struct EnemyDie : IEventArgs {
        public EnemyBase enemyDie;
        public EnemyDie(EnemyBase enemyDie) {
            this.enemyDie = enemyDie;
        }
    }

    public struct PlayerChange : IEventArgs {

    }

    public struct BossGetDame : IEventArgs {

    }

    public struct SpinChange : IEventArgs {
        public bool GetNew;

        public SpinChange(bool getNew) {
            this.GetNew = getNew;
        }
    }

    public struct EventSkinChange : IEventArgs {
        public ItemID IDBefor;
        public ItemID IDAfter;

        public EventSkinChange(ItemID idBefor, ItemID idAfter) {
            this.IDBefor = idBefor;
            this.IDAfter = idAfter;
        }
    }

    public struct EventUpdatePower : IEventArgs {

    }

    public struct EventSetupNewGame : IEventArgs {

    }

    public struct SoundChange : IEventArgs {
        public bool Enable;

        public SoundChange(bool enable) {
            this.Enable = enable;
        }
    }

    public struct BossArea : IEventArgs {
        public bool Enter;

        public BossArea(bool enter) {
            this.Enter = enter;
        }
    }
}
