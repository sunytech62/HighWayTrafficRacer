//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2025 BoneCracker Games
// https://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

/// <summary>
/// UI button for purchasing the items.
/// </summary>
public class HR_UI_PurchaseItem : MonoBehaviour, IPointerClickHandler {
    /// <summary>
    /// The cart item associated with this button.
    /// </summary>
    public HR_CartItem item;

    /// <summary>
    /// The panel displaying the price.
    /// </summary>
    public GameObject pricePanel;

    /// <summary>
    /// The text displaying the item's price.
    /// </summary>
    public TextMeshProUGUI priceText;

    /// <summary>
    /// The button component for purchasing the item.
    /// </summary>
    private Button button;

    /// <summary>
    /// Indicates whether the item is purchased.
    /// </summary>
    public bool isPurchased = false;

    /// <summary>
    /// Initializes the button and UI elements.
    /// </summary>
    private void Awake() {
        // Get required references
        button = GetComponent<Button>();

        // Basic null checks
        if (item == null)
            Debug.LogError($"{name}: No HR_CartItem assigned!");
        if (!pricePanel)
            Debug.LogError($"{name}: No pricePanel assigned!");
        if (!priceText)
            Debug.LogError($"{name}: No priceText assigned!");

        // Update UI on awake
        UpdatePurchaseState();
    }

    /// <summary>
    /// Updates the UI elements when the object is enabled.
    /// </summary>
    public void OnEnable() {
        UpdatePurchaseState();
    }

    /// <summary>
    /// Refreshes the purchase state and updates the UI (price panel, text, etc).
    /// </summary>
    private void UpdatePurchaseState() {
        // Check whether the user already purchased the item
        isPurchased = item != null && PlayerPrefs.HasKey(item.saveKey);

        // Toggle the price panel and update the text
        if (pricePanel)
            pricePanel.SetActive(!isPurchased);

        if (priceText)
            priceText.text = isPurchased ? "" : $"${item.price:F0}";
    }

    /// <summary>
    /// Returns true if the item is already purchased, otherwise false.
    /// (Kept for compatibility in case other scripts call it.)
    /// </summary>
    public bool CheckPurchase() {
        UpdatePurchaseState();
        return isPurchased;
    }

    /// <summary>
    /// Handles the click event to purchase the item (or trigger purchase flow in another script).
    /// </summary>
    /// <param name="eventData">Event data for the pointer click.</param>
    public void OnPointerClick(PointerEventData eventData) {
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
