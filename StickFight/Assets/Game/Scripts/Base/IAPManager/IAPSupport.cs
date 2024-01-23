using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Linq;

public class IAPSupport : MonoBehaviour {
    [SerializeField] private string idProduct;
    [Space]
    [SerializeField] private bool upDatePrice;
    [SerializeField] private string priceDefaul;
    [SerializeField] private TextMeshProUGUI txt_Price;
    [SerializeField] private Button btn_Buy;

    private void Awake() {
        if(btn_Buy != null) {
            btn_Buy.onClick.AddListener(Buy);
        }
    }


    private void Start() {
        if(upDatePrice) {
            string price = IAPManager.Instance.GetLocalPrice(idProduct);
            if(string.IsNullOrEmpty(price)) {
                txt_Price.text = priceDefaul;
            } else {
                txt_Price.text = price;
            }
        }
    }

    public void Buy(Action<bool> callBack) {
        if(IAPManager.Instance != null) {
            IAPManager.Instance.BuyItem(idProduct, callBack);
        } else {
            Debug.Log("Dont has IAPManager");
            callBack?.Invoke(false);
        }
    }

    public void Buy() {
        if(IAPManager.Instance != null) {
            IAPManager.Instance.BuyItem(idProduct, (value) => {
                if(value) {
                    var product = IAPConfig.Instance.GetProductByID(idProduct);
                    var listItem = product.LstReward.ToList<ItemStack>();
                    CollectionController.Instance.GetItemStack(listItem, Camera.main.WorldToScreenPoint(transform.position), () => {
                        DataManager.Instance.PlayerData.AddItem(product.LstReward);
                    });
                }
            });
        } else {
            Debug.Log("Dont has IAPManager");
        }
    }
}
