using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_FirebaseAnalytics : MonoBehaviour
{
    public void LevelSend(int level)
    {
        FirebaseAnalyticsManager.Instance.SendAnalytics("OnlyLevel_"+level);
    }
    public void LevelWithParameter(int level)
    {
        FirebaseAnalyticsManager.Instance.SendAnalytics("LevelWithParam", "LevelNumber",level.ToString());
    }

    public void LevelWithDictionary(int level)
    {
        Dictionary<string,object> dict = new Dictionary<string, object>
        {
            { "LevelDictionary", level },
            { "Mode", "NewMode" },
            { "Health", 80 }
        };
        FirebaseAnalyticsManager.Instance.SendAnalytics("LevelDictionary", dict);
    }
}