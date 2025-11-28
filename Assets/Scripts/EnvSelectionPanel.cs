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

    public static EnvNames SelectedEnv
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

    void Start()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        sunnyEnv.selected.SetActive(SelectedEnv == EnvNames.Sunny ? true : false);
        sunnyEnv.unSelected.SetActive(SelectedEnv == EnvNames.Sunny ? false : true);

        nightEnv.selected.SetActive(SelectedEnv == EnvNames.Night ? true : false);
        nightEnv.unSelected.SetActive(SelectedEnv == EnvNames.Night ? false : true);

        foggyEnv.selected.SetActive(SelectedEnv == EnvNames.Rainy ? true : false);
        foggyEnv.unSelected.SetActive(SelectedEnv == EnvNames.Rainy ? false : true);

        eveningEnv.selected.SetActive(SelectedEnv == EnvNames.Evening ? true : false);
        eveningEnv.unSelected.SetActive(SelectedEnv == EnvNames.Evening ? false : true);

        var selectedTraffic = GameState.SelectedTraffic;
        oneWay.selected.SetActive(selectedTraffic == TrafficType.OneWay);
        oneWay.unSelected.SetActive(selectedTraffic != TrafficType.OneWay);

        twoWay.selected.SetActive(selectedTraffic == TrafficType.TwoWay);
        twoWay.unSelected.SetActive(selectedTraffic != TrafficType.TwoWay);
    }

    public void SelectEnv(int index)
    {
        SelectedEnv = index switch
        {
            0 => EnvNames.Sunny,
            1 => EnvNames.Night,
            2 => EnvNames.Rainy,
            3 => EnvNames.Evening,
            _ => EnvNames.Sunny,
        };
        UpdateUI();
        FirebaseAnalyticsManager.SendAnalyticCus($"Selected_Env_{SelectedEnv.ToString()}",
            $"Mode_{GameManager.SelectedMode.ToString()}_Session_{GameManager.SessionCount}");
    }
    public void SelectTraffic(bool isTwoWaySelected)
    {
        GameState.SelectedTraffic = isTwoWaySelected ? TrafficType.TwoWay : TrafficType.OneWay;
        UpdateUI();
        FirebaseAnalyticsManager.SendAnalyticCus($"Selected_Traffic_{GameState.SelectedTraffic.ToString()}",
            $"Mode_{GameManager.SelectedMode.ToString()}_Session_{GameManager.SessionCount}");
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
