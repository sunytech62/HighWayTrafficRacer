using System;
using UnityEngine;
using UnityEngine.Serialization;
using static AdsManager_AdmobMediation;

public class ModeSelectionPanel : MonoBehaviour
{
    [FormerlySerializedAs("missionMode")]
    [SerializeField] ModeRef lowSpeedBombMode;
    [SerializeField] ModeRef timeTrialMode;
    [SerializeField] ModeRef endlessMode;
    [SerializeField] ModeRef challengeMode;
    [SerializeField] ModeRef policeChaseMode;

    static int modeSelected;
    public static bool IsChallengeModeUnlocked
    {
        get => PlayerPrefs.GetInt("IsChallengeModeUnlocked") != 0;
        set => PlayerPrefs.SetInt("IsChallengeModeUnlocked", 1);
    }
    public static bool IsTimeTrialUnlocked
    {
        get => PlayerPrefs.GetInt("IsTimeTrialUnlocked") != 0;
        set => PlayerPrefs.SetInt("IsTimeTrialUnlocked", 1);
    }
    public static bool IsLowSpeedBomb
    {
        get => PlayerPrefs.GetInt("IsLowSpeedBomb") == 1;
        set => PlayerPrefs.SetInt("IsLowSpeedBomb", 1);
    }
    public static bool IsPoliceChaseModeUnlocked
    {
        get => PlayerPrefs.GetInt("IsPoliceChaseModeUnlocked") != 0;
        set => PlayerPrefs.SetInt("IsPoliceChaseModeUnlocked", 1);
    }


    void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        endlessMode.selected.SetActive(GameManager.SelectedMode == GameMode.Endless ? true : false);
        endlessMode.unSelected.SetActive(GameManager.SelectedMode == GameMode.Endless ? false : true);

        if (IsChallengeModeUnlocked)
        {
            challengeMode.locker.SetActive(false);
            challengeMode.selected.SetActive(GameManager.SelectedMode == GameMode.Challenge ? true : false);
            challengeMode.unSelected.SetActive(GameManager.SelectedMode == GameMode.Challenge ? false : true);
        }
        else
        {
            challengeMode.locker.SetActive(true);
            challengeMode.selected.SetActive(false);
            challengeMode.unSelected.SetActive(true);
        }


        if (IsTimeTrialUnlocked)
        {
            timeTrialMode.locker.SetActive(false);
            timeTrialMode.selected.SetActive(GameManager.SelectedMode == GameMode.TimeTrial ? true : false);
            timeTrialMode.unSelected.SetActive(GameManager.SelectedMode == GameMode.TimeTrial ? false : true);
        }
        else
        {
            timeTrialMode.locker.SetActive(true);
            timeTrialMode.selected.SetActive(false);
            timeTrialMode.unSelected.SetActive(true);
        }

        if (IsLowSpeedBomb)
        {
            lowSpeedBombMode.locker.SetActive(false);
            lowSpeedBombMode.selected.SetActive(GameManager.SelectedMode == GameMode.Endless ? true : false);
            lowSpeedBombMode.unSelected.SetActive(GameManager.SelectedMode == GameMode.Endless ? false : true);
        }
        else
        {
            lowSpeedBombMode.locker.SetActive(true);
            lowSpeedBombMode.selected.SetActive(false);
            lowSpeedBombMode.unSelected.SetActive(true);
        }

        if (IsPoliceChaseModeUnlocked)
        {
            policeChaseMode.locker.SetActive(false);
            policeChaseMode.selected.SetActive(GameManager.SelectedMode == GameMode.PolliceChase ? true : false);
            policeChaseMode.unSelected.SetActive(GameManager.SelectedMode == GameMode.PolliceChase ? false : true);
        }
        else
        {
            policeChaseMode.locker.SetActive(true);
            policeChaseMode.selected.SetActive(false);
            policeChaseMode.unSelected.SetActive(true);
        }
    }

    public void SelectMode(int index)
    {
        GameManager.SelectedMode = index switch
        {
            0 => GameMode.Endless,
            1 => GameMode.Challenge,
            2 => GameMode.TimeTrial,
            3 => GameMode.LowSpeedBomb,
            4 => GameMode.PolliceChase,
            _ => GameMode.Endless,
        };
        UpdateUI();
        if (GameManager.SelectedMode == GameMode.Challenge)
            HR_UI_MainmenuPanel.Instance.SelectPanel(6);
        else
            HR_UI_MainmenuPanel.Instance.SelectPanel(4);

        FirebaseAnalyticsManager.SendAnalyticCus($"Selected_Mode_{GameManager.SelectedMode.ToString()}",
            $"Session_{GameManager.SessionCount}");
    }
    public void UnlockLowSpeedBombMode()
    {
        CustomAd.ShowRewarded(() =>
        {
            IsLowSpeedBomb = true;
            UpdateUI();
            FirebaseAnalyticsManager.SendAnalyticCus($"UnLocked_BombMode", $"Session_{GameManager.SessionCount}");
        });
    }
    public void UnlockChallengeMode()
    {
        CustomAd.ShowRewarded(() =>
        {
            IsChallengeModeUnlocked = true;
            UpdateUI();
            FirebaseAnalyticsManager.SendAnalyticCus($"UnLocked_ChallengeMode", $"Session_{GameManager.SessionCount}");
        });
    }
    public void UnlockTimeTrialMode()
    {
        CustomAd.ShowRewarded(() =>
        {
            IsTimeTrialUnlocked = true;
            UpdateUI();
            FirebaseAnalyticsManager.SendAnalyticCus($"UnLocked_TimeTrialMode", $"Session_{GameManager.SessionCount}");
        });
    }
    public void UnlockPoliceChaseMode()
    {
        CustomAd.ShowRewarded(() =>
        {
            IsPoliceChaseModeUnlocked = true;
            UpdateUI();
            FirebaseAnalyticsManager.SendAnalyticCus($"UnLocked_PoliceChaseMode", $"Session_{GameManager.SessionCount}");
        });
    }

    [Serializable]
    public class ModeRef
    {
        public GameObject selected;
        public GameObject unSelected;
        public GameObject locker;
    }

}
