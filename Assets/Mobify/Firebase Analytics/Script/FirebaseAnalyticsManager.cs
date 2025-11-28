#if UNITY_EDITOR
using UnityEditor;
#endif
#if Firebase_Analytics
using Firebase.Extensions;
#endif
using UnityEngine;

public class FirebaseAnalyticsManager : MonoBehaviour
{
    #region Instance

    static FirebaseAnalyticsManager instance = null;
    public static FirebaseAnalyticsManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FirebaseAnalyticsManager>();
                if (instance != null)
                {
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        }
    }

    #endregion

    #region Variables

    [SerializeField] bool isAutoInitilized = true;
    [SerializeField] bool isStartingInitilizeDelay = true;
    [SerializeField][Range(0.1f, 5.0f)] float startingDelay = 0.1f;
    [SerializeField] bool isDebugLog = false;
    [SerializeField] UnityEngine.Events.UnityEvent onFirebaseInitilizedEvent;

    #region ListOfUnsendEvent

    System.Collections.Generic.List<string> pendingEventList = null;
    System.Collections.Generic.List<EventWithParameter> pendingEventWithParameterList = null;
    System.Collections.Generic.List<EventWithDictionary> pendingEventWithDictionary = null;

    #endregion

    #endregion

    #region Initilization

    System.Collections.IEnumerator Start()
    {
#if UNITY_EDITOR
        initilizeStatus = InitilizeStatus.None;
#endif
        if (isAutoInitilized)
        {
            if (isStartingInitilizeDelay)
                yield return new WaitForSecondsRealtime(startingDelay);
        }
        else yield break;
        InitilizeFirebaseAnalytics();
    }
    public void InitilizeFirebaseAnalytics()
    {
        try
        {
#if RemoteConfigManager_Firebase
            if (RemoteConfigManager_Firebase.initilizeStatus == RemoteConfigManager_Firebase.InitilizeStatus.Initilizing) return;
#endif
#if CrashlyticsManager_Firebase
            if (CrashlyticsManager_Firebase.initilizeStatus == CrashlyticsManager_Firebase.InitilizeStatus.Initilizing) return;
#endif

            if (initilizeStatus == InitilizeStatus.Initilizing || initilizeStatus == InitilizeStatus.Initilized) return;
            if (!IsInternetConnection()) return;
            if (isDebugLog) ShowDebugLog("Initilizing...");

            initilizeStatus = InitilizeStatus.Initilizing;
#if Firebase_Analytics
            Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    FirebaseInitilizedSuccess();
#if RemoteConfigManager_Firebase
                    if (RemoteConfigManager_Firebase.initilizeStatus != RemoteConfigManager_Firebase.InitilizeStatus.Initilized) RemoteConfigManager_Firebase.Instance?.FirebaseInitilizedSuccess();
#endif
#if CrashlyticsManager_Firebase
                    if (CrashlyticsManager_Firebase.initilizeStatus != CrashlyticsManager_Firebase.InitilizeStatus.Initilized) CrashlyticsManager_Firebase.Instance?.FirebaseInitilizedSuccess();
#endif
                }
                else
                {
                    if (isDebugLog) ShowDebugLog("Failed to Initilized");
                    initilizeStatus = InitilizeStatus.Failed;
                    IsFirebaseInitialized = false;
                }
            });
