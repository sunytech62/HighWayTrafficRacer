#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
public class AdsManager_AdmobMediation : MonoBehaviour
{
    #region ID's - variables

    #region Ads Variable

    [SerializeField] System.Collections.Generic.List<string> interstitialStaticID;
    [SerializeField] System.Collections.Generic.List<string> interstitialID;
    [SerializeField] System.Collections.Generic.List<string> rewardedInterstitialID;
    [SerializeField] System.Collections.Generic.List<string> rewardedID;
    [SerializeField] System.Collections.Generic.List<string> appOpenID;
    [SerializeField] System.Collections.Generic.List<string> smallBannerID;
    [SerializeField] System.Collections.Generic.List<string> largeBannerID;
    [SerializeField] System.Collections.Generic.List<string> smallBannerGamePlayID;
    [SerializeField] System.Collections.Generic.List<string> adaptiveBannerID;
    [SerializeField] System.Collections.Generic.List<string> smartBannerID;
    [SerializeField] System.Collections.Generic.List<string> nativeID;
    [SerializeField] System.Collections.Generic.List<string> nativeFullScreenID;
    [SerializeField] System.Collections.Generic.List<string> customBannerID;

    #endregion

    #region Editor Variable
    [SerializeField] bool isTestMode = false;
    [SerializeField] bool isSmallBannerIdAvailable = false;
    [SerializeField] bool isSmallBannerGamePlayIdAvailable = false;
    [SerializeField] bool isLargeBannerIdAvailable = false;
    [SerializeField] bool isAdaptiveBannerIdAvailable = false;
    [SerializeField] bool isSmartBannerIdAvailable = false;
    [SerializeField] bool isNativeIdAvailable = false;
    [SerializeField] bool isNativeFullScreenIdAvailable = false;
    [SerializeField] bool isCustomBannerIdAvailable = false;
    [SerializeField] bool isInterstitialIdAvailable = false;
    [SerializeField] bool isInterstitialStaticIdAvailable = false;
    [SerializeField] bool isRewardedVideoIdAvailable = false;
    [SerializeField] bool isRewardedInterstitialIdAvailable = false;
    [SerializeField] bool isAppOpenIdAvailable = false;

    [SerializeField] bool isAutoInitilize = true;
    [SerializeField] bool isStartingInitilizeDelay = false;
    [SerializeField][Range(0.1f, 10.0f)] float startingDelay = 0.1f;
    [SerializeField] System.Collections.Generic.List<AdLoadPriority> adLoadPriority;

    [SerializeField] bool isAutoReload = false;
    [SerializeField] bool isInterstitialReloadAfterShow = false;
    [SerializeField] bool isRewardedReloadAfterShow = false;
    [SerializeField] bool isAppOpenReloadAfterShow = false;

    [SerializeField] bool isDebugLog = false;
    [SerializeField] bool isBannerBGEnable = false;
    [SerializeField] bool isAdsAnalytics = true;
    [SerializeField] bool isSendAnalyticsToCrashlytics = false;
    static bool isSendAnalyticsToCrashlytics_Static = false;
#if UNITY_EDITOR
    [SerializeField] bool isEditorShowAds = true;
#endif

    [SerializeField] bool isMemoryThreshold = false;
    [SerializeField] int NoInitilizationBelow = 100;
    [SerializeField] int NoSmallBannerBelow = 150;
    [SerializeField] int NoLargeBannerBelow = 150;
    [SerializeField] int NoNativeBelow = 150;
    [SerializeField] int NoAnyInterstitialBelow = 200;
    [SerializeField] int NoInterstitialStaticAbove = 300;
    [SerializeField] int NoAnyRewardedBelow = 250;
    [SerializeField] int NoRewardedInterstitialAbove = 350;
    [SerializeField] int NoAppOpenBelow = 200;
    [SerializeField] bool isLowMemoryAnalytics = false;

    #endregion

    #endregion

    #region Instance

    static AdsManager_AdmobMediation _instance = null;
    public static AdsManager_AdmobMediation Instance
    {
        get
        {
            if (_instance == null)
            {
                if (FindObjectOfType<AdsManager_AdmobMediation>())
                {
                    _instance = FindObjectOfType<AdsManager_AdmobMediation>();
                }
                if (_instance != null)
                    DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    void Awake()
    {
        if (firebaseAnalyticsQueue == null) firebaseAnalyticsQueue = this.gameObject.AddComponent<FirebaseAnalyticsDirectSendWithQueue>();
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
            return;
        }
    }

    #endregion

    #region Initilization

    void Start() { StartCoroutine(StartingFunctionality()); }
    System.Collections.IEnumerator StartingFunctionality()
    {
        yield return null; yield return null;
        if (isTestMode)
        {
            for (int i = 0; i < smallBannerID.Count; i++) smallBannerID[i] = "ca-app-pub-3940256099942544/6300978111";
            for (int i = 0; i < largeBannerID.Count; i++) largeBannerID[i] = "ca-app-pub-3940256099942544/6300978111";
            for (int i = 0; i < smallBannerGamePlayID.Count; i++) smallBannerGamePlayID[i] = "ca-app-pub-3940256099942544/6300978111";
            for (int i = 0; i < adaptiveBannerID.Count; i++) adaptiveBannerID[i] = "ca-app-pub-3940256099942544/6300978111";
            for (int i = 0; i < smartBannerID.Count; i++) smartBannerID[i] = "ca-app-pub-3940256099942544/6300978111";
            for (int i = 0; i < customBannerID.Count; i++) customBannerID[i] = "ca-app-pub-3940256099942544/6300978111";
            for (int i = 0; i < nativeID.Count; i++) nativeID[i] = "ca-app-pub-3940256099942544/2247696110";
            for (int i = 0; i < nativeFullScreenID.Count; i++) nativeFullScreenID[i] = "ca-app-pub-3940256099942544/2247696110";
            for (int i = 0; i < interstitialID.Count; i++) interstitialID[i] = "ca-app-pub-3940256099942544/8691691433";
            for (int i = 0; i < interstitialStaticID.Count; i++) interstitialStaticID[i] = "ca-app-pub-3940256099942544/1033173712";
            for (int i = 0; i < rewardedID.Count; i++) rewardedID[i] = "ca-app-pub-3940256099942544/5224354917";
            for (int i = 0; i < rewardedInterstitialID.Count; i++) rewardedInterstitialID[i] = "ca-app-pub-3940256099942544/5354046379";
            for (int i = 0; i < appOpenID.Count; i++) appOpenID[i] = "ca-app-pub-3940256099942544/9257395921";
        }
#if UNITY_EDITOR
        else CheckAdmobAdIDsSameAsAppID();
#endif
        if (isAutoInitilize)
        {
            float minimumDelay = 2.0f;
            if (isStartingInitilizeDelay)
            {
                if (startingDelay < minimumDelay) startingDelay = minimumDelay;
                yield return new WaitForSecondsRealtime(startingDelay);
            }
            else yield return new WaitForSecondsRealtime(minimumDelay);
            CheckInitialization(true);
        }

#if RemoteConfigManager_Firebase
        if (RemoteConfigManager_Firebase.Instance)
        {
            if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(ANRSupervisorRemoteValue))
            {
#endif
                ANRHandlerThreadStart();
#if RemoteConfigManager_Firebase
            }
        }
#endif

#if UNITY_IOS
        if (SystemInfo.deviceModel.Contains("iPhone16,")) is_iPhone_16 = true;
#endif
    }
    bool CheckInitialization(bool isCallingFromStartFunc = false, System.Action actionAfterInitilize = null)
    {
        bool returnValue = false;
        switch (initilizeStatus)
        {
            case InitilizeStatus.None:
                {
                    if (IsInternetConnection())
                        if (!IsLowMemory(NoInitilizationBelow))
                            InitializeAds(isCallingFromStartFunc, actionAfterInitilize);
                    returnValue = false;
                    break;
                }
            case InitilizeStatus.Initilizing:
                {
                    returnValue = false;
                    break;
                }
            case InitilizeStatus.Failed:
                {
                    if (IsInternetConnection())
                        if (!IsLowMemory(NoInitilizationBelow))
                            InitializeAds(isCallingFromStartFunc, actionAfterInitilize);
                    returnValue = false;
                    break;
                }
            case InitilizeStatus.Initilized:
                {
                    returnValue = true;
                    break;
                }
        }
        return returnValue;
    }
    void InitializeAds(bool isCallingFromStartFunc = false, System.Action actionAfterInitilize = null)
    {
#if UNITY_EDITOR
        if (!isEditorShowAds) return;
#endif
        try
        {
            initilizeStatus = InitilizeStatus.Initilizing;

            #region IOS
            if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXEditor)
                MobileAds.SetiOSAppPauseOnBackground(true);
            #endregion

            SendAnalytics(EventName.Ad_Initilization, ParameterKey.myParameter, ParameterValue.AdRequestSend);
            MobileAds.Initialize((initStatus) =>
            {
                if (initStatus == null)
                {
                    initilizeStatus = InitilizeStatus.Failed;
                    SendAnalytics(EventName.Ad_Initilization, ParameterKey.myParameter, ParameterValue.AdRequestFail);
                    return;
                }
                if (isDebugLog)
                {
                    System.Collections.Generic.Dictionary<string, AdapterStatus> map = initStatus.getAdapterStatusMap();
                    foreach (System.Collections.Generic.KeyValuePair<string, AdapterStatus> keyValuePair in map)
                    {
                        string className = keyValuePair.Key;
                        AdapterStatus status = keyValuePair.Value;
                        switch (status.InitializationState)
                        {
                            case AdapterState.NotReady:
                                ShowDebugLog("Adapter: " + className + " not ready.");
                                break;
                            case AdapterState.Ready:
                                ShowDebugLog("Adapter: " + className + " initialized.");
                                break;
                        }
                    }
                }

                GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    initilizeStatus = InitilizeStatus.Initilized;
                    ShowDebugLog("Google Mobile Ads Initilized.");

                    ActionsPerformWithDelay(() => { ShowUserConsent(); }, 0.2f);
                    ActionsPerformWithDelay(() => { LoadInitialAds(); actionAfterInitilize?.Invoke(); }, 0.5f);
                });

                SendAnalytics(EventName.Ad_Initilization, ParameterKey.myParameter, ParameterValue.AdRequestSuccess);
                Invoke(nameof(ANRHandlerThreadStart), 5.0f);
            });
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
    }
    void LoadInitialAds()
    {
        if (adLoadPriority.Count <= 0)
        {
            adLoadPriority.Add(AdLoadPriority.AppOpen);
            adLoadPriority.Add(AdLoadPriority.SmallBanner);
            adLoadPriority.Add(AdLoadPriority.Native);
            adLoadPriority.Add(AdLoadPriority.NativeFullScreen);
            adLoadPriority.Add(AdLoadPriority.Interstitial);
            adLoadPriority.Add(AdLoadPriority.InterstitialStatic);
            adLoadPriority.Add(AdLoadPriority.LargeBanner);
            adLoadPriority.Add(AdLoadPriority.AdaptiveBanner);
            adLoadPriority.Add(AdLoadPriority.SmartBanner);
            adLoadPriority.Add(AdLoadPriority.Rewarded);
        }

        float timeDuration = 0.1f;
        float delayTime = 2.0f;
        foreach (AdLoadPriority priority in adLoadPriority)
        {
            switch (priority)
            {
                case AdLoadPriority.AppOpen:
                    {
#if RemoteConfigManager_Firebase
                        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isAppOpenLoad))
                        {
                            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isAppOpenLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isAppOpenFromRemote);
                            if (isAppOpenFromRemote)
                            {
#endif
                                ActionsPerformWithDelay(new System.Action(() => { LoadAppOpenAd(); }), timeDuration);
                                timeDuration += delayTime;
#if RemoteConfigManager_Firebase
                            }
                        }
#endif
                        break;
                    }
                case AdLoadPriority.SmallBanner:
                    {
#if RemoteConfigManager_Firebase
                        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isSmallBannerLoad))
                        {
                            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isSmallBannerLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isSmallBannerFromRemote);
                            if (isSmallBannerFromRemote)
                            {
#endif
                                ActionsPerformWithDelay(new System.Action(() => { LoadAd(BannerType.SmallBannerType, AdSize.Banner, lastSmallBannerPosition); }), timeDuration);
                                timeDuration += delayTime;
#if RemoteConfigManager_Firebase
                            }
                        }
#endif
                        break;
                    }
                case AdLoadPriority.LargeBanner:
                    {
#if RemoteConfigManager_Firebase
                        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isLargeBannerLoad))
                        {
                            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isLargeBannerLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isLargeBannerFromRemote);
                            if (isLargeBannerFromRemote)
                            {
#endif
                                ActionsPerformWithDelay(new System.Action(() => { LoadAd(BannerType.LargeBannerType, AdSize.MediumRectangle, lastLargeBannerPosition); }), timeDuration);
                                timeDuration += delayTime;
#if RemoteConfigManager_Firebase
                            }
                        }
#endif
                        break;
                    }
                case AdLoadPriority.AdaptiveBanner:
                    {
#if RemoteConfigManager_Firebase
                        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isAdaptiveBannerLoad))
                        {
                            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isAdaptiveBannerLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isAdaptiveBannerFromRemote);
                            if (isAdaptiveBannerFromRemote)
                            {
#endif
                                ActionsPerformWithDelay(new System.Action(() => { LoadAd(BannerType.AdaptiveBannerType, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), lastAdaptiveBannerPosition); }), timeDuration);
                                timeDuration += delayTime;
#if RemoteConfigManager_Firebase
                            }
                        }
#endif
                        break;
                    }
                case AdLoadPriority.SmartBanner:
                    {
#if RemoteConfigManager_Firebase
                        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isSmartBannerLoad))
                        {
                            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isSmartBannerLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isSmartBannerFromRemote);
                            if (isSmartBannerFromRemote)
                            {
#endif
                                ActionsPerformWithDelay(new System.Action(() => { LoadAd(BannerType.SmartBannerType, AdSize.SmartBanner, lastSmartBannerPosition); }), timeDuration);
                                timeDuration += delayTime;
#if RemoteConfigManager_Firebase
                            }
                        }
#endif
                        break;
                    }
                case AdLoadPriority.CustomBanner:
                    {
#if RemoteConfigManager_Firebase
                        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isLargeBannerLoad))
                        {
                            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isLargeBannerLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isCustomBannerFromRemote);
                            if (isCustomBannerFromRemote)
                            {
#endif
                                ActionsPerformWithDelay(new System.Action(() => { PreLoadCustomBanner(); }), timeDuration);
                                timeDuration += delayTime;
#if RemoteConfigManager_Firebase
                            }
                        }
#endif
                        break;
                    }
                case AdLoadPriority.Interstitial:
                    {
#if RemoteConfigManager_Firebase
                        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isInterstitialLoad))
                        {
                            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isInterstitialLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isInterstitialFromRemote);
                            if (isInterstitialFromRemote)
                            {
#endif
                                ActionsPerformWithDelay(new System.Action(() => { LoadInterstitial(InterstitialType.interstitial); }), timeDuration);
                                timeDuration += delayTime;
#if RemoteConfigManager_Firebase
                            }
                        }
#endif
                        break;
                    }
                case AdLoadPriority.InterstitialStatic:
                    {
#if RemoteConfigManager_Firebase
                        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isInterstitialStaticLoad))
                        {
                            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isInterstitialStaticLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isInterstitialStaticFromRemote);
                            if (isInterstitialStaticFromRemote)
                            {
#endif
                                ActionsPerformWithDelay(new System.Action(() => { LoadInterstitial(InterstitialType.interstitialStatic); }), timeDuration);
                                timeDuration += delayTime;
#if RemoteConfigManager_Firebase
                            }
                        }
#endif
                        break;
                    }
                case AdLoadPriority.Rewarded:
                    {
#if RemoteConfigManager_Firebase
                        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isRewardedLoad))
                        {
                            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isRewardedLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isRewardedFromRemote);
                            if (isRewardedFromRemote)
                            {
#endif
                                ActionsPerformWithDelay(new System.Action(() => { LoadRewardedAd(); }), timeDuration);
                                timeDuration += delayTime;
#if RemoteConfigManager_Firebase
                            }
                        }
#endif
                        break;
                    }
                case AdLoadPriority.RewardedInterstitial:
                    {
#if RemoteConfigManager_Firebase
                        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isRewardedInterstitialLoad))
                        {
                            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isRewardedInterstitialLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isRewardedInterstitialFromRemote);
                            if (isRewardedInterstitialFromRemote)
                            {
#endif
                                ActionsPerformWithDelay(new System.Action(() => { LoadRewardedAd(RewardedType.rewardedInterstitial); }), timeDuration);
                                timeDuration += delayTime;
#if RemoteConfigManager_Firebase
                            }
                        }
#endif
                        break;
                    }
                case AdLoadPriority.Native:
                    {
#if RemoteConfigManager_Firebase
                        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isNativeOverlayLoad))
                        {
                            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isNativeOverlayLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isNativeOverlayFromRemote);
                            if (isNativeOverlayFromRemote)
                            {
#endif
                                ActionsPerformWithDelay(new System.Action(() => { LoadNativeInternal(); }), timeDuration);
                                timeDuration += delayTime;
#if RemoteConfigManager_Firebase
                            }
                        }
#endif
                        break;
                    }
                case AdLoadPriority.NativeFullScreen:
                    {
#if RemoteConfigManager_Firebase
                        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isNativeOverlayFullScreenLoad))
                        {
                            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isNativeOverlayFullScreenLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isNativeOverlayFullScreenFromRemote);
                            if (isNativeOverlayFullScreenFromRemote)
                            {
#endif
                                ActionsPerformWithDelay(new System.Action(() => { LoadNativeInternal(isFullScreenNative: true); }), timeDuration);
                                timeDuration += delayTime;
#if RemoteConfigManager_Firebase
                            }
                        }
#endif
                        break;
                    }
            }
        }
    }

    InitilizeStatus initilizeStatus = InitilizeStatus.None;
    enum InitilizeStatus
    {
        None,
        Initilizing,
        Initilized,
        Failed
    }

    #endregion

    #region Banner
    public void ShowBanner(BannerType banner, BannerPosition pos = BannerPosition.Centered)
    {
        ShowBanner(banner: banner, pos: GetAdmobBannerPosition(pos));
    }
    public void ShowBanner(BannerType banner, AdPosition pos = AdPosition.Center)
    {
        if (!IsInternetConnection()) { HideBanners(); ShowDebugLog("No " + banner + " Shown.\nReason: No Internet"); return; }
        if (IsRemoveAd()) { HideBanners(); ShowDebugLog("No " + banner + " Shown.\nReason: Remove Ads"); return; }

        ref bool refIsShow = ref isSmallBannerShow;
        ref AdPosition refPos = ref lastSmallBannerPosition;
        AdSize refSize = AdSize.Banner;

        switch (banner)
        {
            case BannerType.SmallBannerType:
                {
                    refIsShow = ref isSmallBannerShow; refSize = AdSize.Banner; refPos = ref lastSmallBannerPosition;
#if MonetizationAnalytics
                    MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_SmallBanner, MonetizationAnalytics.ParameterKey.myParameter,
                        MonetizationAnalytics.ParameterValue.ShowCall);
#endif
                    break;
                }
            case BannerType.SmallBannerGamePlayType:
                {
                    refIsShow = ref isSmallBannerGamePlayShow; refSize = AdSize.Banner; refPos = ref lastSmallBannerGamePlayPosition;

#if MonetizationAnalytics
                    MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_SmallBanner, MonetizationAnalytics.ParameterKey.myParameter,
                        MonetizationAnalytics.ParameterValue.ShowCall);
#endif
                    break;
                }
            case BannerType.LargeBannerType:
                {
                    refIsShow = ref isLargeBannerShow; refSize = AdSize.MediumRectangle; refPos = ref lastLargeBannerPosition;
#if MonetizationAnalytics
                    MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_LargeBanner, MonetizationAnalytics.ParameterKey.myParameter,
                        MonetizationAnalytics.ParameterValue.ShowCall); 
#endif
                    break;
                }
            case BannerType.SmartBannerType:
                {
                    refIsShow = ref isSmartBannerShow; refSize = AdSize.SmartBanner; refPos = ref lastSmartBannerPosition;
#if MonetizationAnalytics
                    MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_SmallBanner, MonetizationAnalytics.ParameterKey.myParameter,
                        MonetizationAnalytics.ParameterValue.ShowCall); 
#endif
                    break;
                }
            case BannerType.CustomBannerType: { return; }
            case BannerType.AdaptiveBannerType:
                {
                    refIsShow = ref isAdaptiveBannerShow; refPos = ref lastAdaptiveBannerPosition;
                    refSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth); break;
                }
        }

        foreach (var currentBannerType in bannersList)
        {
            if (!currentBannerType.Equals(banner)) if (IsBannerShowing(currentBannerType)) HideBannerInternal(currentBannerType);
        }

        refIsShow = true;
        refPos = pos.Equals(AdPosition.Center) ? refPos : pos;
        if (IsBannerLoaded(banner, loadIfNotLoaded: true, isShowAfterLoad: true)) ShowBannerInternal(banner, refSize, pos);
    }
    public void ShowBanner(BannerType banner, RectTransform rectTransform)
    {
        if (!IsInternetConnection()) { HideBanners(); ShowDebugLog("No " + banner + " Shown.\nReason: No Internet"); return; }
        if (IsRemoveAd()) { HideBanners(); ShowDebugLog("No " + banner + " Shown.\nReason: Remove Ads"); return; }
        if (!banner.Equals(BannerType.CustomBannerType)) { ShowDebugLog("No " + banner + " Shown.\nReason: No CustomBanner Calls"); return; }
        if (rectTransform == null) { ShowDebugLog("No " + banner + " Shown.\nReason: rectTransform = null"); return; }

        foreach (var currentBannerType in bannersList)
        {
            if (!currentBannerType.Equals(banner)) { if (IsBannerShowing(currentBannerType)) HideBannerInternal(currentBannerType); }
        }

        isCustomBannerShow = true;
        Vector2 BannerSize;
        BannerSize.x = (int)ConvertPixelsToDp(RectTransformToScreenSpace(rectTransform).size.x);
        BannerSize.y = (int)ConvertPixelsToDp(RectTransformToScreenSpace(rectTransform).size.y);
        AdSize refSize = new AdSize(Mathf.FloorToInt(BannerSize.x), Mathf.FloorToInt(BannerSize.y));
        lastCustomBannerRectTransform = rectTransform;
        lastCustomBannerAdSize = refSize;

        if (IsBannerLoaded(banner, loadIfNotLoaded: true, isShowAfterLoad: true)) ShowBannerInternal(banner, refSize, rectTransform);
    }
    public void HideBanners()
    {
        foreach (var currentBannerType in bannersList) { if (IsBannerShowing(currentBannerType)) HideBannerInternal(currentBannerType); }
        afterLoadingBannerShowAction = null;
    }
    public void HideBanner(BannerType banner)
    {
        if (IsBannerShowing(banner)) HideBannerInternal(banner);
    }
    public bool IsBannerLoaded(BannerType banner, bool loadIfNotLoaded = false, bool isShowAfterLoad = false)
    {
        if (!IsInternetConnection()) return false;
        if (IsRemoveAd()) return false;

        switch (banner)
        {
            case BannerType.SmallBannerType:
                {
                    foreach (var data in BannerData.Values) if (data.AdType.Equals(banner) && data.IsLoaded) return true;
                    if (loadIfNotLoaded) LoadAd(banner, AdSize.Banner, lastSmallBannerPosition, isShowAfterLoad);
                    break;
                }
            case BannerType.SmallBannerGamePlayType:
                {
                    foreach (var data in BannerData.Values) if (data.AdType.Equals(banner) && data.IsLoaded) return true;
                    if (loadIfNotLoaded) LoadAd(banner, AdSize.Banner, lastSmallBannerGamePlayPosition, isShowAfterLoad);
                    break;
                }
            case BannerType.LargeBannerType:
                {
                    foreach (var data in BannerData.Values) if (data.AdType.Equals(banner) && data.IsLoaded) return true;
                    if (loadIfNotLoaded) LoadAd(banner, AdSize.MediumRectangle, lastLargeBannerPosition, isShowAfterLoad);
                    break;
                }
            case BannerType.SmartBannerType:
                {
                    foreach (var data in BannerData.Values) if (data.AdType.Equals(banner) && data.IsLoaded) return true;
                    if (loadIfNotLoaded) LoadAd(banner, AdSize.SmartBanner, lastSmartBannerPosition, isShowAfterLoad);
                    break;
                }
            case BannerType.AdaptiveBannerType:
                {
                    foreach (var data in BannerData.Values) if (data.AdType.Equals(banner) && data.IsLoaded) return true;
                    if (loadIfNotLoaded) LoadAd(banner, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), lastAdaptiveBannerPosition, isShowAfterLoad);
                    break;
                }
            case BannerType.CustomBannerType:
                {
                    if (lastCustomBannerAdSize == null) return false;
                    foreach (var data in BannerData.Values)
                    {
                        if (data.AdType.Equals(banner))
                        {
                            if (data.IsLoaded)
                            {
                                if (data.GetAd() is BannerView bannerAd)
                                {
                                    try
                                    {
                                        if (lastCustomBannerRectTransform != null)
                                        {
                                            float rectWidth = lastCustomBannerRectTransform.rect.width * lastCustomBannerRectTransform.GetComponentInParent<Canvas>().scaleFactor;
                                            float rectHeight = lastCustomBannerRectTransform.rect.height * lastCustomBannerRectTransform.GetComponentInParent<Canvas>().scaleFactor;
                                            float bannerWidth = -1;
                                            float bannerHeight = -1;
                                            try
                                            {
                                                bannerWidth = bannerAd.GetWidthInPixels();
                                                bannerHeight = bannerAd.GetHeightInPixels();
                                            }
                                            catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                                            if (bannerWidth == -1 || bannerHeight == -1) { ShowDebugLog("No " + banner.ToString() + " Available.\nReason : Banner Widht/Heigh NULL"); return false; }

                                            float pixelDiffrenceThreshold = 10;
                                            if ((Mathf.Abs(rectWidth - bannerWidth) < pixelDiffrenceThreshold) && (Mathf.Abs(rectHeight - bannerHeight) < pixelDiffrenceThreshold))
                                            {
                                                return true;
                                            }
                                        }
                                    }
                                    catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                                }
                            }
                        }
                    }
                    if (loadIfNotLoaded)
                    {
                        LoadAd(banner, lastCustomBannerAdSize, lastCustomBannerRectTransform, isShowAfterLoad);
                    }
                    break;
                }
        }
        return false;
    }
    public bool IsBannerShowing(BannerType banner)
    {
        switch (banner)
        {
            case BannerType.SmallBannerType: { return isSmallBannerShow; }
            case BannerType.SmallBannerGamePlayType: { return isSmallBannerGamePlayShow; }
            case BannerType.LargeBannerType: { return isLargeBannerShow; }
            case BannerType.SmartBannerType: { return isSmartBannerShow; }
            case BannerType.AdaptiveBannerType: { return isAdaptiveBannerShow; }
            case BannerType.CustomBannerType: { return isCustomBannerShow; }
        }
        return false;
    }
    public AdPosition LastBannerPosition(BannerType banner)
    {
        switch (banner)
        {
            case BannerType.SmallBannerType: { return lastSmallBannerPosition; }
            case BannerType.SmallBannerGamePlayType: { return lastSmallBannerGamePlayPosition; }
            case BannerType.LargeBannerType: { return lastLargeBannerPosition; }
            case BannerType.SmartBannerType: { return lastSmartBannerPosition; }
            case BannerType.AdaptiveBannerType: { return lastAdaptiveBannerPosition; }
        }
        return AdPosition.Center;
    }
    public enum BannerType
    {
        SmallBannerType,
        SmallBannerGamePlayType,
        LargeBannerType,
        AdaptiveBannerType,
        SmartBannerType,
        CustomBannerType
    }

    #region Banner Internal

    AdPosition lastSmallBannerPosition = AdPosition.Top;
    bool isSmallBannerShow = false;
    AdPosition lastSmallBannerGamePlayPosition = AdPosition.Top;
    bool isSmallBannerGamePlayShow = false;
    AdPosition lastLargeBannerPosition = AdPosition.BottomRight;
    bool isLargeBannerShow = false;
    AdPosition lastAdaptiveBannerPosition = AdPosition.Bottom;
    bool isAdaptiveBannerShow = false;
    AdPosition lastSmartBannerPosition = AdPosition.Top;
    bool isSmartBannerShow = false;
    public RectTransform lastCustomBannerRectTransform;
    AdSize lastCustomBannerAdSize = null;
    bool isCustomBannerShow = false;

    void ShowBannerInternal(BannerType adType, AdSize adSize, AdPosition pos)
    {
        if (IsRemoveAd()) { ShowDebugLog("No " + adType + " Shown.\nReason : Remove Ads."); return; }
        if (!IsInternetConnection()) { ShowDebugLog("No " + adType + " Shown.\nReason : No Internet."); return; }
        if (!CheckInitialization(false, new System.Action(() => { ShowBannerInternal(adType, adSize, pos); }))) { ShowDebugLog("No " + adType + " Shown.\nReason : No SDK Initilized."); return; }
        if (IsLowMemory(NoSmallBannerBelow)) { ShowDebugLog("No " + adType + " Shown.\nReason : Low Available Memory"); return; }
        //if (LoadingAdBG.activeSelf) { ShowDebugLog("No " + adType + " Shown.\nReason : Loading AD BG ActiveSelf"); return; }
#if UTM_Class
        if (UTM.UTM_Handler.IsUTMShowingFullScreenAd) { ShowDebugLog("No " + adType + " Shown.\nReason : UTM Showing."); return; }
#endif

        ref bool isIDAvailable = ref isSmallBannerIdAvailable;
        ref bool isShowBanner = ref isSmallBannerShow;
        ref AdPosition lastPos = ref lastSmallBannerPosition;

        System.Collections.Generic.List<string> genericList = new System.Collections.Generic.List<string>();
        int listCount = 0;
        switch (adType)
        {
            case BannerType.SmallBannerType:
                {
#if AdRevenueAnalytics
                    genericList = AdRevenueAnalytics.Instance.GetAdIDsList(AdRevenueAnalytics.AdType.SmallBanner);
                    genericList ??= smallBannerID;
#else
                    genericList = smallBannerID;
#endif
                    isShowBanner = ref isSmallBannerShow;
                    lastPos = ref lastSmallBannerPosition;
                    isIDAvailable = ref isSmallBannerIdAvailable;
                    listCount = smallBannerID.Count;
#if RemoteConfigManager_Firebase
                    if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isSmallBannerLoad))
                    {
                        RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isSmallBannerLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isSmallBannerFromRemote);
                        if (!isSmallBannerFromRemote) { ShowDebugLog("No " + adType + " Shown.\nReason : RemoteConfig Not Allowed.\nKey: " + isSmallBannerLoad + ", value :" + isSmallBannerFromRemote); return; }
                    }
#endif
                    break;
                }
            case BannerType.LargeBannerType:
                {
#if AdRevenueAnalytics
                    genericList = AdRevenueAnalytics.Instance.GetAdIDsList(AdRevenueAnalytics.AdType.LargeBanner);
                    genericList ??= largeBannerID;
#else
                    genericList = largeBannerID;
#endif
                    isShowBanner = ref isLargeBannerShow;
                    lastPos = ref lastLargeBannerPosition;
                    isIDAvailable = ref isLargeBannerIdAvailable;
                    listCount = largeBannerID.Count;
#if RemoteConfigManager_Firebase
                    if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isLargeBannerLoad))
                    {
                        RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isLargeBannerLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isLargeBannerFromRemote);
                        if (!isLargeBannerFromRemote) { ShowDebugLog("No " + adType + " Shown.\nReason : RemoteConfig Not Allowed.\nKey: " + isLargeBannerLoad + ", value :" + isLargeBannerFromRemote); return; }
                    }
#endif
                    break;
                }
            case BannerType.AdaptiveBannerType:
                {
#if AdRevenueAnalytics
                    genericList = AdRevenueAnalytics.Instance.GetAdIDsList(AdRevenueAnalytics.AdType.AdaptiveBanner);
                    genericList ??= adaptiveBannerID;
#else
                    genericList = adaptiveBannerID;
#endif
                    isShowBanner = ref isAdaptiveBannerShow;
                    lastPos = ref lastAdaptiveBannerPosition;
                    isIDAvailable = ref isAdaptiveBannerIdAvailable;
                    listCount = adaptiveBannerID.Count;
#if RemoteConfigManager_Firebase
                    if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isAdaptiveBannerLoad))
                    {
                        RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isAdaptiveBannerLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isAdaptiveBannerFromRemote);
                        if (!isAdaptiveBannerFromRemote) { ShowDebugLog("No " + adType + " Shown.\nReason : RemoteConfig Not Allowed.\nKey: " + isAdaptiveBannerLoad + ", value :" + isAdaptiveBannerFromRemote); return; }
                    }
#endif
                    break;
                }
            case BannerType.SmartBannerType:
                {
#if AdRevenueAnalytics
                    genericList = AdRevenueAnalytics.Instance.GetAdIDsList(AdRevenueAnalytics.AdType.SmartBanner);
                    genericList ??= smartBannerID;
#else
                    genericList = smartBannerID;
#endif
                    isShowBanner = ref isSmartBannerShow;
                    lastPos = ref lastSmartBannerPosition;
                    isIDAvailable = ref isSmartBannerIdAvailable;
                    listCount = smartBannerID.Count;
#if RemoteConfigManager_Firebase
                    if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isSmartBannerLoad))
                    {
                        RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isSmartBannerLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isSmartBannerFromRemote);
                        if (!isSmartBannerFromRemote) { ShowDebugLog("No " + adType + " Shown.\nReason : RemoteConfig Not Allowed.\nKey: " + isSmartBannerLoad + ", value :" + isSmartBannerFromRemote); return; }
                    }
#endif
                    break;
                }
            case BannerType.SmallBannerGamePlayType:
                {
#if AdRevenueAnalytics
                    genericList = AdRevenueAnalytics.Instance.GetAdIDsList(AdRevenueAnalytics.AdType.SmallBannerGamePlay);
                    genericList ??= smallBannerGamePlayID;
#else
                    genericList = smallBannerGamePlayID;
#endif
                    isShowBanner = ref isSmallBannerGamePlayShow;
                    lastPos = ref lastSmallBannerGamePlayPosition;
                    isIDAvailable = ref isSmallBannerGamePlayIdAvailable;
                    listCount = smallBannerGamePlayID.Count;
#if RemoteConfigManager_Firebase
                    if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isSmallBannerLoad))
                    {
                        RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isSmallBannerLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isSmallBannerFromRemote);
                        if (!isSmallBannerFromRemote) { ShowDebugLog("No " + adType + " Shown.\nReason : RemoteConfig Not Allowed.\nKey: " + isSmallBannerLoad + ", value :" + isSmallBannerFromRemote); return; }
                    }
