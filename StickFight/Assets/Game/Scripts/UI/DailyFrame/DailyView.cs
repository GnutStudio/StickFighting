using UnityEngine;
using UnityEngine.UI;

public class DailyView : MonoBehaviour {

    [SerializeField] private Button btn_Buy;
    [SerializeField] private int Index;
    [SerializeField] protected ItemStackView itemStackV;
    [Space]
    [SerializeField] private GameObject imgActive;
    [SerializeField] private GameObject tickV;
    private DailyFrame dailyFrame;
    protected Status status;
    protected ItemStack reward;
    private DailySave dailySave => DataManager.Instance.PlayerData.DailySave;
    private void Awake() {
        btn_Buy.onClick.AddListener(OnClaim);
    }

    public void Init(DailyFrame dailyFrame, int index) {
        this.dailyFrame = dailyFrame;
        this.Index = index;
    }

    public virtual void Show(ItemStack itemStack) {
        this.reward = itemStack;
        itemStackV.Show(itemStack);
        this.status = GetStatus();
        Show(this.status);
    }

    #region Status
    private void SetUpNoActive() {
        imgActive.SetActive(false);
        tickV.SetActive(false);
    }

    private void SetUpActive() {
        imgActive.SetActive(true);
        tickV.SetActive(false);
    }

    private void SetUpReceived() {
        imgActive.SetActive(true);
        tickV.SetActive(true);
    }
    #endregion

    private void OnClaim() {
        if(status == Status.ACTIVE) {
            CollectionController.Instance.GetItemStack(reward, Camera.main.WorldToScreenPoint(btn_Buy.transform.position), () => {
                DataManager.Instance.PlayerData.AddItem(reward);
                DataManager.Instance.PlayerData.DailySave.AddIndexDay(Index);
                this.status = Status.RECEIVED;
                Show(this.status);
            });
        }
    }

    protected Status GetStatus() {
        if(dailySave.CheckReceived(Index)) {
            return Status.RECEIVED;
        } else if(Index == dailySave.GetCurIndex()) {
            return Status.ACTIVE;
        } else {
            return Status.NOACTIVE;
        }
    }

    protected void Show(Status status) {
        switch(status) {
            case Status.NOACTIVE:
                SetUpNoActive();
                break;
            case Status.ACTIVE:
                SetUpActive();
                break;
            case Status.RECEIVED:
                SetUpReceived();
                break;
        }
    }

    public enum Status {
        NONE,
        NOACTIVE,
        ACTIVE,
        RECEIVED,
    }
}
