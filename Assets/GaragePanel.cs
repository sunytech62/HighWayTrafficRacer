using System;
using System.Collections.Generic;
using UnityEngine;

public class GaragePanel : MonoBehaviour
{
    [SerializeField] PanelitemRef[] selectionBtnRefs;

    [SerializeField] References upgrade;
    [SerializeField] References paint;
    [SerializeField] References tyre;
    [SerializeField] References rim;
    [SerializeField] References spoiler;

    [SerializeField] PanelitemRef[] carBtnRefs;
    [SerializeField] PanelitemRef[] upgradeBtnRefs;
    [SerializeField] PanelitemRef[] paintBtnRefs;
    [SerializeField] PanelitemRef[] tyreBtnRefs;
    [SerializeField] PanelitemRef[] rimBtnRefs;
    [SerializeField] PanelitemRef[] spoilerBtnRefs;

    [SerializeField] GameObject buyBtnObj;
    [SerializeField] GameObject playBtnObj;

    [SerializeField] Sprite selectedSpr;
    [SerializeField] Sprite unSelectedSpr;

    [SerializeField] SelectedCustomization selectedCustomization;

    [SerializeField] int[] carUnlockPrice;

    public static int selectedCar;
    int selectedPaint;
    int selectedTyre;
    int selectedRim;
    int selectedSpoiler;

    [SerializeField] GameObject statsObj;
    [SerializeField] Camera garageCam;

    private void OnEnable()
    {
        garageCam.enabled = true;
    }

    private void OnDisable()
    {
        garageCam.enabled = false;
    }

    private void Start()
    {
        selectedCar = GameManager.SelectedCar;
        selectedPaint = SelectedPaint;
        selectedTyre = SelectedTyre;
        selectedRim = SelectedRim;
        selectedSpoiler = SelectedSpoiler;

        selectedCustomization = SelectedCustomization.None;
        UpdateUI();

        RCCP_CustomizationLoadout load = RCCP_Customizer.instance.GetLoadout();
        Debug.LogError(load.paint);
        Debug.LogError(load.spoiler);
    }