#endif
                    break;
                }
            case BannerType.CustomBannerType: { ShowDebugLog("No " + adType + " Shown.\nReason : " + adType + " Invalid Function Calling."); return; }
        }

        if (!isIDAvailable && !isTestMode) { ShowDebugLog("No " + adType + " Shown.\nReason : Not ID enable nor Testing Mode"); return; }

        // hide previous all this type of banners
        foreach (var currentCustomBanner in BannerData)
        {
            if (currentCustomBanner.Value.IsLoaded) if (currentCustomBanner.Value.GetAd() is BannerView bannerAd) { try { MobileAdsEventExecutor.ExecuteInUpdate(() => bannerAd?.Hide()); } catch (System.Exception e) { SendExceptionToCrashlytics(e); } }
        }
        isShowBanner = false;

        if (genericList == null) { ShowDebugLog("No " + adType + " Shown.\nReason : Banner GenericList Null."); return; }
        //show priority banner
        foreach (string genericID in genericList)
        {
            foreach (var kvp in BannerData)
            {
                if (kvp.Key.Contains(genericID) && kvp.Value.AdType.Equals(adType) && kvp.Value.IsLoaded)
                {
                    ShowDebugLog("Show " + adType + ".\nID: " + kvp.Key+"\nPosition: "+pos);
                    if (kvp.Value.GetAd() is BannerView bannerAd) { try { MobileAdsEventExecutor.ExecuteInUpdate(() => bannerAd?.SetPosition(pos)); } catch (System.Exception e) { SendExceptionToCrashlytics(e); } ShowBannerBG(bannerAd, pos, adSize); }
                    try { kvp.Value.ShowAd(); } catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                    isShowBanner = true;
                    lastPos = pos.Equals(AdPosition.Center) ? lastPos : pos;
                    SendAnalytics("Ad_" + adType.ToString() + "_Show");

#if AdRevenueAnalytics
                    BannerType[] bannerAdType =
                        {
                        BannerType.SmallBannerType,
                        BannerType.SmallBannerGamePlayType,
                        BannerType.LargeBannerType,
                        BannerType.AdaptiveBannerType,
                        BannerType.SmartBannerType,
                        BannerType.CustomBannerType,
                    };
                    AdRevenueAnalytics.AdType[] adRevenueAdType =
                    {
                        AdRevenueAnalytics.AdType.SmallBanner,
                        AdRevenueAnalytics.AdType.SmallBannerGamePlay,
                        AdRevenueAnalytics.AdType.LargeBanner,
                        AdRevenueAnalytics.AdType.AdaptiveBanner,
                        AdRevenueAnalytics.AdType.SmartBanner,
                        AdRevenueAnalytics.AdType.CustomBanner,
                    };
                    for (int i = 0; i < bannerAdType.Length; i++)
                    {
                        if (bannerAdType[i] == adType)
                        {
                            AdRevenueAnalytics.Instance.SendRevenueAnalytics(adRevenueAdType[i], AdRevenueAnalytics.AdRevenueEventType.Ad_Rev_Progression, status: AdRevenueAnalytics.Status.Showing);
                            break;
                        }
                    }
#endif

#if MonetizationAnalytics
                    switch (adType)
                    {
                        case BannerType.SmallBannerType:
                            {
                                MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_SmallBanner, MonetizationAnalytics.ParameterKey.myParameter,
                                    MonetizationAnalytics.ParameterValue.Showing);
                                break;
                            }
                        case BannerType.SmallBannerGamePlayType:
                            {
                                MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_SmallBanner, MonetizationAnalytics.ParameterKey.myParameter,
                                    MonetizationAnalytics.ParameterValue.Showing);
                                break;
                            }
                        case BannerType.LargeBannerType:
                            {
                                MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_LargeBanner, MonetizationAnalytics.ParameterKey.myParameter,
                                    MonetizationAnalytics.ParameterValue.Showing);
                                break;
                            }
                        case BannerType.SmartBannerType:
                            {
                                MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_SmallBanner, MonetizationAnalytics.ParameterKey.myParameter,
                                    MonetizationAnalytics.ParameterValue.Showing);
                                break;
                            }
                    }
#endif

                    return;
                }
            }
        }

#if AdRevenueAnalytics
        LoadAd(adType, adSize, pos);
        ShowDebugLog("No " + adType + " Ads Available\nReason: AdPilot Provided ID's Not Loaded.");
        return;
#endif
        foreach (var data in BannerData.Values)
        {
            if (adType.Equals(data.AdType))
            {
                if (data.IsLoaded)
                {
                    ShowDebugLog("Show " + adType + "\nID: " + data.ID);
                    if (data.GetAd() is BannerView bannerAd) { try { MobileAdsEventExecutor.ExecuteInUpdate(() => bannerAd?.SetPosition(pos)); } catch (System.Exception e) { SendExceptionToCrashlytics(e); } ShowBannerBG(bannerAd, pos, adSize); }
                    MobileAdsEventExecutor.ExecuteInUpdate(() => data.ShowAd());
                    isShowBanner = true;
                    lastPos = pos;
                    SendAnalytics("Ad_" + adType.ToString() + "_Show");

#if AdRevenueAnalytics
                    BannerType[] bannerAdType =
                        {
                        BannerType.SmallBannerType,
                        BannerType.SmallBannerGamePlayType,
                        BannerType.LargeBannerType,
                        BannerType.AdaptiveBannerType,
                        BannerType.SmartBannerType,
                        BannerType.CustomBannerType,
                    };
                    AdRevenueAnalytics.AdType[] adRevenueAdType =
                    {
                        AdRevenueAnalytics.AdType.SmallBanner,
                        AdRevenueAnalytics.AdType.SmallBannerGamePlay,
                        AdRevenueAnalytics.AdType.LargeBanner,
                        AdRevenueAnalytics.AdType.AdaptiveBanner,
                        AdRevenueAnalytics.AdType.SmartBanner,
                        AdRevenueAnalytics.AdType.CustomBanner,
                    };
                    for (int i = 0; i < bannerAdType.Length; i++)
                    {
                        if (bannerAdType[i] == adType)
                        {
                            AdRevenueAnalytics.Instance.SendRevenueAnalytics(adRevenueAdType[i], AdRevenueAnalytics.AdRevenueEventType.Ad_Rev_Progression, status: AdRevenueAnalytics.Status.Showing);
                            break;
                        }
                    }
#endif

#if MonetizationAnalytics
                    switch (adType)
                    {
                        case BannerType.SmallBannerType:
                            {
                                MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_SmallBanner, MonetizationAnalytics.ParameterKey.myParameter,
                                    MonetizationAnalytics.ParameterValue.Showing);
                                break;
                            }
                        case BannerType.SmallBannerGamePlayType:
                            {
                                MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_SmallBanner, MonetizationAnalytics.ParameterKey.myParameter,
                                    MonetizationAnalytics.ParameterValue.Showing);
                                break;
                            }
                        case BannerType.LargeBannerType:
                            {
                                MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_LargeBanner, MonetizationAnalytics.ParameterKey.myParameter,
                                    MonetizationAnalytics.ParameterValue.Showing);
                                break;
                            }
                        case BannerType.SmartBannerType:
                            {
                                MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_SmallBanner, MonetizationAnalytics.ParameterKey.myParameter,
                                    MonetizationAnalytics.ParameterValue.Showing);
                                break;
                            }
                    }
#endif
                    return;
                }
            }
        }
        ShowDebugLog("No " + adType + " Available.");
    }
    void ShowBannerInternal(BannerType adType, AdSize adSize, RectTransform rectTransform)
    {
        if (IsRemoveAd()) { ShowDebugLog("No " + adType + " Shown.\nReason : Remove Ads."); return; }
        if (!IsInternetConnection()) { ShowDebugLog("No " + adType + " Shown.\nReason : No Internet."); return; }
        if (!isCustomBannerIdAvailable && !isTestMode) { ShowDebugLog("No " + adType + " Shown.\nReason : Not CustomBannerID enable Nor TestMode Enable"); return; }
        if (!CheckInitialization(false, new System.Action(() => { ShowBannerInternal(adType, adSize, rectTransform); }))) { ShowDebugLog("No " + adType + " Shown.\nReason : No SDK Initilized."); return; }
        if (IsLowMemory(NoSmallBannerBelow)) { ShowDebugLog("No " + adType + " Shown.\nReason : Low Available Memory"); return; }
        if (!adType.Equals(BannerType.CustomBannerType)) { ShowDebugLog("No " + adType + " Shown.\nReason : AdType is Not CustomBannerType"); return; }
        //if (LoadingAdBG.activeSelf) return;
#if RemoteConfigManager_Firebase
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isCustomBannerLoad))
        {
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isCustomBannerLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isCustomBannerFromRemote);
            if (!isCustomBannerFromRemote) { ShowDebugLog("No " + adType + " Shown.\nReason : RemoteConfig Not Allowed.\nKey: " + isCustomBannerLoad + ", value :" + isCustomBannerFromRemote); return; }
        }
#endif
#if UTM_Class
        if (UTM.UTM_Handler.IsUTMShowingFullScreenAd) { ShowDebugLog("No " + adType + " Shown.\nReason : UTM Showing."); return; }
#endif
        // hide previous all this type of custom banners
        foreach (var currentCustomBanner in BannerData.Values)
        {
            if (currentCustomBanner.AdType.Equals(BannerType.CustomBannerType))
                if (currentCustomBanner.IsLoaded)
                    if (currentCustomBanner.GetAd() is BannerView bannerAd)
                    {
                        try
                        {
                            MobileAdsEventExecutor.ExecuteInUpdate(() => bannerAd?.Hide());
                        }
                        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                    }
        }

        System.Collections.Generic.List<string> genericList = new System.Collections.Generic.List<string>();
#if AdRevenueAnalytics
        genericList = AdRevenueAnalytics.Instance.GetAdIDsList(AdRevenueAnalytics.AdType.CustomBanner);
        genericList ??= customBannerID;
#else
        genericList = customBannerID;
#endif
        isCustomBannerShow = false;
        if (genericList == null) { ShowDebugLog("No " + adType + " Shown.\nReason : Banner GenericList Null."); return; }

        //show priority banner
        foreach (string genericID in genericList)
        {
            foreach (var kvp in BannerData)
            {
                if (kvp.Key.Contains(genericID) && kvp.Value.AdType.Equals(adType) && kvp.Value.IsLoaded)
                {
                    Vector2 BannerPosition;
                    BannerPosition.x = (int)ConvertPixelsToDp(RectTransformToScreenSpace(rectTransform).position.x);
                    BannerPosition.y = (int)ConvertPixelsToDp(RectTransformToScreenSpace(rectTransform).position.y);
                    if (kvp.Value.GetAd() is BannerView bannerAd)
                    {
                        try
                        {
                            if (lastCustomBannerRectTransform == null) lastCustomBannerRectTransform = rectTransform;
                            float rectWidth = lastCustomBannerRectTransform.rect.width * lastCustomBannerRectTransform.GetComponentInParent<Canvas>().scaleFactor;
                            float rectHeight = lastCustomBannerRectTransform.rect.height * lastCustomBannerRectTransform.GetComponentInParent<Canvas>().scaleFactor;
                            float bannerWidth = -1;
                            float bannerHeight = -1;
                            try
                            {
                                bannerWidth = bannerAd.GetWidthInPixels();
                                bannerHeight = bannerAd.GetHeightInPixels();
                            }
                            catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                            if (bannerWidth == -1 || bannerHeight == -1) { ShowDebugLog("No " + adType + " Shown.\nReason : Banner Width/Height NULL"); return; }

                            float pixelDiffrenceThreshold = 10;
                            if ((Mathf.Abs(rectWidth - bannerWidth) < pixelDiffrenceThreshold) && (Mathf.Abs(rectHeight - bannerHeight) < pixelDiffrenceThreshold))
                            {
                                ShowDebugLog("Show " + adType + ", Pos: " + BannerPosition + ", Size: " + rectTransform.rect.size + ".\nID: " + kvp.Key);
                                try
                                {
                                    MobileAdsEventExecutor.ExecuteInUpdate(() =>
                                    {
                                        kvp.Value.Ad?.SetPosition(Mathf.FloorToInt(BannerPosition.x), Mathf.FloorToInt(BannerPosition.y));
                                        kvp.Value.ShowAd();
                                    });
                                }
                                catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                                lastCustomBannerRectTransform = rectTransform;
                                isCustomBannerShow = true;
                                SendAnalytics("Ad_" + adType.ToString() + "_Show");
#if AdRevenueAnalytics
                                AdRevenueAnalytics.Instance.SendRevenueAnalytics(AdRevenueAnalytics.AdType.CustomBanner,
                                    AdRevenueAnalytics.AdRevenueEventType.Ad_Rev_Progression, status: AdRevenueAnalytics.Status.Showing);
#endif
                                break;
                            }
                        }
                        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                    }
                    else LoadAd(adType, adSize, rectTransform);
                }
            }
        }
#if AdRevenueAnalytics
        ShowDebugLog("No " + adType + " Ads Available\nReason: AdPilot Provided ID's Not Loaded.");
        return;
#endif
        foreach (var data in BannerData.Values)
        {
            if (adType.Equals(data.AdType))
            {
                if (data.IsLoaded)
                {
                    Vector2 BannerPosition;
                    BannerPosition.x = (int)ConvertPixelsToDp(RectTransformToScreenSpace(rectTransform).position.x);
                    BannerPosition.y = (int)ConvertPixelsToDp(RectTransformToScreenSpace(rectTransform).position.y);

                    ShowDebugLog("Show " + adType + "\nID: " + data.ID);
                    if (data.GetAd() is BannerView bannerAd) { try { MobileAdsEventExecutor.ExecuteInUpdate(() => bannerAd?.SetPosition(Mathf.FloorToInt(BannerPosition.x), Mathf.FloorToInt(BannerPosition.y))); } catch (System.Exception e) { SendExceptionToCrashlytics(e); } }
                    MobileAdsEventExecutor.ExecuteInUpdate(() => data.ShowAd());
                    lastCustomBannerRectTransform = rectTransform;
                    isCustomBannerShow = true;
                    SendAnalytics("Ad_" + adType.ToString() + "_Show");
#if AdRevenueAnalytics
                    AdRevenueAnalytics.Instance.SendRevenueAnalytics(AdRevenueAnalytics.AdType.CustomBanner,
                        AdRevenueAnalytics.AdRevenueEventType.Ad_Rev_Progression, status: AdRevenueAnalytics.Status.Showing);
#endif
                    return;
                }
            }
        }
        ShowDebugLog("No " + adType + " Available.");
    }
    void HideBannerInternal(BannerType adType, bool isFromFocus = false)
    {
        ref bool refIsShow = ref isSmallBannerShow;
        AdSize refAdSize = AdSize.Banner;
        switch (adType)
        {
            case BannerType.SmallBannerType: { refIsShow = ref isSmallBannerShow; refAdSize = AdSize.Banner; break; }
            case BannerType.SmallBannerGamePlayType: { refIsShow = ref isSmallBannerGamePlayShow; refAdSize = AdSize.Banner; break; }
            case BannerType.LargeBannerType: { refIsShow = ref isLargeBannerShow; refAdSize = AdSize.MediumRectangle; break; }
            case BannerType.AdaptiveBannerType:
                {
                    refIsShow = ref isAdaptiveBannerShow;
                    refAdSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth); break;
                }
            case BannerType.SmartBannerType: { refIsShow = ref isSmartBannerShow; refAdSize = AdSize.SmartBanner; break; }
            case BannerType.CustomBannerType:
                {
                    refIsShow = ref isCustomBannerShow;
                    refAdSize = null;
                    break;
                }
        }
        foreach (var ad in BannerData.Values)
        {
            if (adType.Equals(ad.AdType))
            {
                if (ad.IsLoaded)
                {
                    ShowDebugLog("Hide " + ad.AdType + "\nID: " + ad.ID + "\nFrom Plugin :" + isFromFocus);
                    if (ad.GetAd() is BannerView bannerAd) { try { MobileAdsEventExecutor.ExecuteInUpdate(() => bannerAd?.Hide()); } catch (System.Exception e) { SendExceptionToCrashlytics(e); } }
                }
            }
        }
        if (!isFromFocus) refIsShow = false;
        HideBannerBG(refAdSize ?? AdSize.Banner);
    }
    public void SetBannerShowing(BannerType banner,bool status)
    {
        switch (banner)
        {
            case BannerType.SmallBannerType: { isSmallBannerShow = status; break; }
            case BannerType.SmallBannerGamePlayType: { isSmallBannerGamePlayShow = status; break; }
            case BannerType.LargeBannerType: { isLargeBannerShow = status; break; }
            case BannerType.SmartBannerType: { isSmartBannerShow = status; break; }
            case BannerType.AdaptiveBannerType: { isAdaptiveBannerShow = status; break; }
            case BannerType.CustomBannerType: { isCustomBannerShow = status; break; }
        }
    }
    System.Collections.Generic.List<BannerType> bannersList = new System.Collections.Generic.List<BannerType>
        {
            BannerType.SmallBannerType, BannerType.SmallBannerGamePlayType, BannerType.LargeBannerType,
            BannerType.AdaptiveBannerType, BannerType.SmartBannerType,BannerType.CustomBannerType
        };

    #endregion

    #region Banner Generic

    readonly System.Collections.Concurrent.ConcurrentDictionary<string, BannerClass> BannerData = new System.Collections.Concurrent.ConcurrentDictionary<string, BannerClass>();
    public class BannerClass
    {
        public string ID { get; set; }
        public bool IsLoaded { get; set; }
        public bool IsShowAfterLoad { get; set; } = false;
        public BannerType AdType { get; set; }
        public BannerView Ad { get; private set; }

        // Keep strong refs for unsubscribe
        public System.Action<LoadAdError> OnLoadFailedHandler;
        public System.Action OnLoadedHandler;
        public System.Action<AdValue> OnPaidHandler;
        public System.Action OnFullScreenOpenedHandler;
        public System.Action OnClickedHandler;

        public BannerClass(string id, BannerType type, bool load = false, BannerView ad = null)
        {
            ID = id;
            AdType = type;
            IsLoaded = load;
            Ad = ad;
        }
        public void Loaded(object ad)
        {
            if (ad is BannerView bannerAd) { IsLoaded = true; Ad = bannerAd; }
            else throw new System.ArgumentException("Invalid ad type passed to Loaded method.");
        }

        public void Remove() // safe destroy wrapper
        {
            try
            {
                if (Ad != null)
                {
                    // Unsubscribe first (if previously subscribed)
                    if (OnLoadFailedHandler != null) Ad.OnBannerAdLoadFailed -= OnLoadFailedHandler;
                    if (OnLoadedHandler != null) Ad.OnBannerAdLoaded -= OnLoadedHandler;
                    if (OnPaidHandler != null) Ad.OnAdPaid -= OnPaidHandler;
                    if (OnFullScreenOpenedHandler != null) Ad.OnAdFullScreenContentOpened -= OnFullScreenOpenedHandler;
                    if (OnClickedHandler != null) Ad.OnAdClicked -= OnClickedHandler;

                    try { GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() => Ad?.Hide()); }
                    catch { /* no-op */ }

                    Ad.Destroy();
                }
            }
            catch (System.Exception e) { SendExceptionToCrashlytics_Static(e); }
            finally
            {
                Ad = null;
                OnLoadFailedHandler = null;
                OnLoadedHandler = null;
                OnPaidHandler = null;
                OnFullScreenOpenedHandler = null;
                OnClickedHandler = null;
                IsLoaded = false;
                IsShowAfterLoad = false;
            }
        }
        public void ShowAd(System.Action<bool> action = null)
        {
            try { MobileAdsEventExecutor.ExecuteInUpdate(() => Ad?.Show()); } catch (System.Exception e) { SendExceptionToCrashlytics_Static(e); }
            action?.Invoke(true);
        }
        public void UpdateAd(object ad)
        {
            if (ad is BannerView bannerAd) { Ad = bannerAd; }
            else throw new System.ArgumentException("Invalid ad type passed to Loaded method.");
        }
        public object GetAd()
        {
            return Ad;
        }
    }
    public void LoadAd(BannerType adType, AdSize adSize, AdPosition pos, bool isShowAfterLoad = false)
    {
        if (IsRemoveAd()) { ShowDebugLog("No " + adType + " Load.\nReason : Remove Ads."); return; }
        if (!IsInternetConnection()) { ShowDebugLog("No " + adType + " Load.\nReason : No Internet."); return; }
        if (!CheckInitialization(false, new System.Action(() => { ShowBannerInternal(adType, adSize, pos); }))) { ShowDebugLog("No " + adType + " Load.\nReason : No SDK Initilized."); return; }
        if (IsLowMemory(NoSmallBannerBelow)) { ShowDebugLog("No " + adType + " Load.\nReason : Low Available Memory"); return; }

        System.Collections.Generic.List<string> genericList = new System.Collections.Generic.List<string>();
        System.Action<string, BannerType, AdSize, AdPosition, EventName, bool> deferredFunction = LoadBannerGeneral;
        EventName AdEventName = EventName.Ad_SmallBanner;
        ref bool isIDAvailable = ref isSmallBannerIdAvailable;

        switch (adType)
        {
            case BannerType.SmallBannerType:
                {
                    genericList = smallBannerID; AdEventName = EventName.Ad_SmallBanner; isIDAvailable = ref isSmallBannerIdAvailable;
#if RemoteConfigManager_Firebase
                    if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isSmallBannerLoad))
                    {
                        RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isSmallBannerLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isSmallBannerFromRemote);
                        if (!isSmallBannerFromRemote) { ShowDebugLog("No " + adType + " Shown.\nReason : RemoteConfig Not Allowed.\nKey: " + isSmallBannerLoad + ", value :" + isSmallBannerFromRemote); return; }
                    }
#endif
                    break;
                }
            case BannerType.LargeBannerType:
                {
                    genericList = largeBannerID; AdEventName = EventName.Ad_LargeBanner; isIDAvailable = ref isLargeBannerIdAvailable;
#if RemoteConfigManager_Firebase
                    if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isLargeBannerLoad))
                    {
                        RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isLargeBannerLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isLargeBannerFromRemote);
                        if (!isLargeBannerFromRemote) { ShowDebugLog("No " + adType + " Shown.\nReason : RemoteConfig Not Allowed.\nKey: " + isLargeBannerLoad + ", value :" + isLargeBannerFromRemote); return; }
                    }
#endif
                    break;
                }
            case BannerType.AdaptiveBannerType:
                {
                    genericList = adaptiveBannerID; AdEventName = EventName.Ad_AdaptiveBanner; isIDAvailable = ref isAdaptiveBannerIdAvailable;
#if RemoteConfigManager_Firebase
                    if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isAdaptiveBannerLoad))
                    {
                        RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isAdaptiveBannerLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isAdaptiveBannerFromRemote);
                        if (!isAdaptiveBannerFromRemote) { ShowDebugLog("No " + adType + " Shown.\nReason : RemoteConfig Not Allowed.\nKey: " + isAdaptiveBannerLoad + ", value :" + isAdaptiveBannerFromRemote); return; }
                    }
#endif
                    break;
                }
            case BannerType.SmartBannerType:
                {
                    genericList = smartBannerID; AdEventName = EventName.Ad_SmartBanner; isIDAvailable = ref isSmartBannerIdAvailable;
#if RemoteConfigManager_Firebase
                    if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isSmartBannerLoad))
                    {
                        RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isSmartBannerLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isSmartBannerFromRemote);
                        if (!isSmartBannerFromRemote) { ShowDebugLog("No " + adType + " Shown.\nReason : RemoteConfig Not Allowed.\nKey: " + isSmartBannerLoad + ", value :" + isSmartBannerFromRemote); return; }
                    }
#endif
                    break;
                }
            case BannerType.SmallBannerGamePlayType:
                {
                    genericList = smallBannerGamePlayID; AdEventName = EventName.Ad_SmallBannerGamePlay; isIDAvailable = ref isSmallBannerGamePlayIdAvailable;
#if RemoteConfigManager_Firebase
                    if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isSmallBannerLoad))
                    {
                        RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isSmallBannerLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isSmallBannerFromRemote);
                        if (!isSmallBannerFromRemote) { ShowDebugLog("No " + adType + " Shown.\nReason : RemoteConfig Not Allowed.\nKey: " + isSmallBannerLoad + ", value :" + isSmallBannerFromRemote); return; }
                    }
#endif
                    break;
                }
        }
        if (!isTestMode && !isIDAvailable) { ShowDebugLog("No " + adType + " Shown.\nReason : ID's & Test Mode Disabled"); return; }

        for (int i = 0; i < genericList.Count; i++)
        {
            string currentID = genericList[i];
            if (string.IsNullOrEmpty(currentID) || (!IsValidID(ref currentID, adType.ToString()))) continue;

            currentID = adType + "_" + i + "_:" + currentID;
            if (!BannerData.ContainsKey(currentID)) { deferredFunction?.Invoke(currentID, adType, adSize, pos, AdEventName, isShowAfterLoad); isShowAfterLoad = false; }
        }
    }
    public void LoadAd(BannerType adType, AdSize adSize, RectTransform rectTransform, bool isShowAfterLoad = false)
    {
        if (IsRemoveAd()) { ShowDebugLog("No " + adType + " Load.\nReason : Remove Ads."); return; }
        if (!IsInternetConnection()) { ShowDebugLog("No " + adType + " Load.\nReason : No Internet."); return; }
        if (!CheckInitialization(false, new System.Action(() => { ShowBannerInternal(adType, adSize, rectTransform); }))) { ShowDebugLog("No " + adType + " Load.\nReason : No SDK Initilized."); return; }
        if (IsLowMemory(NoSmallBannerBelow)) { ShowDebugLog("No " + adType + " Load.\nReason : Low Available Memory."); return; }

        System.Action<string, BannerType, AdSize, RectTransform, EventName, bool> deferredFunction = LoadBannerGeneral;
        EventName AdEventName = EventName.Ad_CustomBanner;
        ref bool isIDAvailable = ref isSmallBannerIdAvailable;

        if (!isTestMode && !isCustomBannerIdAvailable) { ShowDebugLog("No " + adType + " Load.\nReason : ID's & Test Mode Disabled"); return; }

        for (int i = 0; i < customBannerID.Count; i++)
        {
            string currentNewID = adType + "_(" + /*Mathf.FloorToInt*/(rectTransform.rect.width) + "," + /*Mathf.FloorToInt*/(rectTransform.rect.height) + ")_:" + customBannerID[i];
            if (!BannerData.ContainsKey(currentNewID)) { deferredFunction?.Invoke(currentNewID, adType, adSize, rectTransform, AdEventName, isShowAfterLoad); }
        }
    }

    void LoadBannerGeneral(string key, BannerType type, AdSize adSize, AdPosition pos, EventName AdEventName, bool isShowAfterLoad = false)
    {
        if (IsRemoveAd()) { ShowDebugLog("No " + type + " Load.\nReason : Remove Ads."); return; }

        var splittingArray = key.Split(':');
        if (splittingArray.Length <= 1 || !splittingArray[1].StartsWith("ca-app-pub-"))
        {
            Debug.LogError("Invalid Key Found against " + type + ".\nID : " + key); return;
        }
        string adUnitID = splittingArray[1];
        if (BannerData.ContainsKey(key)) return;
        BannerData[key] = new BannerClass(id: key, load: false, type: type);
        BannerView ad = new BannerView(adUnitID, adSize, pos);
        if (BannerData.TryGetValue(key, out var n)) n.UpdateAd(ad);

        if (BannerData.TryGetValue(key, out var bc))
        {
            bc.OnLoadFailedHandler = (LoadAdError e) =>
            {
                ShowDebugLog(type + " Failed To Load.\nID: " + key+"\nReason: "+e.GetMessage());
                SendAnalytics(AdEventName, ParameterKey.myParameter, ParameterValue.AdRequestFail);
                SafeRemoveBanner(key);
            };

            bc.OnLoadedHandler = () =>
            {
                if (BannerData.TryGetValue(key, out var d)) d.IsLoaded = true;
                ShowDebugLog(type + " Loaded.\nID: " + key);
                SendAnalytics(AdEventName, ParameterKey.myParameter, ParameterValue.AdRequestSuccess);

                switch (type)
                {
                    case BannerType.SmallBannerType: if (!isSmallBannerShow) { ShowDebugLog("No " + type + " Load.\nReason : No ID's Enabled."); return; } break;
                    case BannerType.LargeBannerType: if (!isLargeBannerShow) { ShowDebugLog("No " + type + " Load.\nReason : No ID's Enabled."); return; } break;
                    case BannerType.AdaptiveBannerType: if (!isAdaptiveBannerShow) { ShowDebugLog("No " + type + " Load.\nReason : No ID's Enabled."); return; } break;
                    case BannerType.SmartBannerType: if (!isSmartBannerShow) { ShowDebugLog("No " + type + " Load.\nReason : No ID's Enabled."); return; } break;
                    case BannerType.SmallBannerGamePlayType: if (!isSmallBannerGamePlayShow) { ShowDebugLog("No " + type + " Load.\nReason : No ID's Enabled."); return; } break;
                }

                BannerView toShow = null;
                if (BannerData.TryGetValue(key, out var data))
                {
                    if (data.IsShowAfterLoad && data.GetAd() is BannerView b) toShow = b;
                    data.IsShowAfterLoad = false;
                }
                if (toShow != null)
                {
                    try { GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() => toShow?.Show()); }
                    catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                }
            };

            bc.OnPaidHandler = (AdValue e) =>
            {
                SendAnalytics(AdEventName, ParameterKey.ECPM, e.Value);
#if SingularSDKManager
        MainThreadQueue.Enqueue(() => SingularSDKManager.Instance.SendEvent_AdRevenue(SingularSDKManager.NetworkType.Admob, e.CurrencyCode, e.Value));
#endif
#if AdRevenueAnalytics                
                AdRevenueAnalytics.AdType adType = AdRevenueAnalytics.AdType.SmallBanner;
                switch(type)
                {
                    case BannerType.SmallBannerGamePlayType: adType = AdRevenueAnalytics.AdType.SmallBannerGamePlay; break;
                    case BannerType.LargeBannerType: adType = AdRevenueAnalytics.AdType.LargeBanner; break;
                    case BannerType.AdaptiveBannerType: adType = AdRevenueAnalytics.AdType.AdaptiveBanner; break;
                    case BannerType.SmartBannerType: adType = AdRevenueAnalytics.AdType.SmartBanner; break;
                    case BannerType.CustomBannerType: adType = AdRevenueAnalytics.AdType.CustomBanner; break;
                }
                SendRevenueAnalytics(adType, e, ad?.GetAdUnitID(), ad?.GetResponseInfo()?.GetMediationAdapterClassName(), ad?.GetType().Name);
#endif
                AdImpressionCustomEvent(e.Value);
            };

            bc.OnFullScreenOpenedHandler = () => { IsAppOpenCanShow = false; };
            bc.OnClickedHandler = () => { IsAppOpenCanShow = false; };

            // finally subscribe
            ad.OnBannerAdLoadFailed += bc.OnLoadFailedHandler;
            ad.OnBannerAdLoaded += bc.OnLoadedHandler;
            ad.OnAdPaid += bc.OnPaidHandler;
            ad.OnAdFullScreenContentOpened += bc.OnFullScreenOpenedHandler;
            ad.OnAdClicked += bc.OnClickedHandler;
        }

        BannerData[key].IsShowAfterLoad = isShowAfterLoad;
        ad.LoadAd(new AdRequest());
        try { MobileAdsEventExecutor.ExecuteInUpdate(() => ad?.Hide()); } catch (System.Exception e) { SendExceptionToCrashlytics(e); }

        SendAnalytics(AdEventName, ParameterKey.myParameter, ParameterValue.AdRequestSend);

        ShowDebugLog(type + " Request Send, Pos: " + pos + ".\nID: " + key);
    }
    void LoadBannerGeneral(string key, BannerType type, AdSize adSize, RectTransform rectTransform, EventName AdEventName, bool isShowAfterLoad = false)
    {
        var splittingArray = key.Split(':');
        if (splittingArray.Length <= 1 || !splittingArray[1].StartsWith("ca-app-pub-"))
        {
            Debug.LogError("Invalid Key Found against " + type + ".\nID : " + key); return;
        }
        string adUnitID = splittingArray[1];
        if (BannerData.ContainsKey(key)) return;
        BannerData[key] = new BannerClass(id: key, load: false, type: type);
        Vector2 BannerPosition;
        BannerPosition.x = (int)ConvertPixelsToDp(RectTransformToScreenSpace(rectTransform).position.x);
        BannerPosition.y = (int)ConvertPixelsToDp(RectTransformToScreenSpace(rectTransform).position.y);
        BannerView ad = new BannerView(adUnitID, adSize, Mathf.FloorToInt(BannerPosition.x), Mathf.FloorToInt(BannerPosition.y));
        if (BannerData.TryGetValue(key, out var data)) data.UpdateAd(ad);

        if (BannerData.TryGetValue(key, out var bc))
        {
            bc.OnLoadFailedHandler = (LoadAdError e) =>
            {
                ShowDebugLog(type + " Failed To Load.\nID: " + key + "\nReason: "+e.GetMessage());
                SendAnalytics(AdEventName, ParameterKey.myParameter, ParameterValue.AdRequestFail);
                SafeRemoveBanner(key);
            };

            bc.OnLoadedHandler = () =>
            {
                if (BannerData.TryGetValue(key, out var d)) d.IsLoaded = true;
                ShowDebugLog(type + " Loaded.\nID: " + key);
                SendAnalytics(AdEventName, ParameterKey.myParameter, ParameterValue.AdRequestSuccess);

                switch (type)
                {
                    case BannerType.CustomBannerType: { if (!isCustomBannerShow) { ShowDebugLog("No " + type + " Load.\nReason : No CustomBannerShow"); return; } break; }
                    default: return;
                }

                BannerView toShow = null;
                if (BannerData.TryGetValue(key, out var data))
                {
                    if (data.IsShowAfterLoad && data.GetAd() is BannerView b) toShow = b;
                    data.IsShowAfterLoad = false;
                }
                if (toShow != null)
                {
                    try { GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() => toShow?.Show()); }
                    catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                }
            };

            bc.OnPaidHandler = (AdValue e) =>
            {
                SendAnalytics(AdEventName, ParameterKey.ECPM, e.Value);
#if SingularSDKManager
        MainThreadQueue.Enqueue(() => SingularSDKManager.Instance.SendEvent_AdRevenue(SingularSDKManager.NetworkType.Admob, e.CurrencyCode, e.Value));
#endif
#if AdRevenueAnalytics
                SendRevenueAnalytics(AdRevenueAnalytics.AdType.CustomBanner, e, ad?.GetAdUnitID(), ad?.GetResponseInfo()?.GetMediationAdapterClassName(), ad?.GetType().Name);
#endif
                AdImpressionCustomEvent(e.Value);
            };

            bc.OnFullScreenOpenedHandler = () => { IsAppOpenCanShow = false; };
            bc.OnClickedHandler = () => { IsAppOpenCanShow = false; };

            // finally subscribe
            ad.OnBannerAdLoadFailed += bc.OnLoadFailedHandler;
            ad.OnBannerAdLoaded += bc.OnLoadedHandler;
            ad.OnAdPaid += bc.OnPaidHandler;
            ad.OnAdFullScreenContentOpened += bc.OnFullScreenOpenedHandler;
            ad.OnAdClicked += bc.OnClickedHandler;
        }

        if (BannerData.TryGetValue(key, out var d)) d.IsShowAfterLoad = isShowAfterLoad;
        ad.LoadAd(new AdRequest());
        try { MainThreadQueue.Enqueue(() => ad?.Hide()); } catch (System.Exception e) { SendExceptionToCrashlytics(e); }

        SendAnalytics(AdEventName, ParameterKey.myParameter, ParameterValue.AdRequestSend);
        ShowDebugLog(type + " Request Send, Pos: " + BannerPosition + ", Size: " + rectTransform.rect.size + ".\nID: " + key);
    }

    void PreLoadCustomBanner()
    {
        if (lastCustomBannerRectTransform == null) { ShowDebugLog("CustomBanner Not Loaded.\nReason : lastCustomBannerRectTransform is NULL"); return; }

        Vector2 BannerSize;
        BannerSize.x = (int)ConvertPixelsToDp(RectTransformToScreenSpace(lastCustomBannerRectTransform).size.x);
        BannerSize.y = (int)ConvertPixelsToDp(RectTransformToScreenSpace(lastCustomBannerRectTransform).size.y);
        AdSize refSize = new AdSize(Mathf.FloorToInt(BannerSize.x), Mathf.FloorToInt(BannerSize.y));
        lastCustomBannerAdSize = refSize;
        LoadAd(BannerType.CustomBannerType, refSize, lastCustomBannerRectTransform);
    }

    #endregion

    #region Banners BG

#if !BannerHolder
    Texture2D bannersBGTexture = null;
    Rect bannerBGRectPosition;
    AdSize bannerBGAdSize = AdSize.Banner;
    bool isShowBannerBG;
    void OnGUI()
    {
        if (isShowBannerBG)
        {
            GUI.DrawTexture(bannerBGRectPosition, bannersBGTexture);
        }
    }
