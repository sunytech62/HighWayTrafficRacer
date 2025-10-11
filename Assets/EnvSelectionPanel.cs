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

    static int envSelected;
    static int trafficSelected;

    void Start()
    {
        trafficSelected = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        sunnyEnv.selected.SetActive(envSelected == 0 ? true : false);
        sunnyEnv.unSelected.SetActive(envSelected == 0 ? false : true);

        eveningEnv.selected.SetActive(envSelected == 1 ? true : false);
        eveningEnv.unSelected.SetActive(envSelected == 1 ? false : true);

        nightEnv.selected.SetActive(envSelected == 2 ? true : false);
        nightEnv.unSelected.SetActive(envSelected == 2 ? false : true);

        foggyEnv.selected.SetActive(envSelected == 3 ? true : false);
        foggyEnv.unSelected.SetActive(envSelected == 3 ? false : true);

        oneWay.selected.SetActive(trafficSelected == 0 ? true : false);
        oneWay.unSelected.SetActive(trafficSelected == 0 ? false : true);

        twoWay.selected.SetActive(trafficSelected == 1 ? true : false);
        twoWay.unSelected.SetActive(trafficSelected == 1 ? false : true);
    }

    public void SelectEnv(int index)
    {
        envSelected = index;
        UpdateUI();
    }
    public void SelectTraffic(int index)
    {
        trafficSelected = index;
        UpdateUI();
    }

    [Serializable]
    public class EnvRef
    {
        public GameObject selected;
        public GameObject unSelected;
    }
}
