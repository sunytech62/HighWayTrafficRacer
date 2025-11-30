#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Linq;
using Unity.Services.Core.Environments;
using UnityEngine;

public class InAppPurchase : MonoBehaviour
{
    #region Instance

    static InAppPurchase _instance = null;
    public static InAppPurchase Instance
    {
        get
        {
            if (_instance == null)
            {
                if (FindObjectOfType<InAppPurchase>())
                {
                    _instance = FindObjectOfType<InAppPurchase>();
                }
                if (_instance != null)
                    DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(_instance.gameObject);
            return;
        }
    }

    #endregion

    #region Variable

    #region IAP Variables

    [SerializeField] IAP_Scriptable scriptableObject;
    System.Collections.Generic.List<InAppData> InappKeys = new System.Collections.Generic.List<InAppData>();
    System.Collections.Generic.Dictionary<string, InAppDataType> inappKeysDict = new System.Collections.Generic.Dictionary<string, InAppDataType>();
    UnityEngine.Purchasing.StoreController m_StoreController;
    System.Action actionAfterInitilize = null;
    System.Action<bool> actionAfterSuccessIAP = null;

    //Trigger when Initilization Successfully Completed.
    public event InitilizeCompleteDelegate InitilizeCompleteEvent;
    public delegate void InitilizeCompleteDelegate();

    //Trigger all Informations send to this event
    public event IAPInfoDelegate InfoEvent;
    public delegate void IAPInfoDelegate(string msg);

    //Trigger when IAP Successfully purchased
    public event IAPSuccessDelegate IAPSuccessEvent;
    public delegate void IAPSuccessDelegate(string msg);

    #endregion

    #region Editor Variables

    [SerializeField] bool isAutoInitilize = true;
    [SerializeField] bool isStartingDelay = true;
    [Range(0.1f, 5.0f)]
    [SerializeField] float startingDelay = 0.5f;
    [SerializeField] bool isDebugLog = false;
    [SerializeField] bool isAnalytics = false;
    [SerializeField] bool isMemoryThreshold = false;
    [SerializeField] int NoIAPBelowAvailableRAM = 200;
    [SerializeField] MemoryThreshold NoIAPBelowTotalRAM = MemoryThreshold._1024MB;
    public enum MemoryThreshold
    {
        _NoThreshold,
        _512MB,
        _1024MB,
        _1536MB,
        _2048MB,
        _3072MB,
        _4096MB
    }

    #endregion

    #region Refrences

    [SerializeField] IAP_WaitingPanel waitingPanel = null;
    IAP_WaitingPanel WaitingPanel
    {
        get
        {
            if (waitingPanel == null)
            {
                try
                {
                    waitingPanel = transform.GetComponentInChildren<IAP_WaitingPanel>();
                }
                catch (System.Exception) { }
                if (waitingPanel == null)
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        if (transform.GetChild(i).GetComponent<IAP_WaitingPanel>() != null)
                        {
                            waitingPanel = transform.GetChild(i).GetComponent<IAP_WaitingPanel>();
                            break;
                        }
                    }
                }

            }
            return waitingPanel;
        }
    }

    #endregion

    #endregion

    #region Initilization

    System.Collections.IEnumerator Start()
    {
        if (isAutoInitilize)
        {
            if (isStartingDelay) yield return new WaitForSeconds(startingDelay);
            _ = InitilizeAsync();
        }
        resetIAPUNderProcessAction = () => ResetIAPUnderProcess();
    }

    public async System.Threading.Tasks.Task InitilizeAsync(System.Action actionAfterInit = null)
    {
        try
        {
            if (!IsInternetConnection()) return;
            if (IsLowTotalRAM()) return;
            if (IsLowMemory(NoIAPBelowAvailableRAM)) return;
            if (initilizeStatus.Equals(InitilizeStatus.Initilizing) || initilizeStatus.Equals(InitilizeStatus.Initilized)) return;

            initilizeStatus = InitilizeStatus.Initilizing;
            ShowDebugLog("Initilizing...");
            var options = new Unity.Services.Core.InitializationOptions().SetEnvironmentName("production");
            await Unity.Services.Core.UnityServices.InitializeAsync(options);

            m_StoreController = UnityEngine.Purchasing.UnityIAPServices.StoreController();
            m_StoreController.OnPurchasePending += OnPurchasePending;
            m_StoreController.OnPurchaseConfirmed += OnPurchaseConfirmed;
            m_StoreController.OnPurchaseFailed += OnPurchaseConfirmed;
            m_StoreController.OnPurchasesFetchFailed += OnPurchaseFetchFailed;
            m_StoreController.OnProductsFetched += OnProductsFetched;

            SendAnalytics(EventName.IAP_Purchase, ParameterKey.myParameter, ParameterValue.InitilizationRequestSend);
            await m_StoreController.Connect();
            await System.Threading.Tasks.Task.Delay(500);
            FetchProducts();
            ShowDebugLog("Initilized");
            actionAfterInitilize = actionAfterInit;

            initilizeStatus = InitilizeStatus.Initilized;
            InitilizeCompleteEvent?.Invoke();
            actionAfterInitilize?.Invoke();
            SendAnalytics(EventName.IAP_Purchase, ParameterKey.myParameter, ParameterValue.InitilizationRequestSuccess);

            CheckPreRegistrationReward();
        }
        catch (System.Exception) { }
    }

    void FillProductsMetaData()
    {
        inappKeysDict.Clear();
        foreach (var p in m_StoreController.GetProducts())
        {
            if (p == null || p.metadata == null) continue;

            string id = p.definition.id;
            string title = p.metadata.localizedTitle;
            string desc = p.metadata.localizedDescription;
            string price = p.metadata.localizedPriceString;

            inappKeysDict[id] = new InAppDataType(p.definition.type, title, desc, price);
        }
    }

    void FetchProducts()
    {
        InappKeys.Clear();
        InappKeys = scriptableObject.IAP_Keys;
        InappKeys = RemoveDuplicatesKeys(InappKeys);

        var productDefs = new System.Collections.Generic.List<UnityEngine.Purchasing.ProductDefinition>();
        foreach (var key in InappKeys) productDefs.Add(new UnityEngine.Purchasing.ProductDefinition(key.name, key.type));
        m_StoreController.FetchProducts(productDefs);
    }
    public bool IsInitilized()
    {
        return initilizeStatus.Equals(InitilizeStatus.Initilized);
    }

    InitilizeStatus initilizeStatus = InitilizeStatus.None;
    enum InitilizeStatus
    {
        None,
        Initilizing,
        Initilized,
        Failed
    }

    #endregion

    #region Public Function

    public void Purchase(int index, System.Action<bool> actionAfterSuccess = null)
    {
        if (!IsInternetConnection()) return;
        if (IsLowMemory(NoIAPBelowAvailableRAM)) return;
        if (!IsInitilized())
        {
            _ = InitilizeAsync(new System.Action(() => { Purchase(index, actionAfterSuccess); }));
            return;
        }
        Purchase(InappKeys[index].name, actionAfterSuccess);
    }
    public void Purchase(IAP_Key_Enum enumKey, System.Action<bool> actionAfterSuccess = null)
    {
        Purchase(enumKey.ToString(), actionAfterSuccess);
    }
    public void Purchase(string productKey, System.Action<bool> actionAfterSuccess = null)
    {
        SendAnalytics(EventName.IAP_Purchase, ParameterKey.myParameter, ParameterValue.ButtonClick);
        SendAnalytics(EventName.IAP_ButtonClick, ParameterKey.myParameter, productKey);

        if (!IsInternetConnection()) return;
        if (IsLowMemory(NoIAPBelowAvailableRAM)) return;
        if (!IsInitilized())
        {
            _ = InitilizeAsync(new System.Action(() => { Purchase(productKey, actionAfterSuccess); }));
            return;
        }
        actionAfterSuccessIAP = actionAfterSuccess;
        try
        {
            SendAnalytics(EventName.IAP_Purchase, ParameterKey.myParameter, ParameterValue.OpenDialog);
            SendAnalytics(EventName.IAP_OpenDialog, ParameterKey.myParameter, productKey);
            WaitingPanel.EnablePanel(IAP_WaitingPanel.Status.Purchasing);
#if AdsManager_AdmobMediation
            AdsManager_AdmobMediation.Instance.IsAppOpenCanShow = false;
#endif
#if AdsManager_Applovin
            AdsManager_Applovin.Instance.IsAppOpenCanShow = false;
            AdsManager_Applovin.Instance.SaveLastBannerStatus();
#endif
            isIAPUnderProcess = true;
            ActionsPerformWithDelay(resetIAPUNderProcessAction, 20f);
            isPurchasingFromMyself = true;
            var product = m_StoreController?.GetProducts().FirstOrDefault(p => p.definition.id == productKey);
            if (product != null) m_StoreController?.PurchaseProduct(product);
        }
        catch (System.Exception) { }
        ShowDebugLog("Purchase(" + productKey + ")");
#if UNITY_EDITOR
        WaitingPanel.DisablePanel();
#endif
    }
    public string GetData(string productKey, InAppDataType.MetaDataType type)
    {
        if (!IsInitilized())
        {
            _ = InitilizeAsync();
            return "Not Available";
        }

        string result = "Not Available";
        if (inappKeysDict.TryGetValue(productKey, out InAppDataType resultData))
        {
            result = resultData.GetData(type);
        }
        ShowDebugLog("GetData( Key:" + productKey + ", Type:" + type.ToString() + ", Value:" + result);
        return result;
    }
    public void RestorePurchases()
    {
        if (!IsInitilized()) return;
#if UNITY_EDITOR
        ShowDebugLog("Restore Purchases Trigger");
        return;
#endif

        WaitingPanel.EnablePanel(IAP_WaitingPanel.Status.Restoring);
#if AdsManager_AdmobMediation
        AdsManager_AdmobMediation.Instance.IsAppOpenCanShow = false;
#endif
#if AdsManager_Applovin
        AdsManager_Applovin.Instance.IsAppOpenCanShow = false;
#endif
        try
        {
            m_StoreController.RestoreTransactions((bool returnValue, string data) =>
            {
                try
                {
                    if (returnValue) WaitingPanel.EnablePanel(IAP_WaitingPanel.Status.Restored);
                    else WaitingPanel.EnablePanel(IAP_WaitingPanel.Status.RestoreFailed);
                }
                catch (System.Exception) { WaitingPanel.EnablePanel(IAP_WaitingPanel.Status.RestoreFailed); }
            });
        }
        catch (System.Exception) { WaitingPanel.EnablePanel(IAP_WaitingPanel.Status.RestoreFailed); }
        ShowDebugLog("Restore Purchases Trigger");
    }

    #endregion

    #region Handlers

    private void OnProductsFetched(System.Collections.Generic.List<UnityEngine.Purchasing.Product> list)
    {
        FillProductsMetaData();
    }
    void OnPurchasePending(UnityEngine.Purchasing.PendingOrder order)
    {
        ShowDebugLog($"Purchase Pending: {order.CartOrdered.Items().First().Product.definition.id}");
        m_StoreController.ConfirmPurchase(order);
    }
    private void OnPurchaseFetchFailed(UnityEngine.Purchasing.PurchasesFetchFailureDescription description)
    {
        ShowDebugLog($"Purchase Fetch Failed. Reason : {description.failureReason}");
    }

    void OnPurchaseConfirmed(UnityEngine.Purchasing.Order order)
    {
        switch (order)
        {
            case UnityEngine.Purchasing.ConfirmedOrder confirmedOrder:
                {
                    var product = confirmedOrder.CartOrdered.Items().First().Product;
                    InAppSuccess(product);
                    IAPSuccessEvent?.Invoke(product.definition.id);
                    actionAfterSuccessIAP?.Invoke(true);
                    if (isPurchasingFromMyself)
                    {
                        SendAnalytics(EventName.IAP_Purchase_Success, ParameterKey.myParameter, product.definition.id);
                        SendAnalytics(EventName.IAP_Purchase, ParameterKey.myParameter, ParameterValue.PurchaseSuccess);
#if SingularSDKManager
            SingularSDKManager.Instance.SendEvent_InAppPurchase(product);
#endif
                    }
                    break;
                }
            case UnityEngine.Purchasing.FailedOrder failedOrder:
                {
                    ShowDebugLog($"Purchase failed: {failedOrder.CartOrdered.Items().First().Product.definition.id}, {failedOrder.FailureReason}, {failedOrder.Details}");
                    actionAfterSuccessIAP?.Invoke(false);
                    SendAnalytics(EventName.IAP_Purchase_Failure, ParameterKey.myParameter, failedOrder.FailureReason.ToString());
                    SendAnalytics(EventName.IAP_Purchase, ParameterKey.myParameter, ParameterValue.PurchaseFail);
                    break;
                }
        }
        actionAfterSuccessIAP = null;
        WaitingPanel.DisablePanel();
        ActionsPerformWithDelay(resetIAPUNderProcessAction, 2f);
    }

    #endregion

    #region Additional

    public static bool isPurchasingFromMyself = false;
    public static bool isIAPUnderProcess = false;
    System.Action resetIAPUNderProcessAction = null;
    void ResetIAPUnderProcess()
    {
        isIAPUnderProcess = false;
    }

    #region Supportive

    void ShowDebugLog(string msg)
    {
        if (isDebugLog) Debug.Log("<color=cyan>IAP Log: </color>" + msg);
        InfoEvent?.Invoke(msg);
    }

    bool IsInternetConnection()
    {
        try
        {
            return Application.internetReachability != NetworkReachability.NotReachable;
        }
        catch (System.Exception) { return false; }
    }
    public System.Collections.Generic.List<InAppData> RemoveDuplicatesKeys(System.Collections.Generic.List<InAppData> inAppDataList)
    {
        System.Collections.Generic.HashSet<string> seenNames = new System.Collections.Generic.HashSet<string>();
        System.Collections.Generic.List<InAppData> uniqueList = new System.Collections.Generic.List<InAppData>();

        foreach (var item in inAppDataList)
        {
            if (seenNames.Add(item.name)) uniqueList.Add(item);
            else ShowDebugLog("Remove Duplicate ID :" + item.name);
        }
        return uniqueList;
    }

    bool IsLowTotalRAM()
    {
        if (PlayerPrefs.HasKey("IsLowTotalRAM"))
            return PlayerPrefs.GetInt("IsLowTotalRAM", 0) == 1;

        int totalMemory = SystemInfo.systemMemorySize;
        ShowDebugLog("Fetch Total RAM : " + totalMemory);
        bool isLowRAM = false;
        switch (NoIAPBelowTotalRAM)
        {
            case MemoryThreshold._512MB:
                {
                    if (totalMemory <= 512) isLowRAM = true;
                    break;
                }
            case MemoryThreshold._1024MB:
                {
                    if (totalMemory <= 1024) isLowRAM = true;
                    break;
                }
            case MemoryThreshold._1536MB:
                {
                    if (totalMemory <= 1536) isLowRAM = true;
                    break;
                }
            case MemoryThreshold._2048MB:
                {
                    if (totalMemory <= 2048) isLowRAM = true;
                    break;
                }
            case MemoryThreshold._3072MB:
                {
                    if (totalMemory <= 3072) isLowRAM = true;
                    break;
                }
            case MemoryThreshold._4096MB:
                {
                    if (totalMemory <= 4096) isLowRAM = true;
                    break;
                }
        }
        PlayerPrefs.SetInt("IsLowTotalRAM", isLowRAM == true ? 1 : 0);
        return isLowRAM;
    }
    public IAP_Key_Enum GetEnumValue(string value) => System.Enum.TryParse(value, true, out IAP_Key_Enum result) ? result : (IAP_Key_Enum)IAP_Key_Enum.Other;

    #endregion

    #region ActionWithDelay
    private System.Collections.Generic.Dictionary<System.Action, Coroutine> runningActionDictionary = new System.Collections.Generic.Dictionary<System.Action, Coroutine>();
    public void ActionsPerformWithDelay(System.Action action, float delay = 1f)
    {
        if (runningActionDictionary.TryGetValue(action, out var oldCoroutine))
        {
            StopCoroutine(oldCoroutine);
            runningActionDictionary.Remove(action);
        }
        Coroutine c = StartCoroutine(ActionPerformEnumerator(action, delay));
        runningActionDictionary[action] = c;
    }
    private System.Collections.IEnumerator ActionPerformEnumerator(System.Action action, float delay)
    {
        yield return new WaitForSecondsRealtime(delay);

        runningActionDictionary.Remove(action);

        try { action?.Invoke(); }
        catch { }
    }
    #endregion

    #region Memory Information

    readonly int memoryThreshHold = 500; //in MBs
    static int memoryAvailable = 0;
    static System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(@"\d+");

    // return True if memory low by defined threshHold
    public bool IsLowMemory(int threshold = -1)
    {
        #region IOS
        if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.OSXEditor) return false;
        #endregion
        try
        {
            if (!isMemoryThreshold) return false;

            threshold = threshold.Equals(-1) ? memoryThreshHold : threshold;
            return LoadMemoryInfo().Equals(true) ? (memoryAvailable / 1024) <= threshold : false;
        }
        catch (System.Exception) { return true; }
    }
    static bool LoadMemoryInfo()
    {
        try
        {
            //if file not exist retrun from here
            if (!System.IO.File.Exists("/proc/meminfo")) return false;
            System.IO.FileStream fs = new System.IO.FileStream("/proc/meminfo", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read);
            System.IO.StreamReader sr = new System.IO.StreamReader(fs);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                line = line.ToLower().Replace(" ", "");
                if (line.Contains("memavailable")) { memoryAvailable = int.Parse(re.Match(line).Value); }
            }
            sr.Close(); fs.Close(); fs.Dispose();
            return true;
        }
        catch (System.Exception) { return false; }
    }

    #endregion

    #region Analytics

    #region Analytics Structure
    public enum EventName
    {
        IAP_Purchase,
        IAP_Purchase_Success,
        IAP_Purchase_Failure,
        IAP_ButtonClick,
        IAP_OpenDialog
    }
    public enum ParameterKey
    {
        myParameter
    }
    public enum ParameterValue
    {
        InitilizationRequestSend,
        InitilizationRequestSuccess,
        InitilizationRequestFail,
        ButtonClick,
        OpenDialog,
        PurchaseFail,
        PurchaseSuccess
    }

    class CustomClass
    {
        public string eventName;
        public string parameterKey;
        public string parameterValue;

        public CustomClass(string eventName, string parameterName, string parameterValue)
        {
            this.eventName = eventName;
            this.parameterKey = parameterName;
            this.parameterValue = parameterValue;
        }
    }
    #endregion

    #region Functions
    void SendAnalytics(EventName eventName, ParameterKey parameterKey, ParameterValue parameterValue)
    {
        if (!isAnalytics) return;
        try
        {
#if Firebase_Analytics

#if FirebaseAnalyticsManager
            FirebaseAnalyticsManager.Instance.SendAnalytics(eventName.ToString(), parameterKey.ToString(), parameterValue.ToString());
#else
                Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName.ToString(), parameterKey.ToString(), parameterValue.ToString());
#endif

#endif

#if SingularSDKManager
            SingularSDKManager.Instance?.SendEvent(eventName.ToString(),parameterKey.ToString(), parameterValue.ToString());
#endif
            ShowDebugLog("Event Name: " + eventName.ToString() + ", Key: " + parameterKey.ToString() + ", Value: " + parameterValue.ToString());
        }
        catch (System.Exception) { }
    }
    void SendAnalytics(EventName eventName, ParameterKey parameterKey, string parameterValue)
    {
        if (!isAnalytics) return;
        try
        {
#if Firebase_Analytics
#if FirebaseAnalyticsManager
            FirebaseAnalyticsManager.Instance.SendAnalytics(eventName.ToString(), parameterKey.ToString(), parameterValue.ToString());
#else
            Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName.ToString(), parameterKey.ToString(), parameterValue);
#endif
#endif

#if SingularSDKManager
            SingularSDKManager.Instance?.SendEvent(eventName.ToString(), new System.Collections.Generic.Dictionary<string, object> { { parameterKey.ToString(), parameterValue } });
#endif

            ShowDebugLog("Event Name: " + eventName.ToString() + ", Key: " + parameterKey.ToString() + ", Value: " + parameterValue);
        }
        catch (System.Exception) { }
    }
    #endregion

    #endregion

    #region Pre-Registration Reward

    void CheckPreRegistrationReward()
    {
        if (PlayerPrefs.GetInt("PreRegRewardClaimed", 0) == 1) return;
        if (m_StoreController == null) return;

        // Replace with your pre-registration product ID
        string preRegistrationProductID = "preRegistrationReward";

        var rewardProduct = m_StoreController.GetProducts().FirstOrDefault(p => p.definition.id == preRegistrationProductID);
        if (rewardProduct != null && rewardProduct.hasReceipt)
        {
            Debug.Log($"Pre-Registration Reward Granted: {preRegistrationProductID}");
            InAppSuccess(rewardProduct);
            PlayerPrefs.SetInt("PreRegRewardClaimed", 1);
        }
    }

    #endregion

    #endregion

    #region Data Structure

    [System.Serializable]
    public class InAppData
    {
        public string name;
        public UnityEngine.Purchasing.ProductType type;
    }

    [System.Serializable]
    public class InAppDataType : InAppData
    {
        string title;
        string description;
        string price;

        public InAppDataType(UnityEngine.Purchasing.ProductType pt, string t, string d, string p)
        {
            type = pt;
            title = t;
            description = d;
            price = p;
        }

        public void SetData(string title, string description, string price)
        {
            this.title = title;
            this.description = description;
            this.price = price;
        }
        public string GetData(MetaDataType dataType)
        {
            return dataType switch
            {
                MetaDataType.Title => title,
                MetaDataType.Description => description,
                MetaDataType.Price => price,
                _ => "",
            };
        }
        public enum MetaDataType
        {
            Title, Description, Price
        }


    }

    #endregion

    #region Editor Properties
    private const string plugin_version = "1.1.1";