    private void UpdateUI()
    {
        //statsObj.SetActive(selectedCustomization != SelectedCustomization.None);

        upgrade.selectorHighlighter.SetActive(selectedCustomization == SelectedCustomization.Upgrade);
        upgrade.panel.SetActive(selectedCustomization == SelectedCustomization.Upgrade);

        paint.selectorHighlighter.SetActive(selectedCustomization == SelectedCustomization.Paint);
        paint.panel.SetActive(selectedCustomization == SelectedCustomization.Paint);

        tyre.selectorHighlighter.SetActive(selectedCustomization == SelectedCustomization.Tyre);
        tyre.panel.SetActive(selectedCustomization == SelectedCustomization.Tyre);

        rim.selectorHighlighter.SetActive(selectedCustomization == SelectedCustomization.Rim);
        rim.panel.SetActive(selectedCustomization == SelectedCustomization.Rim);

        spoiler.selectorHighlighter.SetActive(selectedCustomization == SelectedCustomization.Spoiler);
        spoiler.panel.SetActive(selectedCustomization == SelectedCustomization.Spoiler);

        buyBtnObj.SetActive(true);
        playBtnObj.SetActive(false);
        switch (selectedCustomization)
        {
            case SelectedCustomization.None:
                for (int i = 0; i < carBtnRefs.Length; i++)
                {
                    if (i == 0) carBtnRefs[i].lockObj.SetActive(false);
                    bool isUnlocked = IsCarUnlocked(i);
                    carBtnRefs[i].lockObj.SetActive(!isUnlocked);
                    if (i == selectedCar)
                    {
                        buyBtnObj.SetActive(!isUnlocked);
                        playBtnObj.SetActive(isUnlocked);
                    }
                    carBtnRefs[i].highlighter.SetActive(selectedCar == i);
                }
                break;
            case SelectedCustomization.Upgrade:
                buyBtnObj.SetActive(false);
                break;
            case SelectedCustomization.Paint:


                for (int i = 0; i < paintBtnRefs.Length; i++)
                {
                    if (i == 0) paintBtnRefs[i].lockObj.SetActive(false);
                    bool isUnlocked = IsPaintUnlocked(i);
                    paintBtnRefs[i].lockObj.SetActive(!isUnlocked);
                    if (i == selectedPaint)
                    {
                        buyBtnObj.SetActive(!isUnlocked);
                        playBtnObj.SetActive(isUnlocked);
                        Color col = paintBtnRefs[i].GetComponent<RCCP_UI_Color>().GetColor();
                        RCCP_SceneManager.Instance.activePlayerVehicle.Customizer.PaintManager.Paint(col, isUnlocked);
                    }
                    paintBtnRefs[i].highlighter.SetActive(selectedPaint == i);
                }


                break;
            case SelectedCustomization.Tyre:

                for (int i = 0; i < tyreBtnRefs.Length; i++)
                {
                    if (i == 0) tyreBtnRefs[i].lockObj.SetActive(false);
                    bool isUnlocked = IsTyreUnlocked(i);
                    tyreBtnRefs[i].lockObj.SetActive(!isUnlocked);
                    if (i == selectedTyre)
                    {
                        buyBtnObj.SetActive(!isUnlocked);
                        playBtnObj.SetActive(isUnlocked);
                        RCCP_SceneManager.Instance.activePlayerVehicle.Customizer.WheelManager.UpdateWheel(i, isUnlocked);
                    }
                    tyreBtnRefs[i].highlighter.SetActive(selectedTyre == i);
                }

                break;
            case SelectedCustomization.Rim:
                for (int i = 0; i < rimBtnRefs.Length; i++)
                {
                    if (i == 0) rimBtnRefs[i].lockObj.SetActive(false);
                    bool isUnlocked = IsRimUnlocked(i);
                    rimBtnRefs[i].lockObj.SetActive(!isUnlocked);
                    if (i == selectedRim)
                    {
                        buyBtnObj.SetActive(!isUnlocked);
                        playBtnObj.SetActive(isUnlocked);
                    }
                    rimBtnRefs[i].highlighter.SetActive(selectedRim == i);
                }
                break;
            case SelectedCustomization.Spoiler:

                for (int i = 0; i < spoilerBtnRefs.Length; i++)
                {
                    if (i == 0) spoilerBtnRefs[i].lockObj.SetActive(false);
                    bool isUnlocked = IsSpowilerUnlocked(i);
                    spoilerBtnRefs[i].lockObj.SetActive(!isUnlocked);
                    if (i == selectedSpoiler)
                    {
                        buyBtnObj.SetActive(!isUnlocked);
                        playBtnObj.SetActive(isUnlocked);
                        RCCP_SceneManager.Instance.activePlayerVehicle.Customizer.SpoilerManager.Upgrade(i, isUnlocked);
                    }
                    spoilerBtnRefs[i].highlighter.SetActive(selectedSpoiler == i);
                }

                break;
        }

        var totalWheels = RCCP_RuntimeSettings.RCCPChangableWheelsInstance.wheels.Length;
        for (int i = 0; i < tyreBtnRefs.Length; i++)
        {
            tyreBtnRefs[i].gameObject.SetActive(i <= totalWheels);
        }

    }

    public void UnlockItem(bool isUnlockOnCurrency)
    {
        switch (selectedCustomization)
        {
            case SelectedCustomization.None:
                {
                    Action unlockWork = () =>
                    {
                        UnlockCar(selectedCar);
                        GameManager.SelectedCar = selectedCar;
                        UpdateUI();
                    };
                    if (isUnlockOnCurrency)
                    {
                        if (HR_API.GetCurrency() >= carUnlockPrice[selectedCar])
                        {
                            unlockWork.Invoke();
                        }
                    }
                    else
                    {
                        unlockWork.Invoke();
                    }
                }
                break;
            case SelectedCustomization.Upgrade:
                break;
            case SelectedCustomization.Paint:

                {
                    Action unlockWork = () =>
                    {
                        UnlockPaint(selectedPaint);
                        SelectedPaint = selectedPaint;
                        UpdateUI();
                    };
                    if (isUnlockOnCurrency)
                    {
                        if (HR_API.GetCurrency() >= HR_API.PaintPrice)
                        {
                            unlockWork.Invoke();
                        }
                    }
                    else
                    {
                        unlockWork.Invoke();
                    }
                }

                break;

            case SelectedCustomization.Tyre:
                {
                    Action unlockWork = () =>
                    {
                        UnlockTyre(selectedTyre);
                        SelectedTyre = selectedTyre;
                        UpdateUI();
                    };
                    if (isUnlockOnCurrency)
                    {
                        if (HR_API.GetCurrency() >= HR_API.TyrePrice)
                        {
                            unlockWork.Invoke();
                        }
                    }
                    else
                    {
                        unlockWork.Invoke();
                    }
                }
                break;

            case SelectedCustomization.Rim:
                {
                    Action unlockWork = () =>
                    {
                        UnlockRim(selectedRim);
                        SelectedRim = selectedRim;
                        UpdateUI();
                    };
                    if (isUnlockOnCurrency)
                    {
                        if (HR_API.GetCurrency() >= HR_API.RimPrice)
                        {
                            unlockWork.Invoke();
                        }
                    }
                    else
                    {
                        unlockWork.Invoke();
                    }
                }
                break;

            case SelectedCustomization.Spoiler:
                {
                    Action unlockWork = () =>
                    {
                        UnlockSpoiler(selectedSpoiler);
                        SelectedSpoiler = selectedSpoiler;
                        UpdateUI();
                    };
                    if (isUnlockOnCurrency)
                    {
                        if (HR_API.GetCurrency() >= HR_API.SpoilerPrice)
                        {
                            unlockWork.Invoke();
                        }
                    }
                    else
                    {
                        unlockWork.Invoke();
                    }
                }
                break;
        }
    }

