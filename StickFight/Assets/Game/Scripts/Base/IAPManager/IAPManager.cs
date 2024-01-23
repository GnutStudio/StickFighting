using STU;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;

public partial class IAPManager : Singleton<IAPManager>, IStoreListener {
    private IStoreController m_StoreController;
    private IExtensionProvider m_StoreExtensionProvider;
    private Action<bool> onCompleted;
    private bool isPurchasing;
    public bool IsPurchasing => isPurchasing;
    public void OnInitialized(IStoreController controller, IExtensionProvider extensions) {
        //Purchasing has succeeded initializing. Collect our Purchasing references.
        Debug.Log("OnInitialized: PASS");
        // Overall Purchasing system, configured with products for this application.
        m_StoreController = controller;
        // Store specific subsystem, for accessing device-specific store features.
        m_StoreExtensionProvider = extensions;
        extensions.GetExtension<IAppleExtensions>().RestoreTransactions(result => {
            if(result) {
                // This does not mean anything was restored,
                // merely that the restoration process succeeded.
                Debug.Log("OnInitialized : " + result);
            } else {
                Debug.LogWarning("Fail restore!");
            }
        });
    }

    public void OnInitializeFailed(InitializationFailureReason error) {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);

        m_StoreController = null;
        m_StoreExtensionProvider = null;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason) {
        Debug.Log(string.Format("OnPurchaseFailed: FAIL. Product: '{0}', PurchaseFailureReason: {1}", product.definition.storeSpecificId, failureReason));
        OnPurchaseFailure(product.definition.storeSpecificId);
    }



    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent) {
        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXPlayer) {
            string productId = purchaseEvent.purchasedProduct.definition.id;
            if(onCompleted==null) {
                RestorePackGame(productId);
            }
            OnPurchaseSuccess(productId);
        } else {
            onCompleted?.Invoke(false);
        }
        onCompleted = null;
        return PurchaseProcessingResult.Complete;
    }

    private void OnPurchaseFailure(string productID) {
        // onCompleted?.Invoke(false);
        isPurchasing = false;
        if(onCompleted != null) {
            onCompleted(false);
            onCompleted = null;
        }
#if FIREBASE
        //FirebaseManager.Instance.LogEventWithString(productID + "_FAIL");
#endif
    }

    private void OnPurchaseSuccess(string productID) {
        //onCompleted?.Invoke(true);
        isPurchasing = false;
        if(onCompleted != null) {
            onCompleted(true);
            onCompleted = null;
        }
#if FIREBASE
        //FirebaseManager.Instance.LogEventWithString(productID + "_SUCC");
#endif
    }

    private void Start() {
        InitializePurchasing();
    }

    private void InitializePurchasing() {
        if(IsInitialized()) {
            Debug.LogError("Init IAP Fails");
            return;
        }
        var builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        IAPConfig config = IAPConfig.Instance;
        if(config) {
            foreach(var product in config.GetProducts()) {
#if IAP
                builder.AddProduct(product.ProductID, product.productType);
#endif
            }
        } else {
            Debug.LogError("Dont have configIAP");
        }

        // Kick off the remainder of the set - up with an asynchrounous call, passing the configuration
        // and this class' instance. Expect a response either in OnInitialized or OnInitializeFailed.
        UnityPurchasing.Initialize(this, builder);
        Debug.Log("INAPP  Sucess");

    }

    private bool IsInitialized() {
        // Only say we are initialized if both the Purchasing references are set.
        return m_StoreController != null && m_StoreExtensionProvider != null;
    }

    public void BuyItem(string key, Action<bool> onCompele) {
        Debug.LogWarning("KEY_INAPP:" + key);
        this.onCompleted = onCompele;
#if IAP
#if UNITY_EDITOR
        onCompele(true);
        return;
#endif
        if(IsInitialized()) {
            Product product = m_StoreController.products.WithID(key);
            if(product != null && product.availableToPurchase) {
                Debug.Log(string.Format("Purchasing product asychronously: '{0}'", product.definition.id));
                m_StoreController.InitiatePurchase(product);
                isPurchasing = true;
            } else {
                onCompele(false);
                Debug.Log("BuyProductID: FAIL. Not purchasing product, either is not found or is not available for purchase");
            }
        }
#else
        onCompele(true);
#endif
    }

    public string GetLocalPrice(string productID) {
#if IAP
        if(Application.internetReachability != NetworkReachability.NotReachable) {
            var meta = GetProductMetadata(productID);
            if(meta != null) {
                return meta.localizedPriceString;
            }
        }
#endif
        return string.Empty;
    }
#if IAP
    public ProductMetadata GetProductMetadata(string productID) {
        if(m_StoreController == null) {
            InitializePurchasing();

            return null;
        }
        if(m_StoreController.products == null)
            return null;

        Product product = m_StoreController.products.WithID(productID);
        if(product == null)
            return null;
        return product.metadata;
    }
#endif

    public void RestorePurchases() {
        // If Purchasing has not yet been set up ...
        if(!IsInitialized()) {
            // ... report the situation and stop restoring. Consider either waiting longer, or retrying initialization.
            Debug.Log("RestorePurchases FAIL. Not initialized.");
            return;
        }

        // If we are running on an Apple device ... 
        if(Application.platform == RuntimePlatform.IPhonePlayer ||
            Application.platform == RuntimePlatform.OSXPlayer) {
            // ... begin restoring purchases
            Debug.Log("RestorePurchases started ...");

            // Fetch the Apple store-specific subsystem.
            var apple = m_StoreExtensionProvider.GetExtension<IAppleExtensions>();
            // Begin the asynchronous process of restoring purchases. Expect a confirmation response in 
            // the Action<bool> below, and ProcessPurchase if there are previously purchased products to restore.
            apple.RestoreTransactions((result) => {
                // The first phase of restoration. If no more responses are received on ProcessPurchase then 
                // no purchases are available to be restored.
                Debug.Log("RestorePurchases continuing: " + result + ". If no further messages, no purchases available to restore.");
            });
        }
        // Otherwise ...
        else {
            // We are not running on an Apple device. No work is necessary to restore purchases.
            Debug.Log("RestorePurchases FAIL. Not supported on this platform. Current = " + Application.platform);
        }
    }

    public void OnInitializeFailed(InitializationFailureReason error, string message) {
        // Purchasing set-up has not succeeded. Check error for reason. Consider sharing this reason with the user.
        Debug.Log("OnInitializeFailed InitializationFailureReason:" + error);

        m_StoreController = null;
        m_StoreExtensionProvider = null;
    }
}
