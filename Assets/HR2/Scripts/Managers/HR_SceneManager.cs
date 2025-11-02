using UnityEngine;
using UnityEngine.EventSystems;

public class HR_SceneManager : HR_Singleton<HR_SceneManager>
{
    public enum LevelType { MainMenu, Gameplay }
    public LevelType levelType = LevelType.Gameplay;

    #region GAMEPLAYCOMPONENTS

    public HR_GamePlayManager GameplayManager
    {
        get
        {
            if (gameplayManager == null)
                gameplayManager = FindFirstObjectByType<HR_GamePlayManager>();
            return gameplayManager;
        }
    }

    public HR_CurvedRoadManager CurvedRoadManager
    {
        get
        {
            if (curvedRoadManager == null)
                curvedRoadManager = FindFirstObjectByType<HR_CurvedRoadManager>();
            return curvedRoadManager;
        }
    }

    public HR_PathManager PathManager
    {
        get
        {
            if (pathManager == null)
                pathManager = FindFirstObjectByType<HR_PathManager>();
            return pathManager;
        }
    }

    public HR_TrafficManager TrafficManager
    {
        get
        {
            if (trafficManager == null)
                trafficManager = FindFirstObjectByType<HR_TrafficManager>();
            return trafficManager;
        }
    }

    public HR_LaneManager LaneManager
    {
        get
        {
            if (laneManager == null)
                laneManager = FindFirstObjectByType<HR_LaneManager>();
            return laneManager;
        }
    }

    public HR_Camera PlayerCamera
    {
        get
        {
            if (playerCamera == null)
                playerCamera = FindFirstObjectByType<HR_Camera>();
            return playerCamera;
        }
    }

    public HR_UI_GameplayPanel GameplayPanel
    {
        get
        {
            if (gameplayPanel == null)
                gameplayPanel = FindFirstObjectByType<HR_UI_GameplayPanel>();
            return gameplayPanel;
        }
    }

    public HR_UI_GameOverPanel GameoverPanel
    {
        get
        {
            if (gameoverPanel == null)
                gameoverPanel = FindFirstObjectByType<HR_UI_GameOverPanel>();
            return gameoverPanel;
        }
    }

    public EventSystem Event
    {
        get
        {
            if (eventSystem == null)
                eventSystem = FindFirstObjectByType<EventSystem>();
            return eventSystem;
        }
    }

    #endregion

    #region MAINMENUCOMPONENTS

    public HR_MainMenuManager MainMenuManager
    {
        get
        {
            if (mainmenuManager == null)
                mainmenuManager = FindFirstObjectByType<HR_MainMenuManager>();
            return mainmenuManager;
        }
    }

    public HR_UI_MainmenuPanel MainMenuPanel
    {
        get
        {
            if (mainmenuPanel == null)
                mainmenuPanel = FindFirstObjectByType<HR_UI_MainmenuPanel>();
            return mainmenuPanel;
        }
    }

    public HR_Camera_Showroom ShowroomCamera
    {
        get
        {
            if (showroomCamera == null)
                showroomCamera = FindFirstObjectByType<HR_Camera_Showroom>();
            return showroomCamera;
        }
    }

    #endregion

    #region RCCPCOMPONENTS

    public RCCP_SceneManager RCCPSceneManager
    {
        get
        {
            if (rccpSceneManager == null)
                rccpSceneManager = FindFirstObjectByType<RCCP_SceneManager>();
            return rccpSceneManager;
        }
    }

    #endregion

    #region GET
    private HR_GamePlayManager gameplayManager;
    private HR_CurvedRoadManager curvedRoadManager;
    private HR_PathManager pathManager;
    private HR_TrafficManager trafficManager;
    private HR_LaneManager laneManager;
    private HR_Camera playerCamera;
    private HR_UI_GameplayPanel gameplayPanel;
    private HR_UI_GameOverPanel gameoverPanel;
    private EventSystem eventSystem;

    private HR_MainMenuManager mainmenuManager;
    private HR_UI_MainmenuPanel mainmenuPanel;
    private HR_Camera_Showroom showroomCamera;

    private RCCP_SceneManager rccpSceneManager;
    #endregion

    [SerializeField] GameObject challengePrefab;

    private void Awake()
    {
        GetAllComponents();
    }

    public void GetAllComponents()
    {
        switch (levelType)
        {
            case LevelType.Gameplay:

                HR_GamePlayManager _gameplayManager = GameplayManager;
                RCCP_SceneManager _rccpSceneManager = RCCPSceneManager;
                HR_CurvedRoadManager _curvedRoadManager = CurvedRoadManager;
                HR_PathManager _pathManager = PathManager;
                HR_TrafficManager _trafficManager = TrafficManager;
                HR_LaneManager _laneManager = LaneManager;
                HR_Camera _playerCamera = PlayerCamera;

                break;
            case LevelType.MainMenu:

                HR_MainMenuManager _mainmenuManager = MainMenuManager;

                break;
        }
    }
}