    public void SelectCustomization(int index)
    {
        if (!IsCarUnlocked(selectedCar)) return;
        RCCP_SceneManager.Instance.activePlayerVehicle.Customizer.PaintManager.Initialize();
        RCCP_SceneManager.Instance.activePlayerVehicle.Customizer.UpgradeManager.Initialize();
        RCCP_SceneManager.Instance.activePlayerVehicle.Customizer.SpoilerManager.Initialize();
        RCCP_SceneManager.Instance.activePlayerVehicle.Customizer.WheelManager.Initialize();

        selectedPaint = SelectedPaint;
        selectedTyre = SelectedTyre;
        selectedRim = SelectedRim;
        selectedSpoiler = SelectedSpoiler;

        if (index == 0) selectedCustomization = SelectedCustomization.None;
        if (index == 1) selectedCustomization = SelectedCustomization.Upgrade;
        if (index == 2) selectedCustomization = SelectedCustomization.Paint;
        if (index == 3) selectedCustomization = SelectedCustomization.Tyre;
        if (index == 4) selectedCustomization = SelectedCustomization.Rim;
        if (index == 5) selectedCustomization = SelectedCustomization.Spoiler;
        UpdateUI();
    }

    public void SelecCar(int index)
    {
        HR_MainMenuManager.Instance.SelectCar(index);
        selectedCustomization = SelectedCustomization.None;
        selectedCar = index;
        if (IsCarUnlocked(index)) GameManager.SelectedCar = selectedCar;
        UpdateUI();
    }

    public void SelectPaint(int index)
    {
        selectedPaint = Mathf.Clamp(index, 0, 100);
        if (IsPaintUnlocked(index))
        {
            SelectedPaint = selectedPaint;
        }
        UpdateUI();
    }

    public void SelectTyre(int index)
    {
        selectedTyre = index;
        if (IsTyreUnlocked(index))
        {
            SelectedTyre = selectedTyre;
        }
        UpdateUI();
    }

    public void SelectRim(int index)
    {
        selectedRim = index;
        if (IsRimUnlocked(index)) SelectedRim = selectedRim;
        UpdateUI();
    }

    public void SelectSpoiler(int index)
    {
        selectedSpoiler = Mathf.Clamp(index, 0, 100);
        if (IsSpowilerUnlocked(index))
        {
            SelectedSpoiler = selectedSpoiler;
        }
        UpdateUI();
    }

    public void upgradeBtn(int index)
    {
        UpdateUI();
    }


    public static bool IsCarUnlocked(int index)
    {
        if (index <= 0) return true;
        return PlayerPrefs.GetInt($"IsCarUnlocked_{index}") == 0 ? false : true;
    }
    public static bool IsPaintUnlocked(int index)
    {
        // if (index < 0) return true;
        return PlayerPrefs.GetInt($"IsPaintUnlocked_{selectedCar}_{index}") == 0 ? false : true;
    }
    public static bool IsTyreUnlocked(int index)
    {
        //if (index < 0) return true;
        return PlayerPrefs.GetInt($"IsTyreUnlocked_{selectedCar}_{index}") == 0 ? false : true;
    }
    public static bool IsRimUnlocked(int index)
    {
        // if (index < 0) return true;
        return PlayerPrefs.GetInt($"IsRimUnlocked_{selectedCar}_{index}") == 0 ? false : true;
    }
    public static bool IsSpowilerUnlocked(int index)
    {
        // if (index < 0) return true;
        return PlayerPrefs.GetInt($"IsSpowilerUnlocked_{selectedCar}_{index}") == 0 ? false : true;
    }


