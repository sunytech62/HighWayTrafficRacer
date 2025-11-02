using System;
using System.Collections;
using UnityEngine;

public class HR_GamePlayManager : MonoBehaviour
{
    #region SINGLETON PATTERN
    private static HR_GamePlayManager instance;
    public static HR_GamePlayManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindFirstObjectByType<HR_GamePlayManager>();
            }

            return instance;
        }
    }
    #endregion

    public bool gameStarted = false;

    public HR_Player player;

    public enum DayOrNight { Day, Night }

    [Header("Time Of The Scene")]
    public DayOrNight dayOrNight = DayOrNight.Day;

    [Header("Spawn Location Of The Cars")]
    public Transform spawnLocation;

    private int selectedCarIndex = 0;
    private int selectedModeIndex = 0;
    private bool paused = false;

    private readonly float minimumSpeedAtStart = 60f;
    public static bool isStartCountDown = false;

    private void Awake()
    {
        // Getting selected player car index and mode index.
        selectedCarIndex = PlayerPrefs.GetInt("SelectedPlayerCarIndex");

        // Setting time scale, volume, unpause, and target frame rate.
        Time.timeScale = 1f;
        AudioListener.volume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        AudioListener.pause = false;

        if (HR_UIOptionsManager.Instance)
            HR_UIOptionsManager.Instance.OnEnable();

        isStartCountDown = false;
    }

    private IEnumerator Start()
    {
        HR_API.SetControllerType(HR_API.GetSelectedControl());
        // Make sure time scale is 1. We are setting volume to 0, we'll be increasing it smoothly in the update method.
        Time.timeScale = 1f;
        AudioListener.volume = 0f;
        AudioListener.pause = false;
        gameStarted = false;

        yield return new WaitForSeconds(0.5f);
        SpawnPlayer();     // Spawning the player vehicle.
        yield return new WaitForSeconds(0.5f);
        if (GameManager.SelectedMode == GameMode.Challenge)
        {
            SpawnChallengeManager();
        }
        else if (GameManager.SelectedMode == GameMode.TimeTrial)
        {
            SpawnTimeTrial();
        }
        else
        {
            isStartCountDown = true;
        }
        StartCoroutine(StartRaceDelayed());     //  Starting the race with a delay.
        StartCoroutine(AdjustAudioOnStart());       //  Adjusting the audiovolume at start.     
    }

    private void SpawnTimeTrial()
    {
        var chaPrefab = GameManager.Instance.timeTrialPrefab;
        Instantiate(chaPrefab, Vector3.zero, Quaternion.identity);
        isStartCountDown = true;
    }

    void SpawnChallengeManager()
    {
        var chaPrefab = GameManager.Instance.challengeModeControllerPrefab;
        Instantiate(chaPrefab);
    }

    private IEnumerator AdjustAudioOnStart()
    {
        float timer = 2f;

        while (timer > 0)
        {
            timer -= Time.deltaTime;

            // Adjusting volume smoothly.
            float targetVolume = HR_API.GetAudioVolume();
            AudioListener.volume = Mathf.MoveTowards(AudioListener.volume, targetVolume, Time.deltaTime * 3f);

            yield return null;
        }

        AudioListener.volume = HR_API.GetAudioVolume();
    }

    private void SpawnPlayer()
    {
        Debug.LogError("Spawn");
        player = (RCCP.SpawnRCC(HR_PlayerCars.Instance.cars[selectedCarIndex].playerCar.GetComponent<RCCP_CarController>(), spawnLocation.position, spawnLocation.rotation, true, false, true)).GetComponent<HR_Player>();
        player.canCrash = true;
        player.Rigid.linearVelocity = new Vector3(0f, 0f, minimumSpeedAtStart / 3.6f);
        StartCoroutine(CheckDayTime());

        if (!PlayerPrefs.HasKey(player.CarController.Customizer.saveFileName))
        {
            SaveCustomization();
        }
        else
        {
            LoadCustomization();
            ApplyCustomization();
        }

        // Listening event when player spawned.
        HR_Events.Event_OnPlayerSpawned(player);
    }
    public IEnumerator StartRaceDelayed()
    {
        while (!isStartCountDown)
        {
            Debug.LogError("fdfdfd");
            yield return new WaitForSecondsRealtime(0.3f);
        }
        GameManager.Instance.LoadingPanel(false);
        HR_Events.Event_OnCountDownStarted();

        yield return new WaitForSeconds(1f);

        gameStarted = true;
        RCCP.SetControl(player.GetComponent<RCCP_CarController>(), true);

        HR_Events.Event_OnRaceStarted();
    }

    private IEnumerator CheckDayTime()
    {
        if (player.GetComponent<RCCP_CarController>().Lights == null)
            yield break;

        yield return new WaitForFixedUpdate();

        if (dayOrNight == DayOrNight.Night)
            player.GetComponent<RCCP_CarController>().Lights.lowBeamHeadlights = true;
        else
            player.GetComponent<RCCP_CarController>().Lights.lowBeamHeadlights = false;
    }

    public void CrashedPlayer(HR_Player player, int[] scores)
    {
        gameStarted = false;

        HR_Events.Event_OnPlayerDied(player, scores);

        StartCoroutine(FinishRaceDelayed(1f));
    }

    public IEnumerator FinishRaceDelayed(float delayTime)
    {
        yield return new WaitForSecondsRealtime(delayTime);
        FinishRace();
    }

    public void FinishRace()
    {
        switch (GameManager.SelectedMode)
        {
            case GameMode.Endless:
                PlayerPrefs.SetInt("bestScoreEndless", (int)player.score);
                break;
            case GameMode.Challenge:
                PlayerPrefs.SetInt("bestScoreChallenge", (int)player.score);
                break;
            case GameMode.TimeTrial:
                PlayerPrefs.SetInt("bestScoreTimeTrial", (int)player.score);
                break;
            case GameMode.LowSpeedBomb:
                PlayerPrefs.SetInt("bestScoreBomb", (int)player.score);
                break;
            case GameMode.PolliceChase:
                PlayerPrefs.SetInt("bestScorePoliceChase", (int)player.score);
                break;
        }
    }

    public void MainMenu()
    {
        HR_API.MainMenu();
    }

    public void RestartGame()
    {
        HR_API.RestartGame();
    }

    public void Paused()
    {
        paused = !paused;

        if (paused)
            HR_Events.Event_OnPaused();
        else
            HR_Events.Event_OnResumed();
    }

    public void SaveCustomization()
    {
        HR_Player currentVehicle = player;

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Save();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");
    }

    public void LoadCustomization()
    {
        HR_Player currentVehicle = player;

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Load();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");
    }

    public void ApplyCustomization()
    {
        HR_Player currentVehicle = player;

        if (currentVehicle.CarController.Customizer)
            currentVehicle.CarController.Customizer.Initialize();
        else
            Debug.LogWarning("Customizer couldn't found on this player vehicle named " + currentVehicle.transform.name + ", please add customizer component through the RCCP_CarController!");
    }

    private void Reset()
    {
        if (spawnLocation == null)
        {
            GameObject spawnLocationGO = GameObject.Find("HR_SpawnLocation");

            if (spawnLocationGO)
                spawnLocation = spawnLocationGO.transform;

            if (spawnLocation) return;

            spawnLocation = new GameObject("HR_SpawnLocation").transform;
            spawnLocation.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
            spawnLocation.position += Vector3.up * 1f;
            spawnLocation.position += Vector3.forward * 10f;
            spawnLocation.position += Vector3.right * 1.8f;
        }
    }
}
