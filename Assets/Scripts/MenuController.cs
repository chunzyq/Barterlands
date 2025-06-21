using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public static MenuController Instance;

    [Header("Меню")]
    [SerializeField] private GameObject pauseMenu = null;
    [SerializeField] private GameObject mainMenu = null;
    public Canvas pauseMenuCanvas;
    public bool isPaused = false;
    public AudioClip clickSound;
    private AudioSource audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenuScene")
        {

            if (mainMenu != null) mainMenu.SetActive(true);
            if (pauseMenu != null) pauseMenu.SetActive(false);

            if (isPaused)
            {
                ResumeGame();
            }
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
        audioSource.PlayOneShot(clickSound);
    }
    public void OnQuitButtoncClicked()
    {
        audioSource.PlayOneShot(clickSound);
    }
    public void OnSettingsButtonClicked()
    {
        audioSource.PlayOneShot(clickSound);
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

    }
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;

    }
    public void OnPauseQuitBtnClicked()
    {
        ResumeGame();
        SceneManager.LoadScene("MainMenuScene");
    }
}
