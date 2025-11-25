using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class DeviceQuality : MonoBehaviour
{
    public static DeviceQuality Instance;

    [Header("Editor Calliber")]
    public DeviceCalliber editorCalliber;

    [Header("Assign URP Asset Variants (Optional)")]
    public UniversalRenderPipelineAsset lowURPAsset;
    public UniversalRenderPipelineAsset mediumURPAsset;
    public UniversalRenderPipelineAsset highURPAsset;
    public UniversalRenderPipelineAsset ultraURPAsset;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        SetDeviceQuality();
    }

    private void SetDeviceQuality()
    {
        switch (GetDeviceCalliber())
        {
            case DeviceCalliber.Lowest:
            case DeviceCalliber.Low: SetLow(); break;
            case DeviceCalliber.Med: SetMedium(); break;
            case DeviceCalliber.High: SetHigh(); break;
            case DeviceCalliber.Ultra: SetUltra(); break;
        }
    }


    // 🟢 LOW
    void SetLow()
    {
        QualitySettings.SetQualityLevel(0);
        QualitySettings.antiAliasing = 0;
        QualitySettings.pixelLightCount = 1;
        QualitySettings.shadows = UnityEngine.ShadowQuality.Disable;
        QualitySettings.shadowDistance = 20f;
        QualitySettings.globalTextureMipmapLimit = 2; // Lower texture res
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        QualitySettings.lodBias = 0.1f;

        if (lowURPAsset != null)
        {
            lowURPAsset.renderScale = 0.7f;
            GraphicsSettings.defaultRenderPipeline = lowURPAsset;
        }

        Application.targetFrameRate = 30;
        QualitySettings.vSyncCount = 0;
        Debug.LogError("Low Setting Set");
    }

    // 🟡 MEDIUM
    void SetMedium()
    {
        QualitySettings.SetQualityLevel(1);
        QualitySettings.antiAliasing = 2;
        QualitySettings.pixelLightCount = 2;
        QualitySettings.shadows = UnityEngine.ShadowQuality.HardOnly;
        QualitySettings.shadowDistance = 50f;
        QualitySettings.globalTextureMipmapLimit = 1;
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
        QualitySettings.lodBias = 0.3f;

        if (mediumURPAsset != null)
        {
            mediumURPAsset.renderScale = 0.85f;
            GraphicsSettings.defaultRenderPipeline = mediumURPAsset;
        }

        Application.targetFrameRate = 45;
        QualitySettings.vSyncCount = 0;
        Debug.LogError("Medium Setting Set");
    }

    // 🔵 HIGH
    void SetHigh()
    {
        QualitySettings.SetQualityLevel(2);
        QualitySettings.antiAliasing = 4;
        QualitySettings.pixelLightCount = 4;
        QualitySettings.shadows = UnityEngine.ShadowQuality.All;
        QualitySettings.shadowDistance = 100f;
        QualitySettings.globalTextureMipmapLimit = 0;
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
        QualitySettings.lodBias = 0.5f;

        if (highURPAsset != null)
        {
            highURPAsset.renderScale = 1f;
            GraphicsSettings.defaultRenderPipeline = highURPAsset;
        }

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 1;
        Debug.LogError("High Setting Set");
    }

    // 🔴 ULTRA
    void SetUltra()
    {
        QualitySettings.SetQualityLevel(3);
        QualitySettings.antiAliasing = 8;
        QualitySettings.pixelLightCount = 6;
        QualitySettings.shadows = UnityEngine.ShadowQuality.All;
        QualitySettings.shadowDistance = 150f;
        QualitySettings.globalTextureMipmapLimit = 0;
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
        QualitySettings.lodBias = 1f;

        if (ultraURPAsset != null)
        {
            ultraURPAsset.renderScale = 1.2f; // Slight super-sampling
            ultraURPAsset.supportsHDR = true;
            GraphicsSettings.defaultRenderPipeline = ultraURPAsset;
        }

        Application.targetFrameRate = 90;
        QualitySettings.vSyncCount = 1;
        Debug.LogError("Ultra Setting Set");
    }


    public static DeviceCalliber GetDeviceCalliber()
    {
#if UNITY_EDITOR
        return Instance.editorCalliber;
#endif
        var ram = SystemInfo.systemMemorySize;

        if (ram >= (1024 * 10)) return DeviceCalliber.Ultra;
        else if (ram >= (1024 * 8)) return DeviceCalliber.High;
        else if (ram >= (1024 * 6)) return DeviceCalliber.Med;
        else if (ram >= (1024 * 2)) return DeviceCalliber.Low;
        else return DeviceCalliber.Lowest;

    }
}

public enum DeviceCalliber
{
    Lowest = 0,
    Low = 1,
    Med = 2,
    High = 3,
    Ultra = 4,
}