#endif
    void ShowBannerBG(BannerView banner, AdPosition pos, AdSize adSize)
    {
        if (!isBannerBGEnable) return;

#if BannerHolder
        if (BannerHolder.Instance != null)
        {
            BannerHolder.Instance.ShowBlackBg(adSize, pos);
            return;
        }
#else
        // generate Banners background once
        if (bannersBGTexture == null)
        {
            try
            {
                bannersBGTexture = new Texture2D(2, 2);
                Color[] blackPixels = new Color[2 * 2];
                for (int i = 0; i < blackPixels.Length; i++)
                    blackPixels[i] = Color.black;
                bannersBGTexture.SetPixels(blackPixels);
                bannersBGTexture.Apply();
            }
            catch (System.Exception e) { SendExceptionToCrashlytics(e); }
        }
        float height = -1;
        float width = -1;
        try
        {
            height = banner.GetHeightInPixels();
            width = banner.GetWidthInPixels();
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
        if(height == -1 || width == -1) { Debug.Log("No BannerBG shown.\nReason : Banner Width/Height NULL"); return; }

        float heightPadding = 15.0f;
        float widthPadding = 30.0f;
        bannerBGAdSize = adSize;
#if UNITY_EDITOR
        if (PlayerSettings.Android.renderOutsideSafeArea)
            Debug.LogError("Please uncheck (PlayerSetting -> Resolution and Presentation -> Render outside safe area) for perfect Banners BG Rendering.");
#endif
        switch (pos)
        {
            case AdPosition.TopLeft:
                {
                    bannerBGRectPosition = new Rect(0, 0, width + widthPadding, height + heightPadding);
                    break;
                }
            case AdPosition.TopRight:
                {
                    bannerBGRectPosition = new Rect((Screen.width - width - widthPadding), 0, (width + widthPadding), (height + heightPadding));
                    break;
                }
            case AdPosition.BottomLeft:
                {
                    bannerBGRectPosition = new Rect(0, Screen.height - height + heightPadding, (width - widthPadding), (height - heightPadding));
                    break;
                }
            case AdPosition.BottomRight:
                {
                    bannerBGRectPosition = new Rect((Screen.width - width - widthPadding), Screen.height - height - heightPadding, (width + widthPadding), (height + heightPadding));
                    break;
                }
            case AdPosition.Top:
                {
                    bannerBGRectPosition = new Rect(((Screen.width / 2) - (width / 2)) - widthPadding / 2, 0, (width + widthPadding), (height + heightPadding));
                    break;
                }
            case AdPosition.Bottom:
                {
                    bannerBGRectPosition = new Rect((Screen.width / 2) - (width / 2) - widthPadding / 2, Screen.height - height - heightPadding, (width + widthPadding), (height + heightPadding));
                    break;
                }
        }
        isShowBannerBG = true;
#endif
    }
    void HideBannerBG(AdSize adSize)
    {
        if (!isBannerBGEnable) return;

#if BannerHolder
        if (BannerHolder.Instance != null)
        {
            BannerHolder.Instance.HideBlackBg();
            return;
        }
#else
        if (adSize.Equals(bannerBGAdSize))
            isShowBannerBG = false;
#endif
    }

    #endregion

    #region Banner Support

    public enum BannerPosition
    {
        TopLeft,
        TopCenter,
        TopRight,
        Centered,
        CenterLeft,
        CenterRight,
        BottomLeft,
        BottomCenter,
        BottomRight
    }

    AdPosition GetAdmobBannerPosition(BannerPosition pos)
    {
        switch (pos)
        {
            case BannerPosition.TopLeft: { return AdPosition.TopLeft; }
            case BannerPosition.TopCenter: { return AdPosition.Top; }
            case BannerPosition.TopRight: { return AdPosition.TopRight; }
            case BannerPosition.Centered: { return AdPosition.Center; }
            case BannerPosition.BottomLeft: { return AdPosition.BottomLeft; }
            case BannerPosition.BottomCenter: { return AdPosition.Bottom; }
            case BannerPosition.BottomRight: { return AdPosition.BottomRight; }
            default: { return AdPosition.Center; }
        }
    }

    BannerSizeState lastBannerSizeState = BannerSizeState.DefaultBannerSize;

    enum BannerSizeState
    {
        DefaultBannerSize,
        CustomBannerSize
    }

    public static int pixelDPI = 160;
    static float ConvertPixelsToDp(float px)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            return px / ((float)Screen.dpi / pixelDPI);
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Screen.dpi == 0) return Mathf.RoundToInt(px / 2f);
            return Mathf.RoundToInt(px * (pixelDPI / Screen.dpi));
        }
        return px;
    }

    static Rect RectTransformToScreenSpace(RectTransform rTransform)
    {
        Vector2 size = Vector2.Scale(rTransform.rect.size, rTransform.lossyScale);
        Rect rect = new Rect(rTransform.position.x, Screen.height - rTransform.position.y, size.x, size.y);
        rect.x -= (rTransform.pivot.x * size.x);
        rect.y -= ((1.0f - rTransform.pivot.y) * size.y);
        return rect;
    }

    static GameObject lastRectTransform = null;
    void UpdateLastRect(RectTransform rect)
    {
        if (lastRectTransform == null)
        {
            lastRectTransform = new GameObject("LastCustomObjRef");
            lastRectTransform.SetActive(false);
        }
        lastRectTransform.transform.SetParent(rect.transform.parent);
        if (lastRectTransform.GetComponent<RectTransform>() == null)
            lastRectTransform.AddComponent<RectTransform>();
        RectTransform newObj = lastRectTransform.GetComponent<RectTransform>();
        newObj.sizeDelta = rect.sizeDelta;
        newObj.anchoredPosition = rect.anchoredPosition;
        newObj.anchorMax = rect.anchorMax;
        newObj.anchorMin = rect.anchorMin;
        newObj.offsetMin = rect.offsetMin;
        newObj.offsetMax = rect.offsetMax;
        newObj.localScale = rect.localScale;
        newObj.localEulerAngles = rect.localEulerAngles;
    }

    float GetHeightInPixels(RectTransform rectTransform)
    {
        return rectTransform.rect.height * rectTransform.GetComponentInParent<Canvas>().scaleFactor;
    }

    void SafeRemoveBanner(string key)
    {
        if (BannerData.TryRemove(key, out var toRemove) && toRemove != null)
        {
            MainThreadQueue.Enqueue(() =>
            {
                ActionsPerformWithDelay(() =>
                {
                    try { toRemove.Remove(); } catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                }, 0.05f);
            });
        }
    }

    void SafeDisposeAllBanners()
    {
        foreach (var kv in BannerData)
        {
            try { kv.Value?.Remove(); } catch (System.Exception e) { SendExceptionToCrashlytics(e); }
        }
        BannerData.Clear();
    }


    #region BannerAutoReload
    public void BannerAutoReload_Start(BannerType bannerType)
    {
        if (lastBannerAutoReload.Equals(bannerType)) return;

        if (AutoCheckBannerToLoadTemp != null) StopCoroutine(AutoCheckBannerToLoadTemp);
        AutoCheckBannerToLoadTemp = AutoCheckBannerToLoad(bannerType);
        StartCoroutine(AutoCheckBannerToLoadTemp);
        lastBannerAutoReload = bannerType;
    }
    public void BannerAudoReload_Stop()
    {
        if (AutoCheckBannerToLoadTemp != null) StopCoroutine(AutoCheckBannerToLoadTemp);
    }

    BannerType lastBannerAutoReload;
    static readonly float timeDuration = 20f;
    readonly WaitForSeconds bannerDelayInSec = new WaitForSeconds(timeDuration);
    System.Collections.IEnumerator AutoCheckBannerToLoadTemp = null;
    System.Collections.IEnumerator AutoCheckBannerToLoad(BannerType bannerType)
    {
        while (true)
        {
            yield return bannerDelayInSec;
            switch (bannerType)
            {
                case BannerType.SmallBannerType:
                    {
                        if (!IsBannerLoaded(bannerType)) LoadAd(BannerType.SmallBannerType, AdSize.Banner, lastSmallBannerPosition);
                        break;
                    }
                case BannerType.SmallBannerGamePlayType:
                    {
                        if (!IsBannerLoaded(bannerType)) LoadAd(BannerType.SmallBannerGamePlayType, AdSize.Banner, lastSmallBannerGamePlayPosition);
                        break;
                    }
                case BannerType.LargeBannerType:
                    {
                        if (!IsBannerLoaded(bannerType)) LoadAd(BannerType.LargeBannerType, AdSize.MediumRectangle, lastLargeBannerPosition);
                        break;
                    }
                case BannerType.SmartBannerType:
                    {
                        if (!IsBannerLoaded(bannerType)) LoadAd(BannerType.SmartBannerType, AdSize.SmartBanner, lastSmartBannerPosition);
                        break;
                    }
                case BannerType.AdaptiveBannerType:
                    {
                        if (!IsBannerLoaded(bannerType)) LoadAd(BannerType.AdaptiveBannerType, AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth), lastAdaptiveBannerPosition);
                        break;
                    }
                case BannerType.CustomBannerType:
                    {
                        if (!IsBannerLoaded(bannerType)) PreLoadCustomBanner();
                        break;
                    }
            }
        }
    }
    #endregion

    #endregion

    #endregion

    #region Native Overlay

    public void LoadNative(RectTransform nativeRect, string specificID = "", bool isFullScreenNative = false)
    {
        LoadNativeInternal(nativeRect, isShowAfterLoad: false, specificID: specificID, isFullScreenNative: isFullScreenNative);
    }

    public void ShowNative(RectTransform nativeRect, NativeAdOptions nativeAdOption = null, NativeTemplateStyle nativeTempStyle = null,
        AdPosition nativeAdPosition = AdPosition.Center, bool isFullScreenNative = false, string specificID = "")
    {
        if (!isFullScreenNative)
        {
            if (nativeAdOption != null) lastNativeAdOptions = nativeAdOption;
            if (nativeTempStyle != null) lastNativeTempStyle = nativeTempStyle;
            lastNativeType = NativeCustomType.SetAsThisRectObject;
            lastNativeAdPosition = nativeAdPosition;
        }
#if !UNITY_EDITOR
        if(!string.IsNullOrEmpty(specificID))
        {
            if (IsNativeLoaded(specificID:specificID, isFullScreenNative: isFullScreenNative))
            {
                ShowNativeInternal(nativeRect, nativeAdOption: nativeAdOption, nativeTempStyle: nativeTempStyle,
               nativeAdPosition: nativeAdPosition, isFullScreenNative: isFullScreenNative, specificID: specificID);
                return;
            }
            else LoadNativeInternal(nativeRect, isShowAfterLoad: !isFullScreenNative, specificID:specificID, isFullScreenNative:isFullScreenNative);
        }
        else if (IsNativeLoaded(specificID: specificID, isFullScreenNative: isFullScreenNative)) 
        {
#endif
        ShowNativeInternal(nativeRect, nativeAdOption: nativeAdOption, nativeTempStyle: nativeTempStyle,
            nativeAdPosition: nativeAdPosition, specificID: specificID, isFullScreenNative: isFullScreenNative);
        return;
#if !UNITY_EDITOR
        }
        else 
#endif
        LoadNativeInternal(nativeRect, isShowAfterLoad: !isFullScreenNative, specificID: specificID);
    }

    public void ShowNative(NativeAdPosition adPosition, NativePreDefineSize adWidth, NativePreDefineSize adHight, NativeAdOptions nativeAdOption = null,
        NativeTemplateStyle nativeTempStyle = null, bool isFullScreenNative = false, string specificID = "")
    {
        if (!isFullScreenNative)
        {
            if (nativeAdOption != null) lastNativeAdOptions = nativeAdOption;
            if (nativeTempStyle != null) lastNativeTempStyle = nativeTempStyle;
            lastNativeAdPositionPreDefine = adPosition;
            lastNativeAdWidth = adWidth;
            lastNativeAdHight = adHight;
            lastNativeType = NativeCustomType.PreDefined;
        }

#if !UNITY_EDITOR
        if (!string.IsNullOrEmpty(specificID))
        {
            if (IsNativeLoaded(specificID: specificID,isFullScreenNative:isFullScreenNative))
            {
                ShowNativeInternal(adPosition: adPosition, adWidth: adWidth, adHight: adHight, nativeAdOption: nativeAdOption,
                    nativeTempStyle: nativeTempStyle, isFullScreenNative: isFullScreenNative, specificID: specificID);
                return;
            }
            else LoadNativeInternal(specificID: specificID, isFullScreenNative: isFullScreenNative);
        }
        else if (IsNativeLoaded(specificID:specificID, isFullScreenNative:isFullScreenNative))
        {
#endif
        ShowNativeInternal(adPosition: adPosition, adWidth: adWidth, adHight: adHight, nativeAdOption: nativeAdOption,
                nativeTempStyle: nativeTempStyle, specificID: specificID, isFullScreenNative: isFullScreenNative);
        return;
#if !UNITY_EDITOR
        }
#endif
        LoadNativeInternal(specificID: specificID);
    }

    public void ShowNativeFullScreen(float afterAdTimeScale = 1, System.Action<bool> actionAfterAd = null, string specificID = "")
    {
        string adType = "NativeFullScreen";
        if (IsRemoveAd()) { Time.timeScale = afterAdTimeScale; actionAfterAd?.Invoke(false); ShowDebugLog("No " + adType + " Shown.\nReason : Remove Ads."); return; }
        if (!IsInternetConnection()) { Time.timeScale = afterAdTimeScale; actionAfterAd?.Invoke(false); ShowDebugLog("No " + adType + " Shown.\nReason : No Internet."); return; }
        if (IsLowMemory(NoNativeBelow)) { Time.timeScale = afterAdTimeScale; actionAfterAd?.Invoke(false); ShowDebugLog("No " + adType + " Load.\nReason : Low Available Memory"); return; }
#if RemoteConfigManager_Firebase
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isNativeOverlayFullScreenLoad))
        {
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isNativeOverlayFullScreenLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isNativeOverlayFullScreenFromRemote);
            if (!isNativeOverlayFullScreenFromRemote)
            {
                ShowDebugLog("No " + adType + " Load.\nReason : RemoteConfig Not Allowed.\nKey: " + isNativeOverlayFullScreenLoad + ", value :" + isNativeOverlayFullScreenFromRemote);
                actionAfterAd?.Invoke(false);
                return;
            }
        }
