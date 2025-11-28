using GoogleMobileAds.Api;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CustomBanner : MonoBehaviour
{
    void OnEnable()
    {
        AdsManager_AdmobMediation.Instance.ShowBanner(AdsManager_AdmobMediation.BannerType.CustomBannerType,GetComponent<RectTransform>());
    }
    void OnDisable()
    {
         AdsManager_AdmobMediation.Instance.HideBanners();
    }
}