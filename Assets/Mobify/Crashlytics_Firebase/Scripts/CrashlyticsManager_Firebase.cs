using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class CrashlyticsManager_Firebase : MonoBehaviour
{
    #region Instance

    private static CrashlyticsManager_Firebase instance = null;
    public static CrashlyticsManager_Firebase Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<CrashlyticsManager_Firebase>();
                if (instance != null)
                {
                    DontDestroyOnLoad(instance.gameObject);
                }
            }
            return instance;
        }
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
        }
    }

    #endregion

    #region Variables

    bool _isInitialized = false;
    [SerializeField] bool isAutoInitilized;
    [SerializeField] bool isStartingDelay;
    [Range(0.1f, 5.0f)]
    [SerializeField] float startingDelay = 0.3f;
    [SerializeField] bool isDebugLog;
    [SerializeField] UnityEngine.Events.UnityEvent onFirebaseInitilizedEvent;

    #endregion

    #region Initilization

    System.Collections.IEnumerator Start()
    {
#if UNITY_EDITOR
        initilizeStatus = InitilizeStatus.None;
#endif
        DontDestroyOnLoad(gameObject);
        if (isAutoInitilized){ if (isStartingDelay) yield return new WaitForSecondsRealtime(startingDelay); }
        else yield break;
        InitializeFirebaseCrashlytics();
    }

    public void InitializeFirebaseCrashlytics()
    {
#if FirebaseAnalyticsManager
            if(FirebaseAnalyticsManager.initilizeStatus == FirebaseAnalyticsManager.InitilizeStatus.Initilizing){ Invoke(nameof(InitializeFirebaseCrashlytics), 5); return;}
#endif
#if RemoteConfigManager_Firebase
            if(RemoteConfigManager_Firebase.initilizeStatus == RemoteConfigManager_Firebase.InitilizeStatus.Initilizing){Invoke(nameof(InitializeFirebaseCrashlytics), 5); return;}
#endif

        if (initilizeStatus.Equals(InitilizeStatus.Initilizing) || initilizeStatus.Equals(InitilizeStatus.Initilized)) return;
        initilizeStatus = InitilizeStatus.Initilizing;

        if (_isInitialized) return;
        try
        {
            ShowDebugLog("Initilization Request Send");
            Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                var dependencyStatus = task.Result;
                if (dependencyStatus == Firebase.DependencyStatus.Available)
                {
                    FirebaseInitilizedSuccess();
#if RemoteConfigManager_Firebase
                    if (RemoteConfigManager_Firebase.initilizeStatus != RemoteConfigManager_Firebase.InitilizeStatus.Initilized) RemoteConfigManager_Firebase.Instance?.FirebaseInitilizedSuccess();
#endif
#if FirebaseAnalyticsManager
                    if (FirebaseAnalyticsManager.initilizeStatus != FirebaseAnalyticsManager.InitilizeStatus.Initilized) FirebaseAnalyticsManager.Instance?.FirebaseInitilizedSuccess();
#endif
                }
                else
                {
                    ShowDebugLog(System.String.Format("Could not resolve all dependencies: {0}", dependencyStatus), LogType.Error);
                    initilizeStatus = InitilizeStatus.Failed;
                }
            });
        }
        catch (System.Exception e) { ShowDebugLog("Exception on InitializeFirebaseRemoteConfig(), Error : " + e, LogType.Error); }
    }

    public void FirebaseInitilizedSuccess()
    {
        Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;
        Firebase.Crashlytics.Crashlytics.ReportUncaughtExceptionsAsFatal = true;
        _isInitialized = true;
        ShowDebugLog("Initilized Successfully...");
        initilizeStatus = InitilizeStatus.Initilized;
        Invoke(nameof(OnFirebaseInitilizedEventWithDelay), 0.3f);
    }
    void OnFirebaseInitilizedEventWithDelay()
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

    #region Additional
    void ShowDebugLog(string message, LogType logType = LogType.msg)
    {
        if (!isDebugLog) return;

        switch (logType)
        {
            case LogType.msg:
                {
                    Debug.Log("<color=#C24F4F>Crashlytics Log :</color> " + message);
                    break;
                }
            case LogType.Error:
                {
                    Debug.LogError("<color=#C24F4F>Crashlytics Log :</color>" + message);
                    break;
                }
        }
    }
    enum LogType
    {
        msg,
        Error
    }
   
    #endregion

    #region Editor Properties
    private const string plugin_version = "1.0.4";
