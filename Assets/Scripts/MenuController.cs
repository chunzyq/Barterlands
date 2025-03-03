using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private GameObject mainMenu = null;
    public Canvas pauseMenuCanvas;
    public bool isPaused = false;
    public Button[] buildingButton;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePauseMenu();
                mainMenu.SetActive(false);

            }
        }
    }
    public void OnStartGameButtonClicked()
    {
        SceneManager.LoadScene("MainGameScene");
    }
    public void OnQuitButtoncClicked()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
    public void TogglePauseMenu()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }
    public void PauseGame()
    {
        pauseMenuCanvas.sortingOrder = 10;

        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;

        foreach (Button button in buildingButton)
        {
            button.interactable = false;
        }
    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

        foreach (Button button in buildingButton)
        {
            button.interactable = true;
        }
    }
    public void OnPauseQuitBtnClicked()
    {
        SceneManager.LoadScene("MainMenuScene");
        ResumeGame();
    }
}
