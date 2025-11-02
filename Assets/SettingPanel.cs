using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI musicTxt;
    [SerializeField] Slider musicSlider;

    [SerializeField] TextMeshProUGUI audioTxt;
    [SerializeField] Slider audioSlider;

    [SerializeField] ControlRef[] controlsHighlighters;


    private void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        var selectedControl = HR_API.GetSelectedControl();
        for (int i = 0; i < controlsHighlighters.Length; i++)
        {
            controlsHighlighters[i].highlighterObj.SetActive(i == selectedControl);
            controlsHighlighters[i].txt.color = i == selectedControl ? new Color(1, 0.9057592f, 0) : Color.white;
            controlsHighlighters[i].renderer.color = i == selectedControl ? new Color(1, 0.9057592f, 0) : Color.white;
        }

        musicTxt.text = ((int)(GameManager.MusicVolume * 100)).ToString();
        musicSlider.value = GameManager.MusicVolume;

        audioTxt.text = ((int)(GameManager.AudioVolume * 100)).ToString();
        audioSlider.value = GameManager.AudioVolume;
    }

    public void SelectControl(int index)
    {
        HR_API.SetControllerType(index);
        UpdateUI();
    }

    public void SetMusicVol(Slider slider)
    {
        GameManager.MusicVolume = slider.value;
        HR_Events.Event_OnOptionsChanged();
        UpdateUI();
    }
    public void SetAudioVol(Slider slider)
    {
        GameManager.AudioVolume = slider.value;
        HR_Events.Event_OnOptionsChanged();
        UpdateUI();
    }


    [Serializable]
    public class ControlRef
    {
        public GameObject highlighterObj;
        public Image renderer;
        public TextMeshProUGUI txt;
    }
}
