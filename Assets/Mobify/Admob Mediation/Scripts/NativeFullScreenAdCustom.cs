using UnityEngine;

public class NativeFullScreenAdCustom : MonoBehaviour
{
    #region Variables
    public NativeFullScreenBackFillClass nativeFullScreenBackFill;
    [SerializeField] UnityEngine.UI.Image Filler;
    [SerializeField] float FillerTime = 5f;
    [Space]
    [SerializeField] bool isShowRemainTimer = true;
    [SerializeField] UnityEngine.UI.Text timerTxt;
    float currentTime = 0f;
    bool isFilling = false;
    float LastTimeScale = 1;
    readonly string remoteFullScreenNativeTimerKey = "FullScreenNativeCloseButtonTimer";
    #endregion

    #region RemoteConfig
    public void FetchData()
    {
        string newTimer = "NotDefine";
#if RemoteConfigManager_Firebase
        if (RemoteConfigManager_Firebase.Instance.HasRemoteConfigValue(remoteFullScreenNativeTimerKey))
            RemoteConfigManager_Firebase.Instance.GetRemoteConfig(remoteFullScreenNativeTimerKey, RemoteConfigManager_Firebase.RemoteConfig_Type.String, out newTimer);
#endif
        if (int.TryParse(newTimer, out int newNumber)) FillerTime = newNumber;
    }
    #endregion

    #region Built-In
    void OnEnable()
    {
        ShowNativeFullAd();
    }

    void Update()
    {
        if (isFilling)
        {
            currentTime += Time.unscaledDeltaTime;
            float fillAmount = Mathf.Clamp01(currentTime / FillerTime);
            if (Filler) Filler.fillAmount = fillAmount;
            timerTxt.text = " Wait " + Mathf.FloorToInt((FillerTime - currentTime)+1).ToString() + " Seconds";
            if (fillAmount >= 1f) OnCompleteFilling();
        }
    }

    #endregion

    #region Functionality
    void StartFilling()
    {
        currentTime = 0f;
        isFilling = true;
        if (Filler) Filler.fillAmount = 0f;
    }
    void OnCompleteFilling()
    {
        isFilling = false;
        timerTxt.text = "Close";
    }
    public void OnCloseBtn()
    {
        if (isFilling == false && Filler && Filler.fillAmount >= 1)
        {
            Time.timeScale = LastTimeScale;
#if AdsManager_AdmobMediation
            AdsManager_AdmobMediation.Instance.HideNativeFullScreen();
#endif
        }
    }
    public void ShowNativeFullAd()
    {
        LastTimeScale = Time.timeScale;
        Time.timeScale = 0;
        timerTxt.gameObject.SetActive(isShowRemainTimer);
        timerTxt.text = " Wait " + Mathf.FloorToInt((FillerTime - currentTime) + 1).ToString() + " Seconds";
        StartFilling();
    }
    [ContextMenu("ShowNative")]
    void ShowNative()
    {
        ShowNativeFullAd();
    }
    #endregion

    #region Structure
    public enum NativeFullScreenBackFillEnum
    {
        NativeFullScreen,
        LargeBanner,
        CustomBanner
    }
    [System.Serializable]
    public class NativeFullScreenBackFillClass
    {
        public System.Collections.Generic.List<NativeFullScreenBackFillEnum> BackFillPriority;
        public NativeFullScreenBackFillClass() { BackFillPriority = new System.Collections.Generic.List<NativeFullScreenBackFillEnum> { NativeFullScreenBackFillEnum.NativeFullScreen }; }
    }

    #endregion
}