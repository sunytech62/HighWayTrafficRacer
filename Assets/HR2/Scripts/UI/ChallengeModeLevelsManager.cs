using TMPro;
using UnityEngine;

public class ChallengeModeLevelsManager : GenericSingleton<ChallengeModeLevelsManager>
{
    [SerializeField] private TextMeshProUGUI levelObjective;
    [SerializeField] private GameObject levelHighlighter;
    [SerializeField] private GameObject levelStartButton;

    private void Start()
    {
        if (GameState.LastCompletedChallenge < ChallengeModeLevels.Instance.levels.Count)
        {
            SelectLevel(GameState.LastCompletedChallenge + 1);
        }
        else
        {
            // In case all levels are already played, select the last level.
            SelectLevel(GameState.LastCompletedChallenge);
        }
    }

    /*
    private void OnEnable()
    {
        HR_MainMenuHandler.bikeUpgradeCheck = false;
        HR_MainMenuHandler.Instance.RequiredBikeForChallengingLevels(GameState.LastPlayedLevel);
    }
    */

    public void SelectLevel(int levelNum)
    {
       // HR_MainMenuHandler.Instance.RequiredBikeForChallengingLevels(levelNum);
        /*levelObjective.text = ChallengeModeLevels.Instance.levels[levelNum - 1].levelObjective;
        GameState.CurrentLevel = levelNum;
        levelHighlighter.transform.position = transform.GetChild(levelNum-1).position;
        levelStartButton.SetActive(levelNum <= GameState.LastCompletedChallenge + 1);*/
        
        if (levelNum <= 0 || levelNum > ChallengeModeLevels.Instance.levels.Count)
        {
            Debug.LogError($"Invalid levelNum: {levelNum}. Must be between 1 and {ChallengeModeLevels.Instance.levels.Count}");
            return;
        }

        if (levelNum > transform.childCount)
        {
            Debug.LogError($"Invalid levelNum for UI: {levelNum}. transform has only {transform.childCount} children.");
            return;
        }

        levelObjective.text = ChallengeModeLevels.Instance.levels[levelNum - 1].levelObjective;
        GameState.CurrentLevel = levelNum;
        levelHighlighter.transform.position = transform.GetChild(levelNum - 1).position;
        levelStartButton.SetActive(levelNum <= GameState.LastCompletedChallenge + 1);
    }
}