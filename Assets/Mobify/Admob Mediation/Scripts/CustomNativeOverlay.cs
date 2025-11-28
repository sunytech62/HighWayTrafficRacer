#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class CustomNativeOverlay : MonoBehaviour
{
    #region Variables
    
    [SerializeField] AdsManager_AdmobMediation.NativeCustomType nativeType = AdsManager_AdmobMediation.NativeCustomType.PreDefined;
    [SerializeField]public AdsManager_AdmobMediation.NativeAdPosition adPosition = AdsManager_AdmobMediation.NativeAdPosition.Top;
    [SerializeField] public AdsManager_AdmobMediation.NativePreDefineSize width = AdsManager_AdmobMediation.NativePreDefineSize._50_Percent;
    [SerializeField] public AdsManager_AdmobMediation.NativePreDefineSize hight = AdsManager_AdmobMediation.NativePreDefineSize._50_Percent;

    [Header("Ads Setting")]
    [SerializeField] GoogleMobileAds.Api.MediaAspectRatio mediaAspectPlacement = GoogleMobileAds.Api.MediaAspectRatio.Any;
    [SerializeField] GoogleMobileAds.Api.AdPosition nativeAdPosition = GoogleMobileAds.Api.AdPosition.Center;
    [SerializeField] AdsManager_AdmobMediation.NativeAdType nativeAdType = AdsManager_AdmobMediation.NativeAdType.MediumRectNative;
    [SerializeField] GoogleMobileAds.Api.AdChoicesPlacement adChoice = GoogleMobileAds.Api.AdChoicesPlacement.TopRightCorner;

    [Header("Custom Colors Templates")]
    [SerializeField] Color backgroundColor = Color.white;

    [Header("Button Color")]
    [SerializeField] Color buttonBGColor = Color.green;
    [SerializeField] Color buttonBGTextColor = Color.white;

    [Header("Heading Color")]
    [SerializeField] Color headingBGColor = Color.white;
    [SerializeField] Color headingTextColor = Color.black;

    [Header("Description Color")]
    [SerializeField] Color descriptionBGColor = Color.white;
    [SerializeField] Color descriptionTextColor = Color.black;

    [Header("Additional Text Color")]
    [SerializeField] Color additionalBGColor = Color.white;
    [SerializeField] Color additionalTextColor = Color.black;

    Canvas canvas = null;
    RenderMode lastMode = RenderMode.ScreenSpaceOverlay;

    [SerializeField] bool isFullScreenNative = false;
    public string specificID = "";

    bool isSmallBannerShowingWhenNativeEnabled = false;
    bool isSmallBannerGamePlayShowingWhenNativeEnabled = false;
    bool isLargeBannerShowingWhenNativeEnabled = false;
    bool isAdaptiveBannerShowingWhenNativeEnabled = false;
    bool isSmartBannerShowingWhenNativeEnabled = false;
    bool isCustomBannerShowingWhenNativeEnabled = false;

    #endregion

    #region Built-In
    void OnEnable()
    {
        try
        {
            isSmallBannerShowingWhenNativeEnabled = AdsManager_AdmobMediation.Instance.IsBannerShowing(AdsManager_AdmobMediation.BannerType.SmallBannerType);
            isSmallBannerGamePlayShowingWhenNativeEnabled = AdsManager_AdmobMediation.Instance.IsBannerShowing(AdsManager_AdmobMediation.BannerType.SmallBannerGamePlayType);
            isLargeBannerShowingWhenNativeEnabled = AdsManager_AdmobMediation.Instance.IsBannerShowing(AdsManager_AdmobMediation.BannerType.LargeBannerType);
            isAdaptiveBannerShowingWhenNativeEnabled = AdsManager_AdmobMediation.Instance.IsBannerShowing(AdsManager_AdmobMediation.BannerType.AdaptiveBannerType);
            isSmartBannerShowingWhenNativeEnabled = AdsManager_AdmobMediation.Instance.IsBannerShowing(AdsManager_AdmobMediation.BannerType.SmartBannerType);
            isCustomBannerShowingWhenNativeEnabled = AdsManager_AdmobMediation.Instance.IsBannerShowing(AdsManager_AdmobMediation.BannerType.CustomBannerType);
            NativeFullScreenAdCustom nativeFullScreenScript = AdsManager_AdmobMediation.Instance.NativeFullScreen?.GetComponent<NativeFullScreenAdCustom>();
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
                                    if (AdsManager_AdmobMediation.Instance.IsNativeLoaded(loadIfNotLoaded: true, specificID: specificID, isFullScreenNative: true,
                                        isFullScreenPriorityCheck: false)) goto ShowNativeObject;
                                    break;
                                }
                            case NativeFullScreenAdCustom.NativeFullScreenBackFillEnum.LargeBanner:
                                {
                                    if (AdsManager_AdmobMediation.Instance.IsBannerLoaded(AdsManager_AdmobMediation.BannerType.LargeBannerType))
                                    {
                                        AdsManager_AdmobMediation.Instance.ShowBanner(AdsManager_AdmobMediation.BannerType.LargeBannerType,
                                            AdsManager_AdmobMediation.BannerPosition.Centered);
                                        return;
                                    }
                                    break;
                                }
                            case NativeFullScreenAdCustom.NativeFullScreenBackFillEnum.CustomBanner:
                                {
                                    if (AdsManager_AdmobMediation.Instance.IsBannerLoaded(AdsManager_AdmobMediation.BannerType.CustomBannerType))
                                    {
                                        AdsManager_AdmobMediation.Instance.ShowBanner(AdsManager_AdmobMediation.BannerType.CustomBannerType,
                                            AdsManager_AdmobMediation.Instance.lastCustomBannerRectTransform);
                                        return;
                                    }
                                    break;
                                }
                        }
                    }
                }
            }
            ShowNativeObject:
            if (canvas == null)
            {
                canvas = GetComponentInParent<Canvas>();
                if (canvas != null)
                {
                    if (canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                    {
                        lastMode = canvas.renderMode;
                        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    }
                }
            }

            GoogleMobileAds.Api.NativeAdOptions nativeAdOptions = new GoogleMobileAds.Api.NativeAdOptions
            {
                AdChoicesPlacement = adChoice,
                MediaAspectRatio = mediaAspectPlacement,
                VideoOptions = new GoogleMobileAds.Api.VideoOptions()
            };
            GoogleMobileAds.Api.NativeTemplateStyle nativeTempStyle = new GoogleMobileAds.Api.NativeTemplateStyle
            {
                TemplateId = GetNativeAdType(nativeAdType),
                MainBackgroundColor = backgroundColor,
                CallToActionText = new GoogleMobileAds.Api.NativeTemplateTextStyle
                {
                    BackgroundColor = buttonBGColor,
                    TextColor = buttonBGTextColor,
                    FontSize = 15,
                    Style = GoogleMobileAds.Api.NativeTemplateFontStyle.Bold
                },
                PrimaryText = new GoogleMobileAds.Api.NativeTemplateTextStyle
                {
                    BackgroundColor = headingBGColor,
                    TextColor = headingTextColor,
                    Style = GoogleMobileAds.Api.NativeTemplateFontStyle.Bold
                },
                SecondaryText = new GoogleMobileAds.Api.NativeTemplateTextStyle
                {
                    BackgroundColor = descriptionBGColor,
                    TextColor = descriptionTextColor,
                    Style = GoogleMobileAds.Api.NativeTemplateFontStyle.Normal
                },
                TertiaryText = new GoogleMobileAds.Api.NativeTemplateTextStyle
                {
                    BackgroundColor = additionalBGColor,
                    TextColor = additionalTextColor,
                    Style = GoogleMobileAds.Api.NativeTemplateFontStyle.Normal
                }
            };
            if (nativeType == AdsManager_AdmobMediation.NativeCustomType.PreDefined)
                AdsManager_AdmobMediation.Instance.ShowNative(adPosition: adPosition, adWidth: width, adHight: hight, nativeAdOption: nativeAdOptions,
                    nativeTempStyle: nativeTempStyle, isFullScreenNative: isFullScreenNative, specificID: specificID);
            else
                AdsManager_AdmobMediation.Instance.ShowNative(nativeRect: GetComponent<RectTransform>(), nativeAdOption: nativeAdOptions,
                    nativeTempStyle: nativeTempStyle, nativeAdPosition: nativeAdPosition, isFullScreenNative: isFullScreenNative, specificID: specificID);
        }
        catch (System.Exception e) { Debug.LogException(e); }
    }
    void OnDisable()
    {
        AdsManager_AdmobMediation.Instance.HideBanner(AdsManager_AdmobMediation.BannerType.LargeBannerType);
        AdsManager_AdmobMediation.Instance.HideBanner(AdsManager_AdmobMediation.BannerType.CustomBannerType);
        AdsManager_AdmobMediation.Instance.SetBannerShowing(AdsManager_AdmobMediation.BannerType.SmallBannerType, isSmallBannerShowingWhenNativeEnabled);
        AdsManager_AdmobMediation.Instance.SetBannerShowing(AdsManager_AdmobMediation.BannerType.SmallBannerGamePlayType, isSmallBannerGamePlayShowingWhenNativeEnabled);
        AdsManager_AdmobMediation.Instance.SetBannerShowing(AdsManager_AdmobMediation.BannerType.LargeBannerType, isLargeBannerShowingWhenNativeEnabled);
        AdsManager_AdmobMediation.Instance.SetBannerShowing(AdsManager_AdmobMediation.BannerType.AdaptiveBannerType, isAdaptiveBannerShowingWhenNativeEnabled);
        AdsManager_AdmobMediation.Instance.SetBannerShowing(AdsManager_AdmobMediation.BannerType.SmartBannerType, isSmartBannerShowingWhenNativeEnabled);
        AdsManager_AdmobMediation.Instance.SetBannerShowing(AdsManager_AdmobMediation.BannerType.CustomBannerType, isCustomBannerShowingWhenNativeEnabled);
        AdsManager_AdmobMediation.Instance.HideNative(isFullScreenNative: isFullScreenNative);
        if (canvas != null)
        {
            if (lastMode != RenderMode.ScreenSpaceOverlay) canvas.renderMode = lastMode;
            canvas = null;
        }
    }

    #endregion

    #region Additional
    string GetNativeAdType(AdsManager_AdmobMediation.NativeAdType nativeType)
    {
        switch (nativeType)
        {
            case AdsManager_AdmobMediation.NativeAdType.SmallBannerNative: { return GoogleMobileAds.Api.NativeTemplateId.Small; }
            case AdsManager_AdmobMediation.NativeAdType.MediumRectNative: { return GoogleMobileAds.Api.NativeTemplateId.Medium; }
        }
        return null;
    }
    #endregion

    #region Structure
    
    #endregion

    #region Editor Properties
