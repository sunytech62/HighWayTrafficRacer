Firebase_Analytics V_1.0.6 (27 Jun 25)
============================
Bug Fix:
* All Firebase Dependency Availability check only once.
* initilizationStatus set none for Editor.

----------------------------------:|:---------------------------------

Firebase_Analytics V_1.0.5 (22 Apr 25)
============================
Addition: 
* Added paramater Firebase.Analytics.Parameter[] SendAnalytics function.

----------------------------------:|:---------------------------------

Firebase_Analytics V_1.0.4 (16 Apr 25)
============================
Bug Fix: 
* Exception Handle by adding delay when fetch completed and invoke FetchCompleteEvent.

----------------------------------:|:---------------------------------

Firebase_Analytics V_1.0.3 (24 Dec 24)
============================
Bug Fix: 
* Resolve Firebase Dependencies by adding native plugin code of RemoteConfig & Crashlytics for check Initilization.

----------------------------------:|:---------------------------------

Firebase_Analytics V_1.0.2 (23 Oct 24)
============================
Bug Fix: 
* Change Color issue Resolved.

----------------------------------:|:---------------------------------

Firebase_Analytics V_1.0.1 (30 Sep 24)
============================
Addition: 
* Added Custom Define Symbols for Firebase Analytics & this plugin.
* Added Firebase_Analytics Region code for ignore bugs either Firebase plugin is not present in the project.
* Added HexaColor variable for change plugin color smoothly.

----------------------------------:|:---------------------------------

Firebase_Analytics V_1.0.0 (13 Sep 24)
============================
Addition: 
* Support Auto-Initialization, Initialization Delay,
* Support Debug Log
* Support Event Handler after On Firebase Initilized.
* Support Pending Event Handle and Send these event when firebase initilized.
* Support Multiple Function Overloading
   - SendAnalytics(string name)
   - SendAnalytics(string name,string paramKey,string paramVal)
   - SendAnalytics(string name,Dictionary dataCollection)
* Editor support for visually easy understandable.

----------------------------------:|:---------------------------------
 
Information:
=============

Check Real Time Events on Firebase Analytics

Step 1: Connect respective Device to the computer via cable.
Step 2: Open CMD & go to your "SDK/plateform-tool" folder where adb.exe file exist
Step 3: write this code on CMD
	adb shell setprop debug.firebase.analytics.app <app-package-name>

Note: Now This Device is successfully sending the debug event to firebase to respective app-package-name.
Go to Firebase Console and select Debug View to see events.
