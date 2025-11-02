using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionButton : MonoBehaviour
{
    public ChallengeLevelSelection challengeModeController;
    public TextMeshProUGUI lvlNumTxt;

    public int levelIndex;
    public GameObject lockPanel;

    public void SelecLevel()
    {
        challengeModeController.SelectLevel(levelIndex);
    }
}
