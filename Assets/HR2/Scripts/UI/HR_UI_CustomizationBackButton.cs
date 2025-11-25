using UnityEngine;

public class HR_UI_CustomizationBackButton : MonoBehaviour
{
    public GameObject mainMenuPanel;

    public void OnClick()
    {
        HR_UI_MainmenuPanel mainmenuPanel = HR_UI_MainmenuPanel.Instance;

        if (!mainmenuPanel)
            return;
    }
}
