using UnityEngine;

public class Testing_InAppPurchase : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Text infoTxt;
    [SerializeField] UnityEngine.UI.InputField inputField;

    private void OnEnable()
    {
        InAppPurchase.Instance.InfoEvent += AddInfoText;
        InAppPurchase.Instance.IAPSuccessEvent += AddInfoText;
    }
    private void OnDisable()
    {
        InAppPurchase.Instance.InfoEvent -= AddInfoText;        
        InAppPurchase.Instance.IAPSuccessEvent -= AddInfoText;
    }
    public void AddInfoText(string text)
    {
        infoTxt.text += "\n- "+text;
    }
    public void Purchase(int index)
    {
        InAppPurchase.Instance.Purchase(index);
    }
    public void Purchase(string IAPKey)
    {
        InAppPurchase.Instance.Purchase(IAPKey);
    }
    public void GetPrice()
    {
        InAppPurchase.Instance.GetData(inputField.textComponent.text, InAppPurchase.InAppDataType.MetaDataType.Price);
    }
    public void GetTitle()
    {
        InAppPurchase.Instance.GetData(inputField.textComponent.text, InAppPurchase.InAppDataType.MetaDataType.Title);
    }
    public void GetDescription()
    {
        InAppPurchase.Instance.GetData(inputField.textComponent.text, InAppPurchase.InAppDataType.MetaDataType.Description);
    }
}