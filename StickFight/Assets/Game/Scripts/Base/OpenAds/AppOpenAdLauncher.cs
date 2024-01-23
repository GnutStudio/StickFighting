using System;
using System.Collections.Generic;
#if !NO_ADS && (UNITY_ANDROID ||UNITY_IPHONE)
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
#endif
using STU;
using UnityEngine.SceneManagement;

public class AppOpenAdLauncher : Singleton<AppOpenAdLauncher> {
#if !NO_ADS
    private bool showFirstOpen = true;
    protected override void Awake() {
        base.Awake();
#if UNITY_ANDROID ||UNITY_IPHONE
        MobileAds.SetiOSAppPauseOnBackground(true);

        List<String> deviceIds = new List<String>() { AdRequest.TestDeviceSimulator };

        // Add some test device IDs (replace with your own device IDs).
#if UNITY_IPHONE
        deviceIds.Add("20B85444041647AE99E8CF3029B11050");
#elif UNITY_ANDROID
        deviceIds.Add("1FF875FC5C6868256B2E3BEEE212678D");
#endif

        // Configure TagForChildDirectedTreatment and test device IDs.
        RequestConfiguration requestConfiguration =
            new RequestConfiguration.Builder()
            .SetTagForChildDirectedTreatment(TagForChildDirectedTreatment.Unspecified)
            .SetTestDeviceIds(deviceIds).build();
        MobileAds.SetRequestConfiguration(requestConfiguration);

        // Initialize the Google Mobile Ads SDK.
        Logs.Log($"[GameAppOpenAds]: Initialize befor");
        MobileAds.Initialize(HandleInitCompleteAction);
#endif
    }
#if UNITY_ANDROID || UNITY_IPHONE
    private void HandleInitCompleteAction(InitializationStatus initstatus) {
        // Callbacks from GoogleMobileAds are not guaranteed to be called on
        // main thread.
        // In this example we use MobileAdsEventExecutor to schedule these calls on
        // the next Update() loop.
        MobileAdsEventExecutor.ExecuteInUpdate(() => {
            Logs.Log($"[GameAppOpenAds]: Initialize succes !!");
            AppOpenAdManager.Instance.LoadAd();
        });
    }
#endif
    private void OnEnable() {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1) {
        if (!showFirstOpen) {
            return;
        }

        if (arg0.name == SceneManagerLoad.SCENE_HOME) {
#if NO_ADS || UNITY_EDITOR
            return;
#else
            if (showFirstOpen && AppOpenAdManager.ConfigOpenApp) {
                AppOpenAdManager.Instance.ShowAdIfAvailable();
                showFirstOpen = false;
            }
#endif

        }
    }

    private void OnApplicationPause(bool pause) {
#if NO_ADS || UNITY_EDITOR
        return;
#else
        if (IAPManager.Instance.IsPurchasing || AdsManager.Instance.IsShowingInterAndReward) {
            return;
        }

        if (!pause && AppOpenAdManager.ConfigResumeApp && !AppOpenAdManager.ResumeFromAdsIAP) {
            AppOpenAdManager.Instance.ShowAdIfAvailable();
            Logs.Log($"[GameAppOpenAds]: Show {pause} and ConfigResumeApp: {AppOpenAdManager.ConfigResumeApp} and ResumeAdsIAP: {AppOpenAdManager.ResumeFromAdsIAP} ");
        } else {
            Logs.Log($"[GameAppOpenAds]: don't Show {pause} and ConfigResumeApp: {AppOpenAdManager.ConfigResumeApp} and ResumeAdsIAP: {AppOpenAdManager.ResumeFromAdsIAP} ");
        }
#endif
    }
#endif
}