#endif
        NativeFullScreenAdCustom nativeFullScreenScript = NativeFullScreen.GetComponent<NativeFullScreenAdCustom>();
        if (nativeFullScreenScript != null)
        {
            NativeFullScreenAdCustom.NativeFullScreenBackFillClass nativePriority = nativeFullScreenScript.nativeFullScreenBackFill;
            if (nativePriority.BackFillPriority.Count > 0)
            {
                foreach (NativeFullScreenAdCustom.NativeFullScreenBackFillEnum priority in nativePriority.BackFillPriority)
                {
                    switch (priority)
                    {
                        case NativeFullScreenAdCustom.NativeFullScreenBackFillEnum.NativeFullScreen:
                            {
                                if (IsNativeLoaded(loadIfNotLoaded: true, specificID: specificID, isFullScreenNative: true, isFullScreenPriorityCheck:false))
                                {
                                    if (!string.IsNullOrEmpty(specificID)) { NativeFullScreenAdContentScript.specificID = specificID; }
                                    goto ShowNativeObject;
                                }
                                else LoadNativeInternal(isFullScreenNative: true);
                                break;
                            }
                        case NativeFullScreenAdCustom.NativeFullScreenBackFillEnum.LargeBanner:
                            {
                                if (IsBannerLoaded(BannerType.LargeBannerType)) goto ShowNativeObject;
                                break;
                            }
                        case NativeFullScreenAdCustom.NativeFullScreenBackFillEnum.CustomBanner:
                            {
                                if (IsBannerLoaded(BannerType.CustomBannerType)) goto ShowNativeObject;
                                break;
                            }
                    }
                }
            }
        }
        if (IsNativeLoaded(loadIfNotLoaded: true, specificID: specificID, isFullScreenNative: true))
        {
            if (!string.IsNullOrEmpty(specificID)) { NativeFullScreenAdContentScript.specificID = specificID; }
            goto ShowNativeObject;
        }
        else { LoadNativeInternal(isFullScreenNative: true); ShowDebugLog("No " + adType + " Shown.\nReason : Not Available."); return; }

        ShowNativeObject:
        IsAdmobShowingFullScreenAd = true;
        IsAppOpenCanShow = false;
        ShowLoadingAdBG();
        nativeFullScreenActionAfterAd = actionAfterAd;
        ActionsPerformWithDelay(new System.Action(() =>
        {
            NativeFullScreen.SetActive(true);
            lastAudioListenerState_NativeFullScreen = AudioListener.pause;
            AudioListener.pause = true;
        }), loadingAdTimeDelay);
    }

    public void HideNativeFullScreen()
    {
        NativeFullScreen.SetActive(false);
        IsAdmobShowingFullScreenAd = false;
        IsAppOpenCanShow = true;
        HideLoadingAdBG();
        AfterLoadingBGBannerToShow();
        nativeFullScreenActionAfterAd?.Invoke(true);
        nativeFullScreenActionAfterAd = null;
        AudioListener.pause = lastAudioListenerState_NativeFullScreen;
    }

    public bool IsNativeLoaded(bool loadIfNotLoaded = true, bool isShowAfterLoad = false, string specificID = "", bool isFullScreenNative = false, bool isFullScreenPriorityCheck = true)
    {
        if (!IsInternetConnection()) return false;
        if (IsRemoveAd()) return false;

#if UNITY_EDITOR
        if (string.IsNullOrEmpty(specificID)) return true;
#endif
        if (string.IsNullOrEmpty(specificID))
        {
            if (isFullScreenNative && isFullScreenPriorityCheck)
            {
                NativeFullScreenAdCustom nativeFullScreenScript = NativeFullScreen.GetComponent<NativeFullScreenAdCustom>();
                if (nativeFullScreenScript != null)
                {
                    NativeFullScreenAdCustom.NativeFullScreenBackFillClass nativePriority = nativeFullScreenScript.nativeFullScreenBackFill;
                    if (nativePriority.BackFillPriority.Count > 0)
                    {
                        foreach (NativeFullScreenAdCustom.NativeFullScreenBackFillEnum priority in nativePriority.BackFillPriority)
                        {
                            switch (priority)
                            {
                                case NativeFullScreenAdCustom.NativeFullScreenBackFillEnum.NativeFullScreen:
                                    {
                                        System.Collections.Generic.List<string> NativeIdsInternal = new System.Collections.Generic.List<string>();
                                        NativeIdsInternal.AddRange(isFullScreenNative ? nativeFullScreenID : nativeID);
                                        foreach (var data in NativeData.Values)
                                        {
                                            for (int i = 0; i < NativeIdsInternal.Count; i++) { if (data.ID.Contains(NativeIdsInternal[i]) && data.IsLoaded) return true; }
                                        }
                                        break;
                                    }
                                case NativeFullScreenAdCustom.NativeFullScreenBackFillEnum.LargeBanner:
                                    {
                                        if (IsBannerLoaded(BannerType.LargeBannerType)) return true;
                                        break;
                                    }
                                case NativeFullScreenAdCustom.NativeFullScreenBackFillEnum.CustomBanner:
                                    {
                                        if (IsBannerLoaded(BannerType.CustomBannerType)) return true;
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
            System.Collections.Generic.List<string> NativeIds = new System.Collections.Generic.List<string>();
            NativeIds.AddRange(isFullScreenNative ? nativeFullScreenID : nativeID);
            foreach (var data in NativeData.Values) { for (int i = 0; i < NativeIds.Count; i++) { if (data.ID.Contains(NativeIds[i]) && data.IsLoaded) return true; } }
        }
        else { foreach (var data in NativeData.Values) if (data.IsLoaded && data.ID.Contains(specificID)) return true; }

        if (loadIfNotLoaded) LoadNativeInternal(isShowAfterLoad: isShowAfterLoad, specificID: specificID, isFullScreenNative: isFullScreenNative);
        return false;
    }

    public void HideNative(bool isFromFocus = false, bool isFullScreenNative = false)
    {
        if (NativeData.Count > 0)
        {
            var keys = new System.Collections.Generic.List<string>(NativeData.Keys);
            foreach (var key in keys)
            {
                var data = NativeData[key];
                if (data.IsLoaded)
                {
                    if (data.GetAd() is NativeOverlayAd nativeAd && data.IsShowing)
                    {
                        data.IsShowing = false;
                        try { MobileAdsEventExecutor.ExecuteInUpdate(() => nativeAd?.Hide()); } catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                        ShowDebugLog("Hide NativeOverlay\nID :" + data.ID + "\nFrom Plugin :" + isFromFocus + "\nFullScreen :" + isFullScreenNative);
                        if (!isFromFocus)
                        {
                            bool isRespectiveNativeCTR = isFullScreenNative ? isNativeFullScreenReloadOnCTR_FromRemote : isNativeReloadOnCTR_FromRemote;
                            bool isRespectiveNativeReloadOnImpression= isFullScreenNative ? isNativeFullScreenReloadOnNumberOfImpression_FromRemote : isNativeReloadOnNumberOfImpression_FromRemote;
                            long respectiveNativeReloadNoOfImpression= isFullScreenNative ? nativeFullScreenReloadOnNumberOfImpression_FromRemote : nativeReloadOnNumberOfImpression_FromRemote;
                            if ((isRespectiveNativeCTR && data.IsCTR) ||
                                (isRespectiveNativeReloadOnImpression && data.NumberOfImpression >= respectiveNativeReloadNoOfImpression))
                            {
                                ActionsPerformWithDelay(new System.Action(() => { DestroyNativeOverlay(key: data.ID, isReload: true, nativeAd: data); }), 0.4f);
                            }
                        }
                    }
                }
            }
        }
        if (!isFromFocus && !isFullScreenNative) IsNativeShow = false;
    }

    #region Native Internal

    public void LoadNativeInternal(RectTransform nativeRect = null, bool isShowAfterLoad = false, string specificID = "", bool isFullScreenNative = false)
    {
        string adType = isFullScreenNative ? "NativeFullScreen" : "NativeOverlay";
        if (IsRemoveAd()) { ShowDebugLog("No " + adType + " Load.\nReason : Remove Ads."); return; }
        if (!IsInternetConnection()) { ShowDebugLog("No " + adType + " Load.\nReason : No Internet."); return; }
        if (IsLowMemory(NoNativeBelow)) { ShowDebugLog("No " + adType + " Load.\nReason : Low Available Memory"); return; }
        if (!isNativeIdAvailable && !isNativeFullScreenIdAvailable && !isTestMode) { ShowDebugLog("No " + adType + " Load.\nReason : No NativeOverlay / FullScreenNative ID Available."); return; }
        if (isFullScreenNative ? (!isNativeFullScreenIdAvailable && !isTestMode) : (!isNativeIdAvailable && !isTestMode)) { ShowDebugLog("No " + adType + " Load.\nReason : No " + adType + " ID Available."); return; }
        if (!CheckInitialization(false, new System.Action(() =>
        {
            LoadNativeInternal(nativeRect: nativeRect, isShowAfterLoad: isShowAfterLoad,
            specificID: specificID, isFullScreenNative: isFullScreenNative);
        }))) { ShowDebugLog("No " + adType + " Load.\nReason : No SDK Initilized."); return; }
#if RemoteConfigManager_Firebase
        string nativeIDCheckToLoad = isFullScreenNative ? isNativeOverlayFullScreenLoad : isNativeOverlayLoad;
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(nativeIDCheckToLoad))
        {
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(nativeIDCheckToLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isNativeOverlayFromRemote);
            if (!isNativeOverlayFromRemote) { ShowDebugLog("No " + adType + " Load.\nReason : RemoteConfig Not Allowed.\nKey: " + nativeIDCheckToLoad + ", value :" + isNativeOverlayFromRemote); return; }
        }
#endif
        if (string.IsNullOrEmpty(specificID))
        {
            if (isNativeIdAvailable && !isFullScreenNative)
            {
                for (int i = 0; i < nativeID.Count; i++)
                {
                    string currentID = currentID = nativeID[i];
                    if (string.IsNullOrEmpty(currentID) || (!IsValidID(ref currentID, NativeAdName.NativeOverlay.ToString()))) continue;
                    currentID = NativeAdName.NativeOverlay.ToString() + "_" + i + "_:" + currentID;
                    if (!NativeData.ContainsKey(currentID)) { LoadNativeGeneral(key: currentID, nativeRect: nativeRect, isShowAfterLoad: isShowAfterLoad, isFullScreenNative: false); isShowAfterLoad = false; }
                }
            }

            if (isNativeFullScreenIdAvailable && isFullScreenNative)
            {
                for (int i = 0; i < nativeFullScreenID.Count; i++)
                {
                    string currentID = currentID = nativeFullScreenID[i];
                    if (string.IsNullOrEmpty(currentID) || (!IsValidID(ref currentID, NativeAdName.NativeOverlayFullScreen.ToString()))) continue;
                    currentID = NativeAdName.NativeOverlayFullScreen.ToString() + "_" + i + "_:" + currentID;
                    if (!NativeData.ContainsKey(currentID))
                    {
                        LoadNativeGeneral(key: currentID, nativeRect: nativeRect, isShowAfterLoad: isShowAfterLoad, isFullScreenNative: true);
                    }
                }
            }
        }
        else
        {
            if (!IsValidID(ref specificID, isFullScreenNative ? NativeAdName.NativeOverlayFullScreen.ToString() : NativeAdName.NativeOverlay.ToString())) { ShowDebugLog("No " + adType + " Load.\nReason: Invalid Ad ID.\nID: " + specificID); return; }
            foreach (var kvp in NativeData) if (kvp.Key.Contains(specificID)) return;
            string currentID = isFullScreenNative ? NativeAdName.NativeOverlayFullScreen.ToString() : NativeAdName.NativeOverlay.ToString() + "_" + (NativeData.Count + 1) + "_:" + specificID;
            LoadNativeGeneral(key: currentID, nativeRect: nativeRect, isShowAfterLoad: isShowAfterLoad); isShowAfterLoad = false;
        }
    }
    void ShowNativeInternal(RectTransform nativeRect, NativeAdOptions nativeAdOption = null, NativeTemplateStyle nativeTempStyle = null,
        AdPosition nativeAdPosition = AdPosition.Center, bool isFullScreenNative = false, string specificID = "")
    {
        string nativeAdType = isFullScreenNative ? NativeAdName.NativeOverlayFullScreen.ToString() : NativeAdName.NativeOverlay.ToString();

        if (IsRemoveAd()) { ShowDebugLog("No " + nativeAdType + " Shown.\nReason : Remove Ads."); return; }
        if (!IsInternetConnection()) { ShowDebugLog("No " + nativeAdType + " Shown.\nReason : No Internet."); return; }
        if (!CheckInitialization(false, new System.Action(() =>
        {
            ShowNativeInternal(nativeRect: nativeRect, nativeAdOption: nativeAdOption, nativeTempStyle: nativeTempStyle,
            nativeAdPosition: nativeAdPosition, isFullScreenNative: isFullScreenNative, specificID: specificID);
        }))) { ShowDebugLog("No " + nativeAdType + " Shown.\nReason : No SDK Initilized."); return; }
        if (IsLowMemory(NoSmallBannerBelow)) { ShowDebugLog("No " + nativeAdType + " Shown.\nReason : Low Available Memory"); return; }
        if (!isFullScreenNative && LoadingAdBG.activeSelf) { ShowDebugLog("No " + nativeAdType + " Shown.\nReason : LoadingAdBG active Self"); return; }
        if (nativeRect == null) { ShowDebugLog("No " + nativeAdType + " Shown.\nReason : nativeRect is NULL"); return; }
        if (!isNativeIdAvailable && !isNativeFullScreenIdAvailable && !isTestMode) { ShowDebugLog("No " + nativeAdType + " Shown.\nReason : Not Native Nor NativeFullScreen ID Available"); return; }
#if !UNITY_EDITOR
        if (!string.IsNullOrEmpty(specificID) && !IsNativeLoaded(specificID: specificID)) { ShowDebugLog("No " + nativeAdType + " Available.\nID: "+specificID + "\nReason : Not Loaded."); return; }
        if (!IsNativeLoaded(loadIfNotLoaded:true, specificID: specificID, isFullScreenNative: isFullScreenNative)) { ShowDebugLog("No " + nativeAdType + " Available.\nID: " + specificID+"\nReason : Not Loaded."); return; }
#endif
#if UTM_Class
        if (UTM.UTM_Handler.IsUTMShowingFullScreenAd) { ShowDebugLog("No " + nativeAdType + " Shown.\nReason: UTM Full Screen is Enabled."); return; }
#endif
        Vector2 bannerPosition;
        bannerPosition.x = (int)ConvertPixelsToDpNative(RectTransformToScreenSpace(nativeRect).position.x, isFullScreenNative: isFullScreenNative);
        bannerPosition.y = (int)ConvertPixelsToDpNative(RectTransformToScreenSpace(nativeRect).position.y, isFullScreenNative: isFullScreenNative);
        Vector2 bannerSize;
        bannerSize.x = (int)ConvertPixelsToDpNative(RectTransformToScreenSpace(nativeRect).size.x, isFullScreenNative: isFullScreenNative);
        bannerSize.y = (int)ConvertPixelsToDpNative(RectTransformToScreenSpace(nativeRect).size.y, isFullScreenNative: isFullScreenNative);

        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            UnityEngine.UI.CanvasScaler canvasScaler = GetComponentInParent<UnityEngine.UI.CanvasScaler>();
            Vector2 tempResolution = canvasScaler.referenceResolution;
            canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
            float tempPixelPerUnit = canvasScaler.referencePixelsPerUnit;
            canvasScaler.referencePixelsPerUnit = Screen.dpi;

            float maxScreenWidthDp = ConvertPixelsToDpNative(Screen.width);
            float maxScreenHeightDp = ConvertPixelsToDpNative(Screen.height);

            float requiredScreenWidthDp = ConvertPixelsToDpNative(px: RectTransformToScreenSpace(nativeRect).size.x, isFullScreenNative: isFullScreenNative);
            float requiredScreenHeightDp = ConvertPixelsToDpNative(px: RectTransformToScreenSpace(nativeRect).size.y, isFullScreenNative: isFullScreenNative);

            bannerSize = new Vector2(Mathf.Min(requiredScreenWidthDp, maxScreenWidthDp), Mathf.Min(requiredScreenHeightDp, maxScreenHeightDp));
            canvasScaler.referenceResolution = tempResolution;
            canvasScaler.referencePixelsPerUnit = tempPixelPerUnit;
        }

        AdSize adSize = new AdSize(Mathf.FloorToInt(bannerSize.x * multiplyerSize), Mathf.FloorToInt(bannerSize.y * multiplyerSize));

        if (Application.platform == RuntimePlatform.Android)
        {
            if (nativeAdPosition == AdPosition.Bottom || nativeAdPosition == AdPosition.BottomRight || nativeAdPosition == AdPosition.BottomLeft)
            {
                if ((Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown) && !isFullScreenNative)
                {
                    if (adSize.Height > adSize.Width)
                    {
                        int previousWidth = adSize.Width;
                        float newHeight = adSize.Width - ((adSize.Width / 10) * 0.25f);
                        adSize = new AdSize(previousWidth, (int)newHeight);
                    }
                }
            }
        }

        System.Collections.Generic.List<NativeOverlayClass> nativeAdListTemp = new System.Collections.Generic.List<NativeOverlayClass>();

        // hide previous all this type of native
        foreach (var currentNative in NativeData)
        {
            if (currentNative.Value.IsLoaded)
            {
                if (currentNative.Value.GetAd() is NativeOverlayAd nativeInternalAd1)
                {
                    try
                    {
                        try { MobileAdsEventExecutor.ExecuteInUpdate(() => nativeInternalAd1?.Hide()); } catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                        currentNative.Value.IsShowing = false;
                    }
                    catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                }
            }
        }
        // add all required native to temp list to show
        System.Collections.Generic.List<string> NativeIds = new System.Collections.Generic.List<string>();
        NativeIds.AddRange(isFullScreenNative ? isNativeFullScreenIdAvailable ? nativeFullScreenID.Count > 0 ? nativeFullScreenID : nativeID : nativeID : nativeID);
        foreach (var currentNative in NativeData)
        {
            for (int j = 0; j < NativeIds.Count; j++)
            {
                if (currentNative.Value.ID.Contains(NativeIds[j]) && currentNative.Value.IsLoaded)
                {
                    NativeOverlayClass overlay = currentNative.Value;
                    if (overlay.GetAd() is NativeOverlayAd nativeInternalAd2)
                    {
                        try
                        {
                            MobileAdsEventExecutor.ExecuteInUpdate(() => nativeInternalAd2?.Hide());
                            overlay.IsShowing = false;
                        }
                        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                        nativeAdListTemp.Add(overlay);
                    }
                }
            }
        }

        if (nativeAdListTemp.Count <= 0) { ShowDebugLog("No " + nativeAdType + " Available.\nReason : Native Not Available.\nAvailable Native Count : " + nativeAdListTemp.Count); return; }
#if AdRevenueAnalytics

        System.Collections.Generic.List<string> genericList = AdRevenueAnalytics.Instance.GetAdIDsList(isFullScreenNative ? AdRevenueAnalytics.AdType.NativeFullScreen : AdRevenueAnalytics.AdType.Native);
        genericList ??= isFullScreenNative ? nativeFullScreenID : nativeID;
        if (genericList == null) { ShowDebugLog("No " + nativeAdType + " Available.\nReason : Native GenericList NULL."); return; }

        int maxLoop = genericList.Count;
        int nativeCount = isFullScreenNative ? nativeFullScreenCurrentIndex : nativeCurrentIndex;

        NativeLoop:

        nativeCount++;
        maxLoop--;
        if (maxLoop < 0) { ShowDebugLog("No " + nativeAdType + " Ads Available\nReason: AdPilot Provided ID's Not Loaded."); return; }
        
        if (nativeCount >= genericList.Count) nativeCount = 0;
        string genericID = genericList[nativeCount];

        if (isFullScreenNative) nativeFullScreenCurrentIndex = nativeCount;
        else nativeCurrentIndex = nativeCount;
        foreach (var kvp in NativeData)
        {
            if (kvp.Key.Contains(genericID) && kvp.Value.IsLoaded)
            {
                if (!string.IsNullOrEmpty(specificID) && !kvp.Key.Contains(specificID)) { ShowDebugLog("No " + nativeAdType + " Available.\nID: " + kvp.Key); continue; }

                if (kvp.Value.GetAd() is NativeOverlayAd nativeInternalAd3 && nativeInternalAd3 != null)
                {
                    if (isFullScreenNative)
                    {
                        float maxScreenAvailableWidth = Screen.width - bannerPosition.x;
                        if (adSize.Width > maxScreenAvailableWidth) adSize = new AdSize((int)(maxScreenAvailableWidth - bannerPosition.x), adSize.Height);
                        try { MobileAdsEventExecutor.ExecuteInUpdate(() => kvp.Value.ShowAd(nativeTempStyle ?? lastNativeTempStyle, adSize, bannerPosition)); } catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                        AdRevenueAnalytics.Instance.SendRevenueAnalytics(AdRevenueAnalytics.AdType.NativeFullScreen, AdRevenueAnalytics.AdRevenueEventType.Ad_Rev_Progression, status: AdRevenueAnalytics.Status.Showing);

                    }
                    else
                    {
                        try
                        {
                            if (lastNativeAdPosition.Equals(AdPosition.Center)) MobileAdsEventExecutor.ExecuteInUpdate(() => kvp.Value.ShowAd(nativeTempStyle ?? lastNativeTempStyle, adSize, bannerPosition));
                            else MobileAdsEventExecutor.ExecuteInUpdate(() => kvp.Value.ShowAd(nativeTempStyle ?? lastNativeTempStyle, adSize, lastNativeAdPosition));
                            AdRevenueAnalytics.Instance.SendRevenueAnalytics(AdRevenueAnalytics.AdType.Native, AdRevenueAnalytics.AdRevenueEventType.Ad_Rev_Progression, status: AdRevenueAnalytics.Status.Showing);

                        }
                        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                    }

                    IsNativeShow = true;
                    ShowDebugLog("Show " + nativeAdType + ".\nID: " + kvp.Value.ID);
                    kvp.Value.IsShowing = true;
                    SendAnalytics("Ad_" + nativeAdType + "_Show");
                    if (!isFullScreenNative) lastNativeOverlayRectTransform = nativeRect;
                    return;
                }
            }
        }
        goto NativeLoop;
#else
        if (!string.IsNullOrEmpty(specificID))
        {
            bool isSpecificIDAvailable = false;
            for (int i = 0; i < nativeAdListTemp.Count; i++)
            {
                if (nativeAdListTemp[i].ID.Contains(specificID))
                {
                    if (isFullScreenNative) nativeFullScreenCurrentIndex = i - 1;
                    else nativeCurrentIndex = i - 1;
                    isSpecificIDAvailable = true;
                    break;
                }
            }
            if (!isSpecificIDAvailable) { ShowDebugLog("No " + nativeAdType + " Available.\nID: " + specificID+"\nReason : Required ID not Available."); return; }
        }

        int nativeCounter = -1;
        if (isFullScreenNative)
        {
            nativeFullScreenCurrentIndex++;
            nativeCounter = nativeFullScreenCurrentIndex;
        }
        else
        {
            nativeCurrentIndex++;
            nativeCounter = nativeCurrentIndex;
        }
        if (nativeCounter >= nativeAdListTemp.Count)
        {
            nativeCounter = 0;
            if (isFullScreenNative) nativeFullScreenCurrentIndex = nativeCounter;
            else nativeCurrentIndex = nativeCounter;
        }

#if !UNITY_EDITOR
        if (nativeAdListTemp[nativeCounter].GetAd() is NativeOverlayAd nativeInternalAd4 && nativeInternalAd4 != null && nativeAdListTemp[nativeCounter].IsLoaded)
#endif
        {
            if (isFullScreenNative) try { MobileAdsEventExecutor.ExecuteInUpdate(() => nativeAdListTemp[nativeCounter].ShowAd(nativeTempStyle ?? lastNativeTempStyle, adSize, bannerPosition)); } catch (System.Exception e) { SendExceptionToCrashlytics(e); }
            else
            {
                if (lastNativeAdPosition.Equals(AdPosition.Center)) try { MobileAdsEventExecutor.ExecuteInUpdate(() => nativeAdListTemp[nativeCounter].ShowAd(nativeTempStyle ?? lastNativeTempStyle, adSize, bannerPosition)); } catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                else try { MobileAdsEventExecutor.ExecuteInUpdate(() => nativeAdListTemp[nativeCounter].ShowAd(nativeTempStyle ?? lastNativeTempStyle, adSize, lastNativeAdPosition)); } catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                IsNativeShow = true;
            }
            ShowDebugLog("Show " + nativeAdType + ".\nID: " + nativeAdListTemp[nativeCounter].ID);
            nativeAdListTemp[nativeCounter].IsShowing = true;
            SendAnalytics("Ad_"+ nativeAdType.ToString() + "_Show");
            if (isFullScreenNative) nativeFullScreenCurrentIndex = nativeCounter;            
            else
            {
                lastNativeOverlayRectTransform = nativeRect;
                nativeCurrentIndex = nativeCounter;
            }
            return;
        }
#endif

        ShowDebugLog("No " + nativeAdType + " Available.");
    }

    void ShowNativeInternal(NativeAdPosition adPosition, NativePreDefineSize adWidth, NativePreDefineSize adHight, NativeAdOptions nativeAdOption = null,
        NativeTemplateStyle nativeTempStyle = null, bool isFullScreenNative = false, string specificID = "")
    {
        string nativeAdType = isFullScreenNative ? NativeAdName.NativeOverlayFullScreen.ToString() : NativeAdName.NativeOverlay.ToString();
        if (IsRemoveAd()) { ShowDebugLog("No " + nativeAdType + " Shown.\nReason : Remove Ads."); return; }
        if (!IsInternetConnection()) { ShowDebugLog("No " + nativeAdType + " Shown.\nReason : No Internet."); return; }
        if (!CheckInitialization(false, new System.Action(() =>
        {
            ShowNativeInternal(adPosition: adPosition, adWidth: adWidth, adHight: adHight,
            nativeAdOption: nativeAdOption, nativeTempStyle: nativeTempStyle, isFullScreenNative: isFullScreenNative, specificID: specificID);
        }))) { ShowDebugLog("No " + nativeAdType + " Shown.\nReason : No SDK Initilized."); return; }
        if (IsLowMemory(NoSmallBannerBelow)) { ShowDebugLog("No " + nativeAdType + " Shown.\nReason : Low Available Memory"); return; }
        if (!isNativeIdAvailable && !isNativeFullScreenIdAvailable && !isTestMode) { ShowDebugLog("No " + nativeAdType + " Shown.\nReason : Not Native Nor NativeFullScreen ID Available"); return; }
#if !UNITY_EDITOR
        if (!string.IsNullOrEmpty(specificID) && !IsNativeLoaded(specificID:specificID)){ ShowDebugLog("No " + nativeAdType + " Available.\nID: " + specificID + "\nReason : Not Loaded."); return; }
        else if (!IsNativeLoaded(isFullScreenNative: isFullScreenNative, specificID: specificID)) { ShowDebugLog("No " + nativeAdType + " Available.\nID: " + specificID + "\nReason : Not Loaded."); return; }
#endif
#if UTM_Class
        if (UTM.UTM_Handler.IsUTMShowingFullScreenAd) { ShowDebugLog("No " + nativeAdType + " Shown.\nReason: UTM Full Screen is Enabled."); return; }
#endif
        Vector2 bannerSize = new Vector2(((Screen.width / 10) * (float)adWidth), (Screen.height / 10) * (float)adHight);
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            UnityEngine.UI.CanvasScaler canvasScaler = GetComponentInParent<UnityEngine.UI.CanvasScaler>();
            Vector2 tempResolution = canvasScaler.referenceResolution;
            canvasScaler.referenceResolution = new Vector2(Screen.width, Screen.height);
            float tempPixelPerUnit = canvasScaler.referencePixelsPerUnit;
            canvasScaler.referencePixelsPerUnit = Screen.dpi;

            float maxScreenWidthDp = ConvertPixelsToDpNative(Screen.width);
            float maxScreenHeightDp = ConvertPixelsToDpNative(Screen.height);
            bool isIphone16 = false;
#if UNITY_IOS
            isIphone16 = is_iPhone_16;
#endif
            float requiredScreenWidthDp = ConvertPixelsToDpNative(px: Screen.width * (isIphone16 ? ((float)adWidth / 14.0f) : Screen.dpi >= 300 ? ((float)adWidth / 10.3f) : (float)adWidth / 12));
            float requiredScreenHeightDp = ConvertPixelsToDpNative(px: Screen.height * (isIphone16 ? ((float)adHight / 14.0f) : Screen.dpi >= 300 ? ((float)adHight / 10.3f) : (float)adHight / 12));

            bannerSize = new Vector2(Mathf.Min(requiredScreenWidthDp, maxScreenWidthDp), Mathf.Min(requiredScreenHeightDp, maxScreenHeightDp));
            canvasScaler.referenceResolution = tempResolution;
            canvasScaler.referencePixelsPerUnit = tempPixelPerUnit;
        }
        AdPosition nativeAdPosition = AdPosition.Center;
        switch (adPosition)
        {
            case NativeAdPosition.Top: { nativeAdPosition = AdPosition.Top; break; }
            case NativeAdPosition.Bottom: { nativeAdPosition = AdPosition.Bottom; break; }
            case NativeAdPosition.TopLeft: { nativeAdPosition = AdPosition.TopLeft; break; }
            case NativeAdPosition.TopRight: { nativeAdPosition = AdPosition.TopRight; break; }
            case NativeAdPosition.BottomLeft: { nativeAdPosition = AdPosition.BottomLeft; break; }
            case NativeAdPosition.BottomRight: { nativeAdPosition = AdPosition.BottomRight; break; }
            case NativeAdPosition.FullScreen: { nativeAdPosition = AdPosition.Top; break; }
        }
        AdSize adSize = new AdSize(Mathf.FloorToInt(bannerSize.x), Mathf.FloorToInt(bannerSize.y));

        if (Application.platform == RuntimePlatform.Android)
        {
            if (nativeAdPosition == AdPosition.Bottom || nativeAdPosition == AdPosition.BottomRight || nativeAdPosition == AdPosition.BottomLeft)
            {
                if ((Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown) && !isFullScreenNative)
                {
                    if (adSize.Height > adSize.Width)
                    {
                        int previousWidth = adSize.Width;
                        float newHeight = adSize.Width - ((adSize.Width / 10) * 0.25f);
                        adSize = new AdSize(previousWidth, (int)newHeight);
                    }
                }
            }
        }

        System.Collections.Generic.List<NativeOverlayClass> nativeAdListTemp = new System.Collections.Generic.List<NativeOverlayClass>();

        // hide previous all this type of native
        foreach (var currentNative in NativeData)
        {
            if (currentNative.Value.IsLoaded)
            {
                if (currentNative.Value.GetAd() is NativeOverlayAd nativeInternalAd5)
                {
                    try
                    {
                        MobileAdsEventExecutor.ExecuteInUpdate(() => nativeInternalAd5?.Hide());
                        currentNative.Value.IsShowing = false;
                    }
                    catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                }
            }
        }

        System.Collections.Generic.List<string> NativeIds = new System.Collections.Generic.List<string>();
        NativeIds.AddRange(isFullScreenNative ? isNativeFullScreenIdAvailable ? nativeFullScreenID.Count > 0 ? nativeFullScreenID : nativeID : nativeID : nativeID);
        foreach (var currentNative in NativeData)
        {
            for (int j = 0; j < NativeIds.Count; j++)
            {
                if (currentNative.Value.ID.Contains(NativeIds[j]) && currentNative.Value.IsLoaded)
                {
                    NativeOverlayClass overlay = currentNative.Value;
                    if (overlay.GetAd() is NativeOverlayAd nativeInternalAd6)
                    {
                        try
                        {
                            MobileAdsEventExecutor.ExecuteInUpdate(() => nativeInternalAd6?.Hide());
                            overlay.IsShowing = false;
                        }
                        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                        nativeAdListTemp.Add(overlay);
                    }
                }
            }
        }

        if (nativeAdListTemp.Count <= 0) { ShowDebugLog("No " + nativeAdType + " Available.\nReason : Native Not Available.\nAvailable Native Count : " + nativeAdListTemp.Count); return; }

#if AdRevenueAnalytics
        System.Collections.Generic.List<string> genericList = AdRevenueAnalytics.Instance.GetAdIDsList(isFullScreenNative ? AdRevenueAnalytics.AdType.NativeFullScreen : AdRevenueAnalytics.AdType.Native);
        genericList ??= isFullScreenNative ? nativeFullScreenID : nativeID;
        if (genericList == null) { ShowDebugLog("No " + nativeAdType + " Available.\nReason : Native GenericList NULL."); return; }

        int maxLoop = genericList.Count;
        int nativeCount = isFullScreenNative ? nativeFullScreenCurrentIndex : nativeCurrentIndex;

        NativeLoop:

        nativeCount++;
        maxLoop--;
        if (maxLoop < 0) { ShowDebugLog("No " + nativeAdType + " Available.\nReason : Maximum Native Checked. Not Found Required ID."); return; }
        if (nativeCount >= genericList.Count) nativeCount = 0;
        string genericID = genericList[nativeCount];

        if (isFullScreenNative) nativeFullScreenCurrentIndex = nativeCount;
        else nativeCurrentIndex = nativeCount;
        foreach (var kvp in NativeData)
        {
            if (kvp.Key.Contains(genericID) && kvp.Value.IsLoaded)
            {
                if (!string.IsNullOrEmpty(specificID) && !kvp.Key.Contains(specificID)) { ShowDebugLog("No " + nativeAdType + " Available.\nID: " + kvp.Key); continue; }
                if (kvp.Value.GetAd() is NativeOverlayAd nativeInternalAd7 && nativeInternalAd7 != null && kvp.Value.IsLoaded)
                {
                    try { MobileAdsEventExecutor.ExecuteInUpdate(() => kvp.Value.ShowAd(nativeTempStyle ?? lastNativeTempStyle, adSize, nativeAdPosition)); } catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                    AdRevenueAnalytics.Instance.SendRevenueAnalytics(isFullScreenNative ? AdRevenueAnalytics.AdType.NativeFullScreen : AdRevenueAnalytics.AdType.Native, AdRevenueAnalytics.AdRevenueEventType.Ad_Rev_Progression, status: AdRevenueAnalytics.Status.Showing);

                    if (!isFullScreenNative) IsNativeShow = true;
                    ShowDebugLog("Show " + nativeAdType + ".\nID: " + kvp.Key);
                    kvp.Value.IsShowing = true;
                    SendAnalytics("Ad_" + nativeAdType + "_Show");
                    return;
                }
            }
        }
        goto NativeLoop;

#else
        if (!string.IsNullOrEmpty(specificID))
        {
            bool isSpecificIDAvailable = false;
            for (int i = 0; i < nativeAdListTemp.Count; i++)
            {
                if (nativeAdListTemp[i].ID.Contains(specificID))
                {
                    if (isFullScreenNative) nativeFullScreenCurrentIndex = i - 1;
                    else nativeCurrentIndex = i - 1;
                    isSpecificIDAvailable = true;
                    break;
                }
            }
            if (!isSpecificIDAvailable) { ShowDebugLog("No " + nativeAdType + " Available.\nID: " + specificID+"\nReason : Required ID not Available."); return; }
        }

        int nativeCounter = -1;
        if (isFullScreenNative)
        {
            nativeFullScreenCurrentIndex++;
            nativeCounter = nativeFullScreenCurrentIndex;
        }
        else
        {
            nativeCurrentIndex++;
            nativeCounter = nativeCurrentIndex;
        }
        if (nativeCounter >= nativeAdListTemp.Count)
        {
            nativeCounter = 0;
            if (isFullScreenNative) nativeFullScreenCurrentIndex = nativeCounter;
            else nativeCurrentIndex = nativeCounter;
        }
#if !UNITY_EDITOR
        if (nativeAdListTemp[nativeCounter].GetAd() is NativeOverlayAd nativeInternalAd8 && nativeInternalAd8 != null && nativeAdListTemp[nativeCounter].IsLoaded)
#endif
        {
            if (isFullScreenNative) try { MobileAdsEventExecutor.ExecuteInUpdate(() => nativeAdListTemp[nativeCounter].ShowAd(nativeTempStyle ?? lastNativeTempStyle, adSize, nativeAdPosition)); } catch (System.Exception e) { SendExceptionToCrashlytics(e); }
            else
            {
                if (lastNativeAdPosition.Equals(AdPosition.Center)) try { MobileAdsEventExecutor.ExecuteInUpdate(() => nativeAdListTemp[nativeCounter].ShowAd(nativeTempStyle ?? lastNativeTempStyle, adSize, nativeAdPosition)); }catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                else try { MobileAdsEventExecutor.ExecuteInUpdate(() => nativeAdListTemp[nativeCounter].ShowAd(nativeTempStyle ?? lastNativeTempStyle, adSize, lastNativeAdPosition)); } catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                IsNativeShow = true;
            }
            ShowDebugLog("Show " + nativeAdType + ".\nID: " + nativeAdListTemp[nativeCounter].ID);
            nativeAdListTemp[nativeCounter].IsShowing = true;
            SendAnalytics("Ad_"+nativeAdType+"_Show");
            if (isFullScreenNative) nativeFullScreenCurrentIndex = nativeCounter;
            else nativeCurrentIndex = nativeCounter;
            return;
        }
#endif
        ShowDebugLog("No " + nativeAdType + " Available.");
    }

    void LoadNativeGeneral(string key, RectTransform nativeRect = null, bool isShowAfterLoad = false, bool isFullScreenNative = false)
    {
        if (IsRemoveAd()) { ShowDebugLog("No NativeOverlay Load.\nReason: Remove Ads."); return; }

        var splittingArray = key.Split(':');
        if (splittingArray.Length <= 1 || !splittingArray[1].StartsWith("ca-app-pub-"))
        {
            Debug.LogError("Invalid Key Found against Native.\nID : " + key); return;
        }
        string adUnitID = splittingArray[1];

        string nativeAdType = isFullScreenNative ? NativeAdName.NativeOverlayFullScreen.ToString() : NativeAdName.NativeOverlay.ToString();
        if (NativeData.ContainsKey(key)) return;
        NativeData[key] = new NativeOverlayClass(id: key, load: false);
        ShowDebugLog(nativeAdType + " Request Send.\nID: " + key);
        SendAnalytics(isFullScreenNative ? EventName.Ad_NativeOverlayFullScreen : EventName.Ad_NativeOverlay, ParameterKey.myParameter, ParameterValue.AdRequestSend);
        NativeOverlayAd.Load(adUnitID, new AdRequest(), lastNativeAdOptions ?? nativeAdOptions,
            (NativeOverlayAd ad, LoadAdError error) =>
            {
                if (error != null)
                {
                    ShowDebugLog(nativeAdType + " Failed to Load,\nID: " + key + "\nReason: " + error);
                    SendAnalytics(isFullScreenNative ? EventName.Ad_NativeOverlayFullScreen : EventName.Ad_NativeOverlay, ParameterKey.myParameter, ParameterValue.AdRequestFail);
                    SafeRemoveNative(key);
                    return;
                }
                if (ad == null)
                {
                    ShowDebugLog(nativeAdType + " Failed to Load,\nID: " + key + "\nReason: Ad returns NULL from Server.");
                    SendAnalytics(isFullScreenNative ? EventName.Ad_NativeOverlayFullScreen : EventName.Ad_NativeOverlay, ParameterKey.myParameter, ParameterValue.AdRequestFail);
                    SafeRemoveNative(key);
                    return;
                }
                ShowDebugLog(nativeAdType + " Loaded.\nID: " + key);
                SendAnalytics(isFullScreenNative ? EventName.Ad_NativeOverlayFullScreen : EventName.Ad_NativeOverlay, ParameterKey.myParameter, ParameterValue.AdRequestSuccess);
                if (NativeData.TryGetValue(key, out var n)) n.Loaded(ad);

                System.Action<AdValue> onPaid = (AdValue e) =>
                {
                    SendAnalytics(isFullScreenNative ? EventName.Ad_NativeOverlayFullScreen : EventName.Ad_NativeOverlay, ParameterKey.ECPM, e.Value);
#if SingularSDKManager
                    MainThreadQueue.Enqueue(() =>SingularSDKManager.Instance.SendEvent_AdRevenue(SingularSDKManager.NetworkType.Admob, e.CurrencyCode, e.Value));
#endif
#if AdRevenueAnalytics
                    AdRevenueAnalytics.AdType adType = isFullScreenNative ? AdRevenueAnalytics.AdType.NativeFullScreen : AdRevenueAnalytics.AdType.Native;
                    SendRevenueAnalytics(adType, e, key.Split(':')[1].ToString(), ad?.GetResponseInfo()?.GetMediationAdapterClassName(), ad?.GetType().Name);                    
#endif
                    AdImpressionCustomEvent(e.Value);
                };
                System.Action onClicked = () => { IsAppOpenCanShow = false; if (NativeData.TryGetValue(key, out var n)) n.IsCTR = true; };
                System.Action onOpen = () => { IsAppOpenCanShow = false; if (NativeData.TryGetValue(key, out var n)) n.IsCTR = true; };
                System.Action onClose = () => { };
                System.Action onImpressionRecord = () => { SendAnalytics(nativeAdType, ParameterKey.myParameter.ToString(), "ImpressionRecord"); };

                NativeData[key].OnPaidHandler = onPaid;
                NativeData[key].OnClickedHandler = onClicked;
                NativeData[key].OnFullScreenOpenedHandler = onOpen;
                NativeData[key].OnLoadCloseHandler = onClose;

                ad.OnAdPaid += onPaid;
                ad.OnAdClicked += onClicked;
                ad.OnAdFullScreenContentOpened += onOpen;
                ad.OnAdFullScreenContentClosed += onClose;
                ad.OnAdImpressionRecorded += onImpressionRecord;

                NativeData[key].UnsubscribeAllHandlers = () =>
                {
                    try
                    {
                        ad.OnAdPaid -= onPaid;
                        ad.OnAdClicked -= onClicked;
                        ad.OnAdFullScreenContentOpened -= onOpen;
                        ad.OnAdFullScreenContentClosed -= onClose;
                        ad.OnAdImpressionRecorded -= onImpressionRecord;

                    }
                    catch { }
                };

                MainThreadQueue.Enqueue(() =>
                {
                    try { ad?.RenderTemplate(nativeTempStyle, AdSize.MediumRectangle, AdPosition.TopLeft); ad?.Hide(); } catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                });
            });
    }

    #region Native Variables 

    public bool IsNativeShow { private set; get; } = false;

    System.Action<bool> nativeFullScreenActionAfterAd = null;
    RectTransform lastNativeOverlayRectTransform = null;
    int nativeCurrentIndex = -1;
    int nativeFullScreenCurrentIndex = -1;
    float multiplyerSize = 1;
    NativeCustomType lastNativeType = NativeCustomType.SetAsThisRectObject;
    NativeAdOptions lastNativeAdOptions = null;
    NativeTemplateStyle lastNativeTempStyle = null;
    AdPosition lastNativeAdPosition = AdPosition.Center;
    NativeAdPosition lastNativeAdPositionPreDefine = NativeAdPosition.BottomRight;
    NativePreDefineSize lastNativeAdWidth = NativePreDefineSize._50_Percent;
    NativePreDefineSize lastNativeAdHight = NativePreDefineSize._50_Percent;

    readonly NativeAdOptions nativeAdOptions = new NativeAdOptions
    {
        AdChoicesPlacement = AdChoicesPlacement.TopRightCorner,
        MediaAspectRatio = MediaAspectRatio.Any,
        VideoOptions = new VideoOptions()
    };
    readonly NativeTemplateStyle nativeTempStyle = new NativeTemplateStyle
    {
        TemplateId = NativeTemplateId.Medium,
        MainBackgroundColor = Color.white,
        CallToActionText = new NativeTemplateTextStyle
        {
            BackgroundColor = Color.green,
            TextColor = Color.white,
            FontSize = 15,
            Style = NativeTemplateFontStyle.Bold
        },
        PrimaryText = new NativeTemplateTextStyle
        {
            BackgroundColor = new Color(0, 0, 0, 1),
            TextColor = Color.white,
            Style = NativeTemplateFontStyle.Bold
        },
        SecondaryText = new NativeTemplateTextStyle
        {
            BackgroundColor = new Color(0, 0, 0, 1),
            TextColor = Color.white,
            Style = NativeTemplateFontStyle.Normal
        },
        TertiaryText = new NativeTemplateTextStyle
        {
            BackgroundColor = new Color(0, 0, 0, 1),
            TextColor = Color.white,
            Style = NativeTemplateFontStyle.Normal
        }
    };
    readonly System.Collections.Concurrent.ConcurrentDictionary<string, NativeOverlayClass> NativeData = new System.Collections.Concurrent.ConcurrentDictionary<string, NativeOverlayClass>();

    bool lastAudioListenerState_NativeFullScreen = false;
#if UNITY_IOS
    bool is_iPhone_16 = false;
#endif

    #endregion

    #region Native Support

    readonly string isNativeReloadOnCTR = "isNativeReloadOnCTR";
    readonly string isNativeFullScreenReloadOnCTR = "isNativeFullScreenReloadOnCTR";
    bool isNativeReloadOnCTR_FromRemote = false;
    bool isNativeFullScreenReloadOnCTR_FromRemote = false;

    readonly string isNativeReloadOnNumberOfImpression = "isNativeReloadOnNumberOfImpression";
    readonly string isNativeFullScreenReloadOnNumberOfImpression = "isNativeFullScreenReloadOnNumberOfImpression";
    bool isNativeReloadOnNumberOfImpression_FromRemote = false;
    bool isNativeFullScreenReloadOnNumberOfImpression_FromRemote = false;

    readonly string nativeReloadOnNumberOfImpressionCount = "nativeReloadOnNumberOfImpressionCount";
    readonly string nativeFullScreenReloadOnNumberOfImpressionCount = "nativeFullScreenReloadOnNumberOfImpressionCount";
    long nativeReloadOnNumberOfImpression_FromRemote = 3;
    long nativeFullScreenReloadOnNumberOfImpression_FromRemote = 3;

    public void DestroyNativeOverlay(string key, bool isReload = false, NativeOverlayClass nativeAd = null)
    {
        try
        {
            NativeOverlayClass toRemove = null;
            if (NativeData.TryRemove(key, out var removed)) toRemove = removed;
            if (toRemove == null && nativeAd != null) toRemove = nativeAd;

            if (toRemove != null)
            {
                MainThreadQueue.Enqueue(() =>
                {
                    ActionsPerformWithDelay(() =>
                    {
                        try { toRemove.Remove(); }
                        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                    }, 0.1f);
                });
            }


            ShowDebugLog(NativeAdName.NativeOverlay + " Destroy.\nID: " + key);
            if (isReload)
            {
                ActionsPerformWithDelay(() =>
                {
                    LoadNativeInternal();
                    LoadNativeInternal(isFullScreenNative: true);
                }, 0.5f);
            }
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
    }
    float ConvertPixelsToDpNative(float px, bool isFullScreenNative = false)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (isFullScreenNative)
            {
                multiplyerSize = Mathf.Clamp(1.7f + (Screen.dpi - 120f) / (480f - 120f) * (3.0f - 1.7f), 1.7f, 3.0f);
                return px / ((float)Screen.dpi / 160);
            }
            else
            {
                multiplyerSize = 1;
                return px;
            }
        }
        else if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if (Screen.dpi == 0) return Mathf.RoundToInt(px / 2f);
            return Mathf.RoundToInt(px * (160f / Screen.dpi));
        }
        return px;
    }

    void UpdateRemote_NativeData()
    {
#if RemoteConfigManager_Firebase
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isNativeReloadOnCTR))
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig(isNativeReloadOnCTR, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out isNativeReloadOnCTR_FromRemote);
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isNativeFullScreenReloadOnCTR))
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig(isNativeFullScreenReloadOnCTR, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out isNativeFullScreenReloadOnCTR_FromRemote);
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isNativeReloadOnNumberOfImpression))
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig(isNativeReloadOnNumberOfImpression, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out isNativeReloadOnNumberOfImpression_FromRemote);
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isNativeFullScreenReloadOnNumberOfImpression))
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig(isNativeFullScreenReloadOnNumberOfImpression, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out isNativeFullScreenReloadOnNumberOfImpression_FromRemote);
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(nativeReloadOnNumberOfImpressionCount))
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig(nativeReloadOnNumberOfImpressionCount, RemoteConfigManager_Firebase.RemoteConfig_Type.Long, out nativeReloadOnNumberOfImpression_FromRemote);
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(nativeFullScreenReloadOnNumberOfImpressionCount))
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig(nativeFullScreenReloadOnNumberOfImpressionCount, RemoteConfigManager_Firebase.RemoteConfig_Type.Long, out nativeFullScreenReloadOnNumberOfImpression_FromRemote);
#endif
    }

    void SafeDisposeAllNatives()
    {
        try
        {
            foreach (var kv in NativeData)
            {
                try
                {
                    kv.Value?.Remove();
                }
                catch (System.Exception e)
                {
                    SendExceptionToCrashlytics(e);
                }
            }
            NativeData.Clear();
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
    }

    void SafeRemoveNative(string key)
    {
        if (NativeData.TryGetValue(key, out var toRemove) && toRemove != null)
        {
            MainThreadQueue.Enqueue(() =>
            {
                ActionsPerformWithDelay(() =>
                {
                    try { toRemove.Remove(); } catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                    NativeData.TryRemove(key, out _);
                }, 0.05f);
            });
        }
    }

    #endregion

    #region Native Structure

    public GameObject nativeFullScreen = null;
    public GameObject NativeFullScreen
    {
        get
        {
            if (nativeFullScreen == null)
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i).name.ToLower().Equals("NativeFullScreen".ToLower()))
                    {
                        nativeFullScreen = transform.GetChild(i).gameObject;
                        break;
                    }
                }
            }
            return nativeFullScreen;
        }
    }
    public CustomNativeOverlay nativeFullScreenAdContentScript = null;
    public CustomNativeOverlay NativeFullScreenAdContentScript
    {
        get
        {
            if (nativeFullScreenAdContentScript == null) nativeFullScreenAdContentScript = NativeFullScreen.GetComponentInChildren<CustomNativeOverlay>();
            return nativeFullScreenAdContentScript;
        }
    }

    public class NativeOverlayClass
    {
        public string ID { get; set; }
        public bool IsLoaded { get; set; }
        public bool IsShowing { get; set; }
        public bool IsCTR { get; set; }
        public int NumberOfImpression { get; set; } = 0;
        public NativeOverlayAd Ad { get; private set; }

        public System.Action UnsubscribeAllHandlers;

        // Keep strong refs for unsubscribe
        public System.Action OnLoadCloseHandler;
        public System.Action OnLoadedHandler;
        public System.Action<AdValue> OnPaidHandler;
        public System.Action OnFullScreenOpenedHandler;
        public System.Action OnClickedHandler;

        public NativeOverlayClass(string id, bool load = false)
        {
            ID = id;
            IsLoaded = load;
            IsShowing = false;
            IsCTR = false;
            NumberOfImpression = 0;
        }
        public void Loaded(object ad)
        {
            if (ad is NativeOverlayAd nativeInternalAd9) { IsLoaded = true; Ad = nativeInternalAd9; }
            else throw new System.ArgumentException("Invalid ad type passed to Loaded method.");
        }
        public void Remove()
        {
            try
            {
                try { UnsubscribeAllHandlers?.Invoke(); } catch { }
                try { Ad?.Hide(); Ad?.Destroy(); } catch { }
            }
            catch (System.Exception e) { SendExceptionToCrashlytics_Static(e); }
            finally
            {
                UnsubscribeAllHandlers = null;
                Ad = null;
                IsLoaded = false;
                IsShowing = false;
                IsCTR = false;
                NumberOfImpression = 0;
            }
        }
        public void ShowAd(NativeTemplateStyle nativeTempStyle, AdSize adSize, Vector2 position)
        {
            try { Ad?.RenderTemplate(nativeTempStyle, adSize, Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y)); } catch (System.Exception e) { SendExceptionToCrashlytics_Static(e); }
            IsShowing = true;
            NumberOfImpression++;
        }
        public void ShowAd(NativeTemplateStyle nativeTempStyle, AdSize adSize, AdPosition position)
        {
            try { Ad?.RenderTemplate(nativeTempStyle, adSize, position); } catch (System.Exception e) { SendExceptionToCrashlytics_Static(e); }
            IsShowing = true;
            NumberOfImpression++;
        }
        public object GetAd()
        {
            return Ad;
        }
    }

    public enum NativeAdType
    {
        SmallBannerNative,
        MediumRectNative,
    }
    public enum NativeAdPosition
    {
        Top = 1,
        Bottom = 2,
        TopLeft = 3,
        TopRight = 4,
        BottomLeft = 5,
        BottomRight = 6,
        FullScreen = 7
    }

    public enum NativePreDefineSize
    {
        _100_Percent = 10,
        _90_Percent = 9,
        _80_Percent = 8,
        _70_Percent = 7,
        _60_Percent = 6,
        _50_Percent = 5,
        _40_Percent = 4,
        _30_Percent = 3,
        _20_Percent = 2,
        _10_Percent = 1,
    }
    public enum NativeCustomType
    {
        PreDefined = 0,
        SetAsThisRectObject = 1
    }
    public enum NativeAdName
    {
        NativeOverlay = 0,
        NativeOverlayFullScreen = 1
    }


    #endregion

    #endregion

    #endregion

    #region Interstitial

    public void LoadInterstitial(InterstitialType type = InterstitialType.interstitial, string specificID = "")
    {
        if (IsRemoveAd()) { ShowDebugLog("No " + type + " Load.\nReason : Remove Ads."); return; }
        if (!IsInternetConnection()) { ShowDebugLog("No " + type + " Load.\nReason : No Internet."); return; }
        if (!CheckInitialization(false, new System.Action(() => { LoadInterstitial(type, specificID: specificID); }))) { ShowDebugLog("No " + type + " Load.\nReason : No SDK Initilized."); return; }
        if (IsLowMemory(NoAnyInterstitialBelow)) { ShowDebugLog("No " + type + " Load.\nReason : Low Available Memory"); return; }
#if RemoteConfigManager_Firebase
        string interstitialRemoteID = type == InterstitialType.interstitial ? isInterstitialLoad : isInterstitialStaticLoad;
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(interstitialRemoteID))
        {
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(interstitialRemoteID, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isInterstitialFromRemote);
            if (!isInterstitialFromRemote) { ShowDebugLog("No " + type + " Load.\nReason : RemoteConfig Not Allowed.\nKey: " + interstitialRemoteID + ", value :" + isInterstitialFromRemote); return; }
        }
#endif

        AdvertisementType adType = AdvertisementType.Interstitial;
        interstitialChangeLabel:

        switch (type)
        {
            case InterstitialType.interstitial:
                {
                    if ((isInterstitialIdAvailable && interstitialID.Count > 0) || isTestMode)
                    {
                        if (IsLowMemory(NoInterstitialStaticAbove))
                        {
                            type = InterstitialType.interstitialStatic;
                            adType = AdvertisementType.InterstitialStatic;
                            goto interstitialChangeLabel;
                        }
                    }
                    else
                    {
                        type = InterstitialType.interstitialStatic;
                        adType = AdvertisementType.InterstitialStatic;
                        goto interstitialChangeLabel;
                    }
                    break;
                }
            case InterstitialType.interstitialStatic:
                {
                    if ((isInterstitialStaticIdAvailable && interstitialStaticID.Count > 0) || isTestMode)
                    {
                        adType = AdvertisementType.InterstitialStatic;
                        break;
                    }
                    else return;
                }
        }
        LoadAd(adType: adType, specificID: specificID);
    }

    public bool IsInterstitialLoaded(InterstitialType interstitialType = InterstitialType.interstitial, bool onlyCheckThisTypeOfAd = false, bool loadIfNotLoaded = true, string specificID = "")
    {
        if (IsRemoveAd()) return false;
        if (!IsInternetConnection()) return false;

        System.Collections.Generic.List<string> genericList = new System.Collections.Generic.List<string>();
        AdvertisementType adType = AdvertisementType.Interstitial;
        switchLabel:
        switch (interstitialType)
        {
            case InterstitialType.interstitial:
                {
                    adType = AdvertisementType.Interstitial;

#if AdRevenueAnalytics
                    genericList = AdRevenueAnalytics.Instance.GetAdIDsList(AdRevenueAnalytics.AdType.Interstitial);
                    genericList ??= interstitialID;
#else
                    genericList = interstitialID; 
#endif
                    adType = AdvertisementType.Interstitial;

                    break;
                }
            case InterstitialType.interstitialStatic:
                {
                    adType = AdvertisementType.InterstitialStatic;

#if AdRevenueAnalytics
                    genericList = AdRevenueAnalytics.Instance.GetAdIDsList(AdRevenueAnalytics.AdType.InterstitialStatic);
                    genericList ??= interstitialStaticID;
#else
                    genericList = interstitialStaticID; 
#endif
                    adType = AdvertisementType.InterstitialStatic;

                    break;
                }
        }

        if (string.IsNullOrEmpty(specificID))
        {
            foreach (string genericID in genericList)
            {
                foreach (var data in AdvertisementData.Values)
                {
                    if (data.AdType.Equals(adType) && data.IsLoaded && data.ID.Contains(genericID)) return true;
                }
            }
        }
        else
        {
            foreach (var data in AdvertisementData.Values)
            {
                if (data.ID.Contains(specificID) && data.IsLoaded) return true;
            }
        }
        if (adType.Equals(AdvertisementType.Interstitial) && !onlyCheckThisTypeOfAd)
        {
#if RemoteConfigManager_Firebase
            if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isInterstitialStaticBackfillToInterstitial))
            {
                RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isInterstitialStaticBackfillToInterstitial, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isInterStaticBackfillToInterFromRemote);
                if (isInterStaticBackfillToInterFromRemote)
                {
#endif
                    adType = AdvertisementType.InterstitialStatic;
                    interstitialType = InterstitialType.interstitialStatic;
                    goto switchLabel;
#if RemoteConfigManager_Firebase
                }
            }
