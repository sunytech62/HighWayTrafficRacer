//----------------------------------------------
//           	   Highway Racer
//
// Copyright © 2014 - 2025 BoneCracker Games
// https://www.bonecrackergames.com
//
//----------------------------------------------

using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public class HR_InitOnLoad {

    [InitializeOnLoadMethod]
    static void InitOnLoad() {

        EditorApplication.delayCall += EditorUpdate;
        EditorApplication.delayCall += CheckPlayerVehicles;

    }

    public static void EditorUpdate() {

        //HR_SetScriptingSymbol.SetEnabled("BCG_HR2", false);

        bool hasKey = false;

#if BCG_HR2
        hasKey = true;
#endif

        if (!hasKey) {

            HR_SetScriptingSymbol.SetEnabled("BCG_HR2", true);
            EditorApplication.ExecuteMenuItem("Window/TextMeshPro/Import TMP Essential Resources");

            EditorUtility.DisplayDialog("Regards from BoneCracker Games", "Thank you for purchasing Highway Racer 2. Please read the documentation before use. Also check out the online documentation for updated info. Have fun :)", "Let's get started");
            EditorUtility.DisplayDialog("Current Controller Type", "Current controller type is ''Desktop''. You can swith it from Highway Racer --> Switch to Keyboard / Mobile. You can set initial money value from Highway Racer --> General Settings.", "Ok");

            EditorUtility.DisplayDialog("Restart Unity", "Please restart Unity after importing the package. Otherwise inputs may not work for the first time.", "Ok");

        }

    }

    public static void CheckPlayerVehicles() {

        HR_PlayerCars playerCars = HR_PlayerCars.Instance;
        if (playerCars == null) return;

        HR_PlayerCars.Cars[] vehicles = playerCars.cars;
        if (vehicles == null || vehicles.Length == 0) return;

        bool anyPrefabChanged = false;

        for (int i = 0; i < vehicles.Length; i++) {

            var entry = vehicles[i];

            if (entry == null || entry.playerCar == null)
                continue;

            // Get the path of that prefab asset in the Project window:
            string prefabPath = AssetDatabase.GetAssetPath(entry.playerCar);

            if (string.IsNullOrEmpty(prefabPath))
                continue;

            // Load a “live” instance of that prefab into memory:
            GameObject prefabRoot = PrefabUtility.LoadPrefabContents(prefabPath);

            // prefabRoot is now a clone of the prefab asset. Run your setup‐check on it:
            HR_Player playerComp = prefabRoot.GetComponent<HR_Player>();

            if (playerComp != null) {

                bool changed = playerComp.CheckVehicleSetup();

                if (changed) {

                    // Mark dirty so Unity knows to write it back:
                    Undo.RecordObject(prefabRoot, "Fix Prefab Vehicle Setup");
                    EditorUtility.SetDirty(prefabRoot);

                    // Save the in‐memory prefab back to disk at the same path:
                    PrefabUtility.SaveAsPrefabAsset(prefabRoot, prefabPath);
                    anyPrefabChanged = true;
                    Debug.Log($"✔ Prefab '{prefabPath}' was updated.");

                }

            }

            // Always unload the in‐memory copy when you’re done:
            PrefabUtility.UnloadPrefabContents(prefabRoot);

        }

        if (anyPrefabChanged) {

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("All modified prefabs have been saved.");

        }

    }

}