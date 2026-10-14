
using UnityEngine;
using UnityEngine.SceneManagement;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    [Header("UI Panels")]
    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject gameOverScreen;

    public bool IsGameStarted { get; private set; }

    private static bool startGameDirectly;

    private void Awake()
    {
        Instance = this;
        IsGameStarted = false;
    }

    private void Start()
    {
        gameOverScreen.SetActive(false);

        if (startGameDirectly)
        {
            startGameDirectly = false;

            homeScreen.SetActive(false);
            IsGameStarted = true;
            Time.timeScale = 1f;

            InputManager inputManager = FindFirstObjectByType<InputManager>();


            if (inputManager != null)
            {
                inputManager.UnlockInput();
            }
        }
        else
        {
            homeScreen.SetActive(true);
            IsGameStarted = false;
            Time.timeScale = 0f;
        }
    }

   
    public void PlayGame()
    {
        StartFreshGame();
    }

    
    public void Retry()
    {
        StartFreshGame();
    }

    private void StartFreshGame()
    {
        startGameDirectly = true;
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);


    }

    
    public void GoToMenu()
    {
        gameOverScreen.SetActive(false);
        homeScreen.SetActive(true);

        IsGameStarted = false;
        startGameDirectly = false;
        Time.timeScale = 0f;
    }

    
    public void ShowGameOver()
    {
        IsGameStarted = false;
        gameOverScreen.SetActive(true);

        Time.timeScale = 0f;
    }
}
