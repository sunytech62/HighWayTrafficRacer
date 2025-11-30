#if UNITY_EDITOR
using System;
using UnityEditor;
#endif
using UnityEngine;

public class IAP_Button : MonoBehaviour
{
    [SerializeField] IAP_Key_Enum key;
    [SerializeField] string newKey;
    [SerializeField] TMPro.TextMeshProUGUI priceTxt;
    [SerializeField] TMPro.TextMeshProUGUI titleTxt;
    [SerializeField] TMPro.TextMeshProUGUI descriptionTxt;
    [SerializeField] UnityEngine.UI.Button button;
    [SerializeField] UnityEngine.Events.UnityEvent successAction;
    private void OnEnable()
    {
        string currentKey = key.Equals(IAP_Key_Enum.Other) ? newKey : key.ToString();
        if (priceTxt != null) priceTxt.text = InAppPurchase.Instance?.GetData(currentKey, InAppPurchase.InAppDataType.MetaDataType.Price);
        if (titleTxt != null) titleTxt.text = InAppPurchase.Instance?.GetData(currentKey, InAppPurchase.InAppDataType.MetaDataType.Title);
        if (descriptionTxt != null) descriptionTxt.text = InAppPurchase.Instance?.GetData(currentKey, InAppPurchase.InAppDataType.MetaDataType.Description);
        if (button != null) button.onClick.AddListener(() => { InAppPurchase.Instance?.Purchase(currentKey, (success) => { if (success) successAction?.Invoke(); }); });

        DisableIfPurchased();
        InAppPurchase.OnSuccessPurchased += DisableIfPurchased;
    }

    private void DisableIfPurchased()
    {
        var isPurchased = PlayerPrefs.GetInt($"{key.ToString()}.Purchased") != 0;
        if (isPurchased) gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        InAppPurchase.OnSuccessPurchased -= DisableIfPurchased;
        if (button != null) button.onClick.RemoveAllListeners();
    }

    #region Editor Properties
#if UNITY_EDITOR
    [CustomEditor(typeof(IAP_Button))]
    public class IAPCustomEditor : Editor
    {
        SerializedProperty key;
        SerializedProperty newKey;
        SerializedProperty priceTxt;
        SerializedProperty titleTxt;
        SerializedProperty descriptionTxt;
        SerializedProperty button;
        SerializedProperty successAction;

        void OnEnable()
        {
            key = serializedObject.FindProperty(nameof(IAP_Button.key));
            newKey = serializedObject.FindProperty(nameof(IAP_Button.newKey));
            priceTxt = serializedObject.FindProperty(nameof(IAP_Button.priceTxt));
            titleTxt = serializedObject.FindProperty(nameof(IAP_Button.titleTxt));
            descriptionTxt = serializedObject.FindProperty(nameof(IAP_Button.descriptionTxt));
            button = serializedObject.FindProperty(nameof(IAP_Button.button));
            successAction = serializedObject.FindProperty(nameof(IAP_Button.successAction));
        }
        public override void OnInspectorGUI()
        {
            EditorGUILayout.PropertyField(key);
            IAP_Key_Enum adType = (IAP_Key_Enum)key.enumValueIndex;

            if (adType == IAP_Key_Enum.Other)
            {
                EditorGUI.indentLevel++;
                EditorGUILayout.PropertyField(newKey);
                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            }
            EditorGUILayout.PropertyField(priceTxt);
            EditorGUILayout.PropertyField(titleTxt);
            EditorGUILayout.PropertyField(descriptionTxt);
            EditorGUILayout.PropertyField(button);
            EditorGUILayout.PropertyField(successAction);
            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
    #endregion
}
