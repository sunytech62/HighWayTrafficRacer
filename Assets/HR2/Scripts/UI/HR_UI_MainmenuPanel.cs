using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HR_UI_MainmenuPanel : MonoBehaviour
{
    #region SINGLETON PATTERN
    private static HR_UI_MainmenuPanel instance;
    public static HR_UI_MainmenuPanel Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<HR_UI_MainmenuPanel>();
            }
            return instance;
        }
    }
    #endregion

    private HR_MainMenuManager mainmenuManager;
    public HR_MainMenuManager MainMenuManager
    {
        get
        {
            if (mainmenuManager == null)
                mainmenuManager = HR_MainMenuManager.Instance;

            return mainmenuManager;
        }
    }


    [Header("Texts")]
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI currency;

    [Header("InputTexts")]
    public TMP_InputField playerNameInputField;

    [Header("Best Score Texts")]
    public TextMeshProUGUI vehicleNameText;
    public TextMeshProUGUI bestScoreBomb;

    [Header("UI Sliders For Vehicle Stats")]
    public Image vehicleStats_Engine;
    public TextMeshProUGUI vehicleStatsEngineTxt;
    public Image vehicleStats_Handling;
    public TextMeshProUGUI vehicleStatsHandlingTxt;
    public Image vehicleStats_Speed;
    public TextMeshProUGUI vehicleStatsSpeedTxt;

    [Space()] public Image vehicleStats_Engine_Upgraded;
    public Image vehicleStats_Handling_Upgraded;
    public Image vehicleStats_Speed_Upgraded;

    [Header("Garage")]
    [SerializeField] GaragePanel garagePanelScr;

    private void Awake()
    {
        bool firstPlay = HR_API.IsFirstGameplay();
    }

    private void Start()
    {
        GameManager.Instance.LoadingPanel(false);
        HR_API_OnPlayerNameChanged();
        HR_API_OnPlayerMoneyChanged();
    }

    private void OnEnable()
    {
        HR_API.OnPlayerNameChanged += HR_API_OnPlayerNameChanged;
        HR_API.OnPlayerMoneyChanged += HR_API_OnPlayerMoneyChanged;
    }

    private void HR_API_OnPlayerMoneyChanged()
    {
        currency.text = "$ " + HR_API.GetCurrency().ToString("F0");
    }

    private void HR_API_OnPlayerNameChanged()
    {
        playerName.text = HR_API.GetPlayerName();
    }

    private void OnDisable()
    {
        HR_API.OnPlayerNameChanged -= HR_API_OnPlayerNameChanged;
        HR_API.OnPlayerMoneyChanged -= HR_API_OnPlayerMoneyChanged;
    }

    #region Test

    public void Testing_AddMoney()
    {
        MainMenuManager.Testing_AddMoney();
    }
    public void Testing_UnlockAllCars()
    {
        MainMenuManager.Testing_UnlockAllCars();
    }

    public void Testing_ResetSave()
    {
        MainMenuManager.Testing_ResetSave();
    }
    #endregion


    public void BuyCar()
    {
        MainMenuManager.BuyCar();
    }

    public void SelectCar()
    {
        MainMenuManager.SelectCar();
    }

    public void PositiveCarIndex()
    {
        MainMenuManager.PositiveCarIndex();
    }

    public void NegativeCarIndex()
    {
        MainMenuManager.NegativeCarIndex();
    }

    public void SelectScene(string levelName)
    {
        MainMenuManager.SelectScene(levelName);
    }

    public void SelectMode(int _modeIndex)
    {
        MainMenuManager.SelectMode(_modeIndex);
    }

    public void StartRace()
    {
        MainMenuManager.StartRace();
    }

    public void QuitGame()
    {
        MainMenuManager.QuitGame();
    }

    private void Update()
    {
        HR_Player currentVehicle = MainMenuManager.currentCar;

        if (currentVehicle)
        {
            CheckCurrentVehicle();
        }
    }

    public void CheckCurrentVehicle()
    {
        if (!HR_MainMenuManager.Instance) return;

        RCCP_CarController currentVehicle = HR_MainMenuManager.Instance.currentCar.CarController;

        if (currentVehicle)
        {
            if (vehicleStats_Engine && currentVehicle.Engine)
                vehicleStats_Engine.fillAmount = Mathf.InverseLerp(-400f, 800f, currentVehicle.Engine.maximumTorqueAsNM);

            if (vehicleStats_Handling && currentVehicle.Stability)
                vehicleStats_Handling.fillAmount = Mathf.InverseLerp(0f, .65f, (currentVehicle.Stability.steerHelperStrength));

            if (vehicleStats_Speed && currentVehicle.Differential)
                vehicleStats_Speed.fillAmount = 1f - Mathf.InverseLerp(3.1f, 5.31f, currentVehicle.Differential.finalDriveRatio);

            if (vehicleStats_Engine_Upgraded && currentVehicle.Customizer && currentVehicle.Customizer.UpgradeManager && currentVehicle.Customizer.UpgradeManager.Engine)
            {
                vehicleStats_Engine_Upgraded.fillAmount = Mathf.InverseLerp(-400f, 800f, currentVehicle.Customizer.UpgradeManager.Engine.defEngine * currentVehicle.Customizer.UpgradeManager.Engine.efficiency);
            }
            else if (vehicleStats_Engine_Upgraded)
                vehicleStats_Engine_Upgraded.fillAmount = 0f;

            if (vehicleStats_Handling_Upgraded && currentVehicle.Customizer && currentVehicle.Customizer.UpgradeManager && currentVehicle.Customizer.UpgradeManager.Handling)
            {
                vehicleStats_Handling_Upgraded.fillAmount = Mathf.InverseLerp(0f, .65f, currentVehicle.Customizer.UpgradeManager.Handling.defHandling * currentVehicle.Customizer.UpgradeManager.Handling.efficiency);
            }
            else if (vehicleStats_Handling_Upgraded)
                vehicleStats_Handling_Upgraded.fillAmount = 0f;

            if (vehicleStats_Speed_Upgraded && currentVehicle.Customizer && currentVehicle.Customizer.UpgradeManager && currentVehicle.Customizer.UpgradeManager.Speed)
            {
                vehicleStats_Speed_Upgraded.fillAmount = 1f - Mathf.InverseLerp(3.1f, 5.31f, Mathf.Lerp(currentVehicle.Customizer.UpgradeManager.Speed.defRatio, currentVehicle.Customizer.UpgradeManager.Speed.defRatio * .6f, currentVehicle.Customizer.UpgradeManager.Speed.efficiency - 1f));
            }
            else if (vehicleStats_Speed_Upgraded)
                vehicleStats_Speed_Upgraded.fillAmount = 0f;
        }
    }

    public void CheckUpgradePurchased(HR_CartItem newItem)
    {

        HR_Player currentVehicle = MainMenuManager.currentCar;

        if (!currentVehicle.CarController.Customizer)
        {

            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");
            return;

        }

        if (PlayerPrefs.HasKey(currentVehicle.CarController.Customizer.saveFileName + newItem.saveKey))
            RemoveItemFromCart(newItem);
        else
            AddItemToCart(newItem);

    }

    public void CheckItemPurchased(HR_CartItem newItem)
    {

        if (PlayerPrefs.HasKey(newItem.saveKey))
            RemoveItemFromCart(newItem);
        else
            AddItemToCart(newItem);

        if (newItem.itemType == HR_CartItem.CartItemType.Customization)
            HR_UI_InfoDisplayer.Instance.ShowInfo("Added Unlocker To The Cart, Purchase It To Use Customization");

    }

    public void AddItemToCart(HR_CartItem newItem)
    {

        MainMenuManager.AddItemToCart(newItem);

    }

    public void RemoveItemFromCart(HR_CartItem newItem)
    {

        MainMenuManager.RemoveItemFromCart(newItem);

    }

    public void ClearCart()
    {

        MainMenuManager.ClearCart();

        HR_UI_PurchaseItem[] uI_PurchaseItems = FindObjectsByType<HR_UI_PurchaseItem>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < uI_PurchaseItems.Length; i++)
            uI_PurchaseItems[i].OnEnable();

        HR_UI_PurchaseUpgrade[] uI_UpgradeItems = FindObjectsByType<HR_UI_PurchaseUpgrade>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < uI_UpgradeItems.Length; i++)
            uI_UpgradeItems[i].OnEnable();

    }

    public void PurchaseCart()
    {

        MainMenuManager.PurchaseCart();

        HR_UI_PurchaseItem[] uI_PurchaseItems = FindObjectsByType<HR_UI_PurchaseItem>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < uI_PurchaseItems.Length; i++)
            uI_PurchaseItems[i].CheckPurchase();

        HR_UI_PurchaseUpgrade[] uI_UpgradeItems = FindObjectsByType<HR_UI_PurchaseUpgrade>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < uI_UpgradeItems.Length; i++)
            uI_UpgradeItems[i].OnEnable();
    }

    public void SaveCustomization()
    {
        MainMenuManager.SaveCustomization();
    }

    public void LoadCustomization()
    {
        MainMenuManager.LoadCustomization();
    }

    public void ApplyCustomization()
    {
        MainMenuManager.ApplyCustomization();
    }

    public void EnterPlayerName()
    {
        HR_API.SetPlayerName(playerNameInputField.text);
        HR_UI_InfoDisplayer.Instance.ShowInfo("Welcome " + HR_API.GetPlayerName() + "!");
    }

    private void BestScores()
    {
        int[] scores = HR_API.GetHighScores();
        bestScoreBomb.text = "BEST SCORE\n" + scores[3];
    }

    public void Quit()
    {
#if UNITY_EDITOR
        // This will stop play mode when running in the Unity Editor
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // This will quit the standalone build
            Application.Quit();
#endif
    }

    [Header("My Properties")]
    [SerializeField] PanelRef[] panels;
    [SerializeField] PanelName selectedPanel;

    public void SelectPanel(int panelIndex)
    {
        selectedPanel = panelIndex switch
        {
            1 => PanelName.MainMenu,
            2 => PanelName.Garage,
            3 => PanelName.ModeSelection,
            4 => PanelName.Environment,
            5 => PanelName.Store,
            6 => PanelName.ChaLvlSelection,
            7 => PanelName.Setting,
            8 => PanelName.Exit,
            _ => PanelName.None,
        };
        UpdateUI();
    }
    public void Back()
    {
        PanelName activePanel = PanelName.None;
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i].panel.activeInHierarchy)
            {
                activePanel = panels[i].panelName;
            }
        }
        if (activePanel == PanelName.Garage && garagePanelScr.IsCustomizationSelected())
        {
            garagePanelScr.SelectCustomization(0);
        }
        else
        {
            selectedPanel = activePanel switch
            {
                // PanelName.MainMenu => PanelName.Exit,
                PanelName.Garage => PanelName.Garage,
                PanelName.ModeSelection => PanelName.Garage,
                PanelName.ChaLvlSelection => PanelName.ModeSelection,
                PanelName.Store => PanelName.Garage,
                PanelName.Setting => PanelName.Garage,
                PanelName.Environment => PanelName.ModeSelection,
                PanelName.Exit => PanelName.Garage,
                _ => PanelName.Garage,
            };
        }
        UpdateUI();
    }

    void UpdateUI()
    {
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i].panelName == selectedPanel)
                panels[i].panel.SetActive(true);
            else
                panels[i].panel.SetActive(false);
        }
    }


    [System.Serializable]
    public class PanelRef
    {
        public PanelName panelName;
        public GameObject panel;
    }
    public enum PanelName
    {
        None = 0,
        MainMenu = 1,
        Garage = 2,
        ModeSelection = 3,
        ChaLvlSelection = 4,
        Store = 5,
        Setting = 6,
        Exit = 7,
        Environment = 8,
    }
}