#if UNITY_EDITOR
    [CustomEditor(typeof(CrashlyticsManager_Firebase))]
    public class FirebaseCrashlyticsCustomEditor : Editor
    {
        SerializedProperty isAutoInitilized;
        SerializedProperty isStartingDelay;
        SerializedProperty startingDelay;
        SerializedProperty isDebugLog;
        SerializedProperty onFirebaseInitilizedEvent;

        Color color = new Color(194f / 255f, 79f / 255f, 79f / 255);
        void OnEnable()
        {
            isAutoInitilized = serializedObject.FindProperty(nameof(CrashlyticsManager_Firebase.isAutoInitilized));
            isStartingDelay = serializedObject.FindProperty(nameof(CrashlyticsManager_Firebase.isStartingDelay));
            startingDelay = serializedObject.FindProperty(nameof(CrashlyticsManager_Firebase.startingDelay));
            isDebugLog = serializedObject.FindProperty(nameof(CrashlyticsManager_Firebase.isDebugLog));
            onFirebaseInitilizedEvent = serializedObject.FindProperty(nameof(CrashlyticsManager_Firebase.onFirebaseInitilizedEvent));
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

            #region Settings
            EditorGUILayout.BeginVertical(headingStyle);
            EditorGUILayout.LabelField("Settings", headingStyle);
            EditorGUILayout.Space();
            EditorGUI.indentLevel++;

            isAutoInitilized.boolValue = EditorGUILayout.Toggle("Auto Initilized", isAutoInitilized.boolValue);
            if (isAutoInitilized.boolValue)
            {
                EditorGUI.indentLevel++;
                isStartingDelay.boolValue = EditorGUILayout.Toggle("Is Starting Initilize Delay", isStartingDelay.boolValue);
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
                EditorGUILayout.HelpBox("You Must Call 'InitializeFirebaseRemoteConfig()' by yourself before using getting Remote Values.", MessageType.Warning);
                EditorGUI.indentLevel--;
            }
            EditorGUILayout.Space();
            isDebugLog.boolValue = EditorGUILayout.Toggle("Debug Log", isDebugLog.boolValue);
            if (isDebugLog.boolValue)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.HelpBox("Debugging Enable, Help you to debug every Event via Debug.Log(msg), Make sure to Disable after Testing.", MessageType.Warning);
                EditorGUILayout.EndHorizontal();
                EditorGUI.indentLevel--;
            }                      

            EditorGUILayout.Space();
            EditorGUI.indentLevel--;
            EditorGUILayout.EndVertical();
            #endregion

            #region Event Handler
            EditorGUILayout.BeginVertical(headingStyle);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Event Handler", headingStyle);
            EditorGUILayout.PropertyField(onFirebaseInitilizedEvent);
            EditorGUILayout.EndVertical();
            #endregion

            #region plugin version
            GUIStyle versionStyle = new GUIStyle(EditorStyles.helpBox)
            {
                alignment = TextAnchor.MiddleRight
            };
            versionStyle.normal.textColor = color;
            EditorGUILayout.LabelField("Crashlytics Firebase Plugin Version: " + plugin_version, versionStyle);
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
            new FileStructure("Assets/Mobify/Crashlytics_Firebase/Scripts/","CrashlyticsManager_Firebase",".cs"),
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
}