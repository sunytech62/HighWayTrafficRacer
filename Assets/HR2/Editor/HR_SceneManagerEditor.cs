//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2025 BoneCracker Games
// https://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

[CustomEditor(typeof(HR_SceneManager))]
public class HR_SceneManagerEditor : Editor {

    HR_SceneManager prop;
    GUISkin skin;
    Color guiColor;
    static bool readme;

    private void OnEnable() {

        skin = Resources.Load<GUISkin>("HR_Gui");
        guiColor = GUI.color;

    }

    public override void OnInspectorGUI() {

        prop = (HR_SceneManager)target;
        serializedObject.Update();
        GUI.skin = skin;

        if (!EditorApplication.isPlaying)
            prop.GetAllComponents();

        EditorGUILayout.BeginVertical(GUI.skin.box);
        EditorGUILayout.HelpBox("HR_SceneManager is responsible for checking and observing the main controller components in the scene. All managers must be added for full functional gameplay. Game would still run without them.", MessageType.None);
        EditorGUILayout.HelpBox("Green buttons means the manager has been found in the scene, it can be selected by clicking the button. Red buttons means the manager couldn't found in the scene, it can be created by clicking the button.", MessageType.None);
        EditorGUILayout.Space();
        EditorGUILayout.BeginVertical();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("levelType"), new GUIContent("Level Type", "Level type."));

        EditorGUILayout.Space();

        EditorGUILayout.BeginVertical(GUI.skin.box);

        if (EditorApplication.isPlaying)
            EditorGUILayout.HelpBox("Managers can't be created at runtime, this means clicking the red buttons won't do anything during gameplay.", MessageType.Info);

        switch (prop.levelType) {

            case HR_SceneManager.LevelType.MainMenu:

                MainMenu();
                break;

            case HR_SceneManager.LevelType.Gameplay:

                Gameplay();
                break;

        }

        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();

        EditorGUILayout.BeginVertical(GUI.skin.box);

        RCCP();

        EditorGUILayout.EndVertical();

        EditorGUILayout.Separator();

        EditorGUILayout.BeginVertical(GUI.skin.box);

        EditorGUILayout.HelpBox("Checks all managers and creates if necessary.", MessageType.None);

        GUI.color = Color.cyan;

        if (EditorApplication.isPlaying)
            GUI.enabled = false;

        if (GUILayout.Button("Check & Create All Managers"))
            CreateAll();

        GUI.enabled = true;

        GUI.color = guiColor;

        EditorGUILayout.EndVertical();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        readme = EditorGUILayout.ToggleLeft(new GUIContent("Info", "Read me if you want, or don't read me if you don't want. It's up to you, or not?"), readme);

        if (readme)
            EditorGUILayout.HelpBox("This main manager will check all sub managers in the scene and let you know if something found not right. However, it won't guarantee a clean gameplay. I would recommend you to check the documentation before use, and don't miss the common mistakes section. Keep an eye on your console and inspector panel for more detailed infos.", MessageType.None);

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndVertical();

        prop.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        if (GUI.changed)
            EditorUtility.SetDirty(prop);

        if (FindFirstObjectByType<RCCP_SceneManager>())
            RCCP_SceneManager.Instance.registerLastVehicleAsPlayer = false;

