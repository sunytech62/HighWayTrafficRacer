Admob Mediation V_1.1.31 (20 Nov 25)
===================================
Addition:
* NativeFullScreen Priority Added (if required)
   - NativeFullScreen
   - LargeBanner
   - CustomBanner

Bug Resolve:
* Plug-in Analytics Events send to Firebase with delay through Queue.

----------------------------------:|:--------------------------------

Admob Mediation V_1.1.30 (10 Nov 25)
===================================
Addition:
* NativeOverlay impressionRecord event send to analytics.
* Interstitial Static & Rewarded Interstitial ad type consider as separate ad type and can't be shown on backfill of any ad.
* Remote ID's added
   - isInterstitialStaticLoad
   - isRewardedInterstitialLoad
   - isInterstitialStaticBackfillToInterstitial
   - isRewardedInterstitialBackfillToRewarded
* Debug added with reason when any code return.

Remove:
* AppOpen_OR_InterstitialStatic type removed.

Modify:
* SendRevenueAnalytics to AdPilot plugin with generic function from all adType of onAdPaid event.
* By Default disable Static Interstitial on backfill of Interstitial.
* By Default disable Rewarded Interstitial on backfill of Rewarded.

----------------------------------:|:--------------------------------

Admob Mediation V_1.1.29 (07 Nov 25)
===================================
Addition:
* public Action added for AppOpen Ad when OnAppStateChange with name "actionAfterAppOpenAdOnlyAppStateChange" 
* remote ID "isCustomBannerLoad" added.
* Native events added (Request Send / Failed /  Success)

Bug Resolve:
* AdCloseHandler of full screen ad not invoked when user close the ad by Android Home button. Resolved by calling required Action & remove recent ad from our available containers. 
* Only Load Native/FSN which one is required from remote.
* NFS remote value get false then actionAfterAd(false) must be invoked.
* NFS & NativeOverlay load seperately by depending upon its own ID's.
* iOS - When Foucs In/Out then OnApplicationPause execute multiple times.

----------------------------------:|:--------------------------------

Admob Mediation V_1.1.28 (21 Oct 25)
===================================
Addition:
* Plugin Exception Send to Crashlytics can be enable/disable by using remote ID "isSendAnalyticsToCrashlytics". Also Added toggle for developer to enable disable in "Other Settings" Tab. By Default Disable. 

Bug Resolve:
* Banners & Native Handlers convert from generic to indivudial handlers to resolve ANR (View.OnLayoutChangeListener).

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.27 (14 Oct 25)
===============================
Addition:
* Starting Delay atleast 2 second for reduce Vitals.
* MainThreadQueue.cs added for perform all main thread task by this script for reduce ANR.
* Introduce FirebaseAnalyticsQueue for send all Ads analytics through this queue for reduce ANR.
* Analytics calling must be from MainThreadQueue. 
* ANR calling must be from MainThreadQueue.
* Exception Send to Crashlytics is public to send from game exceptions.
* Special NativeAd Multiplyer set to iPhone16 for Native size.

Bug Resolve:
* NativeFullScreen focus In/Out sound disable issue resovled.
* Minor Bug Resolve.

Modify:
* change Dictionary type from generic Dictionary to concurrent Dictionary for all BannerData, NativeData, GeneralData for reduce deadlock & ANR.
* Remove lock keyword and its functionality.
* In Ads Handler, execution of heavy load convert into mainThread by using MainThreadQueue.
* Exception Handle on send log to Crashlytics.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.26 (08 Oct 25)
===============================
Addition:
* Send Exception's Logs to Crashlytics.

Bug Resolve:
* Deadlock handle by separate lock keywork and ExecuteInUpdate.
* Handle Exceptions.
* NativeFullScreen Focus In/Out sound issue resolved.
* NativeFullScreen Loaded but not showing Ad, only emply object shown. This issue occurs when using AdPilot plugin too.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.25 (01 Oct 25)
===============================
Addition:
* Debug Added when any Ad Type not load or show then debug give the reason regarding reason.
* Native Ad can be Destory & Reload depends upon CTR & No. of impression data, input can be fetch from remote.
  - isNativeReloadOnCTR (bool)
  - isNativeReloadOnNumberOfImpression (bool)
  - nativeReloadOnNumberOfImpressionCount  (int)

