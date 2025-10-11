using System;
using UnityEngine;

public class ModeSelectionPanel : MonoBehaviour
{
    [SerializeField] ModeRef missionMode;
    [SerializeField] ModeRef dualMode;
    [SerializeField] ModeRef endlessMode;
    [SerializeField] ModeRef challengeMode;
    [SerializeField] ModeRef policeChaseMode;

    static int modeSelected;
    public static bool IsDualModeUnlocked
    {
        get => PlayerPrefs.GetInt("IsDualModeUnlocked") != 0;
        set => PlayerPrefs.SetInt("IsDualModeUnlocked", 1);
    }
    public static bool IsEndlessModeUnlocked
    {
        get => PlayerPrefs.GetInt("IsEndlessModeUnlocked") == 1;
        set => PlayerPrefs.SetInt("IsEndlessModeUnlocked", 1);
    }
    public static bool IsChallengeModeUnlocked
    {
        get => PlayerPrefs.GetInt("IsChallengeModeUnlocked") != 0;
        set => PlayerPrefs.SetInt("IsChallengeModeUnlocked", 1);
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
        Debug.LogError(IsEndlessModeUnlocked);
        missionMode.selected.SetActive(modeSelected == 0 ? true : false);
        missionMode.unSelected.SetActive(modeSelected == 0 ? false : true);

        if (IsDualModeUnlocked)
        {
            dualMode.locker.SetActive(false);
            dualMode.selected.SetActive(modeSelected == 1 ? true : false);
            dualMode.unSelected.SetActive(modeSelected == 1 ? false : true);
        }
        else
        {
            dualMode.locker.SetActive(true);
            dualMode.selected.SetActive(false);
            dualMode.unSelected.SetActive(true);
        }

        if (IsEndlessModeUnlocked)
        {
            Debug.LogError("End UnL");
            endlessMode.locker.SetActive(false);
            endlessMode.selected.SetActive(modeSelected == 2 ? true : false);
            endlessMode.unSelected.SetActive(modeSelected == 2 ? false : true);
        }
        else
        {
            Debug.LogError("End L");
            endlessMode.locker.SetActive(true);
            endlessMode.selected.SetActive(false);
            endlessMode.unSelected.SetActive(true);
        }

        if (IsChallengeModeUnlocked)
        {
            challengeMode.locker.SetActive(false);
            challengeMode.selected.SetActive(modeSelected == 3 ? true : false);
            challengeMode.unSelected.SetActive(modeSelected == 3 ? false : true);
        }
        else
        {
            challengeMode.locker.SetActive(true);
            challengeMode.selected.SetActive(false);
            challengeMode.unSelected.SetActive(true);
        }

        if (IsPoliceChaseModeUnlocked)
        {
            policeChaseMode.locker.SetActive(false);
            policeChaseMode.selected.SetActive(modeSelected == 4 ? true : false);
            policeChaseMode.unSelected.SetActive(modeSelected == 4 ? false : true);
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
        modeSelected = index;
        UpdateUI();
    }
    public void UnlockDualMode()
    {
        IsDualModeUnlocked = true;
        UpdateUI();
    }
    public void UnlockEndlessMode()
    {
        IsEndlessModeUnlocked = true;
        UpdateUI();
    }
    public void UnlocChallengeMode()
    {
        IsChallengeModeUnlocked = true;
        UpdateUI();
    }
    public void UnlockPoliceChaseMode()
    {
        IsPoliceChaseModeUnlocked = true;
        UpdateUI();
    }

    [Serializable]
    public class ModeRef
    {
        public GameObject selected;
        public GameObject unSelected;
        public GameObject locker;
    }

}
