using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
[CreateAssetMenu(menuName = "HighwayRacer/Challenge Mode Levels", fileName = "Challenge Mode Levels")]
public class ChallengeModeLevels : ScriptableObject
{
    private static ChallengeModeLevels _instance;
    public static ChallengeModeLevels Instance
    {
        get
        {
            if (_instance == null)
                _instance = Resources.Load<ChallengeModeLevels>("Challenge Mode Levels/Challenge Mode Levels");
            return _instance;
        }
    }

    [System.Serializable]
    public class Level
    {
        public string levelName = "";
        public string levelObjective = "";
        public float levelWinCoins;
        public ChallengeType challengeType;
        public HR_TrafficManager.TrafficCars trafficMode;
        public HR_GamePlayManager.DayOrNight environment;

        [Tooltip("Minimum distance to complete the challenge")]
     
        public float distance;

        [Tooltip("Time duration for the challenge")]
       
        public float time;

        [Tooltip("Score target to complete the challenge")]
       
        public float score;

        [Tooltip("Speed to maintain during the challenge")]
      
        public float speedToMaintain;

        [Tooltip("Number of near misses required")]
        public int nearMisses;

        [Tooltip("Number of wheelies required")]
        public int wheelies;

        [Tooltip("Time limit to complete the challenge")]
        public float timeToCompleteChallenge;

        public bool HasTimeLimit => ShowTimeToComplete();

        private bool ShowDistance()
        {
            return challengeType == ChallengeType.Distance ||
                   challengeType == ChallengeType.DistanceInTime ||
                   challengeType == ChallengeType.DistanceInOppositeDirection ||
                   challengeType == ChallengeType.WheelieForTimeInDistance;
        }

        private bool ShowTime()
        {
            return challengeType == ChallengeType.Time ||
                   challengeType == ChallengeType.WheelieForTime ||
                   challengeType == ChallengeType.SustainSpeedForTime ||
                   challengeType == ChallengeType.WheelieForTimeInDistance ||
                   challengeType == ChallengeType.TimeInOppositeDirection;
        }

        private bool ShowScore()
        {
            return challengeType == ChallengeType.Score ||
                   challengeType == ChallengeType.ScoreInTime;
        }

        private bool ShowSpeed()
        {
            return challengeType == ChallengeType.SustainSpeedForTime;
        }

        private bool ShowNearMisses()
        {
            return challengeType == ChallengeType.NearMisses ||
                   challengeType == ChallengeType.NearMissesInTime;
        }

        private bool ShowWheelies()
        {
            return challengeType == ChallengeType.Wheelies ||
                   challengeType == ChallengeType.WheeliesInTime;
        }

        private bool ShowTimeToComplete()
        {
            return challengeType == ChallengeType.DistanceInTime ||
                   challengeType == ChallengeType.ScoreInTime ||
                   challengeType == ChallengeType.NearMissesInTime ||
                   challengeType == ChallengeType.WheeliesInTime;
        }
    }
   
    public List<Level> levels = new List<Level>();
}

public enum ChallengeType
{
    Distance,
    DistanceInTime,
    Time,
    Score,
    ScoreInTime,
    NearMisses,
    NearMissesInTime,
    DistanceInOppositeDirection,
    TimeInOppositeDirection,
    SustainSpeedForTime,
    Wheelies,
    WheeliesInTime,
    WheelieForTime,
    WheelieForTimeInDistance,
}
