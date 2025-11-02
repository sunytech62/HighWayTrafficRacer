using DG.Tweening;
using System;
using System.Text.RegularExpressions;
using UnityEngine;
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
                DontDestroyOnLoad(obj);
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
        /*        RCCP_Settings.Instance.mobileControllerEnabled = true;
        #if !UNITY_EDITOR
                RCCP_Settings.Instance.mobileControllerEnabled = true;
        #endif*/
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

    public void LoadingPanel(bool isActive, float timer = 3)
    {
        if (!isActive)
        {
            loadingPanel.SetActive(false);
            return;
        }
        loadingPanel.SetActive(true);
        loadingBar.fillAmount = 0;
        loadingBar.DOFillAmount(1, timer).SetUpdate(true);
    }
}
public enum GameMode
{
    Endless = 1,
    Challenge = 2,
    TimeTrial = 3,
    LowSpeedBomb = 4,
    PolliceChase = 5,
}