* OnStart delay feature added (optional)
* Details Debug added for gives information that if any type of ad is not shown. this debug give the reason. 
* Native event added when status = showing.

Bug Resolve:
* Every Ad calling or Ad Checking handle with exception and work as main Thread for resolving Vitals.    
* By Using AdPilot plugin, Native Ads all available Ad IDs can be shown recursively one by one.
* Index out of bound Exception Handle when Load any Adtype by using lock keywork.
* Native & NativeFullScreen can be disable by its boolean variable.

Modify:
* Native Ad structure modify for calculating number of impressions & CTR.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.24 (24 Sep 25)
===============================
Addition:
* Custom Banner Support added. (verified on Android only)
* Add Support for assign CustomBannerRect for load on Initialization.
* AdRevenueAnalytics support added for NativeFullScreen.
* AppOpen FocusInOut event added for AdPilot plugin.

Bug Resolve:
* If Native Not available to show then sent request again to load.
* Native Index bug resolve.
* bannerAd not found issue resovle in Native region.
* nativeAdType not found in Native region.
* nativeAdType same local variable name issue resolve.
    
----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.23 (17 Sep 25)
===============================
Addition:
* Native Full Screen Added with Unique AdType.
* Any Native Ad can be shown by using specific Ad ID.
* Added New Remote ID for full screen native to fetch independendly.
* isNativeOverlayFullScreenLoad
* nativeOverlayFullScreenAdIDRemoteValue

Bug Resolve:
* Native Full Screen Index issue resolved.
    
----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.22 (10 Sep 25)
===============================
Bug Resolve:
* Native Ads for iOS functionality added and resolve issues using percentage wise.
* Add Filter for Native Ads in Portrait Android Devices.

----------------------------------:|:--------------------------------


Admob 9.x.x Mediation V_1.1.21 (08 Sep 25)
===============================

Addition:
* Use IAP Script Boolean for Exception Handle when IAP Click then AppOpen should not show on OnAppStateChange. Required IAP_Plugin : v_1.0.10.
* SpecificID (optional) feature added for all AdType. Developer Can Load, Check or Show Any Ad with Specific Ad ID.
* Remote ID's Added
   - isAppOpenOnAppStateChange
   - isAppOpenOrNativeOnAppStateChange

Modify:
* Optimized Code for Ad Showing.
* Disable Native Ad for Destroy.

Bug Resolve:
* Minor Bug Resolve.
* ShowInterstitialInternal minor bug resolve because when showing interstitial any available ad type is showing.
 
----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.20 (26 Aug 25)
===============================

Addition:
* NativeOverlay show by Screen Width/Height wise ratio provided.
* isAppOpenOrNativeOnAppStateChange added for remote data.

Modify:
* NativeOverlay show by its rectObject fix issues.
* IsRemoveAd() function set to be public.
 
----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.19 (19 Aug 25)
===============================

Addition:
* NativeOverlay FullScreen & custom size Integrated.
* NativeAd properties publically available for modify developer like Style/Colors/AdType/Position etc.
* Support NativeOverlay for Android/iOS variations like multiplyer & DPI calculations.
* NativeOverlay Destory & Reload when Hide from Developer.

Bug Resolve: 
* If AdRevenueAnalytics plugin not provide List of Ad ID's then set all available ID's List.
* Diffrenciate NativeFullScreen & NativeCustom Size Ads properties.

Modify:
* AppOpen Test ID Modify.
 
----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.18 (06 Aug 25)
===============================

Bug Resolve: 
* After Full Screen Ad Show, Native Overlay Hide/Show automatically depends upon last state of Native Overlay.
* Admob AppID verifying issue resolve for Android and iOS.

Addition:
* Add GetAdIDsList function to return all Ad ID's List.

Remove:
* Remove Additional Debug.Logs.
 
----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.17 (31 Jul 25)
===============================

