using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class ChallengeLevelSelection : MonoBehaviour
{
    [SerializeField] LevelSelectionButton[] levelSelectionButtons;

    [SerializeField] private TextMeshProUGUI levelTitle;
    [SerializeField] private TextMeshProUGUI levelDisc;
    [SerializeField] private TextMeshProUGUI levelNum;
    [SerializeField] private TextMeshProUGUI levelReward;
    [SerializeField] private GameObject playBtn;

    private void Start()
    {
        UpdateLevelsUI();
        UpdateLevelSelectedUI(GameState.ChallengeCompletedLevel);
    }

    private void UpdateLevelsUI()
    {
        var completedLvls = GameState.ChallengeCompletedLevel;
        for (var i = 0; i < levelSelectionButtons.Length; i++)
        {
            levelSelectionButtons[i].lockPanel.SetActive(i > (completedLvls));
            levelSelectionButtons[i].levelIndex = i;
            levelSelectionButtons[i].lvlNumTxt.SetText((i + 1).ToString());
            levelSelectionButtons[i].challengeModeController = this;
        }
    }
    private void UpdateLevelSelectedUI(int levelIndex)
    {
        GameState.ChallengeLevelIndex = levelIndex;
        playBtn.SetActive(levelIndex <= GameState.ChallengeCompletedLevel);

        levelNum.SetText($"{levelIndex + 1}/{ChallengeModeLevels.Instance.levels.Count}");
        levelTitle.SetText($"{GameManager.FormatedTextByCapitals(ChallengeModeLevels.Instance.levels[levelIndex].challengeType.ToString())}");
        levelDisc.SetText($"{ChallengeModeLevels.Instance.levels[levelIndex].levelObjective}");
        levelReward.SetText($"{ChallengeModeLevels.Instance.levels[levelIndex].levelWinCoins}");
    }

    /*
    private void OnEnable()
    {
        HR_MainMenuHandler.bikeUpgradeCheck = false;
        HR_MainMenuHandler.Instance.RequiredBikeForChallengingLevels(GameState.LastPlayedLevel);
    }
    */

    public void SelectLevel(int levelIndex)
    {
        if (levelIndex < 0 || levelIndex > ChallengeModeLevels.Instance.levels.Count)
        {
            Debug.LogError($"Invalid levelNum: {levelIndex}. Must be between 1 and {ChallengeModeLevels.Instance.levels.Count}");
            return;
        }

        UpdateLevelSelectedUI(levelIndex);
    }
    public void Play()
    {
        HR_UI_MainmenuPanel.Instance.StartRace();
    }
}