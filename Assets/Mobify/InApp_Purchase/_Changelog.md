In App Purchasing V_1.1.1 (24 Sep 25)
============================
Bug Resolve:
* Product's MetaData ( price, Title, Desc) fetched issue resolved in IAP Plugin version 5.x.x

----------------------------------:|:---------------------------------

In App Purchasing V_1.1.0 (18 Sep 25)
============================
Addition:
* Support IAP plugin version 5.x.x.
* Add Support Scriptable Symbols InAppPurchase.

----------------------------------:|:---------------------------------

In App Purchasing V_1.0.10 (27 Aug 25)
============================
Resolve:
* isIAUnderProcess boolean added for restrict AppOpen show OnAppStateChange for both Admob & Applovin Plugin.

----------------------------------:|:---------------------------------

In App Purchasing V_1.0.9 (09 Jul 25)
============================
Resolve:
* isAppOpenCanShow handle for Applovin plugin too.

----------------------------------:|:---------------------------------

In App Purchasing V_1.0.8 (30 Jun 25)
============================
Resolve:
* Exception Handle while waitingPanel enable and after that AppOpen is showing. 
* Add LastBannerState function calling for AppLovin Plugin when black screen shown and banner also appear above this screen.
* Rename Editor class "EnumValuesEditorIAP" for resolve conflict.

Addition:
* In InAppSuccess Function added Enum type switch cases for overcome human errors for mistake in string key.

----------------------------------:|:---------------------------------

In App Purchasing V_1.0.7 (14 Apr 25)
============================
Resolve:
* Exception Handle while calling Purchase by passing Index value and currently IAP Plug-in not initialized successful.

----------------------------------:|:---------------------------------

In App Purchasing V_1.0.6 (20 Mar 25)
============================
Addition:
* IsAppOpenCanShow added when IAP panel ready to Open.
* Add Feature "PreRegistrationReward" for pre register users.
* 1st Time Non-Consumable IAP event restrict to send on Analytics Dashboard by using boolean isPurchasingFromMyself

----------------------------------:|:---------------------------------

In App Purchasing V_1.0.5 (24 Dec 24)
============================
Change:
* Exception Handling
  - Find Object to assign by reference for WaitingPanel

Addition:
* AppOpenCanShow handle when IAP in Progress.

----------------------------------:|:---------------------------------

In App Purchasing V_1.0.4 (31 Oct 24)
============================
Addition:
* Exception Handling
  - WaitingPanel Exception Handle
  - Find Object in children in waitingPanel with Optimized way.
  - Restore Purchase Exception Handle.
* Add Purchase(IAP_Key_Enum) function.

----------------------------------:|:---------------------------------

In App Purchasing V_1.0.3 (12 Oct 24)
============================
Addition:
* Singular Analytics Send.
* Add Action<bool> for response.
* Add waiting Panel
* Add Validator Code region
* Add Android and Apple Tangle files.
* Add Success UnityEvent in IAP_Button.cs script.

----------------------------------:|:---------------------------------

In App Purchasing V_1.0.2 (26 Jul 24)
============================
Modify: 
* Exception Handle On Initialization by default List and Dictionary created by default.
* Exception Handle on Initialization by Only One Time Initialization.

Addition:
* Send all FirebaseAnalytics event to Singular dashboard
* Add action<bool> for result of response of IAP Purchase.

----------------------------------:|:---------------------------------

In App Purchasing V_1.0.1 (02 Jul 24)
============================
Addition: 
* Add IAP_Button.cs script for direct apply on buttons. Support (Keys, Text of Price, Title, Description and Button Click.
* Add Scriptable Object for adding IAP Keys Details (Key & Type)
* Add Dynamic IAP_Enum values for using in code for reduce human errors.

----------------------------------:|:---------------------------------

In App Purchasing V_1.0.0 (13 Jun 24)
============================
Addition: 
* Support Auto-Initialization, Initialization Delay,
* Support Debug Log
* Support Default Values
* Supportive Functions
   - Purchase (int InAppIndex, ActionAfterSuccess (optional)) 
   - Purchase (string InAppKey, ActionAfterSuccess (optional)) 
   - GetData (string InAppKey, InAppDataType)
	- return Title, Description & Price
* Introduce Actions
   - Pass the Action into Purchase function for calls when success.
* Introduce Events
   - Initilization Complete Event Trigger ()
   - Information Event Trigger (string Info)
   - InApp Success Event Trigger (string InAppKey)

Verified:
- IAP version : 4.12.0
- Unity Editor : 2020.3.40f1   

----------------------------------:|:---------------------------------