Addition:
* AdRevenueAnalytics feature added for prioritized Ad ID fetched from JSON and show specific Ad ID from specific filters (screenName, Mode, LevelNumber etc defined in AdRevenueAnalytics plugin v_1.1.0(31July25)
* Added Support Firebase Analytics "ad_impression_custom" for send revenue automatically.

Change:
* Modify "isDebugLog" remote value set as "False" by defult in RemoteConfig while added values.
 
----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.16 (10 Jul 25)
===============================

Addition:
* For AdRevenueAnalytics plugin add Status parameter in Ad_Rev_Progression event when confirm adShow.
* Add support of InterstitialStatic to auto load on startup.
 
----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.15 (04 Jul 25)
===============================
Bug Resolve: 
* Bug Resolve when AppOpen show on AppStateChange. Handle Multi-Netowrk Plugin too.

Addition:
* AdRevenueAnalytics plugin functionality added.
* IsRemoveAd() function added instead of checking everywhere via Prefs.
* Custom BannerPosition enum added for reduce admob issues in game code.
 
----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.14 (29 Apr 25)
===============================
Bug Resolve: 
* Native Overlay auto reload either "RemoveAds" occured or "NoInternet". 

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.13 (17 Apr 25)
===============================
Bug Resolve: 
* AppOpen show after AppStateChange only shows when required.

Addition:
* AdLoadPriority added support to load adType (AppOpen, AdaptiveBanner,SmartBanner)
* Added Custom BannerPosition function for reuse code when replacing multi networks but code should be same.

Remove:
* OnAppStateChange removed. Manage over self by using OnApplicationPause.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.12 (08 Apr 25)
===============================
Bug Resolve: 
* Native Overlay multi reference remain issue resolved.

Change:
* AppStateChange for AppOpen removed. Manager only OnApplicationPasue state by us except Admob.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.11 (20 Mar 25)
===============================
Change: 
* Native Overlay manages in LoadingAdBG show/hide.
* Adding delay for calling AppOpenAd at OnApplicationPause for handle Applovin Banner Click listerner.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.10 (06 Mar 25)
===============================
Remove: 
* Remove Revenue Event with name "ad_impression_admob" for all adType.
built-in event "ad_impression" will show event automatically. 

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.9 (04 Mar 25)
===============================
Addition: 
* Add Revenue Event with name "ad_impression_admob" for all adType.

Bug Resolve:
* Minor Bug Resolve by adding region of remoteConfig.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.8 (19 Feb 25)
===============================
Addition: 
* Support Added 'AdsManager_MultiNetwork' plugin by adding function 'AddNewAdID'
* Check from RemoteConfig for Enable/Disable Ads on Load & Show All Type of Ads.
* After SDK Initialization, Priority Ads List must Trigger. 
* AppOpen OnStateChange also call from OnApplicationFoucs.

Change:
* Try to Set the Functions Names & parameter Names as same as Other Mobify Plug-in.

Remove:
* Remove Additional Debugs for Testing.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.7 (20 Jan 25)
===============================
Addition:
* Analytics added for Show all type of Ads when action performed.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.6 (16 Jan 25)
===============================
Addition:
* Remote Values Added for Ads Enable/Disable on Initialization.
* Load Full Screen Ads (Interstitial, Rewarded & App Open) when check ad is loaded or not.

Resolve:
* Native Overlay Impression issue resolves in Meta Network. Change Calculations for AdSize.
* Native Overlay Code re-add as same as resolving Meta impression issue.

Remote:
* Hide GDPR Options from Inspector.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.5 (27 Dec 24)
===============================
Addition:
* Verifying Admob AppID and Ad IDs (on Editor Only)

Resolve:
* When Testing Mode Enable, Do not Fetch Remote Ad IDs.
* Remove GoogleMobileAds.ExecuteInUpdate in OnAdPaid Event.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.4 (24 Dec 24)
===============================
Addition:
* Rewarded/Interstitial/AppOpen Action call after delay.
* Add FirebaseAnalyticsManager code for resolve FirebaseDependencies. 
* Add DebugLog value into FirebaseRemoteValue.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.3 (19 Dec 24)
===============================
Addition:
* Custom Banner Added with New Structure Implementation.
* Add Multiplyer & Showing Percentage variable in NativeOverlay.
* Add Canvas Mode convert to ScreenSpaceOverlay when NativeOverlay show. 
* Add Analytics in NativeOverlay.
* Auto Add Remote Values into RemoteConfigPlugin_Firebase (Required version_1.0.6)

Modify:
* Modify RemoveAdsSuccess Functionality.

Remove:
* Custom Banner Previous Deprecated Code Remove.
* RemoteConfigValue removed by Inspector Access & Editor Script.

Bug Resolve:
* Minor Bug Resolve.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.1.2 (11 Dec 24)
===============================
Addition:
* Multiple Ad ID's Functionality Added.
* Native Overlay Ad Type Integrated.
* Add Remote Config Functionality to Manage Multiple ID's with Unique Keys.

Change:
* Structural Change in Plug-in.

Remove:
* Native Support Removed.

Bug Resolve:
* Minor Bug Resolve.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.0.10 (21 Nov 24)
===============================
Addition:
* Action must return for full screen Ads (Interstitial, Rewarded & AppOpen)

Change:
* IsFullScreenAdLoaded function return false if internet connection fail.

Bug Resolve:
* Banner reload top on the Interstitial / BlackBG / UTM_Full_Screen_Ad.
* BannerHolder Stuck the Full Screen Ad when banner shows after the full screen ad.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.0.9 (19 Nov 24)
===============================
Addition:
* ANR Handler & ANR Supervisor on RemoteConfig.
* MemoryThreshold added on RemoteConfig.
   - Initilization Threshold
   - Banner Threshold
   - Intestitial Threshold
   - Rewarded Threshold
* Foldout functioanlity added for visuals in Editor.

Change:
* IsBannerLoaded(bool loadIfNotLoaded=false) added as an optional parameter. 
* Change ANRHandler calling Functionality handle with or without RemoteConfig.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.0.8 (06 Nov 24)
===============================
Addition:
* RemoteConfig Value available to Developer for modify.

Change:
* Minor Bug Resolve on Backfill Interstitial

Remove:
* IsIntestitialLoaded_Internal() function removed.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.0.7 (01 Nov 24)
===============================
Addition:
* Add Support of Backfill (Interstitial & Rewarded)
* Add Support of Remote Config for Backfill Advertisement.
* Add Support of FirebaseAnalyticsManager region for send event before verify of being initialized SDK.
* Auto Request Send (Interstitial & Rewarded) when failed response from server, by using Time Inteval.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.0.6 (23 Oct 24)
===============================
Bug Resolve:
* Banner BG show centre while Banner show on corner.

Change:
* boolean ad response added in Show Rewarded & Interstitial function calling. (returns success = true, fail = false)

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.0.5 (04 Oct 24)
===============================
Addition:
* Add Support of Custom Banners (custom Size of banner as provided in a RectTransform size)
* OnInitilization Load Ad Priority set.
* All Firebase Analytics events also send to Singular Dashboard.
* CustomBanner.cs script added for easy to use Custom Banner. 

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.0.4 (23 Sep 24)
===============================
Addition:
* Add an Action in ShowInterstital, ShowRewarded, ShowRewardedInterstitial & ShowAppOpen after respective ad close (Optional Action parameter).

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.0.3 (18 Sep 24)
===============================
Modification:
* Change InterstitialStatic event name on their handler.

Remove
* Applovin Ad Type and its dependency in this plugin.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.0.2 (12 Sep 24)
===============================
Addition:
* AdShow & AdComplete built-in event register for Interstitial and Rewarded Ad Type.
* BannerBG show only when respective banner is shown.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.0.1 (26 Aug 24)
===============================
Modification:
* Change admob handler's variable name as same as using in Signular function calling.

----------------------------------:|:--------------------------------

Admob 9.x.x Mediation V_1.0.0 (15 Aug 24)
===============================
Info:
* Using Plug-in Backup Admob 8.x.x version v2.6.4
* Support of Admob 9.x.x+

Changes:
* AppOpen Orientation removed when Ad Request Send.


----------------------------------:|:--------------------------------
