using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WeaponArea : MonoBehaviour
{
    [SerializeField] private RectTransform display;
    [SerializeField] private Button btn_Move,btn_Bow,btn_Sword;
    [SerializeField] private Vector2 positionDisplay;
    [SerializeField] private float speedMove, speedRotation;
    [SerializeField] private ItemStackView swordView,bowView;
    private bool Ondisplay = false;
    private Tween tweenmove;
    private Tween tweenRotate;
    private List<WeaponData> lstBow;
    private List<WeaponData> lstSword;
    private Player player => InGameManager.Instance.Player;
    private void Awake() {
        btn_Move.onClick.AddListener(HalderButtonMove);
        btn_Bow.onClick.AddListener(HalderButtonBow);
        btn_Sword.onClick.AddListener(HalderButtonSword);
    }

    private void Start() {
        display.anchoredPosition = new Vector2(-positionDisplay.x, positionDisplay.y);
        btn_Move.transform.eulerAngles = new Vector3(0, 0, 0);
        Ondisplay = false;
        GenderWeapon();
        GenderView();
    }

    private void GenderWeapon() {
        lstBow = new List<WeaponData>();
        lstSword = new List<WeaponData>();
        foreach(var weapon in DataManager.Instance.LstWeapon) {
            if(weapon.TypeWeapon == TypeWeapon.SORT) {
                lstSword.Add(weapon);
            }else if(weapon.TypeWeapon == TypeWeapon.LONG) {
                lstBow.Add(weapon);
            }
        }
    }

    private void GenderView() {
        ItemStack dataSword = ItemID.ITEMSWORD.GetSaveByID();
        swordView.Show(dataSword);

        ItemStack dataBow = ItemID.ITEMBOW.GetSaveByID();
        bowView.Show(dataBow);
    }

    private void HalderButtonMove() {
        Ondisplay = !Ondisplay;
        tweenmove.CheckKillTween();
        tweenRotate.CheckKillTween();
        float xTarget = 0f;
        float angleTarget = 0f;
        if(Ondisplay) {
            xTarget = positionDisplay.x;
            angleTarget = 180f;
        } else {
            xTarget = -positionDisplay.x;
            angleTarget = 0f;
        }
        float distance = Mathf.Abs(display.anchoredPosition.x - xTarget);
        float time = distance/speedMove;
        tweenmove = display.DOAnchorPosX(xTarget, time).SetEase(Ease.Linear);
        float angle = Mathf.Abs(btn_Move.GetComponent<RectTransform>().eulerAngles.z - angleTarget);
        float timeAngle = angle/speedRotation;
        tweenRotate = btn_Move.transform.DORotate(new Vector3(0,0,angleTarget), timeAngle).SetEase(Ease.Linear);
    }

    private void HalderButtonBow() {
        if(DataManager.Instance.PlayerData.RemoveItem(new ItemStack(ItemID.ITEMBOW,1))) {
            int indexRandom = Random.Range(0,lstBow.Count);
            player.SetWeapon(lstBow[indexRandom]);
            GenderView();
            HalderButtonMove();
        }
    }

    private void HalderButtonSword() {
        if(DataManager.Instance.PlayerData.RemoveItem(new ItemStack(ItemID.ITEMSWORD, 1))) {
            int indexRandom = Random.Range(0,lstSword.Count);
            player.SetWeapon(lstSword[indexRandom]);
            GenderView();
            HalderButtonMove();
        }
    }

    private void OnDisable() {
        tweenmove.CheckKillTween();
        tweenRotate.CheckKillTween();
    }
}
