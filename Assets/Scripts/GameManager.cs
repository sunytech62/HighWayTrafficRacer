using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                var obj = Instantiate(Resources.Load("GameManager"));
                _instance = FindAnyObjectByType(typeof(GameManager)) as GameManager;
            }
            return _instance;
        }
    }


    [SerializeField] public GameObject challengeModeControllerPrefab;
    [SerializeField] public GameObject timeTrialPrefab;

    [Header("Loading Panel")]
    [SerializeField] GameObject loadingPanel;
    [SerializeField] Image loadingBar;

    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
#if !UNITY_EDITOR
                RCCP_Settings.Instance.mobileControllerEnabled = true;
#endif
    }

    public static int SelectedCar
    {
        get => PlayerPrefs.GetInt("SelectedCar");
        set => PlayerPrefs.SetInt("SelectedCar", value);
    }
    public static float MusicVolume
    {
        get => PlayerPrefs.GetFloat("MusicVolume", 1);
        set => PlayerPrefs.SetFloat("MusicVolume", value);
    }
    public static float AudioVolume
    {
        get => PlayerPrefs.GetFloat("AudioVolume", 1);
        set => PlayerPrefs.SetFloat("AudioVolume", value);
    }

    private static GameMode _selectedMode;

    public static GameMode SelectedMode
    {
        get
        {
            switch (PlayerPrefs.GetInt("SelectedModeIndex"))
            {
                case 0:
                    return GameMode.Endless;
                case 1:
                    return GameMode.Challenge;
                case 2:
                    return GameMode.TimeTrial;
                case 3:
                    return GameMode.LowSpeedBomb;
                case 4:
                    return GameMode.PolliceChase;
                default:
                    return GameMode.Endless;
            }
        }
        set
        {
            switch (value)
            {
                case GameMode.Endless:
                    PlayerPrefs.SetInt("SelectedModeIndex", 0);
                    break;
                case GameMode.Challenge:
                    PlayerPrefs.SetInt("SelectedModeIndex", 1);
                    break;
                case GameMode.TimeTrial:
                    PlayerPrefs.SetInt("SelectedModeIndex", 2);
                    break;
                case GameMode.LowSpeedBomb:
                    PlayerPrefs.SetInt("SelectedModeIndex", 3);
                    break;
                case GameMode.PolliceChase:
                    PlayerPrefs.SetInt("SelectedModeIndex", 4);
                    break;
            }
        }
    }

    [SerializeField] GameObject notificationPopup;
    [SerializeField] TextMeshProUGUI notificationTxt;

    public static string FormatedTextByCapitals(string str)
    {
        try
        {
            return Regex.Replace(str, "(?<=.)([A-Z])", " $1");
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            return "";
        }
    }

    public static void TimeScale(float timeScale)
    {
        Time.timeScale = timeScale;
        AudioListener.pause = timeScale == 0 ? true : false;
    }

    public void LoadGamePlayScene()
    {
        int sceneIndex = 2;
        if (GameManager.SelectedMode == GameMode.Challenge)
        {
            switch (ChallengeModeLevels.Instance.GetSelectedLevel().environment)
            {
                case HR_GamePlayManager.DayOrNight.Day:
                    sceneIndex = 2;
                    break;
                case HR_GamePlayManager.DayOrNight.Rain:
                    sceneIndex = 3;
                    break;
                case HR_GamePlayManager.DayOrNight.Night:
                    sceneIndex = 4;
                    break;
            }
        }
        else
        {
            switch (EnvSelectionPanel.SelectedEnv)
            {
                case EnvSelectionPanel.EnvNames.Sunny:
                    sceneIndex = 2;
                    break;
                case EnvSelectionPanel.EnvNames.Rainy:
                    sceneIndex = 3;
                    break;
                case EnvSelectionPanel.EnvNames.Night:
                    sceneIndex = 4;
                    break;
                case EnvSelectionPanel.EnvNames.Evening:
                    break;
            }
        }
        SceneManager.LoadSceneAsync(sceneIndex);
    }


    public void LoadingPanel(bool isActive, float timer = 3)
    {
        if (!isActive)
        {
            loadingPanel.SetActive(false);
            return;
        }
        loadingPanel.SetActive(true);
        loadingBar.fillAmount = 0;
        if (DOTween.IsTweening(loadingBar)) DOTween.Kill(loadingBar);
        loadingBar.DOFillAmount(1, timer).SetUpdate(true).SetEase(Ease.Linear);
    }

    public void ShowNotification(string str, float timeActive = 3f)
    {
        notificationPopup.SetActive(true);
        notificationTxt.text = str;
        if (notificationPopup.TryGetComponent<CanvasGroup>(out var canGr))
        {
            canGr.alpha = 0;
            canGr.DOFade(1f, 0.5f);
        }
        if (IsInvoking(nameof(HideNotification)))
            CancelInvoke(nameof(HideNotification));
        Invoke(nameof(HideNotification), timeActive);
    }
    void HideNotification()
    {
        if (notificationPopup.TryGetComponent<CanvasGroup>(out var canGr2))
        {
            canGr2.DOFade(0f, 0.5f).OnComplete(() => { notificationPopup.SetActive(false); });
        }
    }

    #region All Audio

    [Header("All Audio")]
    [SerializeField] AudioSource cashAudioClip;
    [SerializeField] AudioSource cashCountAudioClip;

    public void PlayCashAudio()
    {
        cashAudioClip.gameObject.SetActive(true);
        cashAudioClip.volume = HR_API.GetAudioVolume();
        cashAudioClip.Play();
    }

    #endregion
}
public enum GameMode
{
    Endless = 1,
    Challenge = 2,
    TimeTrial = 3,
    LowSpeedBomb = 4,
    PolliceChase = 5,
}
