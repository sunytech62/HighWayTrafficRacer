using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GaragePanel : MonoBehaviour
{
    [SerializeField] PanelitemRef[] selectionBtnRefs;

    [SerializeField] References upgrade;
    [SerializeField] References paint;
    [SerializeField] References tyre;
    [SerializeField] References neon;
    [SerializeField] References spoiler;

    [SerializeField] PanelitemRef[] carBtnRefs;
    [SerializeField] PanelitemRef[] upgradeBtnRefs;
    [SerializeField] PanelitemRef[] paintBtnRefs;
    [SerializeField] PanelitemRef[] tyreBtnRefs;
    [SerializeField] PanelitemRef[] neonBtnRefs;
    [SerializeField] PanelitemRef[] spoilerBtnRefs;

    [SerializeField] GameObject buyBtnObj;
    [SerializeField] GameObject playBtnObj;
    [SerializeField] TextMeshProUGUI buyBtnTxt;

    [SerializeField] Sprite selectedSpr;
    [SerializeField] Sprite unSelectedSpr;

    [SerializeField] SelectedCustomization selectedCustomization;

    public static int selectedCar;
    int selectedUpgrade;
    int selectedPaint;
    int selectedTyre;
    int selectedNeon;
    int selectedSpoiler;

    [SerializeField] GameObject statsObj;
    [SerializeField] Camera garageCam;
    [SerializeField] Camera uiCam;

    RCCP_Customizer activeCarCustomize
    {
        get
        {
            return RCCP_SceneManager.Instance.activePlayerVehicle.Customizer;
        }
        set
        {
            RCCP_SceneManager.Instance.activePlayerVehicle.Customizer = value;
        }
    }

    private void OnEnable()
    {
        uiCam.gameObject.SetActive(false);
        garageCam.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        garageCam.gameObject.SetActive(false);
        uiCam.gameObject.SetActive(true);
    }

    private void Start()
    {
        selectedCar = GameManager.SelectedCar;
        selectedPaint = SelectedPaint;
        selectedTyre = SelectedTyre;
        selectedNeon = SelectedNeon;
        selectedSpoiler = SelectedSpoiler;

        selectedCustomization = SelectedCustomization.None;
        UpdateUI();
        SetCarNameOnBtns();
        RCCP_CustomizationLoadout load = RCCP_Customizer.instance.GetLoadout();

        FirebaseAnalyticsManager.SendAnalyticCus("GaragePanel", $"Session_{GameManager.SessionCount}");
    }

    private void SetCarNameOnBtns()
    {
        for (int i = 0; i < carBtnRefs.Length; i++)
        {
            if (i < HR_PlayerCars.Instance.cars.Length)
                carBtnRefs[i].titleTxt.SetText(HR_PlayerCars.Instance.cars[i].vehicleName);
        }
    }

    public void UpdateUI()
    {
        //statsObj.SetActive(selectedCustomization != SelectedCustomization.None);
        if (activeCarCustomize.UpgradeManager.IsAllUpgraded())
        {
            SetBtnState(upgrade.btn.gameObject, false);
            selectedUpgrade = activeCarCustomize.UpgradeManager.WhichUpgradAvilable();
        }
        else
        {
            SetBtnState(upgrade.btn.gameObject, true);
        }

        SetBtnState(spoiler.btn.gameObject, activeCarCustomize.SpoilerManager);

        /*     activeCarCustomize.PaintManager.Initialize();
         activeCarCustomize.UpgradeManager.Initialize();
         activeCarCustomize.WheelManager.Initialize();*/

        upgrade.selectorHighlighter.SetActive(selectedCustomization == SelectedCustomization.Upgrade);
        upgrade.panel.SetActive(selectedCustomization == SelectedCustomization.Upgrade);

        paint.selectorHighlighter.SetActive(selectedCustomization == SelectedCustomization.Paint);
        paint.panel.SetActive(selectedCustomization == SelectedCustomization.Paint);

        tyre.selectorHighlighter.SetActive(selectedCustomization == SelectedCustomization.Tyre);
        tyre.panel.SetActive(selectedCustomization == SelectedCustomization.Tyre);

        neon.selectorHighlighter.SetActive(selectedCustomization == SelectedCustomization.Neon);
        neon.panel.SetActive(selectedCustomization == SelectedCustomization.Neon);

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
                buyBtnTxt.text = HR_PlayerCars.Instance.cars[selectedCar].price.ToString();
                break;
            case SelectedCustomization.Upgrade:

                //   buyBtnObj.SetActive(true);
                int speedLvl = activeCarCustomize.UpgradeManager.SpeedLevel;
                int engineLvl = activeCarCustomize.UpgradeManager.EngineLevel;
                int handlingLvl = activeCarCustomize.UpgradeManager.HandlingLevel;
                int brakeLvl = activeCarCustomize.UpgradeManager.BrakeLevel;
                bool isShowHighLighter = true;
                for (int i = 0; i < upgradeBtnRefs.Length; i++)
                {
                    if (i == 0)
                    {
                        SetBtnState(upgradeBtnRefs[i].gameObject, speedLvl < 5);
                        if (i == selectedUpgrade && speedLvl >= 5)
                        {
                            buyBtnObj.SetActive(false);
                            isShowHighLighter = false;
                        }
                    }
                    if (i == 1)
                    {
                        SetBtnState(upgradeBtnRefs[i].gameObject, engineLvl < 5);
                        if (i == selectedUpgrade && engineLvl >= 5)
                        {
                            buyBtnObj.SetActive(false);
                            isShowHighLighter = false;
                        }
                    }
                    if (i == 2)
                    {
                        SetBtnState(upgradeBtnRefs[i].gameObject, handlingLvl < 5);
                        if (i == selectedUpgrade && handlingLvl >= 5)
                        {
                            buyBtnObj.SetActive(false);
                            isShowHighLighter = false;
                        }
                    }
                    if (i == 3)
                    {
                        SetBtnState(upgradeBtnRefs[i].gameObject, brakeLvl < 5);
                        if (i == selectedUpgrade && brakeLvl >= 5)
                        {
                            buyBtnObj.SetActive(false);
                            isShowHighLighter = false;
                        }
                    }
                    upgradeBtnRefs[i].highlighter.SetActive(isShowHighLighter && selectedUpgrade == i);
                }
                buyBtnTxt.text = HR_API.UpgradeCarPrice.ToString();

                break;
            case SelectedCustomization.Paint:

                for (int i = 0; i < paintBtnRefs.Length; i++)
                {
                    bool isUnlocked = IsPaintUnlocked(i);
                    paintBtnRefs[i].lockObj.SetActive(!isUnlocked);
                    if (i == selectedPaint)
                    {
                        buyBtnObj.SetActive(!isUnlocked);
                        playBtnObj.SetActive(isUnlocked);
                        Color col = paintBtnRefs[i].GetComponent<RCCP_UI_Color>().GetColor();
                        activeCarCustomize.PaintManager.Paint(col, isUnlocked);
                    }
                    paintBtnRefs[i].highlighter.SetActive(selectedPaint == i);
                }
                buyBtnTxt.text = HR_API.PaintPrice.ToString();

                break;
            case SelectedCustomization.Tyre:

                for (int i = 0; i < tyreBtnRefs.Length; i++)
                {
                    bool isUnlocked = IsTyreUnlocked(i);
                    tyreBtnRefs[i].lockObj.SetActive(!isUnlocked);
                    if (i == selectedTyre)
                    {
                        buyBtnObj.SetActive(!isUnlocked);
                        playBtnObj.SetActive(isUnlocked);
                        activeCarCustomize.WheelManager.UpdateWheel(i, isUnlocked);
                    }
                    tyreBtnRefs[i].highlighter.SetActive(selectedTyre == i);
                }

                var totalWheels = RCCP_RuntimeSettings.RCCPChangableWheelsInstance.wheels.Length;
                for (int i = 0; i < tyreBtnRefs.Length; i++)
                {
                    tyreBtnRefs[i].gameObject.SetActive(i < totalWheels);
                }

                buyBtnTxt.text = HR_API.TyrePrice.ToString();

                break;

            case SelectedCustomization.Neon:
                for (int i = 0; i < neonBtnRefs.Length; i++)
                {
                    bool isUnlocked = IsNeonUnlocked(i);
                    neonBtnRefs[i].lockObj.SetActive(!isUnlocked);
                    if (i == selectedNeon)
                    {
                        buyBtnObj.SetActive(!isUnlocked);
                        playBtnObj.SetActive(isUnlocked);
                        activeCarCustomize.NeonManager.Upgrade(activeCarCustomize.NeonManager.GetMaterial(i), isUnlocked);
                    }
                    neonBtnRefs[i].highlighter.SetActive(selectedNeon == i);
                }
                buyBtnTxt.text = HR_API.NeonPrice.ToString();
                break;

            case SelectedCustomization.Spoiler:

                for (int i = 0; i < spoilerBtnRefs.Length; i++)
                {
                    bool isUnlocked = IsSpoilerUnlocked(i);
                    spoilerBtnRefs[i].lockObj.SetActive(!isUnlocked);
                    if (i == selectedSpoiler)
                    {
                        buyBtnObj.SetActive(!isUnlocked);
                        playBtnObj.SetActive(isUnlocked);
                        activeCarCustomize.SpoilerManager.Upgrade(i, isUnlocked);
                    }
                    spoilerBtnRefs[i].highlighter.SetActive(selectedSpoiler == i);
                }
                buyBtnTxt.text = HR_API.SpoilerPrice.ToString();
                break;
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
                        if (HR_API.GetCurrency() >= HR_PlayerCars.Instance.cars[selectedCar].price)
                        {
                            HR_API.ConsumeCurrency(HR_PlayerCars.Instance.cars[selectedCar].price);
                            unlockWork.Invoke();
                            FirebaseAnalyticsManager.SendAnalyticCus("UnLocked_Car_Cash", $"CarIndex_{selectedCar}");
                        }
                        else
                        {
                            GameManager.Instance.Show_GetCoinsPanel();
                            FirebaseAnalyticsManager.SendAnalyticCus("NotEnoughCash_Car", $"CarIndex_{selectedCar}");
                        }
                    }
                    else
                    {
                        CustomAd.ShowRewarded(() =>
                        {
                            unlockWork.Invoke();
                            FirebaseAnalyticsManager.SendAnalyticCus("UnLocked_Car_Ad", $"CarIndex_{selectedCar}");
                        });
                    }
                }
                break;
            case SelectedCustomization.Upgrade:
                {
                    Action unlockWork = () =>
                    {
                        if (selectedUpgrade == 0)
                            activeCarCustomize.UpgradeManager.UpgradeSpeed();
                        else if (selectedUpgrade == 1)
                            activeCarCustomize.UpgradeManager.UpgradeEngine();
                        else if (selectedUpgrade == 2)
                            activeCarCustomize.UpgradeManager.UpgradeHandling();
                        else if (selectedUpgrade == 3)
                            activeCarCustomize.UpgradeManager.UpgradeBrake();

                        activeCarCustomize.UpgradeManager.Save();
                        UpdateUI();
                        HR_UI_MainmenuPanel.Instance.CheckCurrentVehicle();
                    };
                    if (isUnlockOnCurrency)
                    {
                        if (HR_API.GetCurrency() >= HR_API.UpgradeCarPrice)
                        {
                            HR_API.ConsumeCurrency(HR_API.UpgradeCarPrice);
                            unlockWork.Invoke();

                            FirebaseAnalyticsManager.SendAnalyticCus("UnLocked_Upgrade_Cash", GetSelectedUpgradeName());
                        }
                        else
                        {
                            GameManager.Instance.Show_GetCoinsPanel();
                            FirebaseAnalyticsManager.SendAnalyticCus("NotEnoughCash_Upgrade", GetSelectedUpgradeName());
                        }
                    }
                    else
                    {
                        CustomAd.ShowRewarded(() =>
                        {
                            unlockWork.Invoke();
                            FirebaseAnalyticsManager.SendAnalyticCus("UnLocked_Upgrade_Ad", GetSelectedUpgradeName());
                        });
                    }
                }
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
                            HR_API.ConsumeCurrency(HR_API.PaintPrice);
                            unlockWork.Invoke();
                            FirebaseAnalyticsManager.SendAnalyticCus("UnLocked_Paint_Cash", $"PaintIndex_{selectedPaint}");
                        }
                        else
                        {
                            GameManager.Instance.Show_GetCoinsPanel();
                            FirebaseAnalyticsManager.SendAnalyticCus("NotEnoughCash_Paint", $"PaintIndex_{selectedPaint}");
                        }
                    }
                    else
                    {
                        CustomAd.ShowRewarded(() =>
                        {
                            unlockWork.Invoke();
                            FirebaseAnalyticsManager.SendAnalyticCus("UnLocked_Paint_Ad", $"PaintIndex_{selectedPaint}");
                        });
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
                            HR_API.ConsumeCurrency(HR_API.TyrePrice);
                            unlockWork.Invoke();
                            FirebaseAnalyticsManager.SendAnalyticCus("UnLocked_Tyre_Cash", $"TyreIndex_{selectedTyre}");
                        }
                        else
                        {
                            GameManager.Instance.Show_GetCoinsPanel();
                            FirebaseAnalyticsManager.SendAnalyticCus("NotEnoughCash_Tyre", $"TyreIndex_{selectedTyre}");
                        }
                    }
                    else
                    {
                        CustomAd.ShowRewarded(() =>
                        {
                            unlockWork.Invoke();
                            FirebaseAnalyticsManager.SendAnalyticCus("UnLocked_Tyre_Ad", $"TyreIndex_{selectedTyre}");
                        });
                    }
                }
                break;

            case SelectedCustomization.Neon:
                {
                    Action unlockWork = () =>
                    {
                        UnlockNeon(selectedNeon);
                        SelectedNeon = selectedNeon;
                        UpdateUI();
                    };
                    if (isUnlockOnCurrency)
                    {
                        if (HR_API.GetCurrency() >= HR_API.NeonPrice)
                        {
                            HR_API.ConsumeCurrency(HR_API.NeonPrice);
                            unlockWork.Invoke();
                            FirebaseAnalyticsManager.SendAnalyticCus("UnLocked_Neon_Cash", $"NeonIndex_{selectedNeon}");
                        }
                        else
                        {
                            GameManager.Instance.Show_GetCoinsPanel();
                            FirebaseAnalyticsManager.SendAnalyticCus("NotEnoughCash_Neon", $"NeonIndex_{selectedNeon}");
                        }
                    }
                    else
                    {
                        CustomAd.ShowRewarded(() =>
                        {
                            unlockWork.Invoke();
                            FirebaseAnalyticsManager.SendAnalyticCus("UnLocked_Neon_Ad", $"NeonIndex_{selectedNeon}");
                        });
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
                            HR_API.ConsumeCurrency(HR_API.SpoilerPrice);
                            unlockWork.Invoke();
                            FirebaseAnalyticsManager.SendAnalyticCus("UnLocked_Spoiler_Cash", $"SpoilerIndex_{selectedSpoiler}");
                        }
                        else
                        {
                            GameManager.Instance.Show_GetCoinsPanel();
                            FirebaseAnalyticsManager.SendAnalyticCus("NotEnoughCash_Spoiler", $"SpoilerIndex_{selectedSpoiler}");
                        }
                    }
                    else
                    {
                        CustomAd.ShowRewarded(() =>
                        {
                            unlockWork.Invoke();
                            FirebaseAnalyticsManager.SendAnalyticCus("UnLocked_Spoiler_Ad", $"SpoilerIndex_{selectedSpoiler}");
                        });
                    }
                }
                break;
        }
    }

    public void SelectCustomization(int index)
    {
        if (!IsCarUnlocked(selectedCar)) return;
        activeCarCustomize.PaintManager.Restore();
        // activeCarCustomize.UpgradeManager.Restore();
        if (activeCarCustomize.SpoilerManager) activeCarCustomize.SpoilerManager.Restore();
        activeCarCustomize.WheelManager.Restore();
        activeCarCustomize.NeonManager.Restore();

        selectedPaint = SelectedPaint;
        selectedTyre = SelectedTyre;
        selectedNeon = SelectedNeon;
        selectedSpoiler = SelectedSpoiler;

        if (index == 0) selectedCustomization = SelectedCustomization.None;
        if (index == 1) selectedCustomization = SelectedCustomization.Upgrade;
        if (index == 2) selectedCustomization = SelectedCustomization.Paint;
        if (index == 3) selectedCustomization = SelectedCustomization.Tyre;
        if (index == 4) selectedCustomization = SelectedCustomization.Neon;
        if (index == 5) selectedCustomization = SelectedCustomization.Spoiler;
        UpdateUI();

        if (selectedCustomization != SelectedCustomization.None)
            FirebaseAnalyticsManager.SendAnalyticCus("Customize", $"SelectCustomize_{selectedCustomization.ToString()}");
    }

    public void SelecCar(int index)
    {
        HR_MainMenuManager.Instance.SelectCar(index);
        selectedCustomization = SelectedCustomization.None;
        selectedCar = index;
        if (IsCarUnlocked(index)) GameManager.SelectedCar = selectedCar;
        UpdateUI();
        var eventParameter = IsCarUnlocked(index) ? "UnLocked" : "Locked";
        FirebaseAnalyticsManager.SendAnalyticCus("SelectCar", $"CarIndex_{index}_{eventParameter}");
    }

    public void SelectPaint(int index)
    {
        selectedPaint = Mathf.Clamp(index, 0, 100);
        if (IsPaintUnlocked(index))
        {
            SelectedPaint = selectedPaint;
        }
        UpdateUI();
        var eventParameter = IsPaintUnlocked(index) ? "UnLocked" : "Locked";
        FirebaseAnalyticsManager.SendAnalyticCus("SelectPaint", $"PaintIndex_{index}_{eventParameter}");
    }

    public void SelectTyre(int index)
    {
        selectedTyre = index;
        if (IsTyreUnlocked(index))
        {
            SelectedTyre = selectedTyre;
        }
        UpdateUI();
        var eventParameter = IsTyreUnlocked(index) ? "UnLocked" : "Locked";
        FirebaseAnalyticsManager.SendAnalyticCus("SelectTyre", $"TyreIndex_{index}_{eventParameter}");
    }

    public void SelectNeon(int index)
    {
        selectedNeon = index;
        if (IsNeonUnlocked(index)) SelectedNeon = selectedNeon;
        UpdateUI();
        var eventParameter = IsNeonUnlocked(index) ? "UnLocked" : "Locked";
        FirebaseAnalyticsManager.SendAnalyticCus("SelectNeon", $"NeonIndex_{index}_{eventParameter}");
    }

    public void SelectSpoiler(int index)
    {
        selectedSpoiler = Mathf.Clamp(index, 0, 100);
        if (IsSpoilerUnlocked(index))
        {
            SelectedSpoiler = selectedSpoiler;
        }
        UpdateUI();
        var eventParameter = IsSpoilerUnlocked(index) ? "UnLocked" : "Locked";
        FirebaseAnalyticsManager.SendAnalyticCus("SelectSpoiler", $"SpoilerIndex_{index}_{eventParameter}");
    }

    public void upgradeBtn(int index)
    {
        selectedUpgrade = index;
        UpdateUI();
        FirebaseAnalyticsManager.SendAnalyticCus("SelectUpgrade", $"UpgradeIndex_{index}");
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
    public static bool IsNeonUnlocked(int index)
    {
        // if (index < 0) return true;
        return PlayerPrefs.GetInt($"IsNeonUnlocked_{selectedCar}_{index}") == 0 ? false : true;
    }
    public static bool IsSpoilerUnlocked(int index)
    {
        // if (index < 0) return true;
        return PlayerPrefs.GetInt($"IsSpoilerUnlocked_{selectedCar}_{index}") == 0 ? false : true;
    }


    public static void UnlockCar(int carIndex)
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
    void UnlockNeon(int index)
    {
        PlayerPrefs.SetInt($"IsNeonUnlocked_{selectedCar}_{index}", 1);
    }
    void UnlockSpoiler(int spoilerIndex)
    {
        PlayerPrefs.SetInt($"IsSpoilerUnlocked_{selectedCar}_{spoilerIndex}", 1);
    }

    void SetBtnState(GameObject gameObj, bool isActive)
    {
        CanvasGroup canGroup;
        if (!gameObj.TryGetComponent<CanvasGroup>(out canGroup))
        {
            canGroup = gameObj.AddComponent<CanvasGroup>();
        }
        canGroup.interactable = isActive;
        canGroup.alpha = isActive ? 1 : 0.6f;
    }

    string GetSelectedUpgradeName()
    {
        return selectedUpgrade == 0 ? "Speed" : selectedUpgrade == 1 ? "Engine" :
            selectedUpgrade == 2 ? "Hanldling" : selectedUpgrade == 3 ? "Brake" : "";
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
    public int SelectedNeon
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
        if (neon.panel.activeInHierarchy) isCusSelected = true;
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
                neonBtnRefs = child.GetComponentsInChildren<PanelitemRef>(true);
            }
        }
    }

    [System.Serializable]
    public class References
    {
        public Button btn;
        public GameObject selectorHighlighter;
        public GameObject panel;
    }

    public enum SelectedCustomization
    {
        None = 0,
        Upgrade = 1,
        Paint = 2,
        Tyre = 3,
        Neon = 4,
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