    void UnlockCar(int carIndex)
    {
        PlayerPrefs.SetInt($"IsCarUnlocked_{carIndex}", 1);
    }
    void UnlockPaint(int paintIndex)
    {
        PlayerPrefs.SetInt($"IsPaintUnlocked_{selectedCar}_{paintIndex}", 1);
    }
    void UnlockTyre(int tyreIndex)
    {
        PlayerPrefs.SetInt($"IsTyreUnlocked_{selectedCar}_{tyreIndex}", 1);
    }
    void UnlockRim(int rimIndex)
    {
        PlayerPrefs.SetInt($"IsRimUnlocked_{selectedCar}_{rimIndex}", 1);
    }
    void UnlockSpoiler(int spoilerIndex)
    {
        PlayerPrefs.SetInt($"IsSpowilerUnlocked_{selectedCar}_{spoilerIndex}", 1);
    }

    public int SelectedPaint
    {
        get => PlayerPrefs.GetInt($"Selected_{selectedCar}_Paint");
        set => PlayerPrefs.SetInt($"Selected_{selectedCar}_Paint", value);
    }
    public int SelectedTyre
    {
        get => PlayerPrefs.GetInt($"Selected_{selectedCar}_Tyre");
        set => PlayerPrefs.SetInt($"Selected_{selectedCar}_Tyre", value);
    }
    public int SelectedRim
    {
        get => PlayerPrefs.GetInt($"Selected_{selectedCar}_Rim");
        set => PlayerPrefs.SetInt($"Selected_{selectedCar}_Rim", value);
    }
    public int SelectedSpoiler
    {
        get => PlayerPrefs.GetInt($"Selected_{selectedCar}_Spoiler");
        set => PlayerPrefs.SetInt($"Selected_{selectedCar}_Spoiler", value);
    }

    public bool IsCustomizationSelected()
    {
        var isCusSelected = false;
        if (upgrade.panel.activeInHierarchy) isCusSelected = true;
        if (paint.panel.activeInHierarchy) isCusSelected = true;
        if (tyre.panel.activeInHierarchy) isCusSelected = true;
        if (rim.panel.activeInHierarchy) isCusSelected = true;
        if (spoiler.panel.activeInHierarchy) isCusSelected = true;
        return isCusSelected;
    }


    [ContextMenu(nameof(SetReferences))]
    void SetReferences()
    {
        var allChild = GetComponentsInChildren<Transform>(true);

        foreach (var child in allChild)
        {
            if (child.name.Contains("Car Btns"))
            {
                carBtnRefs = child.GetComponentsInChildren<PanelitemRef>(true);
            }
            else if (child.name.Contains("Upgrade"))
            {
                upgradeBtnRefs = child.GetComponentsInChildren<PanelitemRef>(true);
            }
            else if (child.name.Contains("Paint"))
            {
                paintBtnRefs = child.GetComponentsInChildren<PanelitemRef>(true);
            }
            else if (child.name.Contains("Spolers"))
            {
                spoilerBtnRefs = child.GetComponentsInChildren<PanelitemRef>(true);
            }
            else if (child.name.Contains("Tyres"))
            {
                tyreBtnRefs = child.GetComponentsInChildren<PanelitemRef>(true);
            }
            else if (child.name.Contains("Rims"))
            {
                rimBtnRefs = child.GetComponentsInChildren<PanelitemRef>(true);
            }
        }
    }

    [System.Serializable]
    public class References
    {
        public GameObject selectorHighlighter;
        public GameObject panel;
    }

    public enum SelectedCustomization
    {
        None = 0,
        Upgrade = 1,
        Paint = 2,
        Tyre = 3,
        Rim = 4,
        Spoiler = 5,
    }
    public SaveData.Data data = new SaveData.Data();
    public static class SaveData
    {
        public static void Save()
        {
            // JSONManager.Save<Data>(data, Data.fileName);
        }



        public class Data
        {
            public static string fileName = "Car";
            public static List<bool> unlockedCars = new List<bool>();

            public static List<bool> paintUnlocked = new List<bool>();
            public static List<bool> tyreUnlocked = new List<bool>();
            public static List<bool> rimUnlocked = new List<bool>();
            public static List<bool> spoilerUnlocked = new List<bool>();
        }
    }
}
