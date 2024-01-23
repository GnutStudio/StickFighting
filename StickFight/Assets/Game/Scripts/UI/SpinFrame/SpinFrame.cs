using DG.Tweening;
using STU;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpinFrame : FrameBase {
    [SerializeField] private List<ItemStackView> lstView;
    [SerializeField] private List<ItemStack> lstReward;
    [Header("Wheel")]
    [SerializeField] private Transform wheel;
    [SerializeField] private Image lightCell;
    [SerializeField] private AnimationCurve spinCurve;
    [SerializeField] private SpinArrow arrow;
    [Header("Free Spin")]
    [SerializeField] private DisplayObjects obj_FreeSpin; // 0.Active , 1.UnActive
    [SerializeField] private Button btn_FreeSpin, btn_SpinItem;
    [SerializeField] private CountDown countDown;
    [Space]
    [SerializeField] private Button btn_AdsSpin;
    private Tween tween;
    private bool isWheel;
    private bool canGetFree;
    private SpinSave spinSave => DataManager.Instance.PlayerData.SpinSave;
    private void Awake() {
        btn_FreeSpin.onClick.AddListener(DoSpinFree);
        btn_AdsSpin.onClick.AddListener(DoSpinAds);
        btn_SpinItem.onClick.AddListener(DoSpinItem);
    }

    private void OnEnable() {
        EventDispatcher.AddListener<EventKey.SpinChange>(EventShowBtnFree);
    }

    private void OnDisable() {
        EventDispatcher.RemoveListener<EventKey.SpinChange>(EventShowBtnFree);
    }

    public override void OnShow(Action onCompleted = null, bool instant = false) {
        base.OnShow(onCompleted, instant);
        for (int i = 0; i < lstView.Count; i++) {
            lstView[i].Show(lstReward[i]);
        }
        tween = wheel.DORotate(new Vector3(0, 0, -360), 50, RotateMode.WorldAxisAdd).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
        isWheel = false;
        lightCell.gameObject.SetActive(false);
        SetupButtonSpin();
    }

    private void EventShowBtnFree(EventKey.SpinChange evt) {
        if (canGetFree == spinSave.CanGetFree()) {
            return;
        }
        SetupButtonSpin();
    }

    private void SetupButtonSpin() {
        canGetFree = spinSave.CanGetFree();
        obj_FreeSpin.Active(canGetFree ? 0 : 1);
        if (!canGetFree) {
            countDown.Show(spinSave.TimeGetReward() - DateTime.Now, () => {
                SetupButtonSpin();
            });
            ItemStack itemLucky = ItemID.LUCKYWEEL.GetSaveByID();
            btn_AdsSpin.gameObject.SetActive(itemLucky.Amount <= 0);
            btn_SpinItem.gameObject.SetActive(itemLucky.Amount > 0);
        }
    }

    private void DoSpinFree() {
        if (isWheel) {
            TextNotify.Instance.Show("The wheel is spinning !");
            return;
        }
        spinSave.SetTimeStart();
        SetupButtonSpin();
        DoSpin();
        EventDispatcher.Dispatch<EventKey.SpinChange>(new EventKey.SpinChange(false));
    }

    private void DoSpinAds() {
        if (isWheel) {
            TextNotify.Instance.Show("The wheel is spinning !");
            return;
        }
        SetupButtonSpin();
        DoSpin();
    }

    private void DoSpinItem() {
        if (isWheel) {
            TextNotify.Instance.Show("The wheel is spinning !");
            return;
        }
        if (DataManager.Instance.PlayerData.RemoveItem(new ItemStack(ItemID.LUCKYWEEL, 1))) {
            SetupButtonSpin();
            DoSpin();
        }
    }

    private void DoSpin() {
        isWheel = true;
        Extentions.CheckKillTween(tween, true);
        TargetCell();
        int stepMore = UnityEngine.Random.Range(0, lstView.Count);
        float angleSpine = -360f * 4 + stepMore * -45f;
        tween = wheel.DORotate(new Vector3(0, 0, angleSpine), 5, RotateMode.FastBeyond360).SetEase(spinCurve).OnComplete(() => {
            lightCell.gameObject.SetActive(true);
            lightCell.color = Color.white;
            tween = lightCell.DOColor(Color.green, 0.2f).SetEase(Ease.Flash).SetLoops(6).OnComplete(() => {
                isWheel = false;
                lightCell.gameObject.SetActive(false);
                CollectionController.Instance.GetItemStack(arrow.ViewCol.Model, Camera.main.WorldToScreenPoint(arrow.ViewCol.transform.position), () => {
                    DataManager.Instance.PlayerData.AddItem(arrow.ViewCol.Model);
                });
            });
        });
    }

    public void TargetCell() {
        Vector3 angV3 = wheel.eulerAngles;
        float angZ = angV3.z;
        if (angZ > 0) {
            float ofset = angZ % 45;
            wheel.eulerAngles = new Vector3(angV3.x, angV3.y, angV3.z - ofset);
        } else if (angZ < 0) {
            float ofset = Mathf.Abs(angZ) % 45;
            wheel.eulerAngles = new Vector3(angV3.x, angV3.y, angV3.z + ofset - 45f);
        }
    }

    public override void Hide(Action onCompleted = null, bool instant = false) {
        base.Hide(onCompleted, instant);
        tween.CheckKillTween();
    }
}
