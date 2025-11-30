using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "IAP_ScriptableData", menuName = "IAP/Scriptable", order = 1)]
public class IAP_Scriptable : ScriptableObject
{
    public System.Collections.Generic.List<InAppPurchase.InAppData> IAP_Keys;

#if UNITY_EDITOR
    public void UpdateEnum()
    {
        string enumName = "IAP_Key_Enum";
        string filePath = "Assets/Mobify/InApp_Purchase/Script/Additional/IAP_Enum.cs";

        System.Text.StringBuilder enumContent = new System.Text.StringBuilder();
        enumContent.AppendLine("public enum " + enumName);
        enumContent.AppendLine("{");
        foreach (InAppPurchase.InAppData value in IAP_Keys) enumContent.AppendLine("    " + value.name + ",");
        enumContent.AppendLine("    Other,");
        enumContent.AppendLine("}");
        System.IO.File.WriteAllText(filePath, enumContent.ToString());
        AssetDatabase.Refresh();
    }
#endif
}

#if UNITY_EDITOR
[CustomEditor(typeof(IAP_Scriptable))]
public class EnumValuesEditorIAP : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        IAP_Scriptable enumValues = (IAP_Scriptable)target;
        if (GUILayout.Button("Update Enum"))
        {
            enumValues.UpdateEnum();
        }
    }
}
#endif
