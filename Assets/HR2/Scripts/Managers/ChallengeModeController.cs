using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static ChallengeModeLevels;

public class ChallengeModeController : MonoBehaviour
{
    #region Fields
    public enum LevelFinishType
    {
        None = 0,
        Win = 1,
        Fail = 2,
    }


    [SerializeField] HR_Player _player;
    [SerializeField] bool _isTimerRunning;
    [SerializeField] float _timeLimit;
    [SerializeField] float _speedLimit;
    [SerializeField] float _targetValue;
    [SerializeField] int _currentNearMisses;

    [SerializeField] int challengeIndex => GameState.ChallengeLevelIndex;
    [SerializeField] ChallengeType _challengeType;

    public static UnityAction OnLevelStart;
    public static UnityAction<ChaCompleteData> OnLevelFinish;

    public bool isGameRunning;
    [SerializeField] LevelFinishType levelFinishType;

    [Header("Objective UI")]
    [SerializeField] GameObject canvasObj;
    [SerializeField] GameObject objectivePanel;
    [SerializeField] TextMeshProUGUI objectiveTitleTxt;
    [SerializeField] TextMeshProUGUI objectiveDiscTxt;

    #endregion

    #region Properties

    public float TimeElapsed /*{ get; private set; }*/;
    public float CurrentValue/* { get; private set; }*/;
    public ChallengeModeLevels.Level CurrentLevel => ChallengeModeLevels.Instance.levels[challengeIndex];

    #endregion

    #region Unity Methods

    private void Awake()
    {

    }

    private void OnEnable()
    {
        Debug.LogError("Enable");
        levelFinishType = LevelFinishType.None;
        HR_Player.OnPlayerSpawned += OnPlayerSpawned;
        HR_Player.OnNearMiss += OnNearMiss;
    }

    private void OnDisable()
    {
        HR_Player.OnPlayerSpawned -= OnPlayerSpawned;
        HR_Player.OnNearMiss -= OnNearMiss;
    }

    private void Start()
    {
        SetupLevel();
        OnLevelStart?.Invoke();

        StartCoroutine(ShowObjective());
    }

    private IEnumerator ShowObjective()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        Time.timeScale = 0;

        canvasObj.SetActive(true);
        objectiveTitleTxt.SetText($"{GameManager.FormatedTextByCapitals(CurrentLevel.challengeType.ToString())}");
        objectiveDiscTxt.SetText(CurrentLevel.levelObjective.ToString());

        yield return new WaitForSecondsRealtime(5);