#endif
        }
        if (loadIfNotLoaded) LoadInterstitial(specificID: specificID);
        return false;
    }

    public void ShowInterstitial(InterstitialType interstitialType, float afterAdTimeScale = 1, System.Action<bool> actionAfterAd = null, string specificID = "")
    {
        if (IsRemoveAd()) { Time.timeScale = afterAdTimeScale; actionAfterAd?.Invoke(false); ShowDebugLog("No " + interstitialType.ToString() + " Shown.\nReason : Remove Ads"); return; }
        if (!IsInternetConnection()) { Time.timeScale = afterAdTimeScale; actionAfterAd?.Invoke(false); ShowDebugLog("No " + interstitialType.ToString() + " Shown.\nReason : No Internet"); return; }
        if (IsLowMemory(NoAnyInterstitialBelow)) { Time.timeScale = afterAdTimeScale; actionAfterAd?.Invoke(false); ShowDebugLog("No " + interstitialType.ToString() + " Shown.\nReason : Low Memory"); return; }
#if RemoteConfigManager_Firebase
        string interstitialRemoteID = interstitialType == InterstitialType.interstitial ? isInterstitialLoad : isInterstitialStaticLoad;
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(interstitialRemoteID))
        {
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(interstitialRemoteID, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isInterstitialFromRemote);
            if (!isInterstitialFromRemote) { ShowDebugLog("No " + interstitialType + " Shown.\nReason : RemoteConfig Not Allowed."); Time.timeScale = afterAdTimeScale; actionAfterAd?.Invoke(false); return; }
        }
#endif

#if MonetizationAnalytics
        MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_Interstitial, MonetizationAnalytics.ParameterKey.myParameter,
            MonetizationAnalytics.ParameterValue.ShowCall,new System.Collections.Generic.List<MonetizationAnalytics.Networks> { MonetizationAnalytics.Networks.Admob});
#endif

        if (!string.IsNullOrEmpty(specificID) && IsInterstitialLoaded(interstitialType: interstitialType, specificID: specificID))
        {
            interstitialActionAfterAd = actionAfterAd;
            IsAdmobShowingFullScreenAd = true;
            IsAppOpenCanShow = false;
            ShowLoadingAdBG();
            Time.timeScale = afterAdTimeScale;
            ActionsPerformWithDelay(new System.Action(() => { ShowInterstitialInternal(type: interstitialType, specificID: specificID); }), loadingAdTimeDelay);
        }
        else if (IsInterstitialLoaded(interstitialType))
        {
            interstitialActionAfterAd = actionAfterAd;
            IsAdmobShowingFullScreenAd = true;
            IsAppOpenCanShow = false;
            ShowLoadingAdBG();
            Time.timeScale = afterAdTimeScale;
            ActionsPerformWithDelay(new System.Action(() => { ShowInterstitialInternal(type: interstitialType); }), loadingAdTimeDelay);
        }
        else
        {
            ShowDebugLog("No " + interstitialType.ToString() + " Available.\nReason : No Any Ad Available.");
            actionAfterAd?.Invoke(false);
            Time.timeScale = afterAdTimeScale;
            return;
        }
    }

    #region Interstitial Internal

    void ShowInterstitialInternal(InterstitialType type = InterstitialType.interstitial, string specificID = "")
    {
        bool isAdShown = false;
        System.Collections.Generic.List<string> genericList = new System.Collections.Generic.List<string>();
        AdvertisementType adType = AdvertisementType.Interstitial;
        int listCount = 0;
        switch (type)
        {
            case InterstitialType.interstitial:
                {
#if AdRevenueAnalytics
                    genericList = AdRevenueAnalytics.Instance.GetAdIDsList(AdRevenueAnalytics.AdType.Interstitial);
                    genericList ??= interstitialID;
#else
                    genericList = interstitialID; 
#endif
                    adType = AdvertisementType.Interstitial;
                    listCount = interstitialID.Count;
                    break;
                }
            case InterstitialType.interstitialStatic:
                {
#if AdRevenueAnalytics
                    genericList = AdRevenueAnalytics.Instance.GetAdIDsList(AdRevenueAnalytics.AdType.InterstitialStatic);
                    genericList ??= interstitialStaticID;
#else
                    genericList = interstitialStaticID; 
#endif
                    adType = AdvertisementType.InterstitialStatic;
                    listCount = interstitialStaticID.Count;
                    break;
                }
        }

        if (string.IsNullOrEmpty(specificID))
        {
            foreach (string genericID in genericList)
            {
                foreach (var kvp in AdvertisementData)
                {
                    if (kvp.Key.Contains(genericID) && kvp.Value.AdType.Equals(adType) && kvp.Value.IsLoaded)
                    {
                        ShowDebugLog("Show " + adType + "\nID: " + kvp.Key);
#if MonetizationAnalytics
                MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_Interstitial, MonetizationAnalytics.ParameterKey.myParameter,
            MonetizationAnalytics.ParameterValue.Showing,new System.Collections.Generic.List<MonetizationAnalytics.Networks> { MonetizationAnalytics.Networks.Admob});
#endif
                        SetLastAdStatus(genericID, adType, interstitialActionAfterAd);
                        kvp.Value.ShowAd();
                        return;
                    }
                }
            }
        }
        else
        {
            foreach (var kvp in AdvertisementData)
            {
                if (kvp.Key.Contains(specificID) && kvp.Value.AdType.Equals(adType) && kvp.Value.IsLoaded)
                {
                    ShowDebugLog("Show " + adType + "\nID: " + kvp.Key);
#if MonetizationAnalytics
                MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_Interstitial, MonetizationAnalytics.ParameterKey.myParameter,
            MonetizationAnalytics.ParameterValue.Showing,new System.Collections.Generic.List<MonetizationAnalytics.Networks> { MonetizationAnalytics.Networks.Admob});
#endif
                    SetLastAdStatus(kvp.Key, adType, interstitialActionAfterAd);
                    kvp.Value.ShowAd();
                    return;
                }
            }
        }

#if AdRevenueAnalytics
        ShowDebugLog("No " + adType + " Ads Available\nReason: AdPilot Provided ID's Not Loaded.");
        return;
#endif
        if (!isAdShown)
        {
            foreach (var data in AdvertisementData.Values)
            {
                if (adType.Equals(data.AdType) && data.IsLoaded)
                {
                    ShowDebugLog("Show " + adType + "\nID: " + data.ID);
#if MonetizationAnalytics
                    MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_Interstitial, MonetizationAnalytics.ParameterKey.myParameter,
                MonetizationAnalytics.ParameterValue.Showing, new System.Collections.Generic.List<MonetizationAnalytics.Networks> { MonetizationAnalytics.Networks.Admob });
#endif
                    SetLastAdStatus(data.ID, adType, interstitialActionAfterAd);
                    MobileAdsEventExecutor.ExecuteInUpdate(() => data.ShowAd());
                    return;
                }
            }
        }

        if (!isAdShown && adType.Equals(AdvertisementType.Interstitial))
        {
#if RemoteConfigManager_Firebase
            if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isInterstitialStaticBackfillToInterstitial))
            {
                RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isInterstitialStaticBackfillToInterstitial, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isInterStaticBackfillToInterFromRemote);
                if (isInterStaticBackfillToInterFromRemote)
                {
#endif
                    adType = AdvertisementType.InterstitialStatic;
                    foreach (var data in AdvertisementData.Values)
                    {
                        if (adType.Equals(data.AdType))
                        {
                            ShowDebugLog("Show " + adType + "\nID: " + data.ID);
#if MonetizationAnalytics
                    MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_Interstitial, MonetizationAnalytics.ParameterKey.myParameter,
                MonetizationAnalytics.ParameterValue.Showing, new System.Collections.Generic.List<MonetizationAnalytics.Networks> { MonetizationAnalytics.Networks.Admob });
#endif
                            SetLastAdStatus(data.ID, adType, interstitialActionAfterAd);
                            MobileAdsEventExecutor.ExecuteInUpdate(() => data.ShowAd());
                            return;
                        }
                    }
#if RemoteConfigManager_Firebase
                }
            }
#endif
        }
        if (!isAdShown) ShowDebugLog("No "+adType+" Ads Available");
    }

    System.Action<bool> interstitialActionAfterAd = null;
    public enum InterstitialType
    {
        interstitial,
        interstitialStatic
    }

    #endregion

    #endregion

    #region Rewarded

    public void LoadRewardedAd(RewardedType rewardedType = RewardedType.rewardedVideo, string specificID = "")
    {
        if (!IsInternetConnection()) { ShowDebugLog("No " + rewardedType + " Load.\nReason : No Internet."); return; }
        if (!CheckInitialization(false, new System.Action(() => { LoadRewardedAd(rewardedType, specificID: specificID); }))) { ShowDebugLog("No " + rewardedType + " Load.\nReason : No SDK Initilized."); return; }
        if (IsLowMemory(NoAnyRewardedBelow)) { ShowDebugLog("No " + rewardedType + " Load.\nReason : Low Available Memory"); return; }
#if RemoteConfigManager_Firebase
        string rewardedRemoteID = rewardedType == RewardedType.rewardedVideo ? isRewardedLoad : isRewardedInterstitialLoad;
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(rewardedRemoteID))
        {
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(rewardedRemoteID, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isRewardedFromRemote);
            if (!isRewardedFromRemote) { ShowDebugLog("No " + rewardedType + " Load.\nReason : RemoteConfig Not Allowed.\nKey: " + isRewardedLoad + ", value :" + isRewardedFromRemote); return; }
        }
#endif

        AdvertisementType adType = AdvertisementType.Rewarded;
        try
        {
            reCalculateAdType:
            switch (rewardedType)
            {
                case RewardedType.rewardedVideo:
                    {
                        if (isRewardedVideoIdAvailable || isTestMode)
                            if (!IsLowMemory(NoRewardedInterstitialAbove))
                                break;
                        rewardedType = RewardedType.rewardedInterstitial;
                        goto reCalculateAdType;
                    }
                case RewardedType.rewardedInterstitial:
                    {
                        if (isRewardedInterstitialIdAvailable || isTestMode)
                        {
                            if (IsLowMemory(NoRewardedInterstitialAbove)) { ShowDebugLog("No " + rewardedType + " Load.\nReason: Low Memory"); return; }
                            adType = AdvertisementType.RewardedInterstitial;
                            break;
                        }
                        return;
                    }
            }
            LoadAd(adType: adType, specificID: specificID);
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
    }
    public bool IsRewardedLoaded(RewardedType rewardType = RewardedType.rewardedVideo, bool onlyCheckThisTypeOfAd = false, bool loadIfNotLoaded = true, string specificID = "")
    {
        if (!IsInternetConnection()) return false;

        System.Collections.Generic.List<string> genericList = new System.Collections.Generic.List<string>();
        AdvertisementType adType = AdvertisementType.Rewarded;
        switchLabel:
        switch (rewardType)
        {
            case RewardedType.rewardedVideo:
                {
                    adType = AdvertisementType.Rewarded;
#if AdRevenueAnalytics
                    genericList = AdRevenueAnalytics.Instance.GetAdIDsList(AdRevenueAnalytics.AdType.Rewarded);
                    genericList ??= rewardedID;
#else
                    genericList = rewardedID; 
#endif
                    break;
                }
            case RewardedType.rewardedInterstitial:
                {
                    adType = AdvertisementType.RewardedInterstitial;
#if AdRevenueAnalytics
                    genericList = AdRevenueAnalytics.Instance.GetAdIDsList(AdRevenueAnalytics.AdType.RewardedInterstitial);
                    genericList ??= rewardedInterstitialID;
#else
                    genericList = rewardedInterstitialID; 
#endif
                    break;
                }
        }
        if (string.IsNullOrEmpty(specificID))
        {
            foreach (string genericID in genericList)
            {
                foreach (var data in AdvertisementData.Values)
                {
                    if (data.AdType.Equals(adType) && data.IsLoaded && data.ID.Contains(genericID)) return true;
                }
            }
        }
        else
        {
            foreach (var data in AdvertisementData.Values)
            {
                if (data.ID.Contains(specificID) && data.IsLoaded) return true;
            }
        }

        if (adType.Equals(AdvertisementType.Rewarded) && !onlyCheckThisTypeOfAd)
        {
#if RemoteConfigManager_Firebase
            if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isRewardedInterstitialBackfillToRewarded))
            {
                RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isRewardedInterstitialBackfillToRewarded, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isRewardedInterBackfillToRewardedFromRemote);
                if (isRewardedInterBackfillToRewardedFromRemote)
                {
#endif
                    adType = AdvertisementType.RewardedInterstitial;
                    rewardType = RewardedType.rewardedInterstitial;
                    goto switchLabel;
#if RemoteConfigManager_Firebase
                }
            }
#endif
        }
        if (loadIfNotLoaded) LoadRewardedAd(specificID: specificID);
        return false;
    }
    public void ShowRewardedAd(RewardedType rewardedType = RewardedType.rewardedVideo, System.Action<bool> actionAfterAd = null, string specificID = "")
    {
        if (!IsInternetConnection()) { actionAfterAd?.Invoke(false); ShowDebugLog("No " + rewardedType.ToString() + " Shown.\nReason : No Internet"); return; }
        if (IsLowMemory(NoAnyRewardedBelow)) { actionAfterAd?.Invoke(false); ShowDebugLog("No " + rewardedType.ToString() + " Shown.\nReason : Low Memory"); return; }
        if (!IsRewardedLoaded(rewardedType)) { ShowDebugLog("No " + rewardedType.ToString() + " Shown.\nReason : No Any Ad Available."); actionAfterAd?.Invoke(false); return; }
#if RemoteConfigManager_Firebase
        string rewardedRemoteID = rewardedType == RewardedType.rewardedVideo ? isRewardedLoad : isRewardedInterstitialLoad;
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(rewardedRemoteID))
        {
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(rewardedRemoteID, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isRewardedFromRemote);
            if (!isRewardedFromRemote) { ShowDebugLog("No " + rewardedType + " Shown.\nReason : RemoteConfig Not Allowed."); actionAfterAd?.Invoke(false); return; }
        }
#endif
#if MonetizationAnalytics
        MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_Rewarded, MonetizationAnalytics.ParameterKey.myParameter,
    MonetizationAnalytics.ParameterValue.ShowCall, new System.Collections.Generic.List<MonetizationAnalytics.Networks> { MonetizationAnalytics.Networks.Admob,MonetizationAnalytics.Networks.Applovin,MonetizationAnalytics.Networks.UTM });
#endif
        if (!string.IsNullOrEmpty(specificID) && IsRewardedLoaded(rewardType: rewardedType, specificID: specificID))
        {
            actionGetReward = actionAfterAd;
            ShowRewardedInternal(rewardedType, specificID: specificID);
        }
        else if (IsRewardedLoaded(rewardType: rewardedType))
        {
            actionGetReward = actionAfterAd;
            ShowRewardedInternal(rewardedType);
        }
        else
        {
            ShowDebugLog("No " + rewardedType.ToString() + " Available.\nReason : No Any Ad Available.");
            actionAfterAd?.Invoke(false);
        }

    }
    public void GetReward(bool AdCompleted = true)
    {
        System.Action action = new System.Action(() =>
        {
            GetRewardCustomCode();
            ShowDebugLog("GetReward(" + AdCompleted + ")");
            IsAdmobShowingFullScreenAd = false;
        });
        ActionsPerformWithDelay(action, 0.2f);
    }

    #region Rewarded Internal
    void ShowRewardedInternal(RewardedType type = RewardedType.rewardedVideo, string specificID = "")
    {
        System.Collections.Generic.List<string> genericList = new System.Collections.Generic.List<string>();
        AdvertisementType adType = AdvertisementType.Rewarded;
        switch (type)
        {
            case RewardedType.rewardedVideo:
                {
#if AdRevenueAnalytics
                    genericList = AdRevenueAnalytics.Instance.GetAdIDsList(AdRevenueAnalytics.AdType.Rewarded);
                    genericList ??= rewardedID;
#else
                    genericList = rewardedID; 
#endif
                    adType = AdvertisementType.Rewarded;
                    break;
                }
            case RewardedType.rewardedInterstitial:
                {
#if AdRevenueAnalytics
                    genericList = AdRevenueAnalytics.Instance.GetAdIDsList(AdRevenueAnalytics.AdType.RewardedInterstitial);
                    genericList ??= rewardedInterstitialID;
#else
                    genericList = rewardedInterstitialID; 
#endif
                    adType = AdvertisementType.RewardedInterstitial;
                    break;
                }
        }

        System.Action action = null;

        if (string.IsNullOrEmpty(specificID))
        {
            foreach (string genericID in genericList)
            {
                foreach (var kvp in AdvertisementData)
                {
                    if (kvp.Key.Contains(genericID) && kvp.Value.AdType.Equals(adType) && kvp.Value.IsLoaded)
                    {
                        action = new System.Action(() =>
                        {
                            ShowDebugLog("Show " + adType + ".\nID :" + kvp.Key);
#if MonetizationAnalytics
                    MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_Rewarded, MonetizationAnalytics.ParameterKey.myParameter,
                MonetizationAnalytics.ParameterValue.Showing, new System.Collections.Generic.List<MonetizationAnalytics.Networks> { MonetizationAnalytics.Networks.Admob });
#endif
                            SetLastAdStatus(kvp.Key, adType, actionGetReward);
                            kvp.Value.ShowAd((AdCompleted) =>
                            {
                                System.Action CompleteCallBackAction = new System.Action(() =>
                            {
                                GetReward(AdCompleted);
                                actionGetReward?.Invoke(AdCompleted);
                                actionGetReward = null;
                            });
                                ActionsPerformWithDelay(CompleteCallBackAction, 0.1f);
                            });
                        });
                        goto adShowLable;
                    }
                }
            }
        }
        else
        {
            foreach (var kvp in AdvertisementData)
            {
                if (kvp.Key.Contains(specificID) && kvp.Value.AdType.Equals(adType) && kvp.Value.IsLoaded)
                {
                    action = new System.Action(() =>
                    {
                        ShowDebugLog("Show " + adType + ".\nID :" + kvp.Key);
#if MonetizationAnalytics
                    MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_Rewarded, MonetizationAnalytics.ParameterKey.myParameter,
                MonetizationAnalytics.ParameterValue.Showing, new System.Collections.Generic.List<MonetizationAnalytics.Networks> { MonetizationAnalytics.Networks.Admob });
#endif
                        SetLastAdStatus(kvp.Key, adType, actionGetReward);
                        kvp.Value.ShowAd((AdCompleted) =>
                        {
                            System.Action CompleteCallBackAction = new System.Action(() =>
                            {
                                GetReward(AdCompleted);
                                actionGetReward?.Invoke(AdCompleted);
                                actionGetReward = null;
                            });
                            ActionsPerformWithDelay(CompleteCallBackAction, 0.1f);
                        });
                    });
                    goto adShowLable;
                }
            }
        }

#if AdRevenueAnalytics
        ShowDebugLog("No " + adType + " Ads Available\nReason: AdPilot Provided ID's Not Loaded.");
        return;
#endif
        foreach (var data in AdvertisementData.Values)
        {
            if (adType.Equals(data.AdType) && data.IsLoaded)
            {
                action = new System.Action(() =>
                {
                    ShowDebugLog("Show " + adType + ".\nID :" + data.ID);
#if MonetizationAnalytics
                    MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_Rewarded, MonetizationAnalytics.ParameterKey.myParameter,
                MonetizationAnalytics.ParameterValue.Showing, new System.Collections.Generic.List<MonetizationAnalytics.Networks> { MonetizationAnalytics.Networks.Applovin });
#endif
                    SetLastAdStatus(data.ID, adType, actionGetReward);
                    data.ShowAd((AdCompleted) =>
                    {
                        GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                        {
                            System.Action CompleteCallBackAction = new System.Action(() =>
                            {
                                GetReward(AdCompleted);
                                actionGetReward?.Invoke(AdCompleted);
                                actionGetReward = null;
                            });
                            ActionsPerformWithDelay(CompleteCallBackAction, 0.1f);
                        });
                    });
                });
                goto adShowLable;
            }
        }

        if (adType.Equals(AdvertisementType.Rewarded))
        {
#if RemoteConfigManager_Firebase
            if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isRewardedInterstitialBackfillToRewarded))
            {
                RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isRewardedInterstitialBackfillToRewarded, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isRewardedInterBackfillToRewardedFromRemote);
                if (isRewardedInterBackfillToRewardedFromRemote)
                {
#endif
                    adType = AdvertisementType.RewardedInterstitial;
                    foreach (var data in AdvertisementData.Values)
                    {
                        if (adType.Equals(data.AdType) && data.IsLoaded)
                        {
                            action = new System.Action(() =>
                            {
                                ShowDebugLog("Show " + adType + ".\nID :" + data.ID);
#if MonetizationAnalytics
                        MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_Rewarded, MonetizationAnalytics.ParameterKey.myParameter,
                    MonetizationAnalytics.ParameterValue.Showing, new System.Collections.Generic.List<MonetizationAnalytics.Networks> { MonetizationAnalytics.Networks.Applovin });
#endif
                                SetLastAdStatus(data.ID, adType, actionGetReward);
                                data.ShowAd((AdCompleted) =>
                                {
                                    GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                                    {
                                        System.Action CompleteCallBackAction = new System.Action(() =>
                                        {
                                            GetReward(AdCompleted);
                                            actionGetReward?.Invoke(AdCompleted);
                                            actionGetReward = null;
                                        });
                                        ActionsPerformWithDelay(CompleteCallBackAction, 0.1f);
                                    });
                                });
                            });
                            goto adShowLable;
                        }
                    }
#if RemoteConfigManager_Firebase
                }
            }
#endif

            }
            adShowLable:
            if (action != null)
            {
                ShowLoadingAdBG();
                IsAdmobShowingFullScreenAd = true;
                IsAppOpenCanShow = false;
                ActionsPerformWithDelay(action, loadingAdTimeDelay);
            }
            else ShowDebugLog("No " + adType + " Ads Available");
    }

    System.Action<bool> actionGetReward = null;
    public enum RewardedType
    {
        rewardedVideo,
        rewardedInterstitial
    }
    #endregion

    #endregion

    #region App-Open

    public void LoadAppOpenAd(string specificID = "")
    {
        string adType = "AppOpen";
        if (IsRemoveAd()) { ShowDebugLog("No " + adType + " Load.\nReason : Remove Ads."); return; }
        if (!IsInternetConnection()) { ShowDebugLog("No " + adType + " Load.\nReason : No Internet."); return; }
        if (!CheckInitialization(false, new System.Action(() => { LoadAppOpenAd(specificID: specificID); }))) { ShowDebugLog("No " + adType + " Load.\nReason : No SDK Initilized."); return; }
        if (IsLowMemory(NoAppOpenBelow)) { ShowDebugLog("No " + adType + " Load.\nReason : Low Available Memory"); return; }
        if (!isAppOpenIdAvailable && !isTestMode) { ShowDebugLog("No " + adType + " Load.\nReason : No " + adType + " ID Available."); return; }
#if RemoteConfigManager_Firebase
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isAppOpenLoad))
        {
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isAppOpenLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isAppOpenFromRemote);
            if (!isAppOpenFromRemote) { ShowDebugLog("No " + adType + " Load.\nReason : RemoteConfig Not Allowed.\nKey: " + isAppOpenLoad + ", value :" + isAppOpenFromRemote); return; }
        }
#endif
        LoadAd(AdvertisementType.AppOpen, specificID: specificID);
    }
    public bool IsAppOpenAdLoaded(bool loadIfNotLoaded = true, string specificID = "")
    {
        if (IsRemoveAd()) return false;
        if (!IsInternetConnection()) return false;

        AdvertisementType adType = AdvertisementType.AppOpen;
        System.Collections.Generic.List<string> genericList = new System.Collections.Generic.List<string>();
#if AdRevenueAnalytics
        genericList = AdRevenueAnalytics.Instance.GetAdIDsList(AdRevenueAnalytics.AdType.AppOpen);
        genericList ??= appOpenID;
#else
        genericList = appOpenID;
#endif

        if (string.IsNullOrEmpty(specificID))
        {
            foreach (string genericID in genericList)
            {
                foreach (var data in AdvertisementData.Values)
                {
                    if (data.ID.Contains(genericID) && data.AdType.Equals(adType) && data.IsLoaded) return true;
                }
            }
        }
        else
        {
            foreach (var data in AdvertisementData.Values)
            {
                if (data.ID.Contains(specificID) && data.IsLoaded) return true;
            }
        }
        if (loadIfNotLoaded) LoadAppOpenAd(specificID: specificID);
        return false;
    }
    public void ShowAppOpenAd(System.Action<bool> actionAfterAd = null, string specificID = "")
    {
        if (IsRemoveAd()) { actionAfterAd?.Invoke(false); ShowDebugLog("No AppOpen Shown.\nReason : Remove Ads"); return; }
        if (!IsInternetConnection()) { actionAfterAd?.Invoke(false); ShowDebugLog("No AppOpen Shown.\nReason : No Internet."); return; }
        if (IsLowMemory(NoAppOpenBelow)) { actionAfterAd?.Invoke(false); ShowDebugLog("No AppOpen Shown.\nReason : Low Memory"); return; }
        if (!IsAppOpenAdLoaded()) { ShowDebugLog("No AppOpen Shown.\nReason : No Any Ad Available."); actionAfterAd?.Invoke(false); return; }
#if RemoteConfigManager_Firebase
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isAppOpenLoad))
        {
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(isAppOpenLoad, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out bool isAppOpenFromRemote);
            if (!isAppOpenFromRemote) { ShowDebugLog("No AppOpen Shown.\nReason : RemoteConfig not Allowed."); actionAfterAd?.Invoke(false); return; }
        }
#endif
#if MonetizationAnalytics
            MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_AppOpen, MonetizationAnalytics.ParameterKey.myParameter,
        MonetizationAnalytics.ParameterValue.ShowCall, new System.Collections.Generic.List<MonetizationAnalytics.Networks> { MonetizationAnalytics.Networks.UTM });
#endif
        if (!string.IsNullOrEmpty(specificID) && IsAppOpenAdLoaded(specificID: specificID))
        {
            ShowLoadingAdBG();
            IsAdmobShowingFullScreenAd = true;
            System.Action actions = new System.Action(() =>
            {
                appOpenActionAfterAd = actionAfterAd;
                IsAdmobShowingFullScreenAd = false;
                ShowAppOpenInternal(specificID: specificID);
            });
            ActionsPerformWithDelay(actions, loadingAdTimeDelay);
        }
        else if (IsAppOpenAdLoaded())
        {
            ShowLoadingAdBG();
            IsAdmobShowingFullScreenAd = true;
            System.Action actions = new System.Action(() =>
            {
                appOpenActionAfterAd = actionAfterAd;
                IsAdmobShowingFullScreenAd = false;
                ShowAppOpenInternal();
            });
            ActionsPerformWithDelay(actions, loadingAdTimeDelay);
        }
        else
        {
            ShowDebugLog("No " + AdvertisementType.AppOpen.ToString() + " Available.\nReason : No Any Ad Available.");
            actionAfterAd?.Invoke(false);
        }
    }
    void OnAppStateChanged()
    {
        if (isAutoReload && isAppOpenReloadAfterShow)
        {
#if InAppPurchase
            if(InAppPurchase.isIAPUnderProcess) return;
#endif
            if (IsAppOpenCanShow)
            {
                if (isAppOpen_OnAppStateChange)
                {
                    if (isAppOpenOrNative_OnAppStateChange)
                    {
                        GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                        {
#if AdRevenueAnalytics
                            AdRevenueAnalytics.Instance.SetData(AdRevenueAnalytics.AdType.AppOpen, AdRevenueAnalytics.ScreenName.FocusInOut);
#endif

#if AdsManager_MultiNetwork
                        AdsManager_MultiNetwork.Instance.ShowAppOpen(actionAfterAd: actionAfterAppOpenAdOnlyAppStateChange);
#else
                            if (IsAppOpenAdLoaded()) ShowAppOpenAd(actionAfterAd: actionAfterAppOpenAdOnlyAppStateChange);
#if AdsManager_Applovin
                        else if(AdsManager_Applovin.Instance.IsAppOpenAdLoaded()) AdsManager_Applovin.Instance.ShowAppOpenAd(actionAfterAd: actionAfterAppOpenAdOnlyAppStateChange);
#endif
#endif
                            return;
                        });
                    }
                    else
                    {
                        ShowNativeFullScreen();
                    }
                }
            }
            IsAppOpenCanShow = true;
        }
    }

    #region AppOpen Internal

    public bool IsAppOpenCanShow { get; set; } = true;
    readonly string isAppOpenOrNativeOnAppStateChange = "isAppOpenOrNativeOnAppStateChange";
    readonly string isAppOpenOnAppStateChange = "isAppOpenOnAppStateChange";
    bool isAppOpenOrNative_OnAppStateChange = true;
    bool isAppOpen_OnAppStateChange = true;
    System.Action<bool> appOpenActionAfterAd = null;
    public System.Action<bool> actionAfterAppOpenAdOnlyAppStateChange = null;

    void ShowAppOpenInternal(string specificID = "")
    {
        bool isAdShown = false;
        System.Collections.Generic.List<string> genericList = appOpenID;
        int listCount = appOpenID.Count;
#if AdRevenueAnalytics
        genericList = AdRevenueAnalytics.Instance.GetAdIDsList(AdRevenueAnalytics.AdType.AppOpen);
        genericList ??= appOpenID;
#endif
        AdvertisementType adType = AdvertisementType.AppOpen;

        if (string.IsNullOrEmpty(specificID))
        {
            foreach (string genericID in genericList)
            {
                foreach (var kvp in AdvertisementData)
                {
                    if (kvp.Key.Contains(genericID) && kvp.Value.AdType.Equals(adType) && kvp.Value.IsLoaded)
                    {
                        ShowDebugLog("Show " + adType + ".\nID :" + kvp.Key);
#if MonetizationAnalytics
                    MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_AppOpen, MonetizationAnalytics.ParameterKey.myParameter,
                MonetizationAnalytics.ParameterValue.Showing);
#endif
                        SetLastAdStatus(kvp.Key, adType, appOpenActionAfterAd);
                        kvp.Value.ShowAd();
                        return;
                    }
                }
            }
        }
        else
        {
            foreach (var kvp in AdvertisementData)
            {
                if (kvp.Key.Contains(specificID) && kvp.Value.AdType.Equals(adType) && kvp.Value.IsLoaded)
                {
                    ShowDebugLog("Show " + adType + ".\nID :" + kvp.Key);
#if MonetizationAnalytics
                    MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_AppOpen, MonetizationAnalytics.ParameterKey.myParameter,
                MonetizationAnalytics.ParameterValue.Showing);
#endif
                    SetLastAdStatus(kvp.Key, adType, appOpenActionAfterAd);
                    kvp.Value.ShowAd();
                    return;
                }
            }
        }

#if AdRevenueAnalytics
        ShowDebugLog("No " + adType + " Ads Available\nReason: AdPilot Provided ID's Not Loaded.");
        return;
