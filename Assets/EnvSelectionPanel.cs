using System;
using UnityEngine;

public class EnvSelectionPanel : MonoBehaviour
{
    [SerializeField] EnvRef sunnyEnv;
    [SerializeField] EnvRef eveningEnv;
    [SerializeField] EnvRef nightEnv;
    [SerializeField] EnvRef foggyEnv;

    [SerializeField] EnvRef oneWay;
    [SerializeField] EnvRef twoWay;

    EnvNames SelectedSelected
    {
        get
        {
            var savedValue = PlayerPrefs.GetString("SelectedScene");
            if (savedValue.Equals("HR_Scene_Sunny")) return EnvNames.Sunny;
            if (savedValue.Equals("HR_Scene_Night")) return EnvNames.Night;
            if (savedValue.Equals("HR_Scene_Rainy")) return EnvNames.Rainy;
            if (savedValue.Equals("HR_Scene_Evening")) return EnvNames.Evening;
            return EnvNames.Sunny;
        }
        set
        {
            string key = value switch
            {
                EnvNames.Sunny => "HR_Scene_Sunny",
                EnvNames.Night => "HR_Scene_Night",
                EnvNames.Rainy => "HR_Scene_Rainy",
                EnvNames.Evening => "HR_Scene_Evening",
                _ => "HR_Scene_Sunny"
            };
            PlayerPrefs.SetString("SelectedScene", key);
        }
    }
    bool isTwoWayTrafficSelected
    {
        get => PlayerPrefs.GetInt("SelectedTraffic") == 1 ? true : false;
        set => PlayerPrefs.SetInt("SelectedTraffic", value == true ? 1 : 0);
    }

    void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        sunnyEnv.selected.SetActive(SelectedSelected == EnvNames.Sunny ? true : false);
        sunnyEnv.unSelected.SetActive(SelectedSelected == EnvNames.Sunny ? false : true);

        nightEnv.selected.SetActive(SelectedSelected == EnvNames.Night ? true : false);
        nightEnv.unSelected.SetActive(SelectedSelected == EnvNames.Night ? false : true);

        foggyEnv.selected.SetActive(SelectedSelected == EnvNames.Rainy ? true : false);
        foggyEnv.unSelected.SetActive(SelectedSelected == EnvNames.Rainy ? false : true);

        eveningEnv.selected.SetActive(SelectedSelected == EnvNames.Evening ? true : false);
        eveningEnv.unSelected.SetActive(SelectedSelected == EnvNames.Evening ? false : true);

        oneWay.selected.SetActive(!isTwoWayTrafficSelected);
        oneWay.unSelected.SetActive(isTwoWayTrafficSelected);

        twoWay.selected.SetActive(isTwoWayTrafficSelected);
        twoWay.unSelected.SetActive(!isTwoWayTrafficSelected);
    }

    public void SelectEnv(int index)
    {
        SelectedSelected = index switch
        {
            0 => EnvNames.Sunny,
            1 => EnvNames.Night,
            2 => EnvNames.Rainy,
            3 => EnvNames.Evening,
            _ => EnvNames.Sunny,
        };
        UpdateUI();
    }
    public void SelectTraffic(bool isTwoWaySelected)
    {
        isTwoWayTrafficSelected = isTwoWaySelected;
        UpdateUI();
    }

    [Serializable]
    public class EnvRef
    {
        public GameObject selected;
        public GameObject unSelected;
    }
    public enum EnvNames
    {
        Sunny = 1,
        Night = 2,
        Rainy = 3,
        Evening = 4,
    }
}