        canvasObj.SetActive(false);
        Time.timeScale = 1;
        isGameRunning = true;
        HR_GamePlayManager.isStartCountDown = true;
    }

    private void Update()
    {
        if (!isGameRunning || _player == null) return;

        if (_isTimerRunning) TimeElapsed += Time.deltaTime;

        EvaluateChallenge();
    }

    #endregion

    #region Challenge Logic

    private void SetupLevel()
    {
        var level = CurrentLevel;
        _challengeType = level.challengeType;
        TimeElapsed = 0;
        _currentNearMisses = 0;

        switch (_challengeType)
        {
            case ChallengeType.Distance:
                _targetValue = level.distance * 1000f;
                break;

            case ChallengeType.Time:
                _targetValue = level.time;
                break;

            case ChallengeType.Score:
                _targetValue = level.score;
                break;

            case ChallengeType.NearMisses:
                _targetValue = level.nearMisses;
                break;

            case ChallengeType.DistanceInTime:
                _targetValue = level.distance * 1000f;
                _timeLimit = level.timeToCompleteChallenge;
                break;

            case ChallengeType.ScoreInTime:
                _targetValue = level.score;
                _timeLimit = level.timeToCompleteChallenge;
                break;

            case ChallengeType.NearMissesInTime:
                _targetValue = level.nearMisses;
                _timeLimit = level.timeToCompleteChallenge;
                break;

            case ChallengeType.DistanceInOppositeDirection:
                _targetValue = level.distance * 1000f;
                break;

            case ChallengeType.TimeInOppositeDirection:
                _timeLimit = level.timeToCompleteChallenge;
                _targetValue = _timeLimit;
                break;

            case ChallengeType.SustainSpeedForTime:
                _speedLimit = level.speedToMaintain;
                _targetValue = level.time;
                break;

            case ChallengeType.Wheelies:
                _targetValue = level.wheelies;
                break;

            case ChallengeType.WheeliesInTime:
                _targetValue = level.wheelies;
                _timeLimit = level.timeToCompleteChallenge;
                break;

            case ChallengeType.WheelieForTime:
                _targetValue = level.time;
                break;

            case ChallengeType.WheelieForTimeInDistance:
                _targetValue = level.time;
                _timeLimit = level.distance;
                break;
        }
    }

    private void OnPlayerSpawned(HR_Player player)
    {
        _player = player;
        TimeElapsed = 0;
        _isTimerRunning = true;
    }

    private void OnNearMiss(HR_Player player, int combo, HR_UI_DynamicScoreDisplayer.Side side)
    {
        _currentNearMisses++;
    }

    private void EvaluateChallenge()
    {
        switch (_challengeType)
        {
            case ChallengeType.Distance:
                CurrentValue = _player.distance * 1000f;
                levelFinishType = CurrentValue >= _targetValue ? LevelFinishType.Win : LevelFinishType.None;
                break;

            case ChallengeType.Time:
                CurrentValue = TimeElapsed;
                levelFinishType = TimeElapsed >= _targetValue ? LevelFinishType.Win : LevelFinishType.None;
                break;

            case ChallengeType.Score:
                CurrentValue = _player.score;
                levelFinishType = CurrentValue >= _targetValue ? LevelFinishType.Win : LevelFinishType.None;
                break;

            case ChallengeType.NearMisses:
                CurrentValue = _currentNearMisses;
                levelFinishType = _currentNearMisses >= _targetValue ? LevelFinishType.Win : LevelFinishType.None;
                break;

            case ChallengeType.DistanceInTime:
                CurrentValue = _player.distance * 1000f;
                levelFinishType = (CurrentValue >= _targetValue && TimeElapsed <= _timeLimit) ? LevelFinishType.Win : LevelFinishType.None;
                levelFinishType = (TimeElapsed > _timeLimit && levelFinishType != LevelFinishType.Win) ? LevelFinishType.Fail : LevelFinishType.None;
                break;

            case ChallengeType.ScoreInTime:
                CurrentValue = _player.score;
                levelFinishType = (CurrentValue >= _targetValue && TimeElapsed <= _timeLimit) ? LevelFinishType.Win : LevelFinishType.None;
                levelFinishType = (TimeElapsed > _timeLimit && levelFinishType != LevelFinishType.Win) ? LevelFinishType.Fail : LevelFinishType.None;
                break;

            case ChallengeType.NearMissesInTime:
                CurrentValue = _currentNearMisses;
                levelFinishType = (_currentNearMisses >= _targetValue && TimeElapsed <= _timeLimit) ? LevelFinishType.Win : LevelFinishType.None;
                levelFinishType = (TimeElapsed > _timeLimit && levelFinishType != LevelFinishType.Win) ? LevelFinishType.Fail : LevelFinishType.None;
                break;

            case ChallengeType.DistanceInOppositeDirection:
                CurrentValue = _player.opposideDirectionCurrent * 1000f;
                levelFinishType = CurrentValue >= _targetValue ? LevelFinishType.Win : LevelFinishType.None;
                break;

            case ChallengeType.TimeInOppositeDirection:
                /*_isTimerRunning = _player.timeLeft;
                CurrentValue = TimeElapsed;
                win = CurrentValue >= _targetValue;
                fail = TimeElapsed > _timeLimit && !win;*/
                break;

            case ChallengeType.SustainSpeedForTime:
                _isTimerRunning = _player.speed >= _speedLimit;
                CurrentValue = TimeElapsed;
                levelFinishType = CurrentValue >= _targetValue ? LevelFinishType.Win : LevelFinishType.None;
                break;

                /*  case ChallengeType.Wheelies:
                      CurrentValue = _player.W;
                      win = CurrentValue >= _targetValue;
                      break;

                  case ChallengeType.WheeliesInTime:
                      CurrentValue = _player.WheeliesCount;
                      win = CurrentValue >= _targetValue && TimeElapsed <= _timeLimit;
                      fail = TimeElapsed > _timeLimit && !win;
                      break;

                  case ChallengeType.WheelieForTime:
                      _isTimerRunning = _player.IsWheeling;
                      CurrentValue = TimeElapsed;
                      win = TimeElapsed >= _targetValue;
                      break;

                  case ChallengeType.WheelieForTimeInDistance:
                      _isTimerRunning = _player.IsWheeling;
                      if (!_player.IsWheeling) TimeElapsed = 0;
                      CurrentValue = TimeElapsed;
                      win = TimeElapsed >= _targetValue;
                      fail = _player.Distance * 1000f >= _timeLimit && !win;
                      break;*/
        }

        if (levelFinishType != LevelFinishType.None)
            FinishLevel(levelFinishType);
    }

    #endregion

    #region Result Handlers

    private void FinishLevel(LevelFinishType levelFinishType)
    {
        isGameRunning = false;
        //  ChallengeGameState.LastCompletedChallenge = _challengeIndex;
        // ChallengeGameState.ChallengesCompleted++;
        var isWin = levelFinishType == LevelFinishType.Win ? true : false;
        var finishData = new ChaCompleteData(isWin, _challengeType, CurrentLevel, CurrentValue);
        OnLevelFinish?.Invoke(finishData);
        HR_UI_GameOverPanel.Instance.OnChallengeLevelFinish(finishData);
    }

    #endregion

    public class ChaCompleteData
    {
        public bool isWin;
        public ChallengeType challengeType;
        public Level challengeLevel;
        public float currentValue;

        public ChaCompleteData(bool isWin, ChallengeType challengeType, Level challengeLevel, float currentValue)
        {
            this.isWin = isWin;
            this.challengeType = challengeType;
            this.challengeLevel = challengeLevel;
            this.currentValue = currentValue;
        }
    }
}
