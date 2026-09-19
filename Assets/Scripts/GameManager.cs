using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Waiting,
    Playing,
    Paused,
    GameOver
}

public class GameManager : MonoBehaviour
{
    private const string MainMenuSceneName = "MainScene";
    private const string BestScoreKey = "BestScore";

    public static GameManager instance;

    private readonly List<RaycastResult> uiRaycastResults = new List<RaycastResult>();
    private int score;
    private int bestScore;
    private Text startMessage;
    private InputAction pauseAction;
    private InputAction confirmAction;
    private PointerEventData pointerEventData;
    private EventSystem cachedEventSystem;

    [SerializeField] private Text textScore;
    [SerializeField] private GameObject gameStartPanel;
    [SerializeField] private GameObject rightTab;
    [SerializeField] private GameObject leftTab;
    [SerializeField] private GameObject back;

    public event Action<GameState> StateChanged;

    public GameState State { get; private set; } = GameState.Waiting;
    public bool IsPlaying => State == GameState.Playing;
    public bool PointerReleaseRequired { get; private set; }
    public int Score => score;
    public int BestScore => bestScore;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            enabled = false;
            Destroy(gameObject);
            return;
        }

        instance = this;
        Application.targetFrameRate = 60;
        Time.timeScale = 1f;

        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
        startMessage = gameStartPanel != null ? gameStartPanel.GetComponent<Text>() : null;
        if (startMessage != null)
        {
            startMessage.raycastTarget = false;
        }

        pauseAction = new InputAction("Pause", InputActionType.Button);
        pauseAction.AddBinding("<Keyboard>/escape");
        pauseAction.AddBinding("<Keyboard>/p");
        pauseAction.AddBinding("<Gamepad>/start");

        confirmAction = new InputAction("Confirm", InputActionType.Button);
        confirmAction.AddBinding("<Keyboard>/space");
        confirmAction.AddBinding("<Keyboard>/enter");
        confirmAction.AddBinding("<Keyboard>/numpadEnter");
        confirmAction.AddBinding("<Keyboard>/r");
        confirmAction.AddBinding("<Gamepad>/buttonSouth");
    }

    private void OnEnable()
    {
        pauseAction?.Enable();
        confirmAction?.Enable();
    }

    private void OnDisable()
    {
        pauseAction?.Disable();
        confirmAction?.Disable();
    }

    private void OnDestroy()
    {
        pauseAction?.Dispose();
        confirmAction?.Dispose();

        if (instance == this)
        {
            instance = null;
        }
    }

    private void Update()
    {
        if (State == GameState.Playing && pauseAction.WasPressedThisFrame())
        {
            PauseGame();
        }
        else if (State == GameState.Paused && (pauseAction.WasPressedThisFrame() || IsRestartPressed()))
        {
            ResumeGame();
        }
        else if (State == GameState.GameOver && IsRestartPressed())
        {
            Restart();
        }
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void AddScore()
    {
        if (State != GameState.Playing)
        {
            return;
        }

        score++;
        textScore.text = score.ToString();
    }

    public void GameStart()
    {
        if (State != GameState.Waiting)
        {
            return;
        }

        SetState(GameState.Playing);
        PointerReleaseRequired = true;
        gameStartPanel.SetActive(false);
        textScore.gameObject.SetActive(true);
        rightTab.SetActive(false);
        leftTab.SetActive(false);
        back.SetActive(true);
    }

    public void EndGame()
    {
        if (State != GameState.Playing)
        {
            return;
        }

        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt(BestScoreKey, bestScore);
            PlayerPrefs.Save();
        }

        SetState(GameState.GameOver);
        PointerReleaseRequired = true;
        textScore.gameObject.SetActive(false);
        gameStartPanel.SetActive(true);
        back.SetActive(true);

        if (startMessage != null)
        {
            startMessage.text = $"GAME OVER\n{score}   BEST {bestScore}\nTAP / R TO RESTART";
        }
    }

    public void PauseGame()
    {
        if (State != GameState.Playing)
        {
            return;
        }

        SetState(GameState.Paused);
        Time.timeScale = 0f;
        gameStartPanel.SetActive(true);

        if (startMessage != null)
        {
            startMessage.text = "PAUSED\nTAP OR PRESS ESC / P / START";
        }
    }

    public void ResumeGame()
    {
        if (State != GameState.Paused)
        {
            return;
        }

        Time.timeScale = 1f;
        gameStartPanel.SetActive(false);
        SetState(GameState.Playing);
    }

    public void NotifyPointersReleased()
    {
        PointerReleaseRequired = false;
    }

    public void Back()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(MainMenuSceneName);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus && State == GameState.Playing)
        {
            PauseGame();
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus && State == GameState.Playing)
        {
            PauseGame();
        }
    }

    private void SetState(GameState newState)
    {
        State = newState;
        StateChanged?.Invoke(newState);
    }

    private bool IsRestartPressed()
    {
        if (confirmAction.WasPressedThisFrame())
        {
            return true;
        }

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 position = Mouse.current.position.ReadValue();
            return !IsPointerOverUi(position, -1);
        }

        if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            int touchId = Touchscreen.current.primaryTouch.touchId.ReadValue();
            Vector2 position = Touchscreen.current.primaryTouch.position.ReadValue();
            return !IsPointerOverUi(position, touchId);
        }

        return false;
    }

    private bool IsPointerOverUi(Vector2 screenPosition, int pointerId)
    {
        EventSystem currentEventSystem = EventSystem.current;
        if (currentEventSystem == null)
        {
            return false;
        }

        if (cachedEventSystem != currentEventSystem || pointerEventData == null)
        {
            cachedEventSystem = currentEventSystem;
            pointerEventData = new PointerEventData(currentEventSystem);
        }

        pointerEventData.Reset();
        pointerEventData.pointerId = pointerId;
        pointerEventData.position = screenPosition;
        uiRaycastResults.Clear();
        currentEventSystem.RaycastAll(pointerEventData, uiRaycastResults);
        return uiRaycastResults.Count > 0;
    }
}
