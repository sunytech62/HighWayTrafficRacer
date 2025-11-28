using UnityEngine;

public class TestingAdmobMediation : MonoBehaviour
{
    #region Instance
    AdsManager_AdmobMediation adsManager = null;
    AdsManager_AdmobMediation AdsManager
    {
        get
        {
            if (adsManager == null)
                adsManager = GetComponent<AdsManager_AdmobMediation>();
            return adsManager;
        }
    }
    #endregion

    #region Banner
    public void ShowSmallBanner()
    {
        AdsManager.ShowBanner(AdsManager_AdmobMediation.BannerType.SmallBannerType, GoogleMobileAds.Api.AdPosition.Top);
    }
    public void ShowSmallBanner(RectTransform rectTransform)
    {
        rectTransform.gameObject.SetActive(true);
        AdsManager.ShowBanner(AdsManager_AdmobMediation.BannerType.SmallBannerType, rectTransform);
    }
    public void HideSmallBanner(RectTransform rectTransform)
    {
        AdsManager.HideBanners();
        rectTransform.gameObject.SetActive(false);
    }
    public void HideSmallBanner()
    {
        AdsManager.HideBanners();
    }

    public void ShowLargeBanner()
    {
        AdsManager.ShowBanner(AdsManager_AdmobMediation.BannerType.LargeBannerType, GoogleMobileAds.Api.AdPosition.TopLeft);
    }
    public void HideLargeBanner()
    {
        AdsManager.HideBanners();
    }
    public void ShowLargeBanner(RectTransform rectTransform)
    {
        rectTransform.gameObject.SetActive(true);
        AdsManager.ShowBanner(AdsManager_AdmobMediation.BannerType.LargeBannerType, rectTransform);
    }
    public void HideLargeBanner(RectTransform rectTransform)
    {
        AdsManager.HideBanners();
        rectTransform.gameObject.SetActive(false);
    }

    public void ShowAdaptiveBanner()
    {
        AdsManager.ShowBanner(AdsManager_AdmobMediation.BannerType.AdaptiveBannerType, GoogleMobileAds.Api.AdPosition.Bottom);
    }
    public void HideAdaptiveBanner()
    {
        AdsManager.HideBanners();
    }
    public void ShowAdaptiveBanner(RectTransform rectTransform)
    {
        rectTransform.gameObject.SetActive(true);
        AdsManager.ShowBanner(AdsManager_AdmobMediation.BannerType.AdaptiveBannerType, rectTransform);
    }
    public void HideAdaptiveBanner(RectTransform rectTransform)
    {
        AdsManager.HideBanners();
        rectTransform.gameObject.SetActive(false);
    }

    public void ShowSmartBanner()
    {
        AdsManager.ShowBanner(AdsManager_AdmobMediation.BannerType.SmartBannerType, GoogleMobileAds.Api.AdPosition.Top);
    }
    public void HideSmartBanner()
    {
        AdsManager.HideBanners();
    }
    public void ShowSmartBanner(RectTransform rectTransform)
    {
        rectTransform.gameObject.SetActive(true);
        AdsManager.ShowBanner(AdsManager_AdmobMediation.BannerType.SmartBannerType, rectTransform);
    }
    public void HideSmartBanner(RectTransform rectTransform)
    {
        AdsManager.HideBanners();
        rectTransform.gameObject.SetActive(false);
    }

    #endregion

    #region Native

    [SerializeField] GameObject nativeGameObject;
    [SerializeField] UnityEngine.UI.Image mainBG_BG;
    [SerializeField] UnityEngine.UI.Image callToAction_BG;
    [SerializeField] UnityEngine.UI.Image callToAction_TextBG;
    [SerializeField] UnityEngine.UI.Image heading_BG;
    [SerializeField] UnityEngine.UI.Image heading_TextBG;
    [SerializeField] UnityEngine.UI.Image description_BG;
    [SerializeField] UnityEngine.UI.Image description_TextBG;
    [SerializeField] UnityEngine.UI.Image other_BG;
    [SerializeField] UnityEngine.UI.Image other_TextBG;
    [SerializeField] UnityEngine.UI.Text multiplyerTxt;
    [SerializeField] UnityEngine.UI.Text dpiTxt;
       
    public void ChangeColor(UnityEngine.UI.Image img)
    {
        img.color = new Color(Random.Range(0.1f, 1), Random.Range(0.1f, 1), Random.Range(0.1f, 1),1);
    }       
    public void LoadNative(RectTransform rectTransform)
    {
        AdsManager_AdmobMediation.Instance.LoadNativeInternal(rectTransform);
    }
    public void ShowNative()
    {       
        AdsManager_AdmobMediation.Instance.ShowNative(nativeAdPositionCustom, nativeAdWidth, nativeAdHight);
    }
    public void ShowNativeFullScreen()
    {
        AdsManager_AdmobMediation.Instance.ShowNativeFullScreen(actionAfterAd: (success) => { });
    }

