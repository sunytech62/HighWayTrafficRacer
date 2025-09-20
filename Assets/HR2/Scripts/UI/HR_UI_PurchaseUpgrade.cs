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
/// UI upgrader button for engine, handling, and speed.
/// </summary>
public class HR_UI_PurchaseUpgrade : MonoBehaviour, IPointerClickHandler {

    /// <summary>
    /// Upgradable item.
    /// </summary>
    public HR_CartItem item;

    /// <summary>
    /// Gets the upgrade level from the button's level text.
    /// </summary>
    public int UpgradeLevel {

        get {

            // Example: "2" => parse to int 2
            return int.Parse(button.levelText.text);

        }

    }

    /// <summary>
    /// Gets the calculated upgrade price based on the upgrade level.
    /// </summary>
    public int UpgradePrice {

        get {

            return (int)(Mathf.InverseLerp(0, 6, UpgradeLevel + 1) * (defaultPrice * 1.5f));

        }

    }

    /// <summary>
    /// Default price of the item.
    /// </summary>
    private int defaultPrice = 0;

    /// <summary>
    /// Maximum upgrade level.
    /// </summary>
    public int maximumLevel = 5;

    /// <summary>
    /// The panel displaying the upgrade price.
    /// </summary>
    public GameObject pricePanel;

    /// <summary>
    /// The text displaying the upgrade price.
    /// </summary>
    public TextMeshProUGUI priceText;

    /// <summary>
    /// Reference to the upgrade button component.
    /// </summary>
    private RCCP_UI_Upgrade button;

    /// <summary>
    /// Indicates whether the item is fully upgraded.
    /// </summary>
    public bool isUpgraded = false;

    /// <summary>
    /// Initializes the default price and checks the upgrade state.
    /// </summary>
    private void Awake() {

        if (item == null)
            Debug.LogError($"{name}: No HR_CartItem assigned!");

        if (item != null)
            defaultPrice = item.price;

        if (!pricePanel)
            Debug.LogError($"{name}: No pricePanel assigned!");

        if (!priceText)
            Debug.LogError($"{name}: No priceText assigned!");

        // Try to fetch our button reference
        button = GetComponent<RCCP_UI_Upgrade>();

        // Update our state
        UpdateUpgradeState();

    }

    /// <summary>
    /// Updates the button state and UI elements when the object is enabled.
    /// </summary>
    public void OnEnable() {

        // Ensure we have the button reference
        if (!button)
            button = GetComponent<RCCP_UI_Upgrade>();

        UpdateUpgradeState();

    }

    private void Update() {

        //  Is upgraded?
        isUpgraded = CheckPurchase();

    }

    /// <summary>
    /// Central method to recalculate whether item is fully upgraded, 
    /// and refresh the UI accordingly.
    /// </summary>
    private void UpdateUpgradeState() {

        // If we don't have a valid button component, we can't update properly
        if (!button || !pricePanel || !priceText)
            return;

        // Let the button handle its own OnEnable logic
        button.OnEnable();

        // We consider the item upgraded if the upgrade level >= maximumLevel
        isUpgraded = (UpgradeLevel >= maximumLevel);

        // If it's fully upgraded, disable the button, hide the price, etc.
        button.enabled = !isUpgraded;
        pricePanel.SetActive(!isUpgraded);
        priceText.text = isUpgraded ? "" : $"${UpgradePrice:F0}";

    }

    /// <summary>
    /// Public method for external scripts that just returns 
    /// whether the item is fully upgraded (like "CheckPurchase").
    /// </summary>
    public bool CheckPurchase() {

        UpdateUpgradeState();
        return isUpgraded;

    }

    /// <summary>
    /// Handles the click event to purchase the upgrade.
    /// </summary>
    /// <param name="eventData">Event data for the pointer click.</param>
    public void OnPointerClick(PointerEventData eventData) {

        // Ensure references are valid
        if (!button)
            button = GetComponent<RCCP_UI_Upgrade>();

        // If no button or button not active, do nothing
        if (!button || !button.isActiveAndEnabled || !button.gameObject.activeSelf)
            return;

        // If it's already fully upgraded, do nothing
        if (isUpgraded)
            return;

        // Create a local copy for the upgrade logic
        HR_CartItem upgradeItem = item;
        upgradeItem.price = UpgradePrice;

        // Let the main menu panel handle the actual purchase logic
        HR_UI_MainmenuPanel.Instance.CheckUpgradePurchased(upgradeItem);

        button.OnClick();

    }

}
