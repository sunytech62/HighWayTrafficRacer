using UnityEngine;

public class HR_UI_CountDown : MonoBehaviour
{
    private void OnEnable()
    {
        // Subscribe to the OnCountDownStarted event
        HR_Events.OnCountDownStarted += HR_GamePlayHandler_OnCountDownStarted;
    }

    private void HR_GamePlayHandler_OnCountDownStarted()
    {
        Debug.LogError("Count");
        // Trigger the "Count" animation
        GetComponent<Animator>().SetTrigger("Count");
    }

    private void OnDisable()
    {
        // Unsubscribe from the OnCountDownStarted event
        HR_Events.OnCountDownStarted -= HR_GamePlayHandler_OnCountDownStarted;
    }
}