    public void HideNative()
    {
        AdsManager_AdmobMediation.Instance.HideNative(isFromFocus:true);
    }
    public void HideNativeAndDestroy()
    {
        AdsManager_AdmobMediation.Instance.HideNative();
    }

    public void IsNativeLoaded()
    {
        Debug.Log("Native Loaded : "+ AdsManager_AdmobMediation.Instance.IsNativeLoaded());
    }

    AdsManager_AdmobMediation.NativeAdPosition nativeAdPositionCustom = AdsManager_AdmobMediation.NativeAdPosition.Top;
    public void NativeCustomAdPosition(int index)
    {
        nativeAdPositionCustom = (AdsManager_AdmobMediation.NativeAdPosition)index+1;
        Debug.Log("Change Native Ad Position : " + nativeAdPositionCustom.ToString());
        nativeGameObject.GetComponent<CustomNativeOverlay>().adPosition = nativeAdPositionCustom;
    }
    AdsManager_AdmobMediation.NativePreDefineSize nativeAdWidth = AdsManager_AdmobMediation.NativePreDefineSize._100_Percent;
    public void NativeCustomAdWidth(int index)
    {
        nativeAdWidth = 10 - (AdsManager_AdmobMediation.NativePreDefineSize)index;
        Debug.Log("Change Native Ad Width : " + nativeAdWidth.ToString());
        nativeGameObject.GetComponent<CustomNativeOverlay>().width = nativeAdWidth;
    }
    AdsManager_AdmobMediation.NativePreDefineSize nativeAdHight = AdsManager_AdmobMediation.NativePreDefineSize._100_Percent;
    public void NativeCustomAdHight(int index)
    {
        nativeAdHight = 10 - (AdsManager_AdmobMediation.NativePreDefineSize)index;
        Debug.Log("Change Native Ad Hight : " + nativeAdHight.ToString());
        nativeGameObject.GetComponent<CustomNativeOverlay>().hight = nativeAdHight;
    }

    #endregion

    #region Interstitial

    public void LoadInterstitial()
    {
        AdsManager.LoadInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitial);
    }
    public void ShowInterstitial()
    {
        AdsManager.ShowInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitial, 
            actionAfterAd: (complete) => 
            {
                if (complete) Debug.Log("Interstitial Complted");
                else Debug.Log("Interstitial Not Available");
            });
    }

    public void LoadInterstitialStatic()
    {
        AdsManager.LoadInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitialStatic);
    }
    public void ShowInterstitialStatic()
    {
        AdsManager.ShowInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitialStatic,
            actionAfterAd: (complete) =>
            {
                if (complete) Debug.Log("Interstitial Static Complted");
                else Debug.Log("Interstitial Static Not Available");
            });
    }

    #endregion

    #region Rewarded

    public void LoadRewarded()
    {
        AdsManager.LoadRewardedAd(AdsManager_AdmobMediation.RewardedType.rewardedVideo);
    }
    public void ShowRewarded()
    {
        AdsManager.ShowRewardedAd(actionAfterAd:(success) => 
        {
            if(success) Debug.Log("We Got Reward");
            else Debug.Log("No Reward Available"); 
        });
    }
    public void LoadRewardedInterstitial()
    {
        AdsManager.LoadRewardedAd(AdsManager_AdmobMediation.RewardedType.rewardedInterstitial);
    }
    public void ShowRewardedInterstitial()
    {
        AdsManager.ShowRewardedAd(rewardedType:AdsManager_AdmobMediation.RewardedType.rewardedInterstitial, actionAfterAd: (success) =>
        {
            if (success)
                Debug.Log("We Got Reward");
            else
                Debug.Log("No Reward Available");
        });
    }
    #endregion

    #region App-Open

    public void LoadAppOpenAd()
    {
        AdsManager.LoadAppOpenAd();
    }
    public void ShowAppOpenAd()
    {
        AdsManager.ShowAppOpenAd(actionAfterAd: (success) =>
        {
            if (success)
                Debug.Log("AppOpen Close Successfully");
            else
                Debug.Log("No AppOpen Available");
        });
    }

    #endregion

    #region Others
    public void ShowAdInspector()
    {
        AdsManager.ShowAdInspector();
    }
    public void DebugSomething(string msg)
    {
        Debug.Log(msg);
    }
    #endregion
}