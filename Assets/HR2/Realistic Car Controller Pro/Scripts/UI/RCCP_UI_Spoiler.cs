using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller Pro/UI/Modification/RCCP UI Spoiler Button")]
public class RCCP_UI_Spoiler : RCCP_UIComponent
{
    [Min(0)] public int index = 0;

    private void Awake()
    {
        index = transform.GetSiblingIndex();
        GetComponent<Button>().onClick.AddListener(OnClick);
    }

    public void OnClick()
    {

        //  Finding the player vehicle.
        RCCP_CarController playerVehicle = RCCPSceneManager.activePlayerVehicle;

        //  If no player vehicle found, return.
        if (!playerVehicle)
            return;

        //  If player vehicle doesn't have the customizer component, return.
        if (!playerVehicle.Customizer)
            return;

        if (!playerVehicle.Customizer.SpoilerManager)
            return;

        playerVehicle.Customizer.SpoilerManager.Upgrade(index);

    }



}
