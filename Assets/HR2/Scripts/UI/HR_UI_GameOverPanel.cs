using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class HR_UI_GameOverPanel : MonoBehaviour
{
    public static HR_UI_GameOverPanel Instance;

    [Header("UI Texts On Scoreboard")]
    public PanelRefs levelComplete;
    public PanelRefs levelFailed;
    public PanelRefs gameOver;

    ChallengeModeController.ChaCompleteData chaCompleteData;

    private void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    private void OnEnable()
    {
        HR_Events.OnPlayerDied += HR_PlayerHandler_OnPlayerDied;
    }

    private void OnDisable()
    {
        HR_Events.OnPlayerDied -= HR_PlayerHandler_OnPlayerDied;
    }
    public void OnChallengeLevelFinish(ChallengeModeController.ChaCompleteData arg2)
    {
        Debug.LogError("ChallengeCom");
        if (arg2.isWin)
        {
            if ((GameState.ChallengeLevelIndex + 1) == GameState.ChallengeCompletedLevel + 1)
            {
                GameState.ChallengeCompletedLevel += 1;
            }
            levelComplete.panelObj.SetActive(true);
            levelFailed.panelObj.SetActive(false);
        }
        else
        {
            levelComplete.panelObj.SetActive(false);
            levelFailed.panelObj.SetActive(true);
        }

        chaCompleteData = arg2;

        HR_Player.Instance.GameOver();

        switch (arg2.challengeLevel.challengeType)
        {
            case ChallengeType.Distance:
                break;
            case ChallengeType.DistanceInTime:
                break;
            case ChallengeType.Time:
                break;
            case ChallengeType.Score:
                break;
            case ChallengeType.ScoreInTime:
                break;
            case ChallengeType.NearMisses:
                break;
            case ChallengeType.NearMissesInTime:
                break;
            case ChallengeType.DistanceInOppositeDirection:
                break;
            case ChallengeType.TimeInOppositeDirection:
                break;
            case ChallengeType.SustainSpeedForTime:
                break;
            case ChallengeType.Wheelies:
                break;
            case ChallengeType.WheeliesInTime:
                break;
            case ChallengeType.WheelieForTime:
                break;
            case ChallengeType.WheelieForTimeInDistance:
                break;
        }
    }

    private void HR_PlayerHandler_OnPlayerDied(HR_Player player, int[] scores)
    {
        StartCoroutine(DisplayResultGameOver(player, scores));
    }

    public IEnumerator DisplayResultGameOver(HR_Player player, int[] scores)
    {
        yield return new WaitForSecondsRealtime(1f);

        if (GameManager.SelectedMode == GameMode.Challenge)
        {
            ChallengePanelShow(player, scores);
            yield break;
        }

        gameOver.panelObj.SetActive(true);

        gameOver.totalScore.text = Mathf.Floor(player.score).ToString("F0");

        gameOver.distance.num.text = (player.distance).ToString("F2");
        gameOver.distance.money.text = scores[0].ToString("F0");

        gameOver.nearMiss.num.text = (player.nearMisses).ToString("F0");
        gameOver.nearMiss.money.text = scores[1].ToString("F0");

        gameOver.overspeed.num.text = (player.highSpeedTotal).ToString("F1");
        gameOver.overspeed.money.text = scores[2].ToString("F0");

        gameOver.oppositeDirection.num.text = (player.opposideDirectionTotal).ToString("F1");
        gameOver.oppositeDirection.money.text = scores[3].ToString("F0");

        gameOver.totalMoney.text = (scores[0] + scores[1] + scores[2] + scores[3]).ToString();

        /*  foreach (var item in totalScore)
              item.text = Mathf.Floor(player.score).ToString("F0");

          foreach (var item in totalDistance)
              item.text = (player.distance).ToString("F2");

          foreach (var item in totalNearMiss)
              item.text = (player.nearMisses).ToString("F0");

          foreach (var item in totalOverspeed)
              item.text = (player.highSpeedTotal).ToString("F1");

          foreach (var item in totalOppositeDirection)
              item.text = (player.opposideDirectionTotal).ToString("F1");

          foreach (var item in totalDistanceMoney)
              item.text = scores[0].ToString("F0");

          foreach (var item in totalNearMissMoney)
              item.text = scores[1].ToString("F0");

          foreach (var item in totalOverspeedMoney)
              item.text = scores[2].ToString("F0");

          foreach (var item in totalOppositeDirectionMoney)
              item.text = scores[3].ToString("F0");

          foreach (var item in totalMoney)
              item.text = (scores[0] + scores[1] + scores[2] + scores[3]).ToString();

          // gameObject.BroadcastMessage("Animate");
          // gameObject.BroadcastMessage("GetNumber");
        */
    }

    private void ChallengePanelShow(HR_Player player, int[] scores)
    {
        if (chaCompleteData.isWin)
        {
            levelComplete.panelObj.SetActive(true);

            levelComplete.totalScore.transform.parent.gameObject.SetActive(chaCompleteData.challengeType == ChallengeType.Score);

            levelComplete.totalScore.text = Mathf.Floor(player.score).ToString("F0");

            levelComplete.distance.num.text = (player.distance).ToString("F2");
            levelComplete.distance.money.text = scores[0].ToString("F0");

            levelComplete.nearMiss.num.text = (player.nearMisses).ToString("F0");
            levelComplete.nearMiss.money.text = scores[1].ToString("F0");

            levelComplete.overspeed.num.text = (player.highSpeedTotal).ToString("F1");
            levelComplete.overspeed.money.text = scores[2].ToString("F0");

            levelComplete.oppositeDirection.num.text = (player.opposideDirectionTotal).ToString("F1");
            levelComplete.oppositeDirection.money.text = scores[3].ToString("F0");

            levelComplete.totalMoney.text = (scores[0] + scores[1] + scores[2] + scores[3]).ToString();
        }
        else
        {
            levelFailed.panelObj.SetActive(true);

            levelComplete.totalScore.transform.parent.gameObject.SetActive(chaCompleteData.challengeType == ChallengeType.Score);
            levelFailed.totalScore.text = Mathf.Floor(player.score).ToString("F0");

            levelFailed.distance.num.text = (player.distance).ToString("F2");
            levelFailed.distance.money.text = scores[0].ToString("F0");

            levelFailed.nearMiss.num.text = (player.nearMisses).ToString("F0");
            levelFailed.nearMiss.money.text = scores[1].ToString("F0");

            levelFailed.overspeed.num.text = (player.highSpeedTotal).ToString("F1");
            levelFailed.overspeed.money.text = scores[2].ToString("F0");

            levelFailed.oppositeDirection.num.text = (player.opposideDirectionTotal).ToString("F1");
            levelFailed.oppositeDirection.money.text = scores[3].ToString("F0");

            levelFailed.totalMoney.text = (scores[0] + scores[1] + scores[2] + scores[3]).ToString();
        }
    }

    public void MainMenu()
    {
        HR_API.MainMenu();
    }

    public void Restart()
    {
        HR_API.RestartGame();
    }




    [ContextMenu("Set References")]
    void SetReferences()
    {

    }

    [Serializable]
    public class PanelRefs
    {
        public GameObject panelObj;

        public TextMeshProUGUI panelTitle;
        public TextMeshProUGUI panelDisc;

        public TextMeshProUGUI totalScore;
        public TextMeshProUGUI totalMoney;

        public Txt distance;
        public Txt nearMiss;
        public Txt overspeed;
        public Txt oppositeDirection;

        [Serializable]
        public class Txt
        {
            // public GameObject parent;
            public TextMeshProUGUI num;
            public TextMeshProUGUI money;
        }
    }
}