#endif
        if (!isAdShown)
        {
            foreach (var data in AdvertisementData.Values)
            {
                if (adType.Equals(data.AdType) && data.IsLoaded)
                {
                    ShowDebugLog("Show " + adType + ".\nID :" + data.ID);
#if MonetizationAnalytics
                        MonetizationAnalytics.SendAnalytics(MonetizationAnalytics.EventName.Monetize_AppOpen, MonetizationAnalytics.ParameterKey.myParameter,
                    MonetizationAnalytics.ParameterValue.Showing);
#endif
                    SetLastAdStatus(data.ID, adType, appOpenActionAfterAd);
                    MobileAdsEventExecutor.ExecuteInUpdate(() => data.ShowAd());
                    return;
                }
            }
        }
        if (!isAdShown) ShowDebugLog("No AppOpen Ads Available");
    }
    void UpdateRemote_OnAppStateChange()
    {
#if RemoteConfigManager_Firebase
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isAppOpenOnAppStateChange))
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig(isAppOpenOnAppStateChange, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out isAppOpen_OnAppStateChange);
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isAppOpenOrNativeOnAppStateChange))
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig(isAppOpenOrNativeOnAppStateChange, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out isAppOpenOrNative_OnAppStateChange);
#endif
    }

    #endregion

    #endregion

    #region General FullScreen Ad

    readonly System.Collections.Concurrent.ConcurrentDictionary<string, AdvertisementGeneric> AdvertisementData = new System.Collections.Concurrent.ConcurrentDictionary<string, AdvertisementGeneric>();
    public enum AdvertisementType
    {
        Interstitial = 1,
        InterstitialStatic = 2,
        Rewarded = 3,
        RewardedInterstitial = 4,
        AppOpen = 5
    }
    public abstract class AdvertisementGeneric
    {
        public string ID { get; set; }
        public bool IsLoaded { get; set; }
        public AdvertisementType AdType { get; set; }
        public abstract void Loaded(object ad);
        public abstract void Remove();
        public abstract void ShowAd(System.Action<bool> action = null);
        public abstract void UpdateAd(object ad);
        public abstract object GetAd();
        protected AdvertisementGeneric(string id, AdvertisementType type, bool load = false)
        {
            ID = id; IsLoaded = load; AdType = type;
        }
    }
    public class InterstitialClass : AdvertisementGeneric
    {
        public InterstitialAd Ad { get; private set; }
        public InterstitialClass(string id, AdvertisementType type, bool load = false, InterstitialAd ad = null)
        : base(id, type, load) { Ad = ad; }
        public override void Loaded(object ad)
        {
            if (ad is InterstitialAd interstitialAd) { IsLoaded = true; Ad = interstitialAd; }
            else throw new System.ArgumentException("Invalid ad type passed to Loaded method.");
        }
        public override void Remove()
        {
            try { Ad?.Destroy(); } catch (System.Exception e) { SendExceptionToCrashlytics_Static(e); }
        }
        public override void ShowAd(System.Action<bool> action = null)
        {
            try { MobileAdsEventExecutor.ExecuteInUpdate(() => Ad?.Show()); } catch (System.Exception e) { SendExceptionToCrashlytics_Static(e); }
            action?.Invoke(true);
        }
        public override void UpdateAd(object ad)
        {
            if (ad is InterstitialAd interstitialAd) { Ad = interstitialAd; }
            else throw new System.ArgumentException("Invalid ad type passed to Loaded method.");
        }

        public override object GetAd()
        {
            return Ad;
        }
    }
    public class RewardedClass : AdvertisementGeneric
    {
        public RewardedAd Ad;
        public RewardedClass(string id, AdvertisementType type, bool load = false, RewardedAd ad = null)
        : base(id, type, load) { Ad = ad; }
        public override void Loaded(object ad)
        {
            if (ad is RewardedAd rewardedAd) { IsLoaded = true; Ad = rewardedAd; }
            else throw new System.ArgumentException("Invalid ad type passed to Loaded method.");
        }
        public override void Remove()
        {
            try { Ad?.Destroy(); } catch (System.Exception e) { SendExceptionToCrashlytics_Static(e); }
        }
        public override void ShowAd(System.Action<bool> action = null)
        {
            try { MobileAdsEventExecutor.ExecuteInUpdate(() => Ad?.Show((Reward reward) => { action?.Invoke(true); })); } catch (System.Exception e) { SendExceptionToCrashlytics_Static(e); }
        }
        public override void UpdateAd(object ad)
        {
            if (ad is RewardedAd rewardedAd) { Ad = rewardedAd; }
            else throw new System.ArgumentException("Invalid ad type passed to Loaded method.");
        }
        public override object GetAd()
        {
            return Ad;
        }
    }
    public class RewardedInterstitialClass : AdvertisementGeneric
    {
        public RewardedInterstitialAd Ad;
        public RewardedInterstitialClass(string id, AdvertisementType type, bool load = false, RewardedInterstitialAd ad = null)
         : base(id, type, load) { Ad = ad; }
        public override void Loaded(object ad)
        {
            if (ad is RewardedInterstitialAd rewardedInterstitialAd) { IsLoaded = true; Ad = rewardedInterstitialAd; }
            else throw new System.ArgumentException("Invalid ad type passed to Loaded method.");
        }
        public override void Remove()
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() => Ad?.Destroy());
        }
        public override void ShowAd(System.Action<bool> action = null)
        {
            try { MobileAdsEventExecutor.ExecuteInUpdate(() => Ad?.Show((Reward reward) => { action?.Invoke(true); })); } catch (System.Exception e) { SendExceptionToCrashlytics_Static(e); }
        }
        public override void UpdateAd(object ad)
        {
            if (ad is RewardedInterstitialAd rewardedInterstitialAd) { Ad = rewardedInterstitialAd; }
            else throw new System.ArgumentException("Invalid ad type passed to Loaded method.");
        }
        public override object GetAd()
        {
            return Ad;
        }
    }
    public class AppOpenClass : AdvertisementGeneric
    {
        public AppOpenAd Ad;
        public AppOpenClass(string id, AdvertisementType type, bool load = false, AppOpenAd ad = null)
         : base(id, type, load) { Ad = ad; }
        public override void Loaded(object ad)
        {
            if (ad is AppOpenAd appOpenAd) { IsLoaded = true; Ad = appOpenAd; }
            else throw new System.ArgumentException("Invalid ad type passed to Loaded method.");
        }
        public override void Remove()
        {
            try { Ad?.Destroy(); } catch (System.Exception e) { SendExceptionToCrashlytics_Static(e); }
        }
        public override void ShowAd(System.Action<bool> action = null)
        {
            try { MobileAdsEventExecutor.ExecuteInUpdate(() => Ad?.Show()); } catch (System.Exception e) { SendExceptionToCrashlytics_Static(e); }
            action?.Invoke(true);
        }
        public override void UpdateAd(object ad)
        {
            if (ad is AppOpenAd appOpenAd) { Ad = appOpenAd; }
            else throw new System.ArgumentException("Invalid ad type passed to Loaded method.");
        }
        public override object GetAd()
        {
            return Ad;
        }
    }

    void LoadAd(AdvertisementType adType, string specificID = "")
    {
        System.Collections.Generic.List<string> genericList = new System.Collections.Generic.List<string>();
        System.Action<string, AdvertisementType> deferredFunction = null;

        switch (adType)
        {
            case AdvertisementType.Interstitial:
                {
                    if (string.IsNullOrEmpty(specificID)) genericList = interstitialID;
                    else if (!IsInterstitialLoaded(InterstitialType.interstitial, specificID: specificID)) genericList.Add(specificID);
                    else return;
                    deferredFunction = LoadInterstitialGeneral;
                    break;
                }
            case AdvertisementType.InterstitialStatic:
                {
                    if (string.IsNullOrEmpty(specificID)) genericList = interstitialStaticID;
                    else if (!IsInterstitialLoaded(InterstitialType.interstitialStatic, specificID: specificID)) genericList.Add(specificID);
                    else return;
                    deferredFunction = LoadInterstitialGeneral;
                    break;
                }
            case AdvertisementType.Rewarded:
                {
                    if (string.IsNullOrEmpty(specificID)) genericList = rewardedID;
                    else if (!IsRewardedLoaded(RewardedType.rewardedVideo, specificID: specificID)) genericList.Add(specificID);
                    else return;
                    deferredFunction = LoadRewardedGeneral;
                    break;
                }
            case AdvertisementType.RewardedInterstitial:
                {
                    if (string.IsNullOrEmpty(specificID)) genericList = rewardedInterstitialID;
                    else if (!IsRewardedLoaded(RewardedType.rewardedInterstitial, specificID: specificID)) genericList.Add(specificID);
                    else return;
                    deferredFunction = LoadRewardedInterstitialGeneral;
                    break;
                }
            case AdvertisementType.AppOpen:
                {
                    if (string.IsNullOrEmpty(specificID)) genericList = appOpenID;
                    else if (!IsAppOpenAdLoaded(specificID: specificID)) genericList.Add(specificID);
                    else return;
                    deferredFunction = LoadAppOpenGeneral;
                    break;
                }
        }
        for (int i = 0; i < genericList.Count; i++)
        {
            string currentID = genericList[i];
            if (string.IsNullOrEmpty(currentID) || (!IsValidID(ref currentID, adType.ToString()))) continue;

            currentID = adType + "_" + i + "_:" + currentID;
            deferredFunction?.Invoke(currentID, adType);
        }
    }

    void LoadInterstitialGeneral(string key, AdvertisementType type = AdvertisementType.Interstitial)
    {
        if (IsRemoveAd()) { ShowDebugLog("No " + type + " Load.\nReason: Remove Ads"); return; }

        var splittingArray = key.Split(':');
        if (splittingArray.Length <= 1 || !splittingArray[1].StartsWith("ca-app-pub-"))
        {
            Debug.LogError("Invalid Key Found against " + type + ".\nID : " + key);
            return;
        }
        string adUnitID = splittingArray[1];

        if (AdvertisementData.ContainsKey(key)) return;
        AdvertisementData[key] = new InterstitialClass(id: key, load: false, type: type);

        InterstitialAd.Load(adUnitID, new AdRequest(), (InterstitialAd ad, LoadAdError loadError) =>
        {
            if (loadError != null || ad == null)
            {
                ShowDebugLog(type + " Failed To Load.\nID: " + AdvertisementData[key].ID+ "\nReason: " + loadError.GetMessage());
                SendAnalytics(EventName.Ad_Interstitial, ParameterKey.myParameter, ParameterValue.AdRequestFail);

                if (AdvertisementData.ContainsKey(key)) AdvertisementData.TryRemove(key, out _);

                if (isAutoReload && isInterstitialReloadAfterShow)
                {
                    MainThreadQueue.Enqueue(() =>
                    {
                        ActionsPerformWithDelay(new System.Action(() => { LoadInterstitialGeneral(key: key, type: type); }), interstitialAutoReloadTime);
                        interstitialAutoReloadTime = Mathf.Min(interstitialAutoReloadTime += interstitialAutoReloadTime, maxReloadAdTime);
                    });
                }
                return;
            }

            if (AdvertisementData.TryGetValue(key, out var data)) data.Loaded(ad);
            ShowDebugLog(type + " Loaded.\nID: " + AdvertisementData[key].ID);
            SendAnalytics(EventName.Ad_Interstitial, ParameterKey.myParameter, ParameterValue.AdRequestSuccess);
            interstitialAutoReloadTime = autoReloadAdTime;

            ad.OnAdFullScreenContentClosed += () =>
            {
                IsAppOpenCanShow = false;
                MainThreadQueue.Enqueue(() =>
                {
                    System.Action action = () => { interstitialActionAfterAd?.Invoke(true); interstitialActionAfterAd = null; };
                    ActionsPerformWithDelay(action, 0.2f);
                });
                ShowDebugLog(type + " Closed.\nID: " + AdvertisementData[key].ID);
                AdvertisementData.TryRemove(key, out _);

                if (isAutoReload && isInterstitialReloadAfterShow)
                {
                    MainThreadQueue.Enqueue(() =>
                    {
                        LoadInterstitialGeneral(key: key, type: type);
                    });
                }
                ResetAdStatus();
                SendAnalytics(EventName.Ad_Interstitial, ParameterKey.myParameter, ParameterValue.AdComplete);
            };
            ad.OnAdFullScreenContentOpened += () =>
            {
#if AdRevenueAnalytics
                MainThreadQueue.Enqueue(() =>
                {
                    AdRevenueAnalytics.Instance.SendRevenueAnalytics(AdRevenueAnalytics.AdType.Interstitial, AdRevenueAnalytics.AdRevenueEventType.Ad_Rev_Progression, status: AdRevenueAnalytics.Status.Showing);
                });
#endif
                IsAppOpenCanShow = false;
                SendAnalytics(EventName.Ad_Interstitial, ParameterKey.myParameter, ParameterValue.AdShow);
                SendAnalytics(EventName.Ad_Interstitial.ToString() + "_Show");
                HideBannerBG(AdSize.Banner);
            };
            ad.OnAdClicked += () => { IsAppOpenCanShow = false; };
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                ShowDebugLog(type + " Failed To Show." + error.GetMessage() + "\nID: " + AdvertisementData[key].ID+"\nReason: "+error.GetMessage());
                AdvertisementData.TryRemove(key, out _);
                MainThreadQueue.Enqueue(() =>
                {
                    HideLoadingAdBG();
                    System.Action action = () => { interstitialActionAfterAd?.Invoke(false); interstitialActionAfterAd = null; };
                    ActionsPerformWithDelay(action, 0.2f);
                    AfterLoadingBGBannerToShow();
                    Time.timeScale = lastTimeScale;
                    IsAdmobShowingFullScreenAd = false;
                });
                if (isAutoReload && isInterstitialReloadAfterShow)
                {
                    MainThreadQueue.Enqueue(() =>
                    {
                        ActionsPerformWithDelay(new System.Action(() => { LoadInterstitialGeneral(key: key, type: type); }), interstitialAutoReloadTime);
                        interstitialAutoReloadTime = Mathf.Min(interstitialAutoReloadTime += interstitialAutoReloadTime, maxReloadAdTime);
                    });
                }
                ResetAdStatus();
                SendAnalytics(EventName.Ad_Interstitial, ParameterKey.myParameter, ParameterValue.AdRequestFail);
            };
            ad.OnAdPaid += (AdValue e) =>
            {
                SendAnalytics(EventName.Ad_Interstitial, ParameterKey.ECPM, e.Value);
#if SingularSDKManager
                MainThreadQueue.Enqueue(() =>SingularSDKManager.Instance.SendEvent_AdRevenue(SingularSDKManager.NetworkType.Admob, e.CurrencyCode, e.Value));
#endif

#if AdRevenueAnalytics                
                AdRevenueAnalytics.AdType adType = type == AdvertisementType.Interstitial ? AdRevenueAnalytics.AdType.Interstitial : AdRevenueAnalytics.AdType.InterstitialStatic;
                SendRevenueAnalytics(adType, e, ad?.GetAdUnitID(), ad?.GetResponseInfo()?.GetMediationAdapterClassName(), ad?.GetType().Name);
#endif
                AdImpressionCustomEvent(e.Value);
            };
        });
        ShowDebugLog(type + " Request Send.\nID: " + key);
        SendAnalytics(EventName.Ad_Interstitial, ParameterKey.myParameter, ParameterValue.AdRequestSend);
    }

    void LoadRewardedGeneral(string key, AdvertisementType type = AdvertisementType.Rewarded)
    {
        var splittingArray = key.Split(':');
        if (splittingArray.Length <= 1 || !splittingArray[1].StartsWith("ca-app-pub-"))
        {
            Debug.LogError("Invalid Key Found against " + type + ".\nID : " + key); return;
        }
        string adUnitID = splittingArray[1];

        if (AdvertisementData.ContainsKey(key)) return;
        AdvertisementData[key] = new RewardedClass(id: key, load: false, type: type);

        RewardedAd.Load(adUnitID, new AdRequest(), (RewardedAd ad, LoadAdError loadError) =>
        {
            if (loadError != null || ad == null)
            {
                ShowDebugLog(type + " Failed To Load.\nID: " + AdvertisementData[key].ID+ "\nReason: " + loadError.GetMessage());
                SendAnalytics(EventName.Ad_RewardedVideo, ParameterKey.myParameter, ParameterValue.AdRequestFail);
                if (AdvertisementData.ContainsKey(key)) AdvertisementData.TryRemove(key, out _);

                if (isAutoReload && isRewardedReloadAfterShow)
                {
                    MainThreadQueue.Enqueue(() =>
                    {
                        ActionsPerformWithDelay(new System.Action(() => { LoadRewardedGeneral(key: key, type: type); }), rewardedAutoReloadTime);
                        rewardedAutoReloadTime = Mathf.Min(rewardedAutoReloadTime += rewardedAutoReloadTime, maxReloadAdTime);
                    });
                }
                return;
            }

            if (AdvertisementData.TryGetValue(key, out var data)) data.Loaded(ad);
            ShowDebugLog(type + " Loaded.\nID: " + AdvertisementData[key].ID);
            SendAnalytics(EventName.Ad_RewardedVideo, ParameterKey.myParameter, ParameterValue.AdRequestSuccess);
            rewardedAutoReloadTime = autoReloadAdTime;

            ad.OnAdFullScreenContentClosed += () =>
            {
                IsAppOpenCanShow = false;
                ShowDebugLog(type + " Closed.\nID: " + AdvertisementData[key].ID);
                AdvertisementData.TryRemove(key, out _);

                if (isAutoReload && isRewardedReloadAfterShow)
                {
                    MainThreadQueue.Enqueue(() =>
                    {
                        LoadRewardedGeneral(key: key, type: type);
                    });
                }
                ResetAdStatus();
                SendAnalytics(EventName.Ad_RewardedVideo, ParameterKey.myParameter, ParameterValue.AdComplete);
            };
            ad.OnAdFullScreenContentOpened += () =>
            {
#if AdRevenueAnalytics
                MainThreadQueue.Enqueue(() =>
                {
                    AdRevenueAnalytics.Instance.SendRevenueAnalytics(AdRevenueAnalytics.AdType.Rewarded, AdRevenueAnalytics.AdRevenueEventType.Ad_Rev_Progression, status: AdRevenueAnalytics.Status.Showing);
                });
#endif
                IsAppOpenCanShow = false;
                SendAnalytics(EventName.Ad_RewardedVideo, ParameterKey.myParameter, ParameterValue.AdShow);
                SendAnalytics(EventName.Ad_RewardedVideo.ToString() + "_Show");
                HideBannerBG(AdSize.Banner);
            };
            ad.OnAdClicked += () => { IsAppOpenCanShow = false; };
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                ShowDebugLog(type + " Failed To Show.\nID: " + AdvertisementData[key].ID+ "\nReason: " + error.GetMessage());
                AdvertisementData.TryRemove(key, out _);
                MainThreadQueue.Enqueue(() =>
                {
                    HideLoadingAdBG();
                    System.Action action = () => { actionGetReward?.Invoke(false); actionGetReward = null; };
                    ActionsPerformWithDelay(action, 0.2f);
                    AfterLoadingBGBannerToShow();
                    Time.timeScale = lastTimeScale;
                    IsAdmobShowingFullScreenAd = false;
                });
                if (isAutoReload && isRewardedReloadAfterShow)
                {
                    MainThreadQueue.Enqueue(() =>
                    {
                        ActionsPerformWithDelay(new System.Action(() => { LoadRewardedGeneral(key: key, type: type); }), rewardedAutoReloadTime);
                        rewardedAutoReloadTime = Mathf.Min(rewardedAutoReloadTime += rewardedAutoReloadTime, maxReloadAdTime);
                    });
                }
                ResetAdStatus();
                SendAnalytics(EventName.Ad_RewardedVideo, ParameterKey.myParameter, ParameterValue.AdRequestFail);
            };
            ad.OnAdPaid += (AdValue e) =>
            {
                SendAnalytics(EventName.Ad_RewardedVideo, ParameterKey.ECPM, e.Value);
#if SingularSDKManager
                MainThreadQueue.Enqueue(() =>SingularSDKManager.Instance.SendEvent_AdRevenue(SingularSDKManager.NetworkType.Admob, e.CurrencyCode, e.Value));
#endif

#if AdRevenueAnalytics
                SendRevenueAnalytics(AdRevenueAnalytics.AdType.Rewarded, e, ad?.GetAdUnitID(), ad?.GetResponseInfo()?.GetMediationAdapterClassName(), ad?.GetType().Name);
#endif

                AdImpressionCustomEvent(e.Value);
            };
        });
        ShowDebugLog(type + " Request Send.\nID: " + key);
        SendAnalytics(EventName.Ad_RewardedVideo, ParameterKey.myParameter, ParameterValue.AdRequestSend);
    }

    void LoadRewardedInterstitialGeneral(string key, AdvertisementType type = AdvertisementType.RewardedInterstitial)
    {
        var splittingArray = key.Split(':');
        if (splittingArray.Length <= 1 || !splittingArray[1].StartsWith("ca-app-pub-"))
        {
            Debug.LogError("Invalid Key Found against " + type + ".\nID : " + key); return;
        }
        string adUnitID = splittingArray[1];

        if (AdvertisementData.ContainsKey(key)) return;
        AdvertisementData[key] = new RewardedInterstitialClass(id: key, load: false, type: type);

        RewardedInterstitialAd.Load(adUnitID, new AdRequest(), (RewardedInterstitialAd ad, LoadAdError loadError) =>
        {
            if (loadError != null || ad == null)
            {
                ShowDebugLog(type + " Failed To Load.\nID: " + AdvertisementData[key].ID+ "\nReason: " + loadError.GetMessage());
                SendAnalytics(EventName.Ad_RewardedInterstitial, ParameterKey.myParameter, ParameterValue.AdRequestFail);
                if (AdvertisementData.ContainsKey(key)) AdvertisementData.TryRemove(key, out _);

                if (isAutoReload && isRewardedReloadAfterShow)
                {
                    MainThreadQueue.Enqueue(() =>
                    {
                        ActionsPerformWithDelay(new System.Action(() => { LoadRewardedInterstitialGeneral(key: key, type: type); }), rewardedAutoReloadTime);
                        rewardedAutoReloadTime = Mathf.Min(rewardedAutoReloadTime += rewardedAutoReloadTime, maxReloadAdTime);
                    });
                }
                return;
            }

            if (AdvertisementData.TryGetValue(key, out var data)) data.Loaded(ad);
            ShowDebugLog(type + " Loaded.\nID: " + AdvertisementData[key].ID);
            SendAnalytics(EventName.Ad_RewardedInterstitial, ParameterKey.myParameter, ParameterValue.AdRequestSuccess);
            rewardedAutoReloadTime = autoReloadAdTime;

            ad.OnAdFullScreenContentClosed += () =>
            {
                IsAppOpenCanShow = false;
                ShowDebugLog(type + " Closed.\nID: " + AdvertisementData[key].ID);
                AdvertisementData.TryRemove(key, out _);

                if (isAutoReload && isRewardedReloadAfterShow)
                {
                    MainThreadQueue.Enqueue(() =>
                    {
                        LoadRewardedInterstitialGeneral(key: key, type: type);
                    });
                }
                ResetAdStatus();
                SendAnalytics(EventName.Ad_RewardedInterstitial, ParameterKey.myParameter, ParameterValue.AdComplete);
            };
            ad.OnAdFullScreenContentOpened += () =>
            {
#if AdRevenueAnalytics
                MainThreadQueue.Enqueue(() =>
                {
                    AdRevenueAnalytics.Instance.SendRevenueAnalytics(AdRevenueAnalytics.AdType.Rewarded, AdRevenueAnalytics.AdRevenueEventType.Ad_Rev_Progression, status: AdRevenueAnalytics.Status.Showing);
                });
#endif
                IsAppOpenCanShow = false;
                SendAnalytics(EventName.Ad_RewardedInterstitial, ParameterKey.myParameter, ParameterValue.AdShow);
                SendAnalytics(EventName.Ad_RewardedInterstitial.ToString() + "_Show");
                HideBannerBG(AdSize.Banner);
            };
            ad.OnAdClicked += () => { IsAppOpenCanShow = false; };
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                ShowDebugLog(type + " Failed To Show.\nID: " + AdvertisementData[key].ID + "\nReason: " + error.GetMessage());
                AdvertisementData.TryRemove(key, out _);
                MainThreadQueue.Enqueue(() =>
                {
                    HideLoadingAdBG();
                    System.Action action = () => { actionGetReward?.Invoke(false); actionGetReward = null; };
                    ActionsPerformWithDelay(action, 0.2f);
                    AfterLoadingBGBannerToShow();
                    Time.timeScale = lastTimeScale;
                    IsAdmobShowingFullScreenAd = false;
                });
                if (isAutoReload && isRewardedReloadAfterShow)
                {
                    MainThreadQueue.Enqueue(() =>
                    {
                        ActionsPerformWithDelay(new System.Action(() => { LoadRewardedInterstitialGeneral(key: key, type: type); }), rewardedAutoReloadTime);
                        rewardedAutoReloadTime += rewardedAutoReloadTime;
                        rewardedAutoReloadTime = Mathf.Min(rewardedAutoReloadTime, maxReloadAdTime);
                    });
                }
                ResetAdStatus();
                SendAnalytics(EventName.Ad_RewardedInterstitial, ParameterKey.myParameter, ParameterValue.AdRequestFail);
            };
            ad.OnAdPaid += (AdValue e) =>
            {
                SendAnalytics(EventName.Ad_RewardedInterstitial, ParameterKey.ECPM, e.Value);
#if SingularSDKManager
                MainThreadQueue.Enqueue(() =>SingularSDKManager.Instance.SendEvent_AdRevenue(SingularSDKManager.NetworkType.Admob, e.CurrencyCode, e.Value));
#endif

#if AdRevenueAnalytics
                SendRevenueAnalytics(AdRevenueAnalytics.AdType.RewardedInterstitial, e, ad?.GetAdUnitID(), ad?.GetResponseInfo()?.GetMediationAdapterClassName(), ad?.GetType().Name);
#endif
                AdImpressionCustomEvent(e.Value);
            };
        });
        ShowDebugLog(type + " Request Send.\nID: " + key);
        SendAnalytics(EventName.Ad_RewardedInterstitial, ParameterKey.myParameter, ParameterValue.AdRequestSend);
    }

    void LoadAppOpenGeneral(string key, AdvertisementType type = AdvertisementType.AppOpen)
    {
        if (IsRemoveAd()) { ShowDebugLog("No " + type + " Shown.\nReason: Remove Ads."); return; }

        var splittingArray = key.Split(':');
        if (splittingArray.Length <= 1 || !splittingArray[1].StartsWith("ca-app-pub-"))
        {
            Debug.LogError("Invalid Key Found against " + type + ".\nID : " + key); return;
        }
        string adUnitID = splittingArray[1];

        if (AdvertisementData.ContainsKey(key)) return;
        AdvertisementData[key] = new AppOpenClass(id: key, load: false, type: type);

        AppOpenAd.Load(adUnitID, new AdRequest(), (AppOpenAd ad, LoadAdError loadError) =>
        {
            if (loadError != null || ad == null)
            {
                ShowDebugLog(type + " Failed To Load.\nID: " + AdvertisementData[key].ID+ "\nReason: " + loadError.GetMessage());
                SendAnalytics(EventName.Ad_AppOpen, ParameterKey.myParameter, ParameterValue.AdRequestFail);
                if (AdvertisementData.ContainsKey(key)) AdvertisementData.TryRemove(key, out _);

                if (isAutoReload && isAppOpenReloadAfterShow)
                {
                    MainThreadQueue.Enqueue(() =>
                    {
                        ActionsPerformWithDelay(new System.Action(() => { LoadAppOpenGeneral(key: key, type: type); }), appOpenAutoReloadTime);
                        appOpenAutoReloadTime = Mathf.Min(appOpenAutoReloadTime += appOpenAutoReloadTime, maxReloadAdTime);
                    });
                }
                return;
            }

            if (AdvertisementData.TryGetValue(key, out var data)) data.Loaded(ad);
            ShowDebugLog(type + " Loaded.\nID: " + AdvertisementData[key].ID);
            SendAnalytics(EventName.Ad_AppOpen, ParameterKey.myParameter, ParameterValue.AdRequestSuccess);
            appOpenAutoReloadTime = autoReloadAdTime;

            ad.OnAdFullScreenContentClosed += () =>
            {
                IsAppOpenCanShow = false;
                MainThreadQueue.Enqueue(() =>
                {
                    System.Action action = () => { appOpenActionAfterAd?.Invoke(true); appOpenActionAfterAd = null; };
                    ActionsPerformWithDelay(action, 0.2f);
                });
                ShowDebugLog(type + " Closed.\nID: " + AdvertisementData[key].ID);
                AdvertisementData.TryRemove(key, out _);
                if (isAutoReload && isAppOpenReloadAfterShow) { MainThreadQueue.Enqueue(() => { LoadAppOpenGeneral(key: key, type: type); }); }
                ResetAdStatus();
                SendAnalytics(EventName.Ad_AppOpen, ParameterKey.myParameter, ParameterValue.AdComplete);
            };
            ad.OnAdFullScreenContentOpened += () =>
            {
#if AdRevenueAnalytics
                MainThreadQueue.Enqueue(() =>
                {
                    AdRevenueAnalytics.Instance.SendRevenueAnalytics(AdRevenueAnalytics.AdType.AppOpen, AdRevenueAnalytics.AdRevenueEventType.Ad_Rev_Progression, status: AdRevenueAnalytics.Status.Showing);
                });
#endif
                SendAnalytics(EventName.Ad_AppOpen, ParameterKey.myParameter, ParameterValue.AdShow);
                SendAnalytics(EventName.Ad_AppOpen.ToString() + "_Show");
                HideBannerBG(AdSize.Banner);
            };
            ad.OnAdClicked += () => { IsAppOpenCanShow = false; };
            ad.OnAdFullScreenContentFailed += (AdError error) =>
            {
                ShowDebugLog(type + " Failed To Show.\nID: " + AdvertisementData[key].ID+ "\nReason: " + error.GetMessage());
                AdvertisementData.TryRemove(key, out _);
                MainThreadQueue.Enqueue(() =>
                {
                    HideLoadingAdBG();
                    System.Action action = () => { appOpenActionAfterAd?.Invoke(false); appOpenActionAfterAd = null; };
                    ActionsPerformWithDelay(action, 0.2f);
                    AfterLoadingBGBannerToShow();
                    Time.timeScale = lastTimeScale;
                    IsAdmobShowingFullScreenAd = false;
                });
                if (isAutoReload && isAppOpenReloadAfterShow)
                {
                    MainThreadQueue.Enqueue(() =>
                    {
                        ActionsPerformWithDelay(new System.Action(() => { LoadAppOpenGeneral(key: key, type: type); }), appOpenAutoReloadTime);
                        appOpenAutoReloadTime = Mathf.Min(appOpenAutoReloadTime += appOpenAutoReloadTime, maxReloadAdTime);
                    });
                }
                ResetAdStatus();
                SendAnalytics(EventName.Ad_AppOpen, ParameterKey.myParameter, ParameterValue.AdRequestFail);
            };
            ad.OnAdPaid += (AdValue e) =>
            {
                SendAnalytics(EventName.Ad_AppOpen, ParameterKey.ECPM, e.Value);
#if SingularSDKManager
                MainThreadQueue.Enqueue(() =>SingularSDKManager.Instance.SendEvent_AdRevenue(SingularSDKManager.NetworkType.Admob, e.CurrencyCode, e.Value));
#endif

#if AdRevenueAnalytics
                SendRevenueAnalytics(AdRevenueAnalytics.AdType.AppOpen, e, ad?.GetAdUnitID(), ad?.GetResponseInfo()?.GetMediationAdapterClassName(), ad?.GetType().Name);
#endif
                AdImpressionCustomEvent(e.Value);
            };
        });
        ShowDebugLog(type + " Request Send.\nID: " + key);
        SendAnalytics(EventName.Ad_AppOpen, ParameterKey.myParameter, ParameterValue.AdRequestSend);
    }

    void RemoveAdvertisementData(string key)
    {

    }

    #region LastAdStatus
    // Bug Resovle if we close FullScreenAd by Home button Click, then last Ad Not Closed properly

    static LastAdStatus lastAdStatus;
    readonly bool isCheckAdLastStatusToCallAction = true;
    public class LastAdStatus
    {
        public string adID;
        public AdvertisementType adType;
        public System.Action<bool> actionAfterAdClose;

        public LastAdStatus(string id, AdvertisementType type, System.Action<bool> adCloseAction)
        {
            adID = id; adType = type; actionAfterAdClose = adCloseAction;
        }
    }
    public void SetLastAdStatus(string id, AdvertisementType type, System.Action<bool> adCloseAction)
    {
        lastAdStatus ??= new LastAdStatus(id, type, adCloseAction);
        lastAdStatus.adID = id;
        lastAdStatus.adType = type;
        lastAdStatus.actionAfterAdClose = adCloseAction;
    }
    public void ResetAdStatus()
    {
        lastAdStatus.adID = "";
        lastAdStatus.adType = AdvertisementType.RewardedInterstitial;
        lastAdStatus.actionAfterAdClose = null;
    }
    public void CheckAndCallAdStatus()
    {
        if (!isCheckAdLastStatusToCallAction) return;
        ShowDebugLog("CheckAndCallAdStatus Trigger.\nReason : " + lastAdStatus.adType + " Close Handler fail to Invoked.");
        if (lastAdStatus.adID != "")
        {
            if (AdvertisementData.ContainsKey(lastAdStatus.adID))
            {
                AdvertisementData.TryRemove(lastAdStatus.adID, out _);
                lastAdStatus.actionAfterAdClose?.Invoke(true);
                ShowDebugLog("CheckAndCallAdStatus Invoked & Clear Container.\n" +
                    "AdType :" + lastAdStatus.adType + "\n" +
                    "ID :" + lastAdStatus.adID);
                ResetAdStatus();
                return;
            }
            ShowDebugLog("CheckAndCallAdStatus Do Nothing.\n" +
                "Reason\nAdType :" + lastAdStatus.adType + "\n" +
                "ID :" + lastAdStatus.adID + "\n" +
                "Not Contain in Available Container");
        }
    }

    #endregion

    #endregion

    #region Additional Support

    #region Built-in Functions

#if UNITY_IOS
    bool lastPauseState = false;
#endif
    void OnApplicationPause(bool pause)
    {
#if !UNITY_EDITOR

#if UNITY_IOS
        if(lastPauseState == pause) return;
        lastPauseState = pause;
#endif

        if (pause)
        {
            lastTimeScale = Time.timeScale;
            Time.timeScale = 0;
            lastAudioListenerState = AudioListener.pause;
            AudioListener.pause = pause;
        }
        else
        {
            if (LoadingAdBG != null)
            {
                if (LoadingAdBG.activeSelf)
                {
                    IsAppOpenCanShow = true;
                    if (IsAdmobShowingFullScreenAd) { Time.timeScale = lastTimeScale; AudioListener.pause = lastAudioListenerState; return; }
#if UTM_Class
                    if (UTM.UTM_Handler.IsUTMShowingFullScreenAd) return;
#endif
                    HideLoadingAdBG();
                    System.Action actions = new System.Action(() =>
                   {
                       AfterLoadingBGBannerToShow();
                       Time.timeScale = lastTimeScale;
                   });
                    ActionsPerformWithDelay(actions, 0.1f);
                    if (lastAdStatus != null)
                    {
                        if (lastAdStatus.adID != "")
                        {
                            System.Action actionForLastAdStatus = new System.Action(() =>
                            {
                                CheckAndCallAdStatus();
                            });
                            ActionsPerformWithDelay(actionForLastAdStatus, 0.5f);
                        }
                    }
                }
                else
                {
                    ActionsPerformWithDelay(new System.Action(() => { OnAppStateChanged(); }), 0.3f);
                    Time.timeScale = lastTimeScale;
                }
            }
            AudioListener.pause = lastAudioListenerState;
        }
#endif
    }

#if !UNITY_EDITOR
            private void OnApplicationFocus(bool focus)
#else
    public void Focus(bool focus)