#if UNITY_EDITOR
    [CustomEditor(typeof(InAppPurchase))]
    public class IAPCustomEditor : Editor
    {
        SerializedProperty scriptableObject;
        SerializedProperty isAutoInitilize;
        SerializedProperty isStartingDelay;
        SerializedProperty startingDelay;
        SerializedProperty isDebugLog;
        SerializedProperty isAnalytics;
        SerializedProperty isMemoryThreshold;
        SerializedProperty NoIAPBelowAvailableRAM;
        SerializedProperty NoIAPBelowTotalRAM;
        Color color = new Color(0f / 255f, 255f / 255f, 255f / 255f);

        void OnEnable()
        {
            scriptableObject = serializedObject.FindProperty(nameof(InAppPurchase.scriptableObject));
            isAutoInitilize = serializedObject.FindProperty(nameof(InAppPurchase.isAutoInitilize));
            isStartingDelay = serializedObject.FindProperty(nameof(InAppPurchase.isStartingDelay));
            startingDelay = serializedObject.FindProperty(nameof(InAppPurchase.startingDelay));
            isDebugLog = serializedObject.FindProperty(nameof(InAppPurchase.isDebugLog));
            isAnalytics = serializedObject.FindProperty(nameof(InAppPurchase.isAnalytics));
            isMemoryThreshold = serializedObject.FindProperty(nameof(InAppPurchase.isMemoryThreshold));
            NoIAPBelowAvailableRAM = serializedObject.FindProperty(nameof(InAppPurchase.NoIAPBelowAvailableRAM));
            NoIAPBelowTotalRAM = serializedObject.FindProperty(nameof(InAppPurchase.NoIAPBelowTotalRAM));
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            #region GUIStyle
            GUIStyle headingStyle = new GUIStyle(EditorStyles.helpBox);
            headingStyle.normal.textColor = color;
            headingStyle.fontStyle = FontStyle.Bold;
            headingStyle.fontSize = 12;
            #endregion

            #region IAP ID's

            EditorGUILayout.BeginVertical(headingStyle);
            EditorGUILayout.LabelField("InApp ID's", headingStyle);
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(scriptableObject);

            EditorGUILayout.Space();
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            #endregion

            #region Initilization Setup
            EditorGUILayout.BeginVertical(headingStyle);
            EditorGUILayout.LabelField("Initilization", headingStyle);
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;

            isAutoInitilize.boolValue = EditorGUILayout.Toggle("Auto Initilized", isAutoInitilize.boolValue);
            if (isAutoInitilize.boolValue)
            {
                EditorGUI.indentLevel++;
                isStartingDelay.boolValue = EditorGUILayout.Toggle("Is Starting Initilizing Delay", isStartingDelay.boolValue);
                if (isStartingDelay.boolValue)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(startingDelay);
                    EditorGUILayout.EndHorizontal();
                    EditorGUI.indentLevel--;
                }
                EditorGUI.indentLevel--;
            }
            else
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.HelpBox("You Must Call 'InitilizeAsync()' by yourself before using getting IAP.", MessageType.Warning);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            #endregion

            #region Others Settings

            EditorGUILayout.BeginVertical(headingStyle);
            EditorGUILayout.LabelField("Other Settings", headingStyle);
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;

            isDebugLog.boolValue = EditorGUILayout.Toggle("Debug Log", isDebugLog.boolValue);
            if (isDebugLog.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox("Debugging Enable, Help you to debug every Event via Debug.Log(msg), Make sure to Disable after Testing.", MessageType.Warning);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
            isAnalytics.boolValue = EditorGUILayout.Toggle("Analytics", isAnalytics.boolValue);

            EditorGUILayout.Space();
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();

            #endregion

            #region Memory Settings
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(headingStyle);
            EditorGUILayout.LabelField("Memory Settings", headingStyle);
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;

            isMemoryThreshold.boolValue = EditorGUILayout.Toggle("Memory Threshold", isMemoryThreshold.boolValue);
            if (isMemoryThreshold.boolValue)
            {
                EditorGUILayout.PropertyField(NoIAPBelowAvailableRAM);
                EditorGUILayout.PropertyField(NoIAPBelowTotalRAM);
            }
            else
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.HelpBox("No Available/Total Memory Threshold Applied.", MessageType.Warning);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();
            EditorGUILayout.EndVertical();
            EditorGUI.indentLevel--;
            #endregion

            #region plugin version
            GUIStyle versionStyle = new GUIStyle(EditorStyles.helpBox)
            {
                alignment = TextAnchor.MiddleRight
            };
            versionStyle.normal.textColor = color;
            EditorGUILayout.LabelField("IAP Plugin Version: " + plugin_version, versionStyle);
            #endregion

            serializedObject.ApplyModifiedProperties();
        }
    }

    public class CustomScriptDefineSymbols : EditorWindow
    {
        class FileStructure
        {
            public string filePath;
            public string fileName;
            public string fileType;

            public FileStructure(string path, string name, string type)
            {
                filePath = path;
                fileName = name;
                fileType = type;
            }
        }

        static System.Collections.Generic.List<FileStructure> files = new System.Collections.Generic.List<FileStructure>()
                {
                    new FileStructure("Assets/Mobify/InApp_Purchase/Script/","InAppPurchase",".cs")
                };

        [InitializeOnLoad]
        public class InitOnLoad
        {
            static InitOnLoad()
            {
                foreach (FileStructure file in files)
                {
                    bool isAssetPresent = AssetDatabase.LoadAssetAtPath(file.filePath + "" + file.fileName + "" + file.fileType, typeof(UnityEngine.Object)) != null;
                    if (isAssetPresent) SetEnabled(file.fileName, true);
                }
                EditorApplication.projectChanged += OnProjectChanged;
            }
        }
        static void SetEnabled(string defineName, bool enable)
        {
            defineName = new System.Text.RegularExpressions.Regex("['*-.,&#^@]").Replace(defineName, "_");
            foreach (var group in buildTargetGroups)
            {
                var defines = GetDefinesList(group);

                if (enable)
                {
                    if (defines.Contains(defineName))
                        return;
                    defines.Add(defineName);
                }
                else
                {
                    if (!defines.Contains(defineName))
                        return;
                    while (defines.Contains(defineName))
                    {
                        defines.Remove(defineName);
                    }
                }
                string definesString = string.Join(";", defines.ToArray());
                PlayerSettings.SetScriptingDefineSymbolsForGroup(group, definesString);
            }
        }
        static void OnProjectChanged()
        {
            foreach (FileStructure file in files)
            {
                bool isAssetPresent = AssetDatabase.LoadAssetAtPath(file.filePath + "" + file.fileName + "" + file.fileType, typeof(UnityEngine.Object)) != null;
                if (!isAssetPresent) SetEnabled(file.fileName, false);
            }
        }
        static System.Collections.Generic.List<string> GetDefinesList(BuildTargetGroup group)
        {
            return new System.Collections.Generic.List<string>(PlayerSettings.GetScriptingDefineSymbolsForGroup(group).Split(';'));
        }
        private static readonly BuildTargetGroup[] buildTargetGroups = new BuildTargetGroup[] { BuildTargetGroup.Android, BuildTargetGroup.iOS, };
    }

#endif
    #endregion

    #region Custom Region

    public static System.Action OnSuccessPurchased;

    public static bool IsRemoveAdPurchased
    {
        get => PlayerPrefs.GetInt("REMOVEADS") != 0;
        set => PlayerPrefs.SetInt("REMOVEADS", 1);
    }

    void InAppSuccess(UnityEngine.Purchasing.Product product)
    {
        ShowDebugLog("Purchase Success : " + product.definition.id);
        switch (GetEnumValue(product.definition.id))
        {
            case IAP_Key_Enum.remove_ads:
                IsRemoveAdPurchased = true;
                PlayerPrefs.SetInt($"{IAP_Key_Enum.unlock_all_cars.ToString()}.Purchased", 1);
                break;

            case IAP_Key_Enum.unlock_all_cars:
                PlayerPrefs.SetInt($"{IAP_Key_Enum.unlock_all_cars.ToString()}.Purchased", 1);
                for (int i = 0; i < 50; i++)
                    GaragePanel.UnlockCar(i);
                HR_UI_MainmenuPanel.Instance.garagePanelScr.UpdateUI();
                break;

            case IAP_Key_Enum.unlock_all_levels:
                GameState.ChallengeCompletedLevel = 100;
                PlayerPrefs.SetInt($"{IAP_Key_Enum.unlock_all_levels.ToString()}.Purchased", 1);
                break;

            case IAP_Key_Enum.coins_bundle_1:
                HR_API.AddCurrency(10500);
                break;
            case IAP_Key_Enum.coins_bundle_2:
                HR_API.AddCurrency(20500);
                break;
            case IAP_Key_Enum.coins_bundle_3:
                HR_API.AddCurrency(30500);
                break;
            case IAP_Key_Enum.coins_bundle_4:
                HR_API.AddCurrency(50500);
                break;
            case IAP_Key_Enum.coins_bundle_5:
                HR_API.AddCurrency(100500);
                break;
        }

        OnSuccessPurchased?.Invoke();
    }

    #endregion
}