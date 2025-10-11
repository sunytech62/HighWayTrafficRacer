using System.IO;
using UnityEngine;

public static class JSONManager
{
    private static string GetPath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName + ".json");
    }

    public static void Save<T>(T data, string fileName)
    {
        try
        {
            string json = JsonUtility.ToJson(data, true);
            string path = GetPath(fileName);
            File.WriteAllText(path, json);
#if UNITY_EDITOR
            Debug.Log($"✅ Saved: {path}");
#endif
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"❌ Error saving {fileName}: {ex.Message}");
        }
    }

    public static T Load<T>(string fileName)
    {
        string path = GetPath(fileName);
        if (File.Exists(path))
        {
            try
            {
                string json = File.ReadAllText(path);
                T data = JsonUtility.FromJson<T>(json);
#if UNITY_EDITOR
                Debug.Log($"📂 Loaded: {path}");
#endif
                return data;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"❌ Error loading {fileName}: {ex.Message}");
            }
        }
        else
        {
#if UNITY_EDITOR
            Debug.LogWarning($"⚠️ File not found: {path}");
#endif
        }

        return default(T);
    }


    public static void Delete(string fileName)
    {
        string path = GetPath(fileName);
        if (File.Exists(path))
        {
            File.Delete(path);
#if UNITY_EDITOR
            Debug.Log($"🗑 Deleted: {path}");
#endif
        }
    }

    public static bool Exists(string fileName)
    {
        return File.Exists(GetPath(fileName));
    }
}