#endif
    {
        if (focus)
        {
            if (afterLoadingBannerShowAction != null || afterLoadingBannerShowCustomAction != null) return;

            if (isSmallBannerShow)
            {
                if (lastBannerSizeState.Equals(BannerSizeState.DefaultBannerSize)) ShowBanner(BannerType.SmallBannerType, lastSmallBannerPosition);
            }
            else if (isSmallBannerGamePlayShow)
            {
                if (lastBannerSizeState.Equals(BannerSizeState.DefaultBannerSize)) ShowBanner(BannerType.SmallBannerGamePlayType, lastSmallBannerGamePlayPosition);
            }
            else if (isLargeBannerShow)
            {
                if (lastBannerSizeState.Equals(BannerSizeState.DefaultBannerSize)) ShowBanner(BannerType.LargeBannerType, lastLargeBannerPosition);
            }
            else if (isAdaptiveBannerShow)
            {
                if (lastBannerSizeState.Equals(BannerSizeState.DefaultBannerSize)) ShowBanner(BannerType.AdaptiveBannerType, lastAdaptiveBannerPosition);
            }
            else if (isSmartBannerShow)
            {
                if (lastBannerSizeState.Equals(BannerSizeState.DefaultBannerSize)) ShowBanner(BannerType.SmartBannerType, lastSmartBannerPosition);
            }
            else if (isCustomBannerShow)
            {
                ShowBanner(BannerType.CustomBannerType, lastCustomBannerRectTransform);
            }

        }
        else
        {
            foreach (var currentBannerType in bannersList)
            {
                if (IsBannerShowing(currentBannerType)) HideBannerInternal(currentBannerType, true);
            }
        }
    }

    void OnDisable()
    {
        StopAllCoroutines();
        try
        {
#if RemoteConfigManager_Firebase
            RemoteConfigManager_Firebase.Instance.FetchCompleteEvent -= RemoteConfigSuccessTrigger;
#endif
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
    }

    void OnEnable()
    {
#if RemoteConfigManager_Firebase
        AddRemoteValueToRemoteConfig();
        try
        {
            RemoteConfigManager_Firebase.Instance.FetchCompleteEvent += RemoteConfigSuccessTrigger;
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
#endif
        isSendAnalyticsToCrashlytics_Static = isSendAnalyticsToCrashlytics;
    }

    bool _isQuitting = false;

    void OnApplicationQuit() { _isQuitting = true; SafeDisposeAllBanners(); SafeDisposeAllNatives(); }

    #endregion

    #region Remote Config
    void RemoteConfigSuccessTrigger()
    {
        ShowDebugLog("RemoteConfig Triggered.");
        CheckNewIDReceivedFromRemote();
        CheckMemoryThreshold();
        CheckANRSupervisor();
        CheckDebugLog();
        try
        {
            NativeFullScreenAdCustom nativeFullScreenAdCustom = NativeFullScreen?.GetComponent<NativeFullScreenAdCustom>();
            if (nativeFullScreenAdCustom != null) nativeFullScreenAdCustom.FetchData();
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
        UpdateRemote_OnAppStateChange();
        UpdateRemote_NativeData();
        UpdateRemote_Others();
        if (CheckInitialization()) LoadInitialAds();
        else StartCoroutine(LoadInitialAdIenum());
    }
    System.Collections.IEnumerator LoadInitialAdIenum()
    {
        WaitForSeconds delay = new WaitForSeconds(1);
        for (int i = 0; i < 10; i++)
        {
            yield return delay;
            if (CheckInitialization()) { LoadInitialAds(); yield break; }
        }
    }

    #region Advertisement ID's

    #region ID's
    [SerializeField] const string interstitialAdIDRemoteValue = "interstitialAdIDRemoteValue";
    [SerializeField] const string interstitialStaticAdIDRemoteValue = "interstitialStaticAdIDRemoteValue";
    [SerializeField] const string rewardedAdIDRemoteValue = "rewardedAdIDRemoteValue";
    [SerializeField] const string rewardedInterstitialAdIDRemoteValue = "rewardedInterstitialAdIDRemoteValue";
    [SerializeField] const string appOpenAdIDRemoteValue = "appOpenAdIDRemoteValue";

    [SerializeField] const string smallBannerAdIDRemoteValue = "smallBannerAdIDRemoteValue";
    [SerializeField] const string smallBannerGamePlayAdIDRemoteValue = "smallBannerGamePlayAdIDRemoteValue";
    [SerializeField] const string largeBannerAdIDRemoteValue = "largeBannerAdIDRemoteValue";
    [SerializeField] const string adaptiveBannerAdIDRemoteValue = "adaptiveBannerAdIDRemoteValue";
    [SerializeField] const string smartBannerAdIDRemoteValue = "smartBannerAdIDRemoteValue";
    [SerializeField] const string nativeOverlayAdIDRemoteValue = "nativeOverlayAdIDRemoteValue";
    [SerializeField] const string customBannerAdIDRemoteValue = "customBannerAdIDRemoteValue";
    [SerializeField] const string nativeOverlayFullScreenAdIDRemoteValue = "nativeOverlayFullScreenAdIDRemoteValue";
    readonly string[] remoteValueAdIDsList =
        {
            interstitialAdIDRemoteValue ,
            interstitialStaticAdIDRemoteValue ,
            rewardedAdIDRemoteValue ,
            rewardedInterstitialAdIDRemoteValue ,
            appOpenAdIDRemoteValue,
            smallBannerAdIDRemoteValue,
            smallBannerGamePlayAdIDRemoteValue,
            largeBannerAdIDRemoteValue,
            adaptiveBannerAdIDRemoteValue,
            smartBannerAdIDRemoteValue,
            nativeOverlayAdIDRemoteValue,
            customBannerAdIDRemoteValue,
            nativeOverlayFullScreenAdIDRemoteValue,
        };
    #endregion

    void CheckNewIDReceivedFromRemote()
    {
        if (isTestMode) { ShowDebugLog("Test Mode Enabled. No Remote Ad IDs Override."); return; }

        string[] receivedData =
        {
                    interstitialAdIDRemoteValue ,
                    interstitialStaticAdIDRemoteValue ,
                    rewardedAdIDRemoteValue ,
                    rewardedInterstitialAdIDRemoteValue ,
                    appOpenAdIDRemoteValue,
                    smallBannerAdIDRemoteValue,
                    smallBannerGamePlayAdIDRemoteValue,
                    largeBannerAdIDRemoteValue,
                    adaptiveBannerAdIDRemoteValue,
                    smartBannerAdIDRemoteValue,
                    nativeOverlayAdIDRemoteValue,
                    nativeOverlayFullScreenAdIDRemoteValue,
                    customBannerAdIDRemoteValue
                };
        System.Collections.Generic.List<string>[] currentList =
        {
                    interstitialID,
                    interstitialStaticID,
                    rewardedID,
                    rewardedInterstitialID,
                    appOpenID,
                    smallBannerID,
                    smallBannerGamePlayID,
                    largeBannerID,
                    adaptiveBannerID,
                    smartBannerID,
                    nativeID,
                    nativeFullScreenID,
                    customBannerID
                };

        string outputData = "RemoteConfig Ad ID's Update Data\n\n";
#if RemoteConfigManager_Firebase
        for (int i = 0; i < receivedData.Length; i++)
        {
            if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(receivedData[i]))
            {
                RemoteConfigManager_Firebase.Instance.GetRemoteConfig<string>(receivedData[i], RemoteConfigManager_Firebase.RemoteConfig_Type.String, out string result);
                if (string.IsNullOrEmpty(result)) { outputData += "No Remote Config Data Found against :" + receivedData[i] + "\n\n"; continue; }

                try
                {
                    AdvertisementID advertisement = JsonUtility.FromJson<AdvertisementID>(result);
                    if (advertisement != null && advertisement.ReceiveData != null && advertisement.ReceiveData.IDs.Count > 0)
                    {
                        if (advertisement.ReceiveData.IDs[0].StartsWith("ca-app-pub-")) currentList[i].Clear();
                        outputData += receivedData[i] + " Add ID's:\n";
                        foreach (string key in advertisement.ReceiveData.IDs)
                        {
                            if (key.StartsWith("ca-app-pub-")) { currentList[i].Add(key); outputData += key + "\n"; }
                        }
                        outputData += "\n";
                    }
                    else
                    {
#if AdRevenueAnalytics
                        AdRevenueAnalytics.Instance.ReceiveAdsDataFromJSON(result, GetAdType_AdRevenueAnalytics(receivedData[i]));
#endif
                    }
                }
                catch (System.Exception e) { outputData += "Invalid Data '" + receivedData[i] + "' From Remote Config.\nData : " + result + "\n\n"; SendExceptionToCrashlytics(e); }
            }
            else { outputData += "No Remote Config Data Found: " + receivedData[i] + "\n\n"; }
        }
#endif
        ShowDebugLog(outputData);
    }

    public void SetNewAdIDsFromRemoteConfig(string adType, string newAdID)
    {
        switch (adType)
        {
            case "SmallBanner":
                {
                    for (int i = 0; i < smallBannerID.Count; i++)
                        smallBannerID[i] = newAdID;
                    isSmallBannerIdAvailable = true;
                    break;
                }
            case "LargeBanner":
                {
                    for (int i = 0; i < largeBannerID.Count; i++)
                        largeBannerID[i] = newAdID;
                    isLargeBannerIdAvailable = true;
                    break;
                }
            case "Interstitial":
                {
                    for (int i = 0; i < interstitialID.Count; i++)
                        interstitialID[i] = newAdID;
                    isInterstitialIdAvailable = true;
                    break;
                }
            case "InterstitialResession":
                {
                    for (int i = 0; i < interstitialStaticID.Count; i++)
                        interstitialStaticID[i] = newAdID;
                    isInterstitialStaticIdAvailable = true;
                    break;
                }
            case "Rewarded":
                {
                    for (int i = 0; i < rewardedID.Count; i++)
                        rewardedID[i] = newAdID;
                    isRewardedVideoIdAvailable = true;
                    break;
                }
            case "RewardedInterstitial":
                {
                    for (int i = 0; i < rewardedInterstitialID.Count; i++)
                        rewardedInterstitialID[i] = newAdID;
                    isRewardedInterstitialIdAvailable = true;
                    break;
                }
            case "AppOpen":
                {
                    for (int i = 0; i < appOpenID.Count; i++)
                        appOpenID[i] = newAdID;
                    isAppOpenIdAvailable = true;
                    break;
                }
            case "Admob":
                {
                    ShowDebugLog("Modified : AppID : " + newAdID);
                    return;
                }
        }
        ShowDebugLog("Modified : " + adType + " : " + newAdID);
    }

    public void AddNewAdIDs(string adType, System.Collections.Generic.List<string> data)
    {
        string outputData = "RemoteConfig AddNewAd ID's Data\n\n";
        string[] receivedData =
        {
                    interstitialAdIDRemoteValue ,
                    interstitialStaticAdIDRemoteValue ,
                    rewardedAdIDRemoteValue ,
                    rewardedInterstitialAdIDRemoteValue ,
                    appOpenAdIDRemoteValue,
                    smallBannerAdIDRemoteValue,
                    smallBannerGamePlayAdIDRemoteValue,
                    largeBannerAdIDRemoteValue,
                    adaptiveBannerAdIDRemoteValue,
                    smartBannerAdIDRemoteValue,
                    nativeOverlayAdIDRemoteValue,
                    nativeOverlayFullScreenAdIDRemoteValue,
                    customBannerAdIDRemoteValue
                };
        System.Collections.Generic.List<string>[] currentList =
        {
                    interstitialID,
                    interstitialStaticID,
                    rewardedID,
                    rewardedInterstitialID,
                    appOpenID,
                    smallBannerID,
                    smallBannerGamePlayID,
                    largeBannerID,
                    adaptiveBannerID,
                    smartBannerID,
                    nativeID,
                    nativeFullScreenID,
                    customBannerID
                };
        int index = System.Array.IndexOf(receivedData, adType);
        if (index != -1)
        {
            if (data.Count > 0)
            {
                if (data[0].StartsWith("ca-app-pub-")) currentList[index].Clear();
                outputData += receivedData[index] + " Add ID's:\n";
                foreach (string key in data)
                {
                    if (key.StartsWith("ca-app-pub-")) { currentList[index].Add(key); outputData += key + "\n"; }
                }
            }
            else outputData += "No Remote Config Data Receive: " + receivedData[index] + "\n\n";
        }
        else outputData += "No Remote Config Data Found: " + receivedData[index] + "\n\n";
        ShowDebugLog(outputData);
    }

    #region AdRevenueAnalytics

#if AdRevenueAnalytics

    readonly AdRevenueAnalytics.AdType[] adRevenueAnalyticsAdTypeList =
        {
            AdRevenueAnalytics.AdType.Interstitial,
            AdRevenueAnalytics.AdType.InterstitialStatic,
            AdRevenueAnalytics.AdType.Rewarded,
            AdRevenueAnalytics.AdType.RewardedInterstitial,
            AdRevenueAnalytics.AdType.AppOpen,
            AdRevenueAnalytics.AdType.SmallBanner,
            AdRevenueAnalytics.AdType.SmallBannerGamePlay,
            AdRevenueAnalytics.AdType.LargeBanner,
            AdRevenueAnalytics.AdType.AdaptiveBanner,
            AdRevenueAnalytics.AdType.SmartBanner,
            AdRevenueAnalytics.AdType.Native,
            AdRevenueAnalytics.AdType.CustomBanner,
            AdRevenueAnalytics.AdType.NativeFullScreen,
        };

    AdRevenueAnalytics.AdType GetAdType_AdRevenueAnalytics(string key)
    {
        int i = System.Array.IndexOf(remoteValueAdIDsList, key);
        return i >= 0 ? adRevenueAnalyticsAdTypeList[i] : AdRevenueAnalytics.AdType.NotDefine;
    }

    string GetAdTypeRemoteValue_AdRevenueAnalytics(AdRevenueAnalytics.AdType adType) =>
    remoteValueAdIDsList[System.Array.IndexOf(adRevenueAnalyticsAdTypeList, adType)];

    public void UpdateAdIDs_AdRevenueAnalytics(System.Collections.Generic.List<string> ids, AdRevenueAnalytics.AdType adType)
    {
        AddNewAdIDs(GetAdTypeRemoteValue_AdRevenueAnalytics(adType), ids);
    }
#endif

    #endregion

    #region Structure AD IDs

    [System.Serializable]
    public class AdvertisementID
    {
        public ReceivedData ReceiveData;
    }
    [System.Serializable]
    public class ReceivedData
    {
        public System.Collections.Generic.List<string> IDs;
        public string Details;
    }

    #endregion

    #endregion

    #region Memory Threshold

    [SerializeField] bool isMemoryThresholdRemoteConfig = true;
    [SerializeField] string initMemoryThresholdRemoteValue = "InitilizationMemoryThreshold";
    [SerializeField] string bannerMemoryThresholdRemoteValue = "BannerMemoryThreshold";
    [SerializeField] string interstitialMemoryThresholdRemoteValue = "InterstitialMemoryThreshold";
    [SerializeField] string rewardedMemoryThresholdRemoteValue = "RewardedMemoryThreshold";
    void CheckMemoryThreshold()
    {
        if (!isMemoryThreshold || !isMemoryThresholdRemoteConfig) return;

#if RemoteConfigManager_Firebase
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(initMemoryThresholdRemoteValue))
        {
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<long>(initMemoryThresholdRemoteValue, RemoteConfigManager_Firebase.RemoteConfig_Type.Long, out long initMemoryThreshold);
            NoInitilizationBelow = (int)initMemoryThreshold;
        }
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(bannerMemoryThresholdRemoteValue))
        {
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<long>(bannerMemoryThresholdRemoteValue, RemoteConfigManager_Firebase.RemoteConfig_Type.Long, out long bannerMemoryThreshold);
            NoSmallBannerBelow = NoLargeBannerBelow = NoNativeBelow = (int)bannerMemoryThreshold;
        }
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(interstitialMemoryThresholdRemoteValue))
        {
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<long>(interstitialMemoryThresholdRemoteValue, RemoteConfigManager_Firebase.RemoteConfig_Type.Long, out long interstitialMemoryThreshold);
            NoAnyInterstitialBelow = NoAppOpenBelow = (int)interstitialMemoryThreshold;
            NoInterstitialStaticAbove = (int)(interstitialMemoryThreshold + 100);
        }
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(rewardedMemoryThresholdRemoteValue))
        {
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<long>(rewardedMemoryThresholdRemoteValue, RemoteConfigManager_Firebase.RemoteConfig_Type.Long, out long rewardedMemoryThreshold);
            NoAnyRewardedBelow = (int)rewardedMemoryThreshold;
            NoRewardedInterstitialAbove = (int)(rewardedMemoryThreshold + 100);
        }
#endif
    }

    #endregion

    #region Others RemoteValues
    void AddRemoteValueToRemoteConfig()
    {
#if RemoteConfigManager_Firebase
        if (RemoteConfigManager_Firebase.Instance == null) { ShowDebugLog("No RemoveValue Added.\nReason: RemoteConfig.Instance = null"); return; }

        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(smallBannerAdIDRemoteValue) && isSmallBannerIdAvailable) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(smallBannerAdIDRemoteValue, "");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(smallBannerGamePlayAdIDRemoteValue) && isSmallBannerGamePlayIdAvailable) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(smallBannerGamePlayAdIDRemoteValue, "");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(largeBannerAdIDRemoteValue) && isLargeBannerIdAvailable) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(largeBannerAdIDRemoteValue, "");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(adaptiveBannerAdIDRemoteValue) && isAdaptiveBannerIdAvailable) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(adaptiveBannerAdIDRemoteValue, "");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(smartBannerAdIDRemoteValue) && isSmartBannerIdAvailable) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(smartBannerAdIDRemoteValue, "");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(nativeOverlayAdIDRemoteValue) && isNativeIdAvailable) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(nativeOverlayAdIDRemoteValue, "");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(nativeOverlayFullScreenAdIDRemoteValue) && isNativeFullScreenIdAvailable) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(nativeOverlayFullScreenAdIDRemoteValue, "");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(customBannerAdIDRemoteValue) && isCustomBannerIdAvailable) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(customBannerAdIDRemoteValue, "");

        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(interstitialAdIDRemoteValue) && isInterstitialIdAvailable) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(interstitialAdIDRemoteValue, "");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(interstitialStaticAdIDRemoteValue) && isInterstitialStaticIdAvailable) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(interstitialStaticAdIDRemoteValue, "");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(rewardedAdIDRemoteValue) && isRewardedVideoIdAvailable) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(rewardedAdIDRemoteValue, "");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(rewardedInterstitialAdIDRemoteValue) && isRewardedInterstitialIdAvailable) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(rewardedInterstitialAdIDRemoteValue, "");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(appOpenAdIDRemoteValue) && isAppOpenIdAvailable) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(appOpenAdIDRemoteValue, "");

        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(initMemoryThresholdRemoteValue) && isMemoryThreshold) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(initMemoryThresholdRemoteValue, NoInitilizationBelow.ToString());
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(bannerMemoryThresholdRemoteValue) && isMemoryThreshold) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(bannerMemoryThresholdRemoteValue, NoSmallBannerBelow.ToString());
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(interstitialMemoryThresholdRemoteValue) && isMemoryThreshold) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(interstitialMemoryThresholdRemoteValue, NoAnyInterstitialBelow.ToString());
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(rewardedMemoryThresholdRemoteValue) && isMemoryThreshold) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(rewardedMemoryThresholdRemoteValue, NoAnyRewardedBelow.ToString());

        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(ANRSupervisorRemoteValue) && isANRSupervisor) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(ANRSupervisorRemoteValue, "True");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(DebugLog)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(DebugLog, "False");

        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isSmallBannerLoad)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isSmallBannerLoad, "True");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isLargeBannerLoad)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isLargeBannerLoad, "True");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isAdaptiveBannerLoad)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isAdaptiveBannerLoad, "True");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isSmartBannerLoad)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isSmartBannerLoad, "True");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isCustomBannerLoad)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isCustomBannerLoad, "True");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isNativeOverlayLoad)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isNativeOverlayLoad, "True");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isNativeOverlayFullScreenLoad)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isNativeOverlayFullScreenLoad, "True");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isInterstitialLoad)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isInterstitialLoad, "True");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isInterstitialStaticLoad)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isInterstitialStaticLoad, "True");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isRewardedLoad)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isRewardedLoad, "True");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isRewardedInterstitialLoad)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isRewardedInterstitialLoad, "True");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isAppOpenLoad)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isAppOpenLoad, "True");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isInterstitialStaticBackfillToInterstitial)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isInterstitialStaticBackfillToInterstitial, "False");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isRewardedInterstitialBackfillToRewarded)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isRewardedInterstitialBackfillToRewarded, "False");

        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(remoteConfigRevenueThresholdKey)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(remoteConfigRevenueThresholdKey, RevenueThreshold.ToString());

        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isAppOpenOrNativeOnAppStateChange)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isAppOpenOrNativeOnAppStateChange, "True");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isAppOpenOnAppStateChange)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isAppOpenOnAppStateChange, "True");

        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isNativeReloadOnCTR)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isNativeReloadOnCTR, "False");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isNativeFullScreenReloadOnCTR)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isNativeFullScreenReloadOnCTR, "False");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isNativeReloadOnNumberOfImpression)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isNativeReloadOnNumberOfImpression, "False");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isNativeFullScreenReloadOnNumberOfImpression)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isNativeFullScreenReloadOnNumberOfImpression, "False");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(nativeReloadOnNumberOfImpressionCount)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(nativeReloadOnNumberOfImpressionCount, "5");
        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(nativeFullScreenReloadOnNumberOfImpressionCount)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(nativeFullScreenReloadOnNumberOfImpressionCount, "5");

        if (!RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isSendAnalyticsToCrashlyticsKey)) RemoteConfigManager_Firebase.Instance.AddRemoteConfig(isSendAnalyticsToCrashlyticsKey, "False");
#endif
    }

    void UpdateRemote_Others()
    {
#if RemoteConfigManager_Firebase
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(isSendAnalyticsToCrashlyticsKey))
        {
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig(isSendAnalyticsToCrashlyticsKey, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out isSendAnalyticsToCrashlytics);
            isSendAnalyticsToCrashlytics_Static = isSendAnalyticsToCrashlytics;
        }
#endif
    }

    [SerializeField] string ANRSupervisorRemoteValue = "Anr_Supervisor";
    void CheckANRSupervisor()
    {
#if RemoteConfigManager_Firebase
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(ANRSupervisorRemoteValue))
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(ANRSupervisorRemoteValue, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out isANRSupervisor);
#endif
        ANRHandlerThreadStart();
    }

    readonly string DebugLog = "DebugLog";
    void CheckDebugLog()
    {
        if (isDebugLog) return;
#if RemoteConfigManager_Firebase
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(DebugLog))
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<bool>(DebugLog, RemoteConfigManager_Firebase.RemoteConfig_Type.Boolean, out isDebugLog);
#endif
    }

    readonly string isSmallBannerLoad = "isSmallBannerLoad";
    readonly string isLargeBannerLoad = "isLargeBannerLoad";
    readonly string isAdaptiveBannerLoad = "isAdaptiveBannerLoad";
    readonly string isSmartBannerLoad = "isSmartBannerLoad";
    readonly string isCustomBannerLoad = "isCustomBannerLoad";
    readonly string isNativeOverlayLoad = "isNativeOverlayLoad";
    readonly string isNativeOverlayFullScreenLoad = "isNativeOverlayFullScreenLoad";
    readonly string isInterstitialLoad = "isInterstitialLoad";
    readonly string isInterstitialStaticLoad = "isInterstitialStaticLoad";
    readonly string isRewardedLoad = "isRewardedLoad";
    readonly string isRewardedInterstitialLoad = "isRewardedInterstitialLoad";
    readonly string isAppOpenLoad = "isAppOpenLoad";
    readonly string isInterstitialStaticBackfillToInterstitial = "isInterstitialStaticBackfillToInterstitial";
    readonly string isRewardedInterstitialBackfillToRewarded = "isRewardedInterstitialBackfillToRewarded";

    #endregion

    #endregion

    #region Extra

    public void RemovedAdsSuccess()
    {
        // Handle BannerData
        if (BannerData.Count > 0)
        {
            // Collect keys to remove bit later
            foreach (var kvp in BannerData.ToArray())
            {
                BannerClass ad = kvp.Value;
                if (ad == null) continue;

                if (bannersList.Contains(ad.AdType))
                {
                    MainThreadQueue.Enqueue(() =>
                    {
                        ActionsPerformWithDelay(() =>
                        {
                            try { ad.Remove(); }
                            catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                        }, 0.1f);
                    });

                    BannerData.TryRemove(kvp.Key, out _);
                }
            }
        }
        // Handle AdvertisementData
        System.Collections.Generic.List<AdvertisementType> fullScreenAvailableAd = new System.Collections.Generic.List<AdvertisementType>
    {
        AdvertisementType.Interstitial,
        AdvertisementType.InterstitialStatic,
        AdvertisementType.AppOpen
    };

        // Collect keys to remove
        System.Collections.Generic.List<string> adKeysToRemove = new System.Collections.Generic.List<string>();

        foreach (var ad in new System.Collections.Generic.List<AdvertisementGeneric>(AdvertisementData.Values))
        {
            if (fullScreenAvailableAd.Contains(ad.AdType))
            {
                GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
                {
                    ActionsPerformWithDelay(() => { try { ad?.Remove(); } catch (System.Exception e) { SendExceptionToCrashlytics(e); } }, 0.1f);
                });

                adKeysToRemove.Add(ad.ID); // Mark for removal
            }
        }

        // Remove the keys outside the loop
        foreach (string key in adKeysToRemove) AdvertisementData.TryRemove(key, out _);

        // Update PlayerPrefs
        PlayerPrefs.SetInt(removeAdsValue, 1);
        ShowDebugLog(removeAdsValue + " Successful");
    }

    public bool IsInternetConnection()
    {
        return Application.internetReachability == NetworkReachability.NotReachable ? false : true;
    }

    [HideInInspector] public float lastTimeScale = 1.0f;
    bool lastAudioListenerState = false;
    public void ActionsPerformWithDelay(System.Action action, float delay = 1f)
    {
        StartCoroutine(ActionPerformIenumerator(action, delay));
    }

    System.Collections.IEnumerator ActionPerformIenumerator(System.Action action, float delay = 1f)
    {
        yield return new WaitForSecondsRealtime(delay);
        try
        {
            action();
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
    }

    bool IsValidID(ref string ID, string type)
    {
#if !UNITY_EDITOR
                            if (PlayerPrefs.HasKey(type))
                                return System.Convert.ToBoolean(PlayerPrefs.GetInt(type));
#endif
        ID = ID.Replace(" ", System.String.Empty);
        if (!ID.StartsWith("ca-app-pub-") || ID.Equals(""))
        {
            Debug.LogError("Invalid ID Found Against '" + type.ToString() + "'");
            PlayerPrefs.SetInt(type, 0);
            return false;
        }
        PlayerPrefs.SetInt(type, 1);
        return true;
    }

#if UNITY_EDITOR
    void CheckAdmobAdIDsSameAsAppID()
    {
        System.Collections.Generic.List<string>[] AllList =
        {
                    interstitialID,
                    interstitialStaticID,
                    rewardedID,
                    rewardedInterstitialID,
                    appOpenID,
                    smallBannerID,
                    smallBannerGamePlayID,
                    largeBannerID,
                    adaptiveBannerID,
                    smartBannerID,
                    nativeID,
                    nativeFullScreenID,
                    customBannerID
                };
        bool[] allBoolean =
        {
                    isInterstitialIdAvailable,
                    isInterstitialStaticIdAvailable,
                    isRewardedVideoIdAvailable,
                    isRewardedInterstitialIdAvailable,
                    isAppOpenIdAvailable,
                    isSmallBannerIdAvailable,
                    isSmallBannerGamePlayIdAvailable,
                    isLargeBannerIdAvailable,
                    isAdaptiveBannerIdAvailable,
                    isSmartBannerIdAvailable,
                    isNativeIdAvailable,
                    isNativeFullScreenIdAvailable,
                    isCustomBannerIdAvailable
                };

        string appID = GetAdmobAppID();
        if (!appID.StartsWith("ca-app-pub-")) return;
        string appIDAccount = appID.Split('~')[0];
        string result = "";
        for (int i = 0; i < AllList.Length; i++)
        {
            if (AllList[i].Count > 0 && allBoolean[i])
            {
                foreach (string individualList in AllList[i])
                {
                    if (!individualList.StartsWith(appIDAccount)) result += result == "" ?
                            "Given Ad ID's Not Match to Admob Account.\n\nInvalid ID: " + individualList + "\n" : "Invalid ID: " + individualList + "\n";
                }
            }
        }
        if (!result.Equals("")) Debug.LogError(result);
    }
    string GetAdmobAppID()
    {
        string result = "";
        System.Type settingsType = System.Type.GetType("GoogleMobileAds.Editor.GoogleMobileAdsSettings, GoogleMobileAds.Editor");
        if (settingsType == null) { Debug.LogError("Admob App ID Setting Missing"); return result; }
        System.Reflection.MethodInfo loadInstanceMethod = settingsType.GetMethod("LoadInstance", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        if (loadInstanceMethod == null) return result;
        object settingsInstance = loadInstanceMethod.Invoke(null, null);
        if (settingsInstance == null) return result;
        System.Reflection.PropertyInfo androidAppIdProperty = null;
        if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            androidAppIdProperty = settingsType.GetProperty("GoogleMobileAdsAndroidAppId", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        else if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS)
            androidAppIdProperty = settingsType.GetProperty("GoogleMobileAdsIOSAppId", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
        if (androidAppIdProperty == null) return result;
        result = androidAppIdProperty.GetValue(settingsInstance) as string;
        if (result == "") Debug.LogError("Admob App ID Missing");
        return result;
    }
#endif

    public void ShowAdInspector()
    {
        MobileAds.OpenAdInspector(error =>
        {
            ShowDebugLog("Failed to Show Ad Inspector. error :" + error);
        });
    }
    void ShowDebugLog(string msg)
    {
        if (isDebugLog)
        {
            GoogleMobileAds.Common.MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                Debug.Log("<color=red>Admob Log :</color>" + msg);

            });
        }
    }
    bool IsAdmobShowingFullScreenAd { get; set; } = false;

    readonly static float autoReloadAdTime = 5.0f;
    readonly static float maxReloadAdTime = 30.0f;
    float interstitialAutoReloadTime = autoReloadAdTime;
    float rewardedAutoReloadTime = autoReloadAdTime;
    float appOpenAutoReloadTime = autoReloadAdTime;

    public System.Collections.Generic.List<string> GetAdIDsList(AdvertisementType adType)
    {
        switch (adType)
        {
            case AdvertisementType.Interstitial: { return interstitialID; }
            case AdvertisementType.InterstitialStatic: { return interstitialStaticID; }
            case AdvertisementType.Rewarded: { return rewardedID; }
            case AdvertisementType.RewardedInterstitial: { return rewardedInterstitialID; }
            case AdvertisementType.AppOpen: { return appOpenID; }
        }
        return null;
    }
    public System.Collections.Generic.List<string> GetAdIDsList(BannerType adType)
    {
        switch (adType)
        {
            case BannerType.SmallBannerType: { return smallBannerID; }
            case BannerType.SmallBannerGamePlayType: { return smallBannerGamePlayID; }
            case BannerType.LargeBannerType: { return largeBannerID; }
            case BannerType.AdaptiveBannerType: { return adaptiveBannerID; }
            case BannerType.SmartBannerType: { return smartBannerID; }
            case BannerType.CustomBannerType: { return customBannerID; }
        }
        return null;
    }

    public bool IsRemoveAd()
    {
        bool result = false;
        if (PlayerPrefs.GetInt(removeAdsValue, 0) == 1) result = true;
        // Add your other logics here

        return result;
    }

    #endregion

    #region LoadingAdBG

    GameObject LoadingAdsBGRef = null;
    public GameObject LoadingAdBG
    {
        get
        {
            if (LoadingAdsBGRef == null)
            {
                LoadingAdsBGRef = transform.Find("loadingAdBG").gameObject;
                if (LoadingAdsBGRef == null)
                    Debug.LogError("LoadingAdBG can't find under the child of AdsManager_AdmobMediation GameObject");
            }
            return LoadingAdsBGRef;
        }
    }
    public void ShowLoadingAdBG()
    {
        afterLoadingBannerShowAction = null;
        afterLoadingBannerShowCustomAction = null;
        afterLoadingNativeRectShowCustomAction = null;
        if (isSmallBannerShow || isSmallBannerGamePlayShow || isLargeBannerShow || isAdaptiveBannerShow || isSmartBannerShow)
        {
            //if (lastBannerSizeState.Equals(BannerSizeState.DefaultBannerSize))
            afterLoadingBannerShowAction = ShowBanner;
            //else
            //    afterLoadingBannerShowCustomAction = ShowBanner;

            foreach (var currentBannerType in bannersList)
            {
                if (IsBannerShowing(currentBannerType)) HideBannerInternal(currentBannerType, true);
            }
        }
        else if (isCustomBannerShow)
        {
            afterLoadingBannerShowCustomAction = ShowBanner;
            foreach (var currentBannerType in bannersList)
            {
                if (IsBannerShowing(currentBannerType)) HideBannerInternal(currentBannerType, true);
            }
        }
        else if (IsNativeShow)
        {
            if (lastNativeType == NativeCustomType.SetAsThisRectObject)
                afterLoadingNativeRectShowCustomAction = ShowNative;
            else
                afterLoadingNativePreDefineShowCustomAction = ShowNative;

            HideNative(isFromFocus: true);
        }
        else
        {
#if AdsManager_Applovin
                    AdsManager_Applovin.Instance.SaveLastBannerStatus();
#endif
#if AdsManager_Liftoff
                    AdsManager_Liftoff.Instance.SaveLastBannerStatus();
#endif
        }
#if !UNITY_EDITOR
                            LoadingAdBG?.SetActive(true);
#endif

        if (tempAutomaticDisableLoadingBG != null) StopCoroutine(tempAutomaticDisableLoadingBG);
        tempAutomaticDisableLoadingBG = AutomaticDisableLoadingBG();
        StartCoroutine(tempAutomaticDisableLoadingBG);

#if ANRSupervisor
        ANRSupervisorObservation(ANRSupervisorObservingANRState.Start);
#endif
    }
    public void HideLoadingAdBG()
    {
        LoadingAdBG?.SetActive(false);
        if (tempAutomaticDisableLoadingBG != null) StopCoroutine(tempAutomaticDisableLoadingBG);

#if ANRSupervisor
                    ANRSupervisorObservation(ANRSupervisorObservingANRState.Stop);
#endif
    }
    public void AfterLoadingBGBannerToShow(bool isFromApplovin = false)
    {       
        if (isSmallBannerShow || isSmallBannerGamePlayShow || isLargeBannerShow || isAdaptiveBannerShow || isSmartBannerShow)
        {
            if (isSmallBannerShow)
            {
                afterLoadingBannerShowAction?.Invoke(BannerType.SmallBannerType, lastSmallBannerPosition);
            }
            else if (isSmallBannerGamePlayShow)
            {
                afterLoadingBannerShowAction?.Invoke(BannerType.SmallBannerGamePlayType, lastSmallBannerGamePlayPosition);
            }
            else if (isLargeBannerShow)
            {
                afterLoadingBannerShowAction?.Invoke(BannerType.LargeBannerType, lastLargeBannerPosition);
            }
            else if (isAdaptiveBannerShow)
            {
                afterLoadingBannerShowAction?.Invoke(BannerType.AdaptiveBannerType, lastAdaptiveBannerPosition);
            }
            else if (isSmartBannerShow)
            {
                afterLoadingBannerShowAction?.Invoke(BannerType.SmartBannerType, lastSmartBannerPosition);
            }
        }
        else if (isCustomBannerShow)
        {
            afterLoadingBannerShowCustomAction?.Invoke(BannerType.CustomBannerType, lastCustomBannerRectTransform);
        }
        else if (IsNativeShow)
        {
            if (lastNativeType == NativeCustomType.SetAsThisRectObject)
                afterLoadingNativeRectShowCustomAction?.Invoke(lastNativeOverlayRectTransform, lastNativeAdOptions, lastNativeTempStyle, lastNativeAdPosition, false, "");
            else
                afterLoadingNativePreDefineShowCustomAction?.Invoke(lastNativeAdPositionPreDefine, lastNativeAdWidth, lastNativeAdHight, lastNativeAdOptions, lastNativeTempStyle, false, "");
        }
        else
        {
#if AdsManager_Applovin
                if(!isFromApplovin)
                    AdsManager_Applovin.Instance.LoadLastBannerStatus();
#endif
#if AdsManager_Liftoff
                    AdsManager_Liftoff.Instance.LoadLastBannerStatus();
#endif
        }
        afterLoadingBannerShowAction = null;
        afterLoadingNativeRectShowCustomAction = null;
        afterLoadingNativePreDefineShowCustomAction = null;
        IsAppOpenCanShow = true;
    }

    System.Action<BannerType, AdPosition> afterLoadingBannerShowAction = null;
    System.Action<BannerType, RectTransform> afterLoadingBannerShowCustomAction = null;
    System.Action<RectTransform, NativeAdOptions, NativeTemplateStyle, AdPosition, bool, string> afterLoadingNativeRectShowCustomAction = null;
    System.Action<NativeAdPosition, NativePreDefineSize, NativePreDefineSize, NativeAdOptions, NativeTemplateStyle, bool, string> afterLoadingNativePreDefineShowCustomAction = null;
    public readonly float loadingAdTimeDelay = 0.2f;

    System.Collections.IEnumerator tempAutomaticDisableLoadingBG = null;
    System.Collections.IEnumerator AutomaticDisableLoadingBG(float timeDuaration = 3.0f)
    {
        yield return new WaitForSecondsRealtime(timeDuaration);
        if (LoadingAdBG.activeSelf && !NativeFullScreen.activeSelf)
        {
            HideLoadingAdBG();
            AfterLoadingBGBannerToShow();

        }
    }

    #endregion

    #region Memory Information

    readonly int memoryThreshHold = 500; //in MBs
    static int memoryAvailable = 0;
    static System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"\d+");

    // return True if memory low by defined threshHold
    public bool IsLowMemory(int threshold = -1)
    {
#if UNITY_EDITOR
        return false;
#endif
        #region IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXEditor) return false;
        #endregion
        try
        {
            if (!isMemoryThreshold) return false;
            if (isLowMemoryAnalytics) return LowMemoryAndEventSend(threshold);

            threshold = threshold.Equals(-1) ? memoryThreshHold : threshold;
            return LoadMemoryInfo().Equals(true) ? (memoryAvailable / 1024) <= threshold : false;
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); return true; }
    }
    static bool LoadMemoryInfo()
    {
        try
        {
            //if file not exist retrun from here
            if (!System.IO.File.Exists("/proc/meminfo")) return false;
            System.IO.FileStream fs = new System.IO.FileStream("/proc/meminfo", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            System.IO.StreamReader sr = new System.IO.StreamReader(fs);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.ToLower().Replace(" ", "");
                if (line.Contains("memavailable")) { memoryAvailable = int.Parse(re.Match(line).Value); }
            }
            sr.Close(); fs.Close(); fs.Dispose();
            return true;
        }
        catch (System.Exception) { return false; }
    }

    bool LowMemoryAndEventSend(int threshold = -1)
    {
        try
        {
            bool returnValue = false;
            string parameterValue = "defaultValue";
            if (LoadMemoryInfo().Equals(true))
            {
                threshold = threshold.Equals(-1) ? memoryThreshHold : threshold;
                float availableInMB = (memoryAvailable / 1024);
                if (availableInMB <= threshold)
                {
                    if (availableInMB < 50)
                        parameterValue = "less than 50";
                    else if (availableInMB < 100)
                        parameterValue = "less than 100";
                    else if (availableInMB < 150)
                        parameterValue = "less than 150";
                    else if (availableInMB < 200)
                        parameterValue = "less than 200";
                    else if (availableInMB < 250)
                        parameterValue = "less than 250";
                    else if (availableInMB < 300)
                        parameterValue = "less than 300";
                    else if (availableInMB < 350)
                        parameterValue = "less than 350";
                    else if (availableInMB < 400)
                        parameterValue = "less than 400";
                    else if (availableInMB < 450)
                        parameterValue = "less than 450";
                    else if (availableInMB < 500)
                        parameterValue = "less than 500";
                    else
                        parameterValue = "greater than 500";
                    returnValue = true;
                }
                else
                {
                    parameterValue = "greater than 500";
                    returnValue = false;
                }
            }
            if (isLowMemoryAnalytics) SendAnalytics("MemoryInfo", "lowMemory", parameterValue);
            return returnValue;
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); return true; }
    }

    #endregion

    #region ANR Supervisor

    [SerializeField] bool isANRSupervisor = true;
    bool isANRSupervisorInitilized = false;
#if UNITY_ANDROID
    AndroidJavaClass ANRSupervisorClass;
