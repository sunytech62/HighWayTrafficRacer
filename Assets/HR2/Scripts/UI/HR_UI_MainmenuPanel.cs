//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2025 BoneCracker Games
// https://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
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

    public GameObject welcomeMenu;
    public GameObject mainMenu;
    public GameObject carSelectionMenu;
    public GameObject customizationSelectionMenu;
    public GameObject modsSelectionMenu;
    public GameObject sceneSelectionMenu;
    public GameObject carStatsPanel;
    public GameObject optionsMenu;
    public GameObject controlsMenu;

    [Header("UI Loading Section")]
    public GameObject loadingScreen;
    public Slider loadingBar;

    [Header("Buttons")]
    public GameObject buyCarButton;
    public GameObject buyCarButtonAd;
    public GameObject selectCarButton;
    public GameObject controlsButton;

    [Header("Texts")]
    public TextMeshProUGUI playerName;
    public TextMeshProUGUI currency;
    public TextMeshProUGUI panelTitleText;

    [Header("InputTexts")]
    public TMP_InputField playerNameInputField;

    [Header("Best Score Texts")]
    public TextMeshProUGUI vehicleNameText;
    public TextMeshProUGUI bestScoreOneWay;
    public TextMeshProUGUI bestScoreTwoWay;
    public TextMeshProUGUI bestScoreTimeLeft;
    public TextMeshProUGUI bestScoreBomb;

    [Header("UI Sliders For Vehicle Stats")]
    public Image vehicleStats_Engine;
    public Image vehicleStats_Handling;
    public Image vehicleStats_Speed;

    [Space()] public Image vehicleStats_Engine_Upgraded;
    public Image vehicleStats_Handling_Upgraded;
    public Image vehicleStats_Speed_Upgraded;

    [Header("Cart")]
    public GameObject cartPanel;
    public GameObject purchaseCartButton;
    public GameObject cartItemsContent;
    public HR_UI_CartItem cartItemReference;
    private HR_UI_PurchaseItem[] itemPurchaseButtons;
    public List<HR_CartItem> itemsInCart
    {
        get
        {
            return MainMenuManager.itemsInCart;
        }
    }
    private void Awake()
    {

        bool firstPlay = HR_API.IsFirstGameplay();

        if (!firstPlay)
        {

            EnableMenu(mainMenu);

        }
        else
        {

            playerNameInputField.SetTextWithoutNotify("New Player " + Random.Range(0, 999).ToString());
            EnableMenu(welcomeMenu);

        }

        if (controlsButton)
            controlsButton.SetActive(!Application.isMobilePlatform);

    }

    public void EnableMenu(GameObject activeMenu)
    {

        welcomeMenu.SetActive(false);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        controlsMenu.SetActive(false);
        carSelectionMenu.SetActive(false);
        customizationSelectionMenu.SetActive(false);
        modsSelectionMenu.SetActive(false);
        sceneSelectionMenu.SetActive(false);
        loadingScreen.SetActive(false);

        activeMenu.SetActive(true);

        if (activeMenu == modsSelectionMenu)
            BestScores();

    }

    private void Start()
    {

        HR_API_OnPlayerNameChanged();
        HR_API_OnPlayerMoneyChanged();

    }

    private void OnEnable()
    {

        HR_Events.OnVehicleChanged += HR_Events_OnVehicleChanged;
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

        HR_Events.OnVehicleChanged -= HR_Events_OnVehicleChanged;
        HR_API.OnPlayerNameChanged -= HR_API_OnPlayerNameChanged;
        HR_API.OnPlayerMoneyChanged -= HR_API_OnPlayerMoneyChanged;

    }
    private void HR_Events_OnVehicleChanged(int carIndex)
    {

        CheckCurrentVehicle(carIndex);

    }

    private void CheckCurrentVehicle(int carIndex)
    {

        if (vehicleNameText)
            vehicleNameText.text = HR_PlayerCars.Instance.cars[carIndex].vehicleName;

        if (HR_API.OwnedVehicle(carIndex))
        {

            if (buyCarButton.GetComponentInChildren<TextMeshProUGUI>())
                buyCarButton.GetComponentInChildren<TextMeshProUGUI>().text = "";

            buyCarButton.SetActive(false);
            buyCarButtonAd.SetActive(false);
            selectCarButton.SetActive(true);

        }
        else
        {

            if (buyCarButton.GetComponentInChildren<TextMeshProUGUI>())
                buyCarButton.GetComponentInChildren<TextMeshProUGUI>().text = "$ " + HR_PlayerCars.Instance.cars[carIndex].price.ToString("F0");

            selectCarButton.SetActive(false);
            buyCarButton.SetActive(true);
            buyCarButtonAd.SetActive(true);

        }

    }

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

    public void SetPanelTitleText(string title)
    {

        panelTitleText.text = title;

    }

    public void SetModsPanel(bool state)
    {

        carStatsPanel.SetActive(state);

    }

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

            if (cartPanel.activeInHierarchy)
                UpdateCartItemsList();

        }

        if (MainMenuManager)
        {

            if (MainMenuManager.async != null && !MainMenuManager.async.isDone)
                loadingBar.value = MainMenuManager.async.progress;

        }

    }

    public void CheckCurrentVehicle()
    {

        //  Return if main manager couldn't found.
        if (!HR_MainMenuManager.Instance)
            return;

        //  Finding the current player vehicle.
        RCCP_CarController currentVehicle = HR_MainMenuManager.Instance.currentCar.CarController;

        //  If current vehicle is not null, display stats of the vehicle.
        if (currentVehicle)
        {

            //  Fill amount of the engine torque.
            if (vehicleStats_Engine && currentVehicle.Engine)
                vehicleStats_Engine.fillAmount = Mathf.InverseLerp(-400f, 800f, currentVehicle.Engine.maximumTorqueAsNM);

            //  Fill amount of the stability strength.
            if (vehicleStats_Handling && currentVehicle.Stability)
                vehicleStats_Handling.fillAmount = Mathf.InverseLerp(0f, .65f, (currentVehicle.Stability.steerHelperStrength));

            //  Fill amount of the speed.
            if (vehicleStats_Speed && currentVehicle.Differential)
                vehicleStats_Speed.fillAmount = 1f - Mathf.InverseLerp(3.1f, 5.31f, currentVehicle.Differential.finalDriveRatio);

            //  Fill amount of the upgraded engine torque.
            if (vehicleStats_Engine_Upgraded && currentVehicle.Customizer && currentVehicle.Customizer.UpgradeManager && currentVehicle.Customizer.UpgradeManager.Engine)
                vehicleStats_Engine_Upgraded.fillAmount = Mathf.InverseLerp(-400f, 800f, currentVehicle.Customizer.UpgradeManager.Engine.defEngine * currentVehicle.Customizer.UpgradeManager.Engine.efficiency);
            else if (vehicleStats_Engine_Upgraded)
                vehicleStats_Engine_Upgraded.fillAmount = 0f;

            //  Fill amount of the upgraded handling strength.
            if (vehicleStats_Handling_Upgraded && currentVehicle.Customizer && currentVehicle.Customizer.UpgradeManager && currentVehicle.Customizer.UpgradeManager.Handling)
                vehicleStats_Handling_Upgraded.fillAmount = Mathf.InverseLerp(0f, .65f, currentVehicle.Customizer.UpgradeManager.Handling.defHandling * currentVehicle.Customizer.UpgradeManager.Handling.efficiency);
            else if (vehicleStats_Handling_Upgraded)
                vehicleStats_Handling_Upgraded.fillAmount = 0f;

            //Fill amount of the upgraded speed.
            if (vehicleStats_Speed_Upgraded && currentVehicle.Customizer && currentVehicle.Customizer.UpgradeManager && currentVehicle.Customizer.UpgradeManager.Speed)
                vehicleStats_Speed_Upgraded.fillAmount = 1f - Mathf.InverseLerp(3.1f, 5.31f, Mathf.Lerp(currentVehicle.Customizer.UpgradeManager.Speed.defRatio, currentVehicle.Customizer.UpgradeManager.Speed.defRatio * .6f, currentVehicle.Customizer.UpgradeManager.Speed.efficiency - 1f));
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

    public void UpdateCartItemsList()
    {

        HR_UI_CartItem[] items = cartItemsContent.GetComponentsInChildren<HR_UI_CartItem>(true);

        foreach (HR_UI_CartItem item in items)
        {

            if (!Equals(item.gameObject, cartItemReference.gameObject))
                Destroy(item.gameObject);
            else if (cartItemReference.gameObject.activeSelf)
                cartItemReference.gameObject.SetActive(false);

        }

        for (int i = 0; i < itemsInCart.Count; i++)
        {

            HR_UI_CartItem cartItem = Instantiate(cartItemReference.gameObject, cartItemsContent.transform).GetComponent<HR_UI_CartItem>();
            cartItem.gameObject.SetActive(true);
            cartItem.SetItem(itemsInCart[i]);

        }

        int totalPrice = 0;

        for (int i = 0; i < itemsInCart.Count; i++)
        {

            if (itemsInCart[i] != null)
                totalPrice += itemsInCart[i].price;

        }

        if (totalPrice > 0)
            purchaseCartButton.SetActive(true);
        else
            purchaseCartButton.SetActive(false);

        if (purchaseCartButton.activeSelf)
            purchaseCartButton.GetComponentInChildren<TextMeshProUGUI>().text = "Purchase For\n$ " + totalPrice.ToString("F0");

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
        EnableMenu(mainMenu);

    }

    private void BestScores()
    {

        int[] scores = HR_API.GetHighScores();

        bestScoreOneWay.text = "BEST SCORE\n" + scores[0];
        bestScoreTwoWay.text = "BEST SCORE\n" + scores[1];
        bestScoreTimeLeft.text = "BEST SCORE\n" + scores[2];
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
            4 => PanelName.Store,
            5 => PanelName.ChaLvlSelection,
            6 => PanelName.Setting,
            7 => PanelName.Exit,
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
        selectedPanel = activePanel switch
        {
            PanelName.MainMenu => PanelName.Exit,
            PanelName.Garage => PanelName.MainMenu,
            PanelName.ModeSelection => PanelName.Garage,
            PanelName.ChaLvlSelection => PanelName.ModeSelection,
            PanelName.Store => PanelName.MainMenu,
            PanelName.Setting => PanelName.MainMenu,
            PanelName.Exit => PanelName.MainMenu,
            _ => PanelName.None,
        };
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
    }
}