        serializedObject.ApplyModifiedProperties();

    }

    private void MainMenu() {

        if (prop.MainMenuManager != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.MainMenuManager == null ? "Add " : "") + "MainMenu Manager"))) {

            if (prop.MainMenuManager)
                Selection.activeGameObject = prop.MainMenuManager.gameObject;
            else if (!EditorApplication.isPlaying)
                CreateComponent(typeof(HR_MainMenuManager));

        }

        GUI.color = guiColor;

        if (prop.MainMenuPanel != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.MainMenuPanel == null ? "Add " : "") + "UI MainMenu"))) {

            if (prop.MainMenuPanel) {

                Selection.activeGameObject = prop.MainMenuPanel.gameObject;

            } else if (!EditorApplication.isPlaying) {

                GameObject dp = PrefabUtility.InstantiatePrefab(HR_Settings.Instance.UI_MainmenuPanel) as GameObject;
                dp.transform.position = Vector3.zero;
                dp.transform.rotation = Quaternion.identity;
                dp.transform.name = HR_Settings.Instance.UI_MainmenuPanel.transform.name;

            }

        }

        GUI.color = guiColor;

        if (prop.Event != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.Event == null ? "Add " : "") + "UI Event System"))) {

            if (prop.Event) {

                Selection.activeGameObject = prop.Event.gameObject;

            } else if (!EditorApplication.isPlaying) {

                GameObject panel = GameObject.Instantiate(HR_Settings.Instance.UI_EventSystem.gameObject) as GameObject;
                panel.transform.position = Vector3.zero;
                panel.transform.rotation = Quaternion.identity;
                panel.transform.name = HR_Settings.Instance.UI_EventSystem.transform.name;

            }

        }

        GUI.color = guiColor;

        if (prop.ShowroomCamera != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.ShowroomCamera == null ? "Add " : "") + "Showroom Camera"))) {

            if (prop.ShowroomCamera) {

                Selection.activeGameObject = prop.ShowroomCamera.gameObject;

            } else if (!EditorApplication.isPlaying) {

                GameObject dp = PrefabUtility.InstantiatePrefab(HR_Settings.Instance.showroomCamera.gameObject) as GameObject;
                dp.transform.position = Vector3.zero;
                dp.transform.rotation = Quaternion.identity;
                dp.transform.name = HR_Settings.Instance.showroomCamera.transform.name;

            }

        }

        GUI.color = guiColor;

    }

    private void Gameplay() {

        if (prop.GameplayManager != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.GameplayManager == null ? "Add " : "") + "Gameplay Manager"))) {

            if (prop.GameplayManager)
                Selection.activeGameObject = prop.GameplayManager.gameObject;
            else if (!EditorApplication.isPlaying)
                CreateComponent(typeof(HR_GamePlayManager));

        }

        GUI.color = guiColor;

        if (prop.CurvedRoadManager != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.CurvedRoadManager == null ? "Add " : "") + "Curved Road Manager"))) {

            if (prop.CurvedRoadManager)
                Selection.activeGameObject = prop.CurvedRoadManager.gameObject;
            else if (!EditorApplication.isPlaying)
                CreateComponent(typeof(HR_CurvedRoadManager));

        }

        GUI.color = guiColor;

        if (prop.PathManager != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.PathManager == null ? "Add " : "") + "Path Manager"))) {

            if (prop.PathManager)
                Selection.activeGameObject = prop.PathManager.gameObject;
            else if (!EditorApplication.isPlaying)
                CreateComponent(typeof(HR_PathManager));

        }

        GUI.color = guiColor;

        if (prop.TrafficManager != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.TrafficManager == null ? "Add " : "") + "Traffic Manager"))) {

            if (prop.TrafficManager)
                Selection.activeGameObject = prop.TrafficManager.gameObject;
            else if (!EditorApplication.isPlaying)
                CreateComponent(typeof(HR_TrafficManager));

        }

        GUI.color = guiColor;

        if (prop.LaneManager != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.LaneManager == null ? "Add " : "") + "Lane Manager"))) {

            if (prop.LaneManager)
                Selection.activeGameObject = prop.LaneManager.gameObject;
            else if (!EditorApplication.isPlaying)
                CreateComponent(typeof(HR_LaneManager));

        }

        GUI.color = guiColor;

        if (prop.PlayerCamera != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.PlayerCamera == null ? "Add " : "") + "Player Camera"))) {

            if (prop.PlayerCamera) {

                Selection.activeGameObject = prop.PlayerCamera.gameObject;

            } else if (!EditorApplication.isPlaying) {

                GameObject dp = PrefabUtility.InstantiatePrefab(HR_Settings.Instance.gameplayCamera.gameObject) as GameObject;
                dp.transform.position = Vector3.zero;
                dp.transform.rotation = Quaternion.identity;
                dp.transform.name = HR_Settings.Instance.gameplayCamera.transform.name;

            }

        }

        GUI.color = guiColor;

        EditorGUILayout.Space();

        if (prop.GameplayPanel != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.GameplayPanel == null ? "Add " : "") + "UI Gameplay"))) {

            if (prop.GameplayPanel) {

                Selection.activeGameObject = prop.GameplayPanel.gameObject;

            } else if (!EditorApplication.isPlaying) {

                GameObject panel = PrefabUtility.InstantiatePrefab(HR_Settings.Instance.UI_GameplayPanel) as GameObject;
                panel.transform.position = Vector3.zero;
                panel.transform.rotation = Quaternion.identity;
                panel.transform.name = HR_Settings.Instance.UI_GameplayPanel.transform.name;

            }

        }

        GUI.color = guiColor;

        if (prop.GameoverPanel != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.GameoverPanel == null ? "Add " : "") + "UI Gameover"))) {

            if (prop.GameoverPanel) {

                Selection.activeGameObject = prop.GameoverPanel.gameObject;

            } else if (!EditorApplication.isPlaying) {

                GameObject panel = PrefabUtility.InstantiatePrefab(HR_Settings.Instance.UI_GameoverPanel) as GameObject;
                panel.transform.position = Vector3.zero;
                panel.transform.rotation = Quaternion.identity;
                panel.transform.name = HR_Settings.Instance.UI_GameoverPanel.transform.name;

            }

        }

        GUI.color = guiColor;

        if (prop.Event != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.Event == null ? "Add " : "") + "UI Event System"))) {

            if (prop.Event) {

                Selection.activeGameObject = prop.Event.gameObject;

            } else if (!EditorApplication.isPlaying) {

                GameObject panel = GameObject.Instantiate(HR_Settings.Instance.UI_EventSystem.gameObject) as GameObject;
                panel.transform.position = Vector3.zero;
                panel.transform.rotation = Quaternion.identity;
                panel.transform.name = HR_Settings.Instance.UI_EventSystem.transform.name;

            }

        }

        GUI.color = guiColor;

    }

    private void RCCP() {

        if (prop.RCCPSceneManager != null)
            GUI.color = Color.green;
        else
            GUI.color = Color.red;

        if (GUILayout.Button(new GUIContent((prop.RCCPSceneManager == null ? "Add " : "") + "RCCP Scene Manager"))) {

            if (prop.RCCPSceneManager) {

                Selection.activeGameObject = prop.RCCPSceneManager.gameObject;

            } else {

                RCCP_SceneManager sm = RCCP_SceneManager.Instance;
                sm.registerLastVehicleAsPlayer = false;

            }

        }

        GUI.color = guiColor;

    }

    public GameObject CreateComponent(Type monoBehaviour) {

        GameObject newGO = new GameObject(monoBehaviour.FullName);
        newGO.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        newGO.AddComponent(monoBehaviour);
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        return newGO;

    }

    private void CreateAll() {

        switch (prop.levelType) {

            case HR_SceneManager.LevelType.MainMenu:

                LoadSceneTemplate(HR_Settings.Instance.templateScenePath_MainMenu);
                break;

            case HR_SceneManager.LevelType.Gameplay:

                LoadSceneTemplate(HR_Settings.Instance.templateScenePath_Gameplay);
                break;

        }

        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());

    }

    static void LoadSceneTemplate(string templateScenePath) {

        HR_SceneManager sceneManager = FindFirstObjectByType<HR_SceneManager>(FindObjectsInactive.Include);

        Scene activeScene = SceneManager.GetActiveScene();

        // Open the template scene additively
        Scene templateScene = EditorSceneManager.OpenScene(templateScenePath, OpenSceneMode.Additive);

        // Get all root GameObjects from the template scene
        GameObject[] rootObjects = templateScene.GetRootGameObjects();

        // Move each GameObject from the template scene to the active scene
        foreach (GameObject obj in rootObjects) {
            SceneManager.MoveGameObjectToScene(obj, activeScene);
        }

        // Close the template scene without saving it since all objects are now moved
        EditorSceneManager.CloseScene(templateScene, true);

        EditorApplication.delayCall += () => {

            if (sceneManager)
                DestroyImmediate(sceneManager.gameObject);

        };

    }

}