#endif
        }
        catch (System.Exception) { }
    }
    public void FirebaseInitilizedSuccess()
    {
        Firebase.Analytics.FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);
        Firebase.Analytics.FirebaseAnalytics.SetUserProperty(Firebase.Analytics.FirebaseAnalytics.UserPropertySignUpMethod, "Google");
        Firebase.Analytics.FirebaseAnalytics.SetSessionTimeoutDuration(new System.TimeSpan(0, 45, 0));
        IsFirebaseInitialized = true;
        if (isDebugLog) ShowDebugLog("Initilized Successfull");
        initilizeStatus = InitilizeStatus.Initilized;
        SendPendingListEvent();
        Invoke(nameof(onFirebaseInitilizedEventWithDelay), 0.3f);
    }
    void onFirebaseInitilizedEventWithDelay()
    {
        onFirebaseInitilizedEvent?.Invoke();
    }

    public static InitilizeStatus initilizeStatus = InitilizeStatus.None;
    public enum InitilizeStatus
    {
        None,
        Initilizing,
        Initilized,
        Failed
    }
    #endregion

    #region Public Functions

    public bool IsFirebaseInitialized { get; private set; } = false;
    public void SendAnalytics(string eventName)
    {
        try
        {
            if (IsFirebaseInitialized)
            {
#if RemoteConfigManager_Firebase
                if (RemoteConfigManager_Firebase.initilizeStatus == RemoteConfigManager_Firebase.InitilizeStatus.Initilizing) goto getDefault;
#endif
#if CrashlyticsManager_Firebase
                if (CrashlyticsManager_Firebase.initilizeStatus == CrashlyticsManager_Firebase.InitilizeStatus.Initilizing) goto getDefault;
#endif

                if (isDebugLog) ShowDebugLog("EventName: " + eventName);
#if Firebase_Analytics
                Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName);
#endif
                SendPendingListEvent();
                return;
            }
        getDefault:
            {
                pendingEventList ??= new System.Collections.Generic.List<string>();
                pendingEventList.Add(eventName);
                InitilizeFirebaseAnalytics();
            }

        }
        catch (System.Exception) { }
    }
    public void SendAnalytics(string eventName, string parameterKey, string parameterValue)
    {
        try
        {
            if (IsFirebaseInitialized)
            {
#if RemoteConfigManager_Firebase
                if (RemoteConfigManager_Firebase.initilizeStatus == RemoteConfigManager_Firebase.InitilizeStatus.Initilizing) goto getDefault;
#endif
#if CrashlyticsManager_Firebase
                if (CrashlyticsManager_Firebase.initilizeStatus == CrashlyticsManager_Firebase.InitilizeStatus.Initilizing) goto getDefault;
#endif

                if (isDebugLog) ShowDebugLog("EventName: " + eventName + "\nParameterKey: " + parameterKey + ", ParameterValue: " + parameterValue);
#if Firebase_Analytics
                Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, parameterKey, parameterValue);
#endif
                SendPendingListEvent();
                return;
            }
        getDefault:
            {
                pendingEventWithParameterList ??= new System.Collections.Generic.List<EventWithParameter>();
                pendingEventWithParameterList.Add(new EventWithParameter(eventName.ToString(), parameterKey.ToString(), parameterValue.ToString()));
                InitilizeFirebaseAnalytics();
            }
        }
        catch (System.Exception) { }
    }
    public void SendAnalytics(string eventName, System.Collections.Generic.Dictionary<string, object> dataCollection)
    {
        try
        {
            if (IsFirebaseInitialized)
            {
#if RemoteConfigManager_Firebase
                if (RemoteConfigManager_Firebase.initilizeStatus == RemoteConfigManager_Firebase.InitilizeStatus.Initilizing) goto getDefault;
#endif
#if CrashlyticsManager_Firebase
                if (CrashlyticsManager_Firebase.initilizeStatus == CrashlyticsManager_Firebase.InitilizeStatus.Initilizing) goto getDefault;
#endif

                string analyticsData = "EventName: " + eventName;
#if Firebase_Analytics
                System.Collections.Generic.List<Firebase.Analytics.Parameter> parametersList = new System.Collections.Generic.List<Firebase.Analytics.Parameter>();
                foreach (System.Collections.Generic.KeyValuePair<string, object> entry in dataCollection)
                {
                    parametersList.Add(new Firebase.Analytics.Parameter(entry.Key, entry.Value.ToString()));
                    if (isDebugLog) analyticsData += "\nParameterKey: " + entry.Key + ", ParameterValue: " + entry.Value.ToString();
                }
                if (isDebugLog) ShowDebugLog(analyticsData);
                Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, parametersList.ToArray());
#endif
                SendPendingListEvent();
                return;
            }
        getDefault:
            {
                pendingEventWithDictionary ??= new System.Collections.Generic.List<EventWithDictionary>();
                pendingEventWithDictionary.Add(new EventWithDictionary(eventName.ToString(), dataCollection));
                InitilizeFirebaseAnalytics();
            }
        }
        catch (System.Exception) { }
    }

    public void SendAnalytics(string eventName, Firebase.Analytics.Parameter[] dataCollection)
    {
        try
        {
            if (IsFirebaseInitialized)
            {
#if RemoteConfigManager_Firebase
                if (RemoteConfigManager_Firebase.initilizeStatus == RemoteConfigManager_Firebase.InitilizeStatus.Initilizing) goto getDefault;
#endif
#if CrashlyticsManager_Firebase
                if (CrashlyticsManager_Firebase.initilizeStatus == CrashlyticsManager_Firebase.InitilizeStatus.Initilizing) goto getDefault;
#endif
#if Firebase_Analytics
                if (isDebugLog) ShowDebugLog("EventName: " + eventName + "\nNo Parameter can be show due to Firebase.Param");
                Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName, dataCollection);
#endif
                SendPendingListEvent();
                return;
            }
        getDefault:
            {
                InitilizeFirebaseAnalytics();
            }
        }
        catch (System.Exception) { }
    }

    #endregion

    #region Additional

    void SendPendingListEvent()
    {
        try
        {
            if (pendingEventList != null)
            {
                if (pendingEventList.Count > 0)
                {
#if Firebase_Analytics
                    foreach (string eventName in pendingEventList)
                        Firebase.Analytics.FirebaseAnalytics.LogEvent(eventName);
#endif
                    pendingEventList.Clear();
                    pendingEventList = null;
                }
            }
            if (pendingEventWithParameterList != null)
            {
                if (pendingEventWithParameterList.Count > 0)
                {
#if Firebase_Analytics
                    foreach (EventWithParameter data in pendingEventWithParameterList)
                        Firebase.Analytics.FirebaseAnalytics.LogEvent(data.eventName, data.parameterKey, data.parameterValue);
#endif
                    pendingEventWithParameterList.Clear();
                    pendingEventWithParameterList = null;
                }
            }
            if (pendingEventWithDictionary != null)
            {
                if (pendingEventWithDictionary.Count > 0)
                {
#if Firebase_Analytics
                    foreach (EventWithDictionary dataCollection in pendingEventWithDictionary)
                    {
                        System.Collections.Generic.List<Firebase.Analytics.Parameter> parametersList = new System.Collections.Generic.List<Firebase.Analytics.Parameter>();
                        foreach (System.Collections.Generic.KeyValuePair<string, object> entry in dataCollection.dictionary)
                        {
                            parametersList.Add(new Firebase.Analytics.Parameter(entry.Key, entry.Value.ToString()));
                        }
                        Firebase.Analytics.FirebaseAnalytics.LogEvent(dataCollection.eventName, parametersList.ToArray());
                    }
#endif
                    pendingEventWithDictionary.Clear();
                    pendingEventWithDictionary = null;
                }
            }
        }
        catch (System.Exception) { }
    }
    bool IsInternetConnection()
    {
        return Application.internetReachability != NetworkReachability.NotReachable;
    }
    void ShowDebugLog(string msg)
    {
        if (isDebugLog)
        {
            Debug.Log("<color=green>FirebaseAnalytics Log:</color> " + msg);
        }
    }
    private void OnEnable()
    {
        onFirebaseInitilizedEvent.AddListener(AfterInitilizationFunctionCalls);
    }

    #endregion

    #region Custom Structure

    class EventWithParameter
    {
        public string eventName;
        public string parameterKey;
        public string parameterValue;

        public EventWithParameter(string eventName, string parameterName, string parameterValue)
        {
            this.eventName = eventName;
            this.parameterKey = parameterName;
            this.parameterValue = parameterValue;
        }
    }
    class EventWithDictionary
    {
        public string eventName;
        public System.Collections.Generic.Dictionary<string, object> dictionary;

        public EventWithDictionary(string eventName, System.Collections.Generic.Dictionary<string, object> dictionary)
        {
            this.eventName = eventName;
            this.dictionary = dictionary;
        }
    }

    #endregion

    #region Editor Properties

#if UNITY_EDITOR
    [CustomEditor(typeof(FirebaseAnalyticsManager))]
    public class FirebaseAnalyticsCustomEditor : Editor
    {
        readonly string pluginVersion = "1.0.6";

        SerializedProperty isAutoInitilized;
        SerializedProperty isStartingInitilizeDelay;
        SerializedProperty startingDelay;
        SerializedProperty isDebugLog;
        SerializedProperty onFirebaseInitilizedEvent;

        void OnEnable()
        {
            isAutoInitilized = serializedObject.FindProperty(nameof(FirebaseAnalyticsManager.isAutoInitilized));
            isStartingInitilizeDelay = serializedObject.FindProperty(nameof(FirebaseAnalyticsManager.isStartingInitilizeDelay));
            startingDelay = serializedObject.FindProperty(nameof(FirebaseAnalyticsManager.startingDelay));
            isDebugLog = serializedObject.FindProperty(nameof(FirebaseAnalyticsManager.isDebugLog));
            onFirebaseInitilizedEvent = serializedObject.FindProperty(nameof(FirebaseAnalyticsManager.onFirebaseInitilizedEvent));
        }
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            #region GUIStyle
            GUIStyle headingStyle = new GUIStyle(EditorStyles.helpBox);
            headingStyle.normal.textColor = Color.green;
            headingStyle.fontStyle = FontStyle.Bold;
            headingStyle.fontSize = 12;
            #endregion

            #region Initilization Setup
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(headingStyle);
            EditorGUILayout.LabelField("Initilization Setup", headingStyle);
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;

            isAutoInitilized.boolValue = EditorGUILayout.Toggle("Auto Initilized", isAutoInitilized.boolValue);
            if (isAutoInitilized.boolValue)
            {
                isStartingInitilizeDelay.boolValue = EditorGUILayout.Toggle("Is Starting Initilize Delay", isStartingInitilizeDelay.boolValue);
                if (isStartingInitilizeDelay.boolValue)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(startingDelay);
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.HelpBox("You Must Call 'InitilizeFirebaseAnalytics()' by yourself before using Firebase Analytics.", MessageType.Warning);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            #endregion

            #region Other Settings
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(headingStyle);
            EditorGUILayout.LabelField("Other Settings", headingStyle);
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;

            isDebugLog.boolValue = EditorGUILayout.Toggle("Debug Log", isDebugLog.boolValue);
            if (isDebugLog.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.HelpBox("Debugging Enable, Help you to debug every Event via Debug.Log(msg), Make sure to Disable after Testing.", MessageType.Warning);
                //EditorGUILayout.PropertyField(colorValue);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();

            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
            #endregion

            #region Event Handler
            EditorGUILayout.BeginVertical(headingStyle);
            EditorGUILayout.LabelField("Event Handler", headingStyle);
            EditorGUILayout.PropertyField(onFirebaseInitilizedEvent);
            EditorGUILayout.EndVertical();
            #endregion

            #region Plugin Version
            GUIStyle versionStyle = new GUIStyle(EditorStyles.helpBox)
            {
                alignment = TextAnchor.MiddleRight
            };
            versionStyle.normal.textColor = Color.green;
            EditorGUILayout.LabelField("Firebase Analytics Plugin Version: " + pluginVersion, versionStyle);
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
            new FileStructure("Assets/Mobify/Firebase Analytics/Script/","FirebaseAnalyticsManager",".cs"),
            new FileStructure("Assets/Firebase/Plugins/","Firebase.Analytics",".dll"),
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

    #region Custom Code

    void AfterInitilizationFunctionCalls()
    {
        //ShowDebugLog("After Initilization Function Calls");
    }

    public static void SendAnalyticCus(string eventName, string parameterValue)
    {
        Instance?.SendAnalytics(eventName, "myParameter", parameterValue);
    }

    #endregion
}