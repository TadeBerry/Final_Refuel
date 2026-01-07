using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public const int SCORE_PER_COIN = 100;

    private static int levelNumber = 1;
    private static int totalScore = 0;


    public static void ResetStaticData()
    {
        levelNumber = 1;
        totalScore = 0;
    }

    public event EventHandler OnGamePaused;
    public event EventHandler OnGameUnpaused;


    [SerializeField] private List<GameLevel> gameLevelList;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    


    private int score;
    private float time;
    private bool isTimerActive;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
{
    GameLevel levelToLoad = GetGameLevel();

    if (GameInput.Instance != null)
    {
        GameInput.Instance.OnMenuButtonPressed += GameInput_OnMenuButtonPressed;
    }
}

    private void GameInput_OnMenuButtonPressed(object sender, System.EventArgs e)
    {
        PauseUnpauseGame();
    }

    private void Lander_OnStateChanged(object sender, Lander.OnStateChangedEventArgs e)
{
    isTimerActive = e.state == Lander.State.Normal;

    if (e.state == Lander.State.Normal)
    {
        cinemachineCamera.Target.TrackingTarget = Lander.Instance.transform; 
        
        CinemachineCameraZoom2D.Instance.SetNormalOrthographicSize(); 
    }
}

    private void Update()
    {
        if (isTimerActive)
        {
        time += Time.deltaTime;
        }
        if (Keyboard.current.lKey.wasPressedThisFrame) 
        {
        levelNumber = 3;

        ClearCurrentLevel();
        LoadCurrentLevel();

        GameLevel levelToLoad = GetGameLevel();
        if (Lander.Instance != null)
            {
                Lander.Instance.OnCoinPickup -= Lander_OnCoinPickup;
                Lander.Instance.OnLanded -= Lander_OnLanded;
                Lander.Instance.OnStateChanged -= Lander_OnStateChanged;

                Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
                Lander.Instance.OnLanded += Lander_OnLanded;
                Lander.Instance.OnStateChanged += Lander_OnStateChanged;

                Lander.Instance.ResetLander();
            }
        }
    }

    private void LoadCurrentLevel()
{
    GameLevel gameLevel = GetGameLevel();
    
    if (gameLevel == null) return;

    GameLevel spawnedGameLevel = Instantiate(gameLevel, Vector3.zero, Quaternion.identity);

    if (Lander.Instance != null)
    {
        Lander.Instance.TriggerSpawnProtection();

        Lander.Instance.gameObject.SetActive(true); 
        Lander.Instance.transform.position = spawnedGameLevel.GetLanderStartPosition();
        Lander.Instance.transform.rotation = Quaternion.identity;
        
        Rigidbody2D rb = Lander.Instance.GetComponent<Rigidbody2D>();
        
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        if (levelNumber == 4)
        {
            Lander.Instance.transform.rotation = Quaternion.Euler(0, 0, 180f);
        }
    }

    if (cinemachineCamera != null)
    {
        cinemachineCamera.Target.TrackingTarget = spawnedGameLevel.GetCameraStartTargetTransform();
        cinemachineCamera.ForceCameraPosition(spawnedGameLevel.GetCameraStartTargetTransform().position, Quaternion.identity);
        CinemachineCameraZoom2D.Instance.ResetZoom(spawnedGameLevel.GetZoomedOutOrthographicSize());
    }
}

    private GameLevel GetGameLevel()
    {
        foreach (GameLevel gameLevel in gameLevelList)
        {
            if (gameLevel.GetLevelNumber() == levelNumber)
            {
                return gameLevel;
            }
        }
        return null;
    }

    private void Lander_OnLanded(object sender, Lander.OnLandedEventArgs e)
    {
        AddScore(e.score);
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AddScore(100);
    }

    public void AddScore(int addScoreAmount)
    {
        score += addScoreAmount;
        Debug.Log(score);
    }

    public int GetScore()
    {
        return score;
    }

    public float GetTime()
    {
        return time;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    public void GoToNextLevel()
{
    levelNumber++;
    totalScore += score;
    score = 0;
    time = 0;

    if (GetGameLevel() == null)
    {
        SceneLoader.LoadScene(SceneLoader.Scene.GameOverScene);
    }
    else
    {
        ClearCurrentLevel(); 

        if (Lander.Instance != null) 
        {
            Lander.Instance.ResetLander(); 
        }
        
        LoadCurrentLevel();
    }
}

    public void RetryLevel()
{
    Time.timeScale = 1f;

    ClearCurrentLevel();
    
    score = 0;
    time = 0;
    isTimerActive = false;

    if (Lander.Instance != null)
    {
        Rigidbody2D rb = Lander.Instance.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }
        
        Lander.Instance.gameObject.SetActive(true);
        Lander.Instance.ResetLander(); 
        
    }
    
    LoadCurrentLevel();
}

    public void PauseUnpauseGame()
    {
        if (Time.timeScale == 1f)
        {
            PauseGame();
        }
        else
        {
            UnpauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        OnGamePaused?.Invoke(this, EventArgs.Empty);
    }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        OnGameUnpaused?.Invoke(this, EventArgs.Empty);
    }
    public void ReloadCurrentLevel()
    {
    string currentSceneName = SceneManager.GetActiveScene().name;
    SceneManager.LoadScene(currentSceneName);
    }

    private void ClearCurrentLevel()
    {
    GameLevel existingLevel = FindFirstObjectByType<GameLevel>();
    if (existingLevel != null)
    {
        Destroy(existingLevel.gameObject);
    }
    }

    public int GetLevelNumber()
    {
    return levelNumber;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
{
    if (scene.name == SceneLoader.Scene.GameScene.ToString())
    {
        cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
        ClearCurrentLevel();
        LoadCurrentLevel();

        if (Lander.Instance != null)
        {
            Lander.Instance.OnCoinPickup -= Lander_OnCoinPickup;
            Lander.Instance.OnLanded -= Lander_OnLanded;
            Lander.Instance.OnStateChanged -= Lander_OnStateChanged;

            Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
            Lander.Instance.OnLanded += Lander_OnLanded;
            Lander.Instance.OnStateChanged += Lander_OnStateChanged;
            
            Lander.Instance.ResetLander();


        }
    }
    
    }
    private void OnEnable()
    {
    SceneManager.sceneLoaded += OnSceneLoaded;
    }

private void OnDisable()
    {
    SceneManager.sceneLoaded -= OnSceneLoaded;
    }

}
