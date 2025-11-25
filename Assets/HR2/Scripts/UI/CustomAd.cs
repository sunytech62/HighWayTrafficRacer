using System;

public static class CustomAd
{


    public static void ShowInterstitial()
    {
        AdsManager_AdmobMediation.Instance?.ShowInterstitial(AdsManager_AdmobMediation.InterstitialType.interstitial);
    }

    public static void ShowRewarded(Action action)
    {
        AdsManager_AdmobMediation.Instance?.ShowRewardedAd(AdsManager_AdmobMediation.RewardedType.rewardedVideo, (success) =>
        {
            if (success)
            {
                action?.Invoke();
            }
        });
    }
}
