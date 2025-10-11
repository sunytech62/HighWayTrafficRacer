using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HR_API
{
    public const int UpgradeCarPrice = 500;
    public const int PaintPrice = 500;
    public const int TyrePrice = 500;
    public const int RimPrice = 500;
    public const int SpoilerPrice = 500;



    public delegate void onPlayerMoneyChanged();

    public static event onPlayerMoneyChanged OnPlayerMoneyChanged;

    public delegate void onPlayerNameChanged();

    public static event onPlayerNameChanged OnPlayerNameChanged;

    public static bool IsFirstGameplay()
    {
        if (GetTotalPlayedTime() <= 1f)
            return true;
        else
            return false;
    }

    public static void SetPlayerName(string newPlayerName)
    {
        PlayerPrefs.SetString("PlayerName", newPlayerName);
        SetTotalPlayedTime();

        if (OnPlayerNameChanged != null)
            OnPlayerNameChanged();
    }

    public static string GetPlayerName()
    {
        return PlayerPrefs.GetString("PlayerName", "New Player");
    }

    public static int GetCurrency()
    {
        return PlayerPrefs.GetInt("Currency", 0);
    }

    public static void ConsumeCurrency(int consume)
    {
        int current = GetCurrency();

        PlayerPrefs.SetInt("Currency", current - consume);

        if (OnPlayerMoneyChanged != null)
            OnPlayerMoneyChanged();
    }

    public static void AddCurrency(int add)
    {

        int current = GetCurrency();

        PlayerPrefs.SetInt("Currency", current + add);

        if (OnPlayerMoneyChanged != null)
            OnPlayerMoneyChanged();

    }

    public static int[] UnlockedVehicles()
    {
        List<int> unlockeds = new List<int>();

        for (int i = 0; i < HR_PlayerCars.Instance.cars.Length; i++)
        {

            if (PlayerPrefs.HasKey(HR_PlayerCars.Instance.cars[i].playerCar.name + "Owned"))
                unlockeds.Add(i);

        }
        return unlockeds.ToArray();
    }

    public static void UnlockVehicle(int index)
    {
        PlayerPrefs.SetInt(HR_PlayerCars.Instance.cars[index].playerCar.name + "Owned", 1);
    }
    public static void LockVehicle(int index)
    {

        PlayerPrefs.DeleteKey(HR_PlayerCars.Instance.cars[index].playerCar.name + "Owned");

    }

    public static void UnlockAllVehicles()
    {
        for (int i = 0; i < HR_PlayerCars.Instance.cars.Length; i++)
        {

            if (HR_PlayerCars.Instance.cars[i] != null)
                PlayerPrefs.SetInt(HR_PlayerCars.Instance.cars[i].playerCar.name + "Owned", 1);

        }
    }

    public static bool OwnedVehicle(int index)
    {
        if (PlayerPrefs.HasKey(HR_PlayerCars.Instance.cars[index].playerCar.name + "Owned"))
            return true;
        else
            return false;
    }

    public static void SaveHighScores(int[] scores)
    {
        PlayerPrefs.SetInt("bestScoreOneWay", scores[0]);
        PlayerPrefs.SetInt("bestScoreTwoWay", scores[1]);
        PlayerPrefs.SetInt("bestScoreTimeAttack", scores[2]);
        PlayerPrefs.SetInt("bestScoreBomb", scores[3]);
    }

    public static int[] GetHighScores()
    {
        int[] scores = new int[4];

        scores[0] = PlayerPrefs.GetInt("bestScoreOneWay");
        scores[1] = PlayerPrefs.GetInt("bestScoreTwoWay");
        scores[2] = PlayerPrefs.GetInt("bestScoreTimeAttack");
        scores[3] = PlayerPrefs.GetInt("bestScoreBomb");

        return scores;
    }

    public static void MainMenu()
    {
        SceneManager.LoadSceneAsync(0);
        SetTotalPlayedTime();
    }

    public static void RestartGame()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        SetTotalPlayedTime();
    }

    public static void SetAudioVolume(float volume)
    {
        AudioListener.volume = volume;
        PlayerPrefs.SetFloat("AudioVolume", volume);
        HR_Events.Event_OnOptionsChanged();

        //Load();
        //CCDS_SaveGameManager.saveData.audioVolume = volume;
        //Save();
    }

    public static void SetMusicVolume(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        HR_Events.Event_OnOptionsChanged();

        //Load();
        //CCDS_SaveGameManager.saveData.musicVolume = volume;
        //Save();
    }

    public static float GetAudioVolume()
    {
        //Load();
        //return CCDS_SaveGameManager.saveData.audioVolume;
        return PlayerPrefs.GetFloat("AudioVolume", HR_Settings.Instance.defaultAudioVolume);
    }

    public static float GetMusicVolume()
    {

        //Load();
        //return CCDS_SaveGameManager.saveData.musicVolume;

        return PlayerPrefs.GetFloat("MusicVolume", HR_Settings.Instance.defaultMusicVolume);

    }

    public static void SetDrawDistance(float distance)
    {
        PlayerPrefs.SetFloat("DrawDistance", distance);
        HR_Events.Event_OnOptionsChanged();
    }

    public static float GetDrawDistance()
    {
        return PlayerPrefs.GetFloat("DrawDistance", HR_Settings.Instance.defaultDrawDistance);
    }

    public static float GetTotalPlayedTime()
    {
        return PlayerPrefs.GetFloat("TotalPlayedTime", 0f);
    }

    public static void SetTotalPlayedTime()
    {
        float lastPlayedTime = Time.timeSinceLevelLoad;
        PlayerPrefs.SetFloat("TotalPlayedTime", GetTotalPlayedTime() + lastPlayedTime);
    }

    public static void SetControllerType(int controllerIndex)
    {

        switch (controllerIndex)
        {

            case 0:
                PlayerPrefs.SetInt("ControllerType", 0);
                RCCP.SetMobileController(RCCP_Settings.MobileController.TouchScreen);
                break;
            case 1:
                PlayerPrefs.SetInt("ControllerType", 1);
                RCCP.SetMobileController(RCCP_Settings.MobileController.Gyro);
                break;
            case 2:
                PlayerPrefs.SetInt("ControllerType", 2);
                RCCP.SetMobileController(RCCP_Settings.MobileController.SteeringWheel);
                break;
            case 3:
                PlayerPrefs.SetInt("ControllerType", 3);
                RCCP.SetMobileController(RCCP_Settings.MobileController.Joystick);
                break;

        }

        HR_Events.Event_OnOptionsChanged();

    }

    public static int GetControllerType()
    {

        switch (RCCP_Settings.Instance.mobileController)
        {

            case RCCP_Settings.MobileController.TouchScreen:
                return 0;
            case RCCP_Settings.MobileController.Gyro:
                return 1;
            case RCCP_Settings.MobileController.SteeringWheel:
                return 2;
            case RCCP_Settings.MobileController.Joystick:
                return 3;

        }

        return 0;
    }

    public static void SetQuality(int qualityIndex)
    {

        QualitySettings.SetQualityLevel(qualityIndex);

        HR_Events.Event_OnOptionsChanged();

    }

    public static int GetQuality()
    {

        return QualitySettings.GetQualityLevel();

    }
    public static void SetShadows()
    {

        PlayerPrefsX.SetBool("Shadows", !PlayerPrefsX.GetBool("Shadows", HR_Settings.Instance.defaultShadows));
        HR_Events.Event_OnOptionsChanged();

    }

    public static bool GetShadows()
    {

        return PlayerPrefsX.GetBool("Shadows", HR_Settings.Instance.defaultShadows);

    }

    public static void SetPP()
    {

        PlayerPrefsX.SetBool("PP", !PlayerPrefsX.GetBool("PP", HR_Settings.Instance.defaultPP));
        HR_Events.Event_OnOptionsChanged();

    }

    public static bool GetPP()
    {

        return PlayerPrefsX.GetBool("PP", HR_Settings.Instance.defaultPP);

    }

    public static void ResetGame()
    {

        PlayerPrefs.DeleteAll();
        SceneManager.LoadSceneAsync(HR_Settings.Instance.mainMenuSceneIndex);

    }

}