#endif
    void ANRSupervisorObservation(ANRSupervisorObservingANRState state)
    {
        if (!isANRSupervisor) return;
#if UNITY_ANDROID
        if (!isANRSupervisorInitilized)
        {
            {
                if (Application.platform == RuntimePlatform.Android)
                {
                    try
                    {
                        MainThreadQueue.Enqueue(() =>
                        {
#if ANRSupervisor
                                    ANRSupervisorClass = new AndroidJavaClass("ANRSupervisor");
                                    ANRSupervisorClass.CallStatic("create");
                                    ShowDebugLog("ANRSupervisor create");
                                    isANRSupervisorInitilized = true;
#endif
                        });
                    }
                    catch (System.Exception e) { SendExceptionToCrashlytics(e); return; }
                }
                else return;
            }
        }
        switch (state)
        {
            case ANRSupervisorObservingANRState.Start:
                {
                    try
                    {
                        MainThreadQueue.Enqueue(() =>
                        {
#if ANRSupervisor
                                    ANRSupervisorClass.CallStatic("start");
                                    ShowDebugLog("ANRSupervisor start");
#endif
                        });
                    }
                    catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                    break;
                }
            case ANRSupervisorObservingANRState.Stop:
                {
                    try
                    {
                        MainThreadQueue.Enqueue(() =>
                        {
#if ANRSupervisor
                                    ANRSupervisorClass.CallStatic("stop");
                                    ShowDebugLog("ANRSupervisor stop");
#endif
                        });
                    }
                    catch (System.Exception e) { SendExceptionToCrashlytics(e); }
                    break;
                }
        }
#endif
    }
    enum ANRSupervisorObservingANRState
    {
        Start,
        Stop
    }
    #endregion

    #region ANR Handler
    bool isANRHandlerStarted = false;
    void ANRHandlerThreadStart()
    {
        if (!isANRSupervisor) return;
        if (isANRHandlerStarted) return;
#if unity_release
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass pluginClass = null;
            try
            {
                pluginClass = new AndroidJavaClass("com.anr.unity.ProcessExecuter");
            }
            catch (System.Exception e) { SendExceptionToCrashlytics(e); return; }

            if (pluginClass != null)
            {
                try
                {
                    MainThreadQueue.Enqueue(() =>
                    {
                        pluginClass.CallStatic("create");
                        pluginClass.CallStatic("start");
                        ShowDebugLog("ANR Handler Thread Started");
                        isANRHandlerStarted = true;
                    });
                }
                catch (System.Exception e) { SendExceptionToCrashlytics(e); return; }
            }
        }
#endif
    }

    #endregion

    #region AdImpression_CustomEvent

    double custom_revenue = 0;
    const string AdRevenueKey = "AdRevenueKey";
    double RevenueThreshold = 0.001f;
    const string adImpressionCustomEventName = "ad_impression_custom";
    const string remoteConfigRevenueThresholdKey = "ROAS_Threshold_Revenue";
    public void AdImpressionCustomEvent(double _rev)
    {
        try
        {
#if RemoteConfigManager_Firebase
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig<double>(remoteConfigRevenueThresholdKey,
                RemoteConfigManager_Firebase.RemoteConfig_Type.Double, out RevenueThreshold);
#endif
            if (!double.TryParse(PlayerPrefs.GetString(AdRevenueKey, "0"), System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out custom_revenue)) custom_revenue = 0.0;
            double currentAccumulatedRevenue = custom_revenue + (_rev / 1_000_000);
            if (currentAccumulatedRevenue >= RevenueThreshold)
            {
#if Firebase_Analytics
                var impressionParameters = new[]
                {
                         new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterValue, currentAccumulatedRevenue),
                         new Firebase.Analytics.Parameter(Firebase.Analytics.FirebaseAnalytics.ParameterCurrency, "USD"),
                };
                SendAnalytics(adImpressionCustomEventName, impressionParameters);
#endif
                PlayerPrefs.SetString(AdRevenueKey, "0");
            }
            else PlayerPrefs.SetString(AdRevenueKey, currentAccumulatedRevenue.ToString(System.Globalization.CultureInfo.InvariantCulture));
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
    }

    #endregion

    #region GDPR
    [SerializeField] bool isDebugGDPR = false;
    [SerializeField] System.Collections.Generic.List<string> GDPR_Testing_Device_Hash_ID;
    void ShowUserConsent()
    {
        ShowDebugLog("User Consent : Request Send");
#if UNITY_EDITOR
        return;
#endif
        GoogleMobileAds.Ump.Api.ConsentInformation.Update(
            new GoogleMobileAds.Ump.Api.ConsentRequestParameters
            {
                TagForUnderAgeOfConsent = false,
                ConsentDebugSettings = new GoogleMobileAds.Ump.Api.ConsentDebugSettings
                {
                    DebugGeography = GoogleMobileAds.Ump.Api.DebugGeography.EEA,
                    TestDeviceHashedIds = GDPR_Testing_Device_Hash_ID
                    //TestDeviceHashedIds = new System.Collections.Generic.List<string>()
                    //{
                    //    // Add your device Hashed ID to debug
                    //    "6AB0D3B676E033D8CE6FE45F1A825832",
                    //    "E5AEA40025D8C9ACFE9C865905BE9F5B" // OPPO F21 Pro
                    //}
                }
            },
            (consentInfoError) =>
            {
                if (consentInfoError != null)
                {
                    ShowDebugLog("User Consent : OnConsentInfoUpdate Error :" + consentInfoError);
                    return;
                }

                if (GoogleMobileAds.Ump.Api.ConsentInformation.IsConsentFormAvailable())
                {
                    GoogleMobileAds.Ump.Api.ConsentForm.Load((consentForm, loadError) =>
                    {
                        if (loadError != null)
                        {
                            ShowDebugLog("User Consent : OnLoadConsentForm Error :" + loadError);
                            return;
                        }
                        if (GoogleMobileAds.Ump.Api.ConsentInformation.ConsentStatus == GoogleMobileAds.Ump.Api.ConsentStatus.Required)
                        {
                            consentForm.Show((showError) =>
                            {
                                if (showError != null)
                                {
                                    ShowDebugLog("User Consent : OnShowForm Error :" + showError);
                                    return;
                                }
                            });
#if UNITY_EDITOR
                            Time.timeScale = 1;
#endif
                        }
                    });
                }
            }); ; ; ; ;
    }

    #endregion

    #region Analytics

    #region Analytics Enums
    public enum EventName
    {
        Ad_Initilization,
        Ad_SmallBanner,
        Ad_SmallBannerGamePlay,
        Ad_LargeBanner,
        Ad_AdaptiveBanner,
        Ad_SmartBanner,
        Ad_Interstitial,
        Ad_InterstitialStatic,
        Ad_InterstitialBackfill,
        Ad_InterstitialBackfill_2,
        Ad_RewardedVideo,
        Ad_RewardedInterstitial,
        Ad_AppOpen,
        Ad_AppOpen_Backfill,
        Ad_AppOpen_Backfill_2,
        Ad_CustomBanner,
        Ad_NativeOverlay,
        Ad_NativeOverlayFullScreen
    }
    public enum ParameterKey
    {
        myParameter,
        ECPM
    }
    public enum ParameterValue
    {
        AdRequestSend,
        AdRequestSuccess,
        AdRequestFail,
        AdShow,
        AdComplete
    }
    #endregion

    #region Functions
    public void SendAnalytics(EventName eventName, ParameterKey parameterKey, ParameterValue parameterValue)
    {
        if (!isAdsAnalytics) return;
        try
        {
#if Firebase_Analytics
            MainThreadQueue.Enqueue(() => firebaseAnalyticsQueue.Push(eventName.ToString(), new Firebase.Analytics.Parameter[] { new Firebase.Analytics.Parameter(parameterKey.ToString(), parameterValue.ToString()) }));
#endif

#if SingularSDKManager
               MainThreadQueue.Enqueue(() => SingularSDKManager.Instance.SendEvent(eventName.ToString(), parameterKey.ToString(), parameterValue.ToString()));
#endif
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
    }
    public void SendAnalytics(string eventName, ParameterKey parameterKey, ParameterValue parameterValue)
    {
        if (!isAdsAnalytics) return;
        try
        {
#if Firebase_Analytics
            MainThreadQueue.Enqueue(() => firebaseAnalyticsQueue.Push(eventName, new Firebase.Analytics.Parameter[] { new Firebase.Analytics.Parameter(parameterKey.ToString(), parameterValue.ToString()) }));
#endif
#if SingularSDKManager
              MainThreadQueue.Enqueue(() => SingularSDKManager.Instance.SendEvent(eventName, parameterKey.ToString(), parameterValue.ToString()));
#endif
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
    }
    public void SendAnalytics(EventName eventName, ParameterKey parameterKey, double parameterValue)
    {
        if (!isAdsAnalytics) return;
        try
        {
#if Firebase_Analytics
            MainThreadQueue.Enqueue(() => firebaseAnalyticsQueue.Push(eventName.ToString(), new Firebase.Analytics.Parameter[] { new Firebase.Analytics.Parameter(parameterKey.ToString(), parameterValue) }));
#endif
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
    }
    public void SendAnalytics(string eventName, string parameterKey, string parameterValue)
    {
        if (!isAdsAnalytics) return;
        try
        {
#if Firebase_Analytics
            MainThreadQueue.Enqueue(() => firebaseAnalyticsQueue.Push(eventName.ToString(), new Firebase.Analytics.Parameter[] { new Firebase.Analytics.Parameter(parameterKey.ToString(), parameterValue.ToString()) }));
#endif
#if SingularSDKManager
             MainThreadQueue.Enqueue(() => SingularSDKManager.Instance.SendEvent(eventName, parameterKey, parameterValue));
#endif
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
    }
    public void SendAnalytics(string eventName)
    {
        if (!isAdsAnalytics) return;
        try
        {
#if Firebase_Analytics
            MainThreadQueue.Enqueue(() => firebaseAnalyticsQueue.Push(eventName.ToString()));
#endif
#if SingularSDKManager
              MainThreadQueue.Enqueue(() => SingularSDKManager.Instance.SendEvent(eventName));
#endif
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
    }
#if Firebase_Analytics
    public void SendAnalytics(string eventName, Firebase.Analytics.Parameter[] parameters)
    {
        if (!isAdsAnalytics) return;
        try
        {
#if Firebase_Analytics
            MainThreadQueue.Enqueue(() => firebaseAnalyticsQueue.Push(eventName.ToString(), parameters));
#endif
        }
        catch (System.Exception e) { SendExceptionToCrashlytics(e); }
    }
#endif

    readonly string isSendAnalyticsToCrashlyticsKey = "isSendAnalyticsToCrashlytics";
    public void SendExceptionToCrashlytics(System.Exception exception, System.Type script = null, [System.Runtime.CompilerServices.CallerMemberName] string caller = "")
    {
        string message = "__Exception Occurs__\n\n";
        message += "Script Name : " + (script == null ? GetType().Name : script.Name) + "\n";
        message += "Functiona Name :" + caller + "\n";
        message += "Exeption Msg : " + exception.Message + "\n " +
            "Exception Data : " + exception.Data + "\n" +
            "Exception Source : " + exception.Source + "\n" +
            "Exception StackTrace : " + exception.StackTrace + "\n" +
            "Exception All Details : " + exception.StackTrace;
        ShowDebugLog(message);
        if (isSendAnalyticsToCrashlytics)
        {
#if CrashlyticsManager_Firebase
        MainThreadQueue.Enqueue(() => { if (CrashlyticsManager_Firebase.initilizeStatus == CrashlyticsManager_Firebase.InitilizeStatus.Initilized) Firebase.Crashlytics.Crashlytics.Log(message); });
#endif
        }
    }
    public static void SendExceptionToCrashlytics_Static(System.Exception exception, System.Type script = null, [System.Runtime.CompilerServices.CallerMemberName] string caller = "")
    {
        string message = "__Exception Occurs__\n\n";
        message += "Script Name : " + (script == null ? "NotDefine" : script.Name) + "\n";
        message += "Functiona Name : " + caller + "\n";
        message += "Exeption Msg : " + exception.Message + "\n " +
            "Exception Data : " + exception.Data + "\n" +
            "Exception Source : " + exception.Source + "\n" +
            "Exception StackTrace : " + exception.StackTrace + "\n" +
            "Exception All Details : " + exception.StackTrace;
        if (isSendAnalyticsToCrashlytics_Static)
        {
#if CrashlyticsManager_Firebase
        MainThreadQueue.Enqueue(() => { if (CrashlyticsManager_Firebase.initilizeStatus == CrashlyticsManager_Firebase.InitilizeStatus.Initilized) Firebase.Crashlytics.Crashlytics.Log(message); });
#endif
        }
    }

    #endregion

    #region FirebaseAnalyticsQueue

#if Firebase_Analytics
    static FirebaseAnalyticsDirectSendWithQueue firebaseAnalyticsQueue = null;
    public class FirebaseAnalyticsDirectSendWithQueue : MonoBehaviour
    {
        readonly WaitForSecondsRealtime delayToMergeEvent = new WaitForSecondsRealtime(2.5f);
        readonly int maxEventSend = 20;
        readonly System.Collections.Concurrent.ConcurrentQueue<(string evt, Firebase.Analytics.Parameter[] ps)> _queue = new System.Collections.Concurrent.ConcurrentQueue<(string evt, Firebase.Analytics.Parameter[] ps)>();
        bool isFlushing = false;

        public void Push(string evt, Firebase.Analytics.Parameter[] ps = null)
        {
            if (!AdsManager_AdmobMediation.Instance.isAdsAnalytics) return;
            _queue.Enqueue((evt, ps));
            if (!isFlushing) { isFlushing = true; StartCoroutine(SendEventsAfterDelay()); }
        }

        System.Collections.IEnumerator SendEventsAfterDelay()
        {
            yield return delayToMergeEvent; // Delay to merge events
            int count = 0;
            while (_queue.TryDequeue(out var item) && count++ < maxEventSend)
            {
                try
                {
#if FirebaseAnalyticsManager
                    if (item.ps == null) FirebaseAnalyticsManager.Instance.SendAnalytics(item.evt);
                    else FirebaseAnalyticsManager.Instance.SendAnalytics(item.evt, item.ps);
#else
                    if (item.ps == null)  Firebase.Analytics.FirebaseAnalytics.LogEvent(item.evt); 
                    else Firebase.Analytics.FirebaseAnalytics.LogEvent(item.evt, item.ps);
#endif
                }
                catch (System.Exception e) { SendExceptionToCrashlytics_Static(e); }
                yield return null;  // 1 frame delay
            }
            isFlushing = false;
        }
    }
#endif

    #endregion

    #region AdRevenueAnalytics
#if AdRevenueAnalytics

    void SendRevenueAnalytics(AdRevenueAnalytics.AdType adType, AdValue e, string adUnitName, string adSource, string adFormat)
    {
        MainThreadQueue.Enqueue(() =>
        {
            double revenue = e.Value / 1000000.0;
            System.Collections.Generic.List<AdRevenueAnalytics.KeyValuePair> list = new System.Collections.Generic.List<AdRevenueAnalytics.KeyValuePair>();
            string currencyCode = e.CurrencyCode;
            list.Add(new AdRevenueAnalytics.KeyValuePair("AdPlatform", "Admob"));
            if (!string.IsNullOrEmpty(adSource)) list.Add(new AdRevenueAnalytics.KeyValuePair("NetworkType", adSource));
            if (!string.IsNullOrEmpty(adUnitName)) list.Add(new AdRevenueAnalytics.KeyValuePair("AdUnitID", adUnitName));
            if (!string.IsNullOrEmpty(currencyCode)) list.Add(new AdRevenueAnalytics.KeyValuePair("AdCurrency", currencyCode));
            if (!string.IsNullOrEmpty(adFormat)) list.Add(new AdRevenueAnalytics.KeyValuePair("AdFormat", adFormat));

            AdRevenueAnalytics.Instance.SendRevenueAnalytics(adType, AdRevenueAnalytics.AdRevenueEventType.Ad_Rev_General, list, revenue);
        });
    }

#endif

    #endregion

    #endregion

    #region Structure

    public enum AdLoadPriority
    {
        SmallBanner = 1,
        LargeBanner = 2,
        AdaptiveBanner = 7,
        SmartBanner = 8,
        CustomBanner = 11,
        Native = 6,
        NativeFullScreen = 12,
        Interstitial = 3,
        InterstitialStatic = 10,
        Rewarded = 4,
        RewardedInterstitial = 5,
        AppOpen = 9,
    }

    #endregion

    #endregion

    #region Editor Properties

#if UNITY_EDITOR
    [CustomEditor(typeof(AdsManager_AdmobMediation))]
    public class AdsIDsEditor : Editor
    {
        readonly string pluginVersion = "1.1.31";

        #region SerializedProperty

        SerializedProperty isTestMode;
        SerializedProperty isSmallBannerIdAvailable;
        SerializedProperty isSmallBannerGamePlayIdAvailable;
        SerializedProperty smallBannerID;
        SerializedProperty smallBannerGamePlayID;
        SerializedProperty isLargeBannerIdAvailable;
        SerializedProperty largeBannerID;
        SerializedProperty isAdaptiveBannerIdAvailable;
        SerializedProperty adaptiveBannerID;
        SerializedProperty isSmartBannerIdAvailable;
        SerializedProperty smartBannerID;
        SerializedProperty isNativeIdAvailable;
        SerializedProperty isNativeFullScreenIdAvailable;
        SerializedProperty nativeID;
        SerializedProperty nativeFullScreenID;
        SerializedProperty isCustomBannerIdAvailable;
        SerializedProperty customBannerID;
        SerializedProperty isInterstitialIdAvailable;
        SerializedProperty interstitialID;
        SerializedProperty isInterstitialStaticIdAvailable;
        SerializedProperty interstitialStaticID;
        SerializedProperty isRewardedVideoIdAvailable;
        SerializedProperty rewardedIDs;
        SerializedProperty isRewardedInterstitialIdAvailable;
        SerializedProperty rewardedInterstitialID;
        SerializedProperty isAppOpenIdAvailable;
        SerializedProperty appOpenIDs;

        SerializedProperty isAutoInitilize;
        SerializedProperty isStartingInitilizeDelay;
        SerializedProperty startingDelay;
        SerializedProperty adLoadPriority;

        SerializedProperty isAutoReload;
        SerializedProperty isInterstitialReloadAfterShow;
        SerializedProperty isRewardedReloadAfterShow;
        SerializedProperty isAppOpenReloadAfterShow;


        SerializedProperty isDebugLog;
        SerializedProperty isBannerBGEnable;
        SerializedProperty isEditorShowAds;
        SerializedProperty isAdsAnalytics;
        SerializedProperty isSendAnalyticsToCrashlytics;
        SerializedProperty isANRSupervisor;

        SerializedProperty isMemoryThreshold;
        SerializedProperty initilizationRequiredMinValue;
        SerializedProperty smallBannerRequiredMinValue;
        SerializedProperty largeBannerRequiredMinValue;
        SerializedProperty nativeRequiredMinValue;
        SerializedProperty interstitialRequiredMinValue;
        SerializedProperty interstitialStaticRequiredMinValue;
        SerializedProperty rewardedRequiredMinValue;
        SerializedProperty rewardedInterstitialRequiredMinValue;
        SerializedProperty appOpenRequiredMinValue;
        SerializedProperty isLowMemoryAnalytics;

        SerializedProperty isMemoryThresholdRemoteConfig;

        static bool showAdIDs;
        static bool showInitilizationSetup;
        static bool showOtherSettings;
        static bool showMemorySettings;

        #endregion

        void OnEnable()
        {
            isTestMode = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isTestMode));
            isSmallBannerIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isSmallBannerIdAvailable));
            isSmallBannerGamePlayIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isSmallBannerGamePlayIdAvailable));
            smallBannerID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.smallBannerID));
            smallBannerGamePlayID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.smallBannerGamePlayID));
            isLargeBannerIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isLargeBannerIdAvailable));
            largeBannerID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.largeBannerID));
            isAdaptiveBannerIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isAdaptiveBannerIdAvailable));
            adaptiveBannerID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.adaptiveBannerID));
            isSmartBannerIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isSmartBannerIdAvailable));
            smartBannerID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.smartBannerID));
            isNativeIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isNativeIdAvailable));
            isNativeFullScreenIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isNativeFullScreenIdAvailable));
            nativeID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.nativeID));
            nativeFullScreenID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.nativeFullScreenID));
            isCustomBannerIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isCustomBannerIdAvailable));
            customBannerID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.customBannerID));
            isInterstitialIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isInterstitialIdAvailable));
            interstitialID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.interstitialID));
            isInterstitialStaticIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isInterstitialStaticIdAvailable));
            interstitialStaticID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.interstitialStaticID));
            isRewardedVideoIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isRewardedVideoIdAvailable));
            rewardedIDs = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.rewardedID));
            isRewardedInterstitialIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isRewardedInterstitialIdAvailable));
            rewardedInterstitialID = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.rewardedInterstitialID));
            isAppOpenIdAvailable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isAppOpenIdAvailable));
            appOpenIDs = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.appOpenID));

            isAutoInitilize = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isAutoInitilize));
            isStartingInitilizeDelay = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isStartingInitilizeDelay));
            startingDelay = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.startingDelay));

            adLoadPriority = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.adLoadPriority));

            isAutoReload = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isAutoReload));
            isInterstitialReloadAfterShow = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isInterstitialReloadAfterShow));
            isRewardedReloadAfterShow = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isRewardedReloadAfterShow));
            isAppOpenReloadAfterShow = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isAppOpenReloadAfterShow));

            isDebugLog = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isDebugLog));
            isBannerBGEnable = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isBannerBGEnable));
            isEditorShowAds = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isEditorShowAds));
            isAdsAnalytics = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isAdsAnalytics));
            isSendAnalyticsToCrashlytics = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isSendAnalyticsToCrashlytics));
            isANRSupervisor = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isANRSupervisor));

            isMemoryThreshold = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isMemoryThreshold));
            initilizationRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoInitilizationBelow));
            smallBannerRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoSmallBannerBelow));
            largeBannerRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoLargeBannerBelow));
            nativeRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoNativeBelow));
            interstitialRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoAnyInterstitialBelow));
            interstitialStaticRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoInterstitialStaticAbove));
            rewardedRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoAnyRewardedBelow));
            rewardedInterstitialRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoRewardedInterstitialAbove));
            appOpenRequiredMinValue = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.NoAppOpenBelow));
            isLowMemoryAnalytics = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isLowMemoryAnalytics));

            isMemoryThresholdRemoteConfig = serializedObject.FindProperty(nameof(AdsManager_AdmobMediation.isMemoryThresholdRemoteConfig));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            #region GUIStyle
            GUIStyle headingStyle = new GUIStyle(EditorStyles.helpBox);
            headingStyle.normal.textColor = Color.red;
            headingStyle.fontStyle = FontStyle.Bold;
            headingStyle.fontSize = 12;
            #endregion

            #region AD IDs
            EditorGUILayout.BeginVertical(headingStyle);
            showAdIDs = DrawFoldoutLabel("AD ID's", showAdIDs);
            if (showAdIDs)
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical(headingStyle);
                EditorGUILayout.Space();
                EditorGUI.indentLevel++;

                isTestMode.boolValue = EditorGUILayout.Toggle("Test Mode", isTestMode.boolValue);
                EditorGUILayout.Space();
                if (!isTestMode.boolValue)
                {
                    EditorGUILayout.Space();
                    isSmallBannerIdAvailable.boolValue = EditorGUILayout.Toggle("Small Banner", isSmallBannerIdAvailable.boolValue);
                    if (isSmallBannerIdAvailable.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(smallBannerID);
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;
                    }
                    isSmallBannerGamePlayIdAvailable.boolValue = EditorGUILayout.Toggle("Small Banner GamePlay", isSmallBannerGamePlayIdAvailable.boolValue);
                    if (isSmallBannerGamePlayIdAvailable.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(smallBannerGamePlayID);
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;
                    }
                    isLargeBannerIdAvailable.boolValue = EditorGUILayout.Toggle("Large Banner", isLargeBannerIdAvailable.boolValue);
                    if (isLargeBannerIdAvailable.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(largeBannerID);
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;
                    }
                    isAdaptiveBannerIdAvailable.boolValue = EditorGUILayout.Toggle("Adaptive Banner", isAdaptiveBannerIdAvailable.boolValue);
                    if (isAdaptiveBannerIdAvailable.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(adaptiveBannerID);
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;
                    }
                    isSmartBannerIdAvailable.boolValue = EditorGUILayout.Toggle("Smart Banner", isSmartBannerIdAvailable.boolValue);
                    if (isSmartBannerIdAvailable.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(smartBannerID);
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;
                    }
                    isNativeIdAvailable.boolValue = EditorGUILayout.Toggle("Native", isNativeIdAvailable.boolValue);
                    if (isNativeIdAvailable.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(nativeID);
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;
                    }
                    isNativeFullScreenIdAvailable.boolValue = EditorGUILayout.Toggle("NativeFullScreen", isNativeFullScreenIdAvailable.boolValue);
                    if (isNativeFullScreenIdAvailable.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(nativeFullScreenID);
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;
                    }
                    isCustomBannerIdAvailable.boolValue = EditorGUILayout.Toggle("CustomBanner", isCustomBannerIdAvailable.boolValue);
                    if (isCustomBannerIdAvailable.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(customBannerID);
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;
                    }
                    isInterstitialIdAvailable.boolValue = EditorGUILayout.Toggle("Interstitial", isInterstitialIdAvailable.boolValue);
                    if (isInterstitialIdAvailable.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(interstitialID, new GUIContent("Interstitial IDs"), true);
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;
                    }
                    isInterstitialStaticIdAvailable.boolValue = EditorGUILayout.Toggle("Interstitial Static", isInterstitialStaticIdAvailable.boolValue);
                    if (isInterstitialStaticIdAvailable.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(interstitialStaticID);
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;
                    }
                    isRewardedVideoIdAvailable.boolValue = EditorGUILayout.Toggle("Rewarded", isRewardedVideoIdAvailable.boolValue);
                    if (isRewardedVideoIdAvailable.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(rewardedIDs, new GUIContent("Rewarded IDs"), true);
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;
                    }
                    isRewardedInterstitialIdAvailable.boolValue = EditorGUILayout.Toggle("Rewarded Interstitial", isRewardedInterstitialIdAvailable.boolValue);
                    if (isRewardedInterstitialIdAvailable.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(rewardedInterstitialID);
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;
                    }
                    isAppOpenIdAvailable.boolValue = EditorGUILayout.Toggle("App Open", isAppOpenIdAvailable.boolValue);
                    if (isAppOpenIdAvailable.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(appOpenIDs, new GUIContent("AppOpen IDs"), true);
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;
                    }
                }
                else
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox("Testing Mode Enable With Admob Test IDs, Make Sure To Disable After Testing.", MessageType.Warning);
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            #endregion

            #region Initilization Setup
            EditorGUILayout.BeginVertical(headingStyle);
            showInitilizationSetup = DrawFoldoutLabel("Initilization Setup", showInitilizationSetup);
            if (showInitilizationSetup)
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical(headingStyle);
                EditorGUILayout.Space();
                EditorGUI.indentLevel++;

                isAutoInitilize.boolValue = EditorGUILayout.Toggle("Auto Initilize On Start", isAutoInitilize.boolValue);
                if (isAutoInitilize.boolValue)
                {
                    EditorGUI.indentLevel++;
                    isStartingInitilizeDelay.boolValue = EditorGUILayout.Toggle("Initilization Delay", isStartingInitilizeDelay.boolValue);
                    if (isStartingInitilizeDelay.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.PropertyField(startingDelay);
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                    EditorGUILayout.Space();
                }

                isAutoReload.boolValue = EditorGUILayout.Toggle("Auto Reload", isAutoReload.boolValue);
                if (isAutoReload.boolValue)
                {
                    EditorGUI.indentLevel++;
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextArea("Auto Reload when Ad Close", headingStyle);
                    EditorGUI.EndDisabledGroup();
                    EditorGUI.indentLevel++;
                    isInterstitialReloadAfterShow.boolValue = EditorGUILayout.Toggle("Interstitial", isInterstitialReloadAfterShow.boolValue);
                    isRewardedReloadAfterShow.boolValue = EditorGUILayout.Toggle("Rewarded", isRewardedReloadAfterShow.boolValue);
                    isAppOpenReloadAfterShow.boolValue = EditorGUILayout.Toggle("App Open", isAppOpenReloadAfterShow.boolValue);
                    if (isAppOpenReloadAfterShow.boolValue)
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.BeginHorizontal();
                        EditorGUILayout.HelpBox(" - App Open Shows Everytime When App State Change to Foreground.\n " +
                            "- Auto Reload After Shown", MessageType.Info);
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                        EditorGUI.indentLevel--;
                    }
                    EditorGUI.indentLevel--;
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.Space();

                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextArea("Priority After Initilization", headingStyle);
                EditorGUI.EndDisabledGroup();
                EditorGUI.indentLevel++;
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(adLoadPriority);
                EditorGUILayout.Space();
                EditorGUI.indentLevel--;

                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();

            #endregion

            #region Other Settings
            EditorGUILayout.BeginVertical(headingStyle);
            showOtherSettings = DrawFoldoutLabel("Other Settings", showOtherSettings);
            if (showOtherSettings)
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical(headingStyle);
                EditorGUILayout.Space();
                EditorGUI.indentLevel++;

                isDebugLog.boolValue = EditorGUILayout.Toggle("Debug Log", isDebugLog.boolValue);
                if (isDebugLog.boolValue)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox("Debugging Enable, Help you to debug every Request/Response via Debug.Log(msg), Make sure to Disable after Testing.", MessageType.Warning);
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;
                }
                isBannerBGEnable.boolValue = EditorGUILayout.Toggle("Banner BG", isBannerBGEnable.boolValue);
                isEditorShowAds.boolValue = EditorGUILayout.Toggle("Editor Show Ads", isEditorShowAds.boolValue);
                if (!isEditorShowAds.boolValue)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.HelpBox("Editor Not Showing Any Advertisement.", MessageType.Warning);
                    EditorGUILayout.Space();
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;
                }
                isAdsAnalytics.boolValue = EditorGUILayout.Toggle("Advertisement Analytics", isAdsAnalytics.boolValue);
                isSendAnalyticsToCrashlytics.boolValue = EditorGUILayout.Toggle("Analytics Send To Crashlytics", isSendAnalyticsToCrashlytics.boolValue);

#if ANRSupervisor || unity_release
                isANRSupervisor.boolValue = EditorGUILayout.Toggle("ANR Supervisor", isANRSupervisor.boolValue);
#endif

                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            #endregion

            #region Memory Settings
            EditorGUILayout.BeginVertical(headingStyle);
            showMemorySettings = DrawFoldoutLabel("Memory Settings", showMemorySettings);
            if (showMemorySettings)
            {
                EditorGUILayout.Space();
                EditorGUILayout.BeginVertical(headingStyle);
                EditorGUILayout.Space();
                EditorGUI.indentLevel++;

                isMemoryThreshold.boolValue = EditorGUILayout.Toggle("Memory Threshold", isMemoryThreshold.boolValue);
                if (isMemoryThreshold.boolValue)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.PropertyField(initilizationRequiredMinValue);
                    EditorGUILayout.PropertyField(smallBannerRequiredMinValue);
                    EditorGUILayout.PropertyField(largeBannerRequiredMinValue);
                    EditorGUILayout.PropertyField(nativeRequiredMinValue);
                    EditorGUILayout.PropertyField(interstitialRequiredMinValue);
                    EditorGUILayout.PropertyField(interstitialStaticRequiredMinValue);
                    EditorGUILayout.PropertyField(rewardedRequiredMinValue);
                    EditorGUILayout.PropertyField(rewardedInterstitialRequiredMinValue);
                    EditorGUILayout.PropertyField(appOpenRequiredMinValue);
                    EditorGUILayout.Space();

#if RemoteConfigManager_Firebase
                    isMemoryThresholdRemoteConfig.boolValue = EditorGUILayout.Toggle("Remote Config Memory Threshold", isMemoryThresholdRemoteConfig.boolValue);
#endif
                    isLowMemoryAnalytics.boolValue = EditorGUILayout.Toggle("Low Memory Analytics", isLowMemoryAnalytics.boolValue);
                    EditorGUI.indentLevel--;
                }
                else
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.HelpBox("No Available Memory Threshold Applied.", MessageType.Warning);
                    EditorGUI.indentLevel--;
                }
                EditorGUILayout.Space();
                EditorGUILayout.EndVertical();
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            #endregion

            #region plugin version
            GUIStyle versionStyle = new GUIStyle(EditorStyles.helpBox)
            {
                alignment = TextAnchor.MiddleRight
            };
            versionStyle.normal.textColor = Color.red;
            EditorGUILayout.LabelField("AdmobMediation Plugin Version: " + pluginVersion, versionStyle);
            #endregion

            serializedObject.ApplyModifiedProperties();
        }

        private bool DrawFoldoutLabel(string title, bool foldoutState)
        {
            GUIStyle labelStyle = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Bold,
                normal = { textColor = foldoutState ? Color.red : Color.gray }
            };

            Rect rect = GUILayoutUtility.GetRect(20, 20, GUILayout.ExpandWidth(true));
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                foldoutState = !foldoutState;
                Event.current.Use();
            }

            EditorGUI.LabelField(rect, (foldoutState ? "▼ " : "► ") + title, labelStyle);
            return foldoutState;
        }
    }

    public class CustomScriptDefineSymbols : EditorWindow
    {
        class FileStructure
        {
            public string filePath;
            public string fileName;
            public string fileType;

            public FileStructure(string path, string name, string type)
            {
                filePath = path;
                fileName = name;
                fileType = type;
            }
        }

        static System.Collections.Generic.List<FileStructure> files = new System.Collections.Generic.List<FileStructure>()
                {
                    new FileStructure("Assets/Mobify/Admob Mediation/Scripts/","AdsManager_AdmobMediation",".cs"),
                    new FileStructure("Assets/Firebase/Plugins/","Firebase.Analytics",".dll"),
                    new FileStructure("Assets/Plugins/Android/","ANRSupervisor",".java"),
                    new FileStructure("Assets/Plugins/Android/","unity-release",".aar")
                };

        [InitializeOnLoad]
        public class InitOnLoad
        {
            static InitOnLoad()
            {
                foreach (FileStructure file in files)
                {
                    bool isAssetPresent = AssetDatabase.LoadAssetAtPath(file.filePath + "" + file.fileName + "" + file.fileType, typeof(UnityEngine.Object)) != null;
                    if (isAssetPresent) SetEnabled(file.fileName, true);
                }
                EditorApplication.projectChanged += OnProjectChanged;
            }
        }
        static void SetEnabled(string defineName, bool enable)
        {
            defineName = new System.Text.RegularExpressions.Regex("['*-.,&#^@]").Replace(defineName, "_");
            foreach (var group in buildTargetGroups)
            {
                var defines = GetDefinesList(group);

                if (enable)
                {
                    if (defines.Contains(defineName))
                        return;
                    defines.Add(defineName);
                }
                else
                {
                    if (!defines.Contains(defineName))
                        return;
                    while (defines.Contains(defineName))
                    {
                        defines.Remove(defineName);
                    }
                }
                string definesString = string.Join(";", defines.ToArray());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(group, definesString);
            }
        }
        static void OnProjectChanged()
        {
            foreach (FileStructure file in files)
            {
                bool isAssetPresent = AssetDatabase.LoadAssetAtPath(file.filePath + "" + file.fileName + "" + file.fileType, typeof(UnityEngine.Object)) != null;
                if (!isAssetPresent) SetEnabled(file.fileName, false);
            }
        }
        static System.Collections.Generic.List<string> GetDefinesList(BuildTargetGroup group)
        {
            return new System.Collections.Generic.List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';'));
        }
        private static readonly BuildTargetGroup[] buildTargetGroups = new BuildTargetGroup[] { BuildTargetGroup.Android, BuildTargetGroup.iOS, };
    }

#endif

    #endregion

    #region Custom Code

    readonly string removeAdsValue = "REMOVEADS";

    public void GetRewardCustomCode()
    {
        // Write your Rewarded Code Here
    }

    #endregion
}