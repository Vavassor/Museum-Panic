using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    private static Game instance = null;

    public static Game Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }
    private GameState gameState;
    private GameObject retryMenu;

    public GameState GetState()
    {
        return gameState;
    }

    public void SetState(GameState gameState)
    {
        this.gameState = gameState;
        switch (gameState)
        {
            case GameState.RetryMenu:
            {
                Cursor.lockState = CursorLockMode.None;
                retryMenu.SetActive(true);
                break;
            }
            case GameState.Playing:
            {
                SetPlaying();
                break;
            }
        }
    }

    void Awake()
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

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainGame")
        {
            SetPlaying();
        }
    }

    void Start()
    {
        SetPlaying();
    }

    void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }

    private void SetPlaying()
    {
        gameState = GameState.Playing;
        if (!retryMenu)
        {
            retryMenu = GameObject.FindWithTag("RetryMenu");
            if (retryMenu)
            {
                retryMenu.SetActive(false);
            }
        }
    }
}
