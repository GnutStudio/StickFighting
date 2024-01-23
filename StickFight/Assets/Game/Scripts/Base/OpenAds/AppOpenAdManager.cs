using System;
using System.Collections;
#if !NO_ADS && (UNITY_ANDROID ||UNITY_IPHONE)
using GoogleMobileAds.Api;
#endif
using UnityEngine;

public class AppOpenAdManager {
#if UNITY_ANDROID
    private const string ID_TIER_1 = "ca-app-pub-6336405384015455/3555664023";
    private const string ID_TIER_2 = "ca-app-pub-6336405384015455/7487170172";
    private const string ID_TIER_3 = "ca-app-pub-6336405384015455/6205878090";

#elif UNITY_IOS
    private const string ID_TIER_1 = "ca-app-pub-1839004489502882/6902267694";
    private const string ID_TIER_2 = "ca-app-pub-1839004489502882/5780757717";
    private const string ID_TIER_3 = "ca-app-pub-1839004489502882/6902267694";
#else
    private const string ID_TIER_1 = "";
    private const string ID_TIER_2 = "";
    private const string ID_TIER_3 = "";
#endif

    private static AppOpenAdManager instance;
#if !NO_ADS && (UNITY_ANDROID ||UNITY_IPHONE)
    private AppOpenAd ad;
#endif

    private DateTime loadTime;

    private bool isShowingAd = false;

    private bool showFirstOpen = false;

    public static bool ConfigOpenApp = true;
    public static bool ConfigResumeApp = true;

    public static bool ResumeFromAdsIAP = true;

    public static AppOpenAdManager Instance {
        get {
            if (instance == null) {
                instance = new AppOpenAdManager();
            }

            return instance;
        }
    }
#if !NO_ADS && (UNITY_ANDROID ||UNITY_IPHONE)
    private bool IsAdAvailable => ad != null && (System.DateTime.UtcNow - loadTime).TotalHours < 4;
#endif
    private int tierIndex = 1;

    public void LoadAd() {
        // if (IAP_AD_REMOVED)
        //     return;
        Logs.Log("[GameAppOpenAds]");
#if !NO_ADS && (UNITY_ANDROID ||UNITY_IPHONE)
        LoadAOA();
#endif
    }

#if !NO_ADS && (UNITY_ANDROID ||UNITY_IPHONE)
    public void LoadAOA() {

        string id = ID_TIER_1;
        if(tierIndex == 2)
            id = ID_TIER_2;
        else if(tierIndex == 3)
            id = ID_TIER_3;

        Debug.Log("Start request Open App Ads Tier " + tierIndex);

        AdRequest request = new AdRequest.Builder().Build();

        AppOpenAd.LoadAd(id, ScreenOrientation.Portrait, request, ((appOpenAd, error) => {
            if(error != null) {
                // Handle the error.
                Debug.LogFormat("Failed to load the ad. (reason: {0}), tier {1}", error.LoadAdError.GetMessage(), tierIndex);
                tierIndex++;
                if(tierIndex <= 3)
                    LoadAOA();
                else
                    tierIndex = 1;
                return;
            }

            // App open ad is loaded.
            ad = appOpenAd;
            tierIndex = 1;
            loadTime = DateTime.UtcNow;
            //if(!showFirstOpen && ConfigOpenApp) {
            //    ShowAdIfAvailable();
            //    showFirstOpen = true;
            //}
        }));
    }

    public void ShowAdIfAvailable() {
#if UNITY_EDITOR && TURN_OFF_AOA
        return;
#endif
        if(!IsAdAvailable || isShowingAd) {
            Logs.Log($"[GameAppOpenAds]: don't Show IsAdAvailable: {IsAdAvailable} and isShowingAd: {isShowingAd}");
            return;
        }

        ad.OnAdDidDismissFullScreenContent += HandleAdDidDismissFullScreenContent;
        ad.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresentFullScreenContent;
        ad.OnAdDidPresentFullScreenContent += HandleAdDidPresentFullScreenContent;
        ad.OnAdDidRecordImpression += HandleAdDidRecordImpression;
        ad.OnPaidEvent += HandlePaidEvent;

        ad.Show();
    }

    private void HandleAdDidDismissFullScreenContent(object sender, EventArgs args) {
        Debug.Log("Closed app open ad");
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        isShowingAd = false;
        LoadAd();
    }

    private void HandleAdFailedToPresentFullScreenContent(object sender, AdErrorEventArgs args) {
        Debug.LogFormat("Failed to present the ad (reason: {0})", args.AdError.GetMessage());
        // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
        ad = null;
        LoadAd();
    }

    private void HandleAdDidPresentFullScreenContent(object sender, EventArgs args) {
        Debug.Log("Displayed app open ad");
        isShowingAd = true;
    }

    private void HandleAdDidRecordImpression(object sender, EventArgs args) {
        Debug.Log("Recorded ad impression");
    }

    private void HandlePaidEvent(object sender, AdValueEventArgs args) {
        Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
            args.AdValue.CurrencyCode, args.AdValue.Value);
    }
#endif
}