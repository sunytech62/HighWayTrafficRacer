using System;
using UnityEngine;
using UnityEngine.Events;

public class ChallengeModeController : MonoBehaviour
{
    #region Fields

    private HR_Player _player;
    private bool _isTimerRunning;
    private float _timeLimit;
    private float _speedLimit;
    private float _targetValue;
    private int _currentNearMisses;

    private int _challengeIndex;
    private ChallengeType _challengeType;

    public static UnityAction OnLevelStart;
    public static UnityAction<HR_Player> OnLevelWin;
    public static UnityAction<HR_Player> OnLevelFail;

    public bool isGameRunning;

    #endregion

    #region Properties

    public float TimeElapsed { get; private set; }
    public float CurrentValue { get; private set; }
    public ChallengeModeLevels.Level CurrentLevel => ChallengeModeLevels.Instance.levels[_challengeIndex];

    #endregion

    #region Unity Methods

    private void Awake()
    {
        _challengeIndex = GameState.CurrentLevel;
    }

    private void OnEnable()
    {
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
    }

    private void Update()
    {
        if (!isGameRunning || _player == null)
            return;

        if (_isTimerRunning)
            TimeElapsed += Time.deltaTime;

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
        bool win = false, fail = false;

        switch (_challengeType)
        {
            case ChallengeType.Distance:
                CurrentValue = _player.distance * 1000f;
                win = CurrentValue >= _targetValue;
                break;

            case ChallengeType.Time:
                CurrentValue = TimeElapsed;
                win = TimeElapsed >= _targetValue;
                break;

            case ChallengeType.Score:
                CurrentValue = _player.score;
                win = CurrentValue >= _targetValue;
                break;

            case ChallengeType.NearMisses:
                CurrentValue = _currentNearMisses;
                win = _currentNearMisses >= _targetValue;
                break;

            case ChallengeType.DistanceInTime:
                CurrentValue = _player.distance * 1000f;
                win = CurrentValue >= _targetValue && TimeElapsed <= _timeLimit;
                fail = TimeElapsed > _timeLimit && !win;
                break;

            case ChallengeType.ScoreInTime:
                CurrentValue = _player.score;
                win = CurrentValue >= _targetValue && TimeElapsed <= _timeLimit;
                fail = TimeElapsed > _timeLimit && !win;
                break;

            case ChallengeType.NearMissesInTime:
                CurrentValue = _currentNearMisses;
                win = _currentNearMisses >= _targetValue && TimeElapsed <= _timeLimit;
                fail = TimeElapsed > _timeLimit && !win;
                break;

            case ChallengeType.DistanceInOppositeDirection:
                CurrentValue = _player.opposideDirectionCurrent * 1000f;
                win = CurrentValue >= _targetValue;
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
                win = CurrentValue >= _targetValue;
                break;

            /*case ChallengeType.Wheelies:
                CurrentValue = _player.W;
                win = CurrentValue >= _targetValue;
                break;*/

            /*case ChallengeType.WheeliesInTime:
                CurrentValue = _player.WheeliesCount;
                win = CurrentValue >= _targetValue && TimeElapsed <= _timeLimit;
                fail = TimeElapsed > _timeLimit && !win;
                break;*/

            /*case ChallengeType.WheelieForTime:
                _isTimerRunning = _player.IsWheeling;
                CurrentValue = TimeElapsed;
                win = TimeElapsed >= _targetValue;
                break;*/

            /*case ChallengeType.WheelieForTimeInDistance:
                _isTimerRunning = _player.IsWheeling;
                if (!_player.IsWheeling) TimeElapsed = 0;
                CurrentValue = TimeElapsed;
                win = TimeElapsed >= _targetValue;
                fail = _player.Distance * 1000f >= _timeLimit && !win;
                break;*/
        }

        if (win)
            HandleLevelWin();
        else if (fail)
            HandleLevelFail();
    }

    #endregion

    #region Result Handlers

    private void HandleLevelWin()
    {
        isGameRunning = false;
      //  ChallengeGameState.LastCompletedChallenge = _challengeIndex;
       // ChallengeGameState.ChallengesCompleted++;

        OnLevelWin?.Invoke(_player);
      //  _player.;
    }

    private void HandleLevelFail()
    {
        isGameRunning = false;
        OnLevelFail?.Invoke(_player);
      //  _player.DisableBike();
    }

    #endregion
}
