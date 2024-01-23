using STU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

[CreateAssetMenu(fileName = "IAPConfig", menuName = "Game/IAPConfig")]
public class IAPConfig : SingletonSO<IAPConfig> {
    [SerializeField] private Product[] products;

    public IEnumerable<Product> GetProducts() {
        return products;
    }


    public Product GetProductByID(string productID) {
        foreach(var product in products) {
            if(product.ProductID == productID) {
                return product;
            }
        }
        return null;
    }

    [System.Serializable]
    public class Product {
        [SerializeField] private string productID;
        [SerializeField] List<ItemStack> lstReward;
        public string ProductID => productID;
        public IEnumerable<ItemStack> LstReward => lstReward;
#if IAP
        [SerializeField] public ProductType productType;
        public ProductType ProductType => productType;
#endif

    }
}


