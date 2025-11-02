using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HR_UI_PurchaseItem : MonoBehaviour, IPointerClickHandler
{
    public HR_CartItem item;
    public GameObject pricePanel;
    public TextMeshProUGUI priceText;
    private Button button;
    public bool isPurchased = false;

    private void Awake()
    {
        // Get required references
        button = GetComponent<Button>();

        // Basic null checks
        /* if (item == null)
             Debug.LogError($"{name}: No HR_CartItem assigned!");
         if (!pricePanel)
             Debug.LogError($"{name}: No pricePanel assigned!");
         if (!priceText)
             Debug.LogError($"{name}: No priceText assigned!");*/

        // Update UI on awake
        UpdatePurchaseState();
    }

    public void OnEnable()
    {
        UpdatePurchaseState();
    }

    private void UpdatePurchaseState()
    {
        // Check whether the user already purchased the item
        isPurchased = item != null && PlayerPrefs.HasKey(item.saveKey);

        // Toggle the price panel and update the text
        if (pricePanel)
            pricePanel.SetActive(!isPurchased);

        if (priceText)
            priceText.text = isPurchased ? "" : $"${item.price:F0}";
    }

    public bool CheckPurchase()
    {
        UpdatePurchaseState();
        return isPurchased;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // If the button isn't interactable or not active, do nothing
        if (!button.interactable || !button.gameObject.activeSelf)
            return;

        // First, refresh current purchase state
        CheckPurchase();

        // Let another script handle the actual purchase or post-purchase logic
        // (e.g. checking balance, unlocking, saving to PlayerPrefs, etc.)
        // Example:
        // HR_UI_MainmenuPanel.Instance.CheckItemPurchased(item);

        // If you wanted to do everything here, you'd implement the purchase flow.
        // Currently, we just call CheckItemPurchased(...) on the main menu panel.
        HR_UI_MainmenuPanel.Instance.CheckItemPurchased(item);
    }
}
