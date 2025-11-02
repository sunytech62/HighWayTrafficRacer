using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HR_MainMenuManager : MonoBehaviour
{

    #region SINGLETON PATTERN
    private static HR_MainMenuManager instance;
    public static HR_MainMenuManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<HR_MainMenuManager>();
            }

            return instance;
        }
    }
    #endregion

    [Header("Spawn Location Of The Cars")]
    public Transform carSpawnLocation;

    private GameObject[] createdCars;

    public HR_Player currentCar;

    public bool headlightsOn = false;

    public int carIndex = 0;

    public List<HR_CartItem> itemsInCart = new List<HR_CartItem>();

    public AsyncOperation async;
    [SerializeField] GameObject garageModel;

    private void Awake()
    {
        instance = this;
        // Setting time scale, volume, unpause, and target frame rate.
        Time.timeScale = 1f;
        AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.pause = false;

        if (HR_UIOptionsManager.Instance)
            HR_UIOptionsManager.Instance.OnEnable();

        // Getting the money.
        if (!PlayerPrefs.HasKey("Currency"))
            PlayerPrefs.SetInt("Currency", HR_Settings.Instance.initialMoney);

        // Getting last selected car index.
        //carIndex = PlayerPrefs.GetInt("SelectedPlayerCarIndex", 0);
        carIndex = GameManager.SelectedCar;

        CreateCars(); // Creating all selectable cars at once.
        SpawnCar(); // Spawning only target car (carIndex).
    }
    private void Start()
    {
        garageModel.SetActive(true);
    }
    private void CreateCars()
    {
        createdCars = new GameObject[HR_PlayerCars.Instance.cars.Length];

        for (int i = 0; i < createdCars.Length; i++)
        {
            createdCars[i] = (RCCP.SpawnRCC(HR_PlayerCars.Instance.cars[i].playerCar.GetComponent<RCCP_CarController>(), carSpawnLocation.position, carSpawnLocation.rotation, false, false, false)).gameObject;
            createdCars[i].SetActive(false);

            if (createdCars[i].GetComponent<RCCP_CarController>().Lights != null)
                createdCars[i].GetComponent<RCCP_CarController>().Lights.lowBeamHeadlights = headlightsOn;
        }
    }

    private void SpawnCar()
    {
        //if (HR_PlayerCars.Instance.cars[carIndex].price <= 0 || HR_PlayerCars.Instance.cars[carIndex].unlocked)
        //    HR_API.UnlockVehicle(carIndex);

        for (int i = 0; i < createdCars.Length; i++)
        {
            if (createdCars[i].activeInHierarchy)
                createdCars[i].SetActive(false);
        }
        createdCars[carIndex].SetActive(true);
        RCCP.RegisterPlayerVehicle(createdCars[carIndex].GetComponent<RCCP_CarController>(), false, false);

        currentCar = createdCars[carIndex].GetComponent<HR_Player>();

        if (!PlayerPrefs.HasKey(currentCar.CarController.Customizer.saveFileName))
        {
            SaveCustomization();
        }
        else
        {
            LoadCustomization();
            ApplyCustomization();
        }

        HR_Events.Event_OnVehicleChanged(carIndex);
    }

    public void BuyCar()
    {
        if (HR_API.OwnedVehicle(carIndex))
        {
            Debug.LogError("Car is already owned!");
            return;
        }

        // If the currency is enough, save it and consume currency. Otherwise, display the informer.
        if (HR_API.GetCurrency() >= HR_PlayerCars.Instance.cars[carIndex].price)
        {
            HR_API.ConsumeCurrency(HR_PlayerCars.Instance.cars[carIndex].price);
        }
        else
        {
            HR_UI_InfoDisplayer.Instance.ShowInfo("You have to earn " + (HR_PlayerCars.Instance.cars[carIndex].price - HR_API.GetCurrency()).ToString() + " more money to buy this vehicle");
            return;
        }

        // Saving the car.
        HR_API.UnlockVehicle(carIndex);
        // And spawning again to check modders of the car.
        SpawnCar();
    }
    public void SelectCar()
    {
        PlayerPrefs.SetInt("SelectedPlayerCarIndex", carIndex);
    }
    public void PositiveCarIndex()
    {
        carIndex++;

        if (carIndex >= createdCars.Length) carIndex = 0;

        SpawnCar();
    }
    public void NegativeCarIndex()
    {
        carIndex--;

        if (carIndex < 0) carIndex = createdCars.Length - 1;

        SpawnCar();
    }

    public void SelectCar(int index)
    {
        carIndex = index;

        if (carIndex < 0) carIndex = createdCars.Length - 1;

        SpawnCar();
    }
    public void SelectScene(string levelName)
    {

        PlayerPrefs.SetString("SelectedScene", levelName);

    }

    public void SelectMode(int _modeIndex)
    {
        GameManager.SelectedMode = _modeIndex switch
        {
            0 => GameMode.Endless,
            1 => GameMode.Challenge,
            2 => GameMode.TimeTrial,
            3 => GameMode.LowSpeedBomb,
            4 => GameMode.PolliceChase,
            _ => GameMode.Endless,
        };
    }

    public void StartRace()
    {
        SelectCar();
        SaveCustomization();
        GameManager.Instance.LoadingPanel(true);
        async = SceneManager.LoadSceneAsync(PlayerPrefs.GetString("SelectedScene", ""));
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    public void Testing_AddMoney()
    {
        HR_API.AddCurrency(10000);
    }
    public void Testing_UnlockAllCars()
    {
        HR_API.UnlockAllVehicles();
    }
    public void Testing_ResetSave()
    {
        HR_API.ResetGame();
    }

    public void SaveCustomization()
    {
        HR_Player currentVehicle = currentCar;

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Save();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");

        //SGS_UI_Informer.Instance.Info("Customization saved!");
    }
    public void LoadCustomization()
    {
        HR_Player currentVehicle = currentCar;

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Load();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");
    }

    public void ApplyCustomization()
    {
        HR_Player currentVehicle = currentCar;

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Initialize();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");
    }

    private void Reset()
    {
        GameObject carSpawnLocationGO = GameObject.Find("HR_SpawnLocation");

        if (carSpawnLocationGO) carSpawnLocation = carSpawnLocationGO.transform;

        if (carSpawnLocation != null) return;

        carSpawnLocation = new GameObject("HR_SpawnLocation").transform;
        carSpawnLocation.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

    }
    public void AddItemToCart(HR_CartItem newItem)
    {
        Debug.LogError("ffff");
        for (int i = 0; i < itemsInCart.Count; i++)
        {
            if (itemsInCart[i] != null)
            {
                if (Equals(itemsInCart[i].itemType, newItem.itemType))
                    itemsInCart.RemoveAt(i);
            }
        }

        if (!itemsInCart.Contains(newItem)) itemsInCart.Add(newItem);
    }

    public void RemoveItemFromCart(HR_CartItem newItem)
    {
        for (int i = 0; i < itemsInCart.Count; i++)
        {
            if (itemsInCart[i] != null)
            {
                if (Equals(itemsInCart[i].itemType, newItem.itemType))
                    itemsInCart.RemoveAt(i);
            }
        }

        if (itemsInCart.Contains(newItem))
            itemsInCart.Remove(newItem);
    }
    public void ClearCart()
    {
        itemsInCart.Clear();

        LoadCustomization();
        ApplyCustomization();

        HR_Player currentVehicle = currentCar;

        RCCP_VehicleUpgrade_WheelManager wheelManager = currentVehicle.CarController.Customizer.WheelManager;

        if (wheelManager && currentVehicle.CarController.Customizer.WheelManager.wheelIndex == -1)
            currentVehicle.CarController.Customizer.WheelManager.Restore();

        RCCP_VehicleUpgrade_PaintManager paintManager = currentVehicle.CarController.Customizer.PaintManager;

        if (paintManager && currentVehicle.CarController.Customizer.PaintManager.color == new Color(1f, 1f, 1f, 0f))
            currentVehicle.CarController.Customizer.PaintManager.Restore();

        //  Updating all purchasable items in the scene.
        HR_UI_PurchaseItem[] uI_PurchaseItems = FindObjectsByType<HR_UI_PurchaseItem>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < uI_PurchaseItems.Length; i++)
            uI_PurchaseItems[i].OnEnable();

        //  Updating all upgradable items in the scene.
        HR_UI_PurchaseUpgrade[] uI_UpgradeItems = FindObjectsByType<HR_UI_PurchaseUpgrade>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < uI_UpgradeItems.Length; i++)
            uI_UpgradeItems[i].OnEnable();
    }

    public void PurchaseCart()
    {
        //  Calculating the total price.
        int totalPrice = 0;

        //  Calculating the total price.
        for (int i = 0; i < itemsInCart.Count; i++)
        {
            if (itemsInCart[i] != null)
                totalPrice += itemsInCart[i].price;
        }

        //  If player money is enough to purchase the cart, proceed. Otherwise return.
        if (HR_API.GetCurrency() < totalPrice)
        {
            //SGS_UI_Informer.Instance.Info("Not enough money to purchase the cart!");
            return;
        }

        //  Consuming the money.
        HR_API.ConsumeCurrency(totalPrice);

        //  Saving all purchased items.
        for (int i = 0; i < itemsInCart.Count; i++)
        {
            if (itemsInCart[i] != null)
                PlayerPrefs.SetInt(itemsInCart[i].saveKey, 1);
        }

        //  Saving the loadout.
        SaveCustomization();

        //  Clearing the cart.
        itemsInCart.Clear();

        //  Updating all purchasable items in the scene.
        HR_UI_PurchaseItem[] uI_PurchaseItems = FindObjectsByType<HR_UI_PurchaseItem>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < uI_PurchaseItems.Length; i++)
            uI_PurchaseItems[i].CheckPurchase();

        //  Updating all upgradable items in the scene.
        HR_UI_PurchaseUpgrade[] uI_UpgradeItems = FindObjectsByType<HR_UI_PurchaseUpgrade>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < uI_UpgradeItems.Length; i++)
            uI_UpgradeItems[i].OnEnable();
    }

    /// <summary>
    /// Updates all items in the cart list.
    /// </summary>
    //public void UpdateCartItemsList() {

    //    //  Getting all items in the cart.
    //    SGS_UI_CartItem[] items = cartItemsContent.GetComponentsInChildren<SGS_UI_CartItem>(true);

    //    //  Destroying all items before instantiating them.
    //    foreach (SGS_UI_CartItem item in items) {

    //        if (!Equals(item.gameObject, cartItemReference.gameObject))
    //            Destroy(item.gameObject);
    //        else if (cartItemReference.gameObject.activeSelf)
    //            cartItemReference.gameObject.SetActive(false);

    //    }

    //    //  Instantiating all items in the cart and setting them.
    //    for (int i = 0; i < itemsInCart.Count; i++) {

    //        SGS_UI_CartItem cartItem = Instantiate(cartItemReference.gameObject, cartItemsContent.transform).GetComponent<SGS_UI_CartItem>();
    //        cartItem.gameObject.SetActive(true);
    //        cartItem.SetItem(itemsInCart[i]);

    //    }

    //    //  Calculating the total price.
    //    int totalPrice = 0;

    //    //  Calculating the total price.
    //    for (int i = 0; i < itemsInCart.Count; i++) {

    //        if (itemsInCart[i] != null)
    //            totalPrice += itemsInCart[i].price;

    //    }

    //    //  Enable the purchase button if total price is above 0. Disable otherwise.
    //    if (totalPrice > 0)
    //        purchaseCartButton.SetActive(true);
    //    else
    //        purchaseCartButton.SetActive(false);

    //    //  Set price text if purchase button is enabled.
    //    if (purchaseCartButton.activeSelf)
    //        purchaseCartButton.GetComponentInChildren<TextMeshProUGUI>().text = "Purchase For\n$ " + totalPrice.ToString("F0");

    //}

}
