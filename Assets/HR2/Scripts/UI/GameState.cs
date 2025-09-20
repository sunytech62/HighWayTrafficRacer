using UnityEngine;

public static class GameState
{
    // Currently selected challenge level
    public static int CurrentLevel = 1;
    // Last completed challenge level
    public static int LastCompletedChallenge
    {
        get => PlayerPrefs.GetInt("LastCompletedChallenge", 0);
        set => PlayerPrefs.SetInt("LastCompletedChallenge", value);
    }

    // Total number of challenges completed
    public static int ChallengesCompleted
    {
        get => PlayerPrefs.GetInt("ChallengesCompleted", 0);
        set => PlayerPrefs.SetInt("ChallengesCompleted", value);
    }

    // Whether Challenge Mode is unlocked
    public static bool ChallengeModeUnlocked
    {
        get => PlayerPrefs.GetInt("ChallengeModeUnlocked", 0) == 1;
        set => PlayerPrefs.SetInt("ChallengeModeUnlocked", value ? 1 : 0);
    }

    // Store number of stars (0 to 3) for each challenge
    public static int GetChallengeStars(int index)
    {
        return PlayerPrefs.GetInt($"ChallengeStars_{index}", 0);
    }

    public static void SetChallengeStars(int index, int stars)
    {
        PlayerPrefs.SetInt($"ChallengeStars_{index}", stars);
    }
    
    // Reset all challenge-related data (use carefully)
    public static void ResetAllChallengeData()
    {
        PlayerPrefs.DeleteKey("LastCompletedChallenge");
        PlayerPrefs.DeleteKey("ChallengesCompleted");
        PlayerPrefs.DeleteKey("ChallengeModeUnlocked");

        for (int i = 0; i < 30; i++) // If you have 30 levels
        {
            PlayerPrefs.DeleteKey($"ChallengeStars_{i}");
            PlayerPrefs.DeleteKey($"ChallengeRewardCollected_{i}");
        }
    }
}