#if UNITY_EDITOR
    [CustomEditor(typeof(CustomNativeOverlay))]
    public class AdsIDsEditor : Editor
    {
        #region SerializedProperty

        SerializedProperty nativeType;
        SerializedProperty adPosition;
        SerializedProperty width;
        SerializedProperty hight;
        SerializedProperty mediaAspectPlacement;
        SerializedProperty nativeAdPosition;
        SerializedProperty nativeAdType;
        SerializedProperty adChoice;

        SerializedProperty backgroundColor;
        SerializedProperty buttonBGColor;
        SerializedProperty buttonBGTextColor;
        SerializedProperty headingBGColor;
        SerializedProperty headingTextColor;
        SerializedProperty descriptionBGColor;
        SerializedProperty descriptionTextColor;
        SerializedProperty additionalBGColor;
        SerializedProperty additionalTextColor;

        SerializedProperty specificID;
        SerializedProperty isFullScreenNative;

        #endregion

        void OnEnable()
        {
            nativeType = serializedObject.FindProperty(nameof(CustomNativeOverlay.nativeType));
            adPosition = serializedObject.FindProperty(nameof(CustomNativeOverlay.adPosition));
            width = serializedObject.FindProperty(nameof(CustomNativeOverlay.width));
            hight = serializedObject.FindProperty(nameof(CustomNativeOverlay.hight));

            mediaAspectPlacement = serializedObject.FindProperty(nameof(CustomNativeOverlay.mediaAspectPlacement));
            nativeAdPosition = serializedObject.FindProperty(nameof(CustomNativeOverlay.nativeAdPosition));
            nativeAdType = serializedObject.FindProperty(nameof(CustomNativeOverlay.nativeAdType));
            adChoice = serializedObject.FindProperty(nameof(CustomNativeOverlay.adChoice));

            backgroundColor = serializedObject.FindProperty(nameof(CustomNativeOverlay.backgroundColor));
            buttonBGColor = serializedObject.FindProperty(nameof(CustomNativeOverlay.buttonBGColor));
            buttonBGTextColor = serializedObject.FindProperty(nameof(CustomNativeOverlay.buttonBGTextColor));
            headingBGColor = serializedObject.FindProperty(nameof(CustomNativeOverlay.headingBGColor));
            headingTextColor = serializedObject.FindProperty(nameof(CustomNativeOverlay.headingTextColor));
            descriptionBGColor = serializedObject.FindProperty(nameof(CustomNativeOverlay.descriptionBGColor));
            descriptionTextColor = serializedObject.FindProperty(nameof(CustomNativeOverlay.descriptionTextColor));
            additionalBGColor = serializedObject.FindProperty(nameof(CustomNativeOverlay.additionalBGColor));
            additionalTextColor = serializedObject.FindProperty(nameof(CustomNativeOverlay.additionalTextColor));

            specificID = serializedObject.FindProperty(nameof(CustomNativeOverlay.specificID));
            isFullScreenNative = serializedObject.FindProperty(nameof(CustomNativeOverlay.isFullScreenNative));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            // Sirf apna enum draw karo
            EditorGUILayout.PropertyField(nativeType);

            // Conditional check
            if ((AdsManager_AdmobMediation.NativeCustomType)nativeType.enumValueIndex == AdsManager_AdmobMediation.NativeCustomType.PreDefined)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(adPosition);
                EditorGUILayout.PropertyField(width);
                EditorGUILayout.PropertyField(hight);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(mediaAspectPlacement);
                EditorGUILayout.PropertyField(adChoice);
            }
            else if ((AdsManager_AdmobMediation.NativeCustomType)nativeType.enumValueIndex == AdsManager_AdmobMediation.NativeCustomType.SetAsThisRectObject)
            {
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(mediaAspectPlacement);
                EditorGUILayout.PropertyField(adChoice);
                EditorGUILayout.PropertyField(nativeAdPosition);
                EditorGUILayout.PropertyField(nativeAdType);
            }
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(backgroundColor);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(buttonBGColor);
            EditorGUILayout.PropertyField(buttonBGTextColor);
            EditorGUILayout.PropertyField(headingBGColor);
            EditorGUILayout.PropertyField(headingTextColor);
            EditorGUILayout.PropertyField(descriptionBGColor);
            EditorGUILayout.PropertyField(descriptionTextColor);
            EditorGUILayout.PropertyField(additionalBGColor);
            EditorGUILayout.PropertyField(additionalTextColor);
            EditorGUILayout.Space();
            EditorGUILayout.PropertyField(specificID); 
            EditorGUILayout.PropertyField(isFullScreenNative); 

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
#endregion
}