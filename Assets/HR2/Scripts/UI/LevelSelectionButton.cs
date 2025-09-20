using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class LevelSelectionButton : MonoBehaviour
{
    private int _levelNumber;
    private Button _button;

    [SerializeField] private GameObject lockImage;
    [SerializeField] private GameObject levelBadge;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(SelecLevel);

        _levelNumber = transform.GetSiblingIndex() + 1;
        GetComponentInChildren<Text>().text = _levelNumber.ToString();

        _button.interactable = (_levelNumber <= ChallengeModeLevels.Instance.levels.Count);
        
        if (_levelNumber > ChallengeModeLevels.Instance.levels.Count) return;
        
        lockImage.SetActive(!(_levelNumber <= GameState.LastCompletedChallenge + 1));
        
        
        if(_levelNumber < GameState.LastCompletedChallenge + 1)
        {
            var stars = levelBadge.GetComponentsInChildren<Image>();
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].color = Color.yellow;
            }
            levelBadge.GetComponent<Image>().color = Color.green;
        }
        if (_levelNumber == GameState.LastCompletedChallenge + 1)
        {
            levelBadge.GetComponent<Image>().color = Color.yellow;
        }
    }

    void SelecLevel()
    {
        ChallengeModeLevelsManager.Instance.SelectLevel(_levelNumber);
    }
}
