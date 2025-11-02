using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HR_UI_GameplayPanel : MonoBehaviour
{
    private HR_Player player;
    public GameObject content;

    public TextMeshProUGUI score;
    public TextMeshProUGUI timeLeft;
    public TextMeshProUGUI combo;

    public TextMeshProUGUI speed;
    public TextMeshProUGUI distance;
    public TextMeshProUGUI highSpeed;
    public TextMeshProUGUI oppositeDirection;

    public Slider damageSlider;
    public Slider bombSlider;

    private Image comboMImage;
    private Vector2 comboDefPos;

    private Image highSpeedImage;
    private Vector2 highSpeedDefPos;

    private Image oppositeDirectionImage;
    private Vector2 oppositeDirectionDefPos;

    private Image timeAttackImage;

    private RectTransform bombRect;
    private Vector2 bombDefPos;

    private void Awake()
    {
        if (combo)
        {
            comboMImage = combo.GetComponentInParent<Image>();
            comboDefPos = comboMImage.rectTransform.anchoredPosition;
        }

        if (highSpeed)
        {
            highSpeedImage = highSpeed.GetComponentInParent<Image>();
            highSpeedDefPos = highSpeedImage.rectTransform.anchoredPosition;
        }

        if (oppositeDirection)
        {
            oppositeDirectionImage = oppositeDirection.GetComponentInParent<Image>();
            oppositeDirectionDefPos = oppositeDirectionImage.rectTransform.anchoredPosition;
        }

        if (timeLeft)
            timeAttackImage = timeLeft.GetComponentInParent<Image>();

        if (bombSlider)
        {
            bombRect = bombSlider.GetComponent<RectTransform>();
            bombDefPos = bombRect.anchoredPosition;
        }
    }

    private void OnEnable()
    {
        HR_Events.OnPlayerSpawned += HR_PlayerHandler_OnPlayerSpawned;
        HR_Events.OnPlayerDied += HR_PlayerHandler_OnPlayerDied;
    }

    private void HR_PlayerHandler_OnPlayerSpawned(HR_Player _player)
    {
        player = _player;
        content.SetActive(true);
    }

    private void HR_PlayerHandler_OnPlayerDied(HR_Player _player, int[] scores)
    {
        player = null;
        content.SetActive(false);
    }

    private void Update()
    {
        if (!player) return;

        if (combo)
        {
            if (player.combo > 1)
                comboMImage.rectTransform.anchoredPosition = Vector2.Lerp(comboMImage.rectTransform.anchoredPosition, comboDefPos, Time.deltaTime * 5f);
            else
                comboMImage.rectTransform.anchoredPosition = Vector2.Lerp(comboMImage.rectTransform.anchoredPosition, new Vector2(comboDefPos.x - 500, comboDefPos.y), Time.deltaTime * 5f);
        }

        if (highSpeed)
        {
            if (player.highSpeedCurrent > .1f)
                highSpeedImage.rectTransform.anchoredPosition = Vector2.Lerp(highSpeedImage.rectTransform.anchoredPosition, highSpeedDefPos, Time.deltaTime * 5f);
            else
                highSpeedImage.rectTransform.anchoredPosition = Vector2.Lerp(highSpeedImage.rectTransform.anchoredPosition, new Vector2(highSpeedDefPos.x + 500, highSpeedDefPos.y), Time.deltaTime * 5f);
        }

        if (oppositeDirection)
        {
            if (player.opposideDirectionCurrent > .1f)
                oppositeDirectionImage.rectTransform.anchoredPosition = Vector2.Lerp(oppositeDirectionImage.rectTransform.anchoredPosition, oppositeDirectionDefPos, Time.deltaTime * 5f);
            else
                oppositeDirectionImage.rectTransform.anchoredPosition = Vector2.Lerp(oppositeDirectionImage.rectTransform.anchoredPosition, new Vector2(oppositeDirectionDefPos.x - 500, oppositeDirectionDefPos.y), Time.deltaTime * 5f);
        }

        if (timeLeft)
        {
            if (GameManager.SelectedMode == GameMode.TimeTrial)
            {
                if (!timeLeft.gameObject.activeSelf)
                    timeAttackImage.gameObject.SetActive(true);
            }
            else
            {
                if (timeLeft.gameObject.activeSelf)
                    timeAttackImage.gameObject.SetActive(false);
            }
        }

        if (damageSlider)
        {
            damageSlider.value = player.damage;
        }

        if (bombSlider)
        {
            if (GameManager.SelectedMode == GameMode.LowSpeedBomb)
            {
                if (!bombSlider.gameObject.activeSelf)
                    bombSlider.gameObject.SetActive(true);
            }
            else
            {
                if (bombSlider.gameObject.activeSelf)
                    bombSlider.gameObject.SetActive(false);
            }

            if (player.bombTriggered)
                bombRect.anchoredPosition = Vector2.Lerp(bombRect.anchoredPosition, bombDefPos, Time.deltaTime * 5f);
            else
                bombRect.anchoredPosition = Vector2.Lerp(bombRect.anchoredPosition, new Vector2(bombDefPos.x - 500, bombDefPos.y), Time.deltaTime * 5f);
        }
    }

    private void LateUpdate()
    {
        if (!player) return;

        if (score)
            score.text = player.score.ToString("F0");

        if (speed)
            speed.text = player.speed.ToString("F0");

        if (distance)
            distance.text = (player.distance).ToString("F2");

        if (highSpeed)
            highSpeed.text = player.highSpeedCurrent.ToString("F1");

        if (oppositeDirection)
            oppositeDirection.text = player.opposideDirectionCurrent.ToString("F1");

        if (timeLeft)
            timeLeft.text = player.timeLeft.ToString("F1");

        if (combo)
            combo.text = player.combo.ToString();

        if (bombSlider && GameManager.SelectedMode == GameMode.LowSpeedBomb)
            bombSlider.value = player.bombHealth / 100f;
    }

    private void OnDisable()
    {
        HR_Events.OnPlayerSpawned -= HR_PlayerHandler_OnPlayerSpawned;
        HR_Events.OnPlayerDied -= HR_PlayerHandler_OnPlayerDied;
    }

}
