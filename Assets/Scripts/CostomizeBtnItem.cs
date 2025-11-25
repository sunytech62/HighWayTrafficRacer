using UnityEngine;
using UnityEngine.UI;

public class CostomizeBtnItem : RCCP_UIComponent
{
    [Min(0)] public int wheelIndex = 0;

    private void Awake()
    {
        wheelIndex = transform.GetSiblingIndex();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        RCCP_CarController playerVehicle = RCCPSceneManager.activePlayerVehicle;

        if (!playerVehicle) return;

        if (!playerVehicle.Customizer) return;

        if (!playerVehicle.Customizer.WheelManager) return;

        playerVehicle.Customizer.WheelManager.UpdateWheel(wheelIndex);
    }
}
