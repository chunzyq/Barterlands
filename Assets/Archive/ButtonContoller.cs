using UnityEngine.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class ButtonContoller : MonoBehaviour
{
    // UI Elements
    private VisualElement _mainMenu;
    private VisualElement _settingsMenu;
    private VisualElement _escapeMenu;

    // Buttons
    private Button _startButton;
    private Button _settingsButton;
    private Button _exitButton;
    private Button _doSomethingButton; // add logic in future
    private Button _goBackButton;
    private Button _continueButton;
    private Button _backToMainMenuButton;

    // Game state
    private bool _isGamePlaying;

    [SerializeField] private VisualTreeAsset mainMenuUI;
    [SerializeField] private VisualTreeAsset settingsMenuUI;
    [SerializeField] private VisualTreeAsset escapeMenuUI;
    
    private VisualElement _currentUI;

    private void OnEnable()
    {

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        UIDocument mainMenuDoc = GameObject.Find("MainMenuUI").GetComponent<UIDocument>();
        UIDocument settingsMenuDoc = GameObject.Find("SettingsUI").GetComponent<UIDocument>();
        UIDocument pauseMenuDoc = GameObject.Find("PauseUI").GetComponent<UIDocument>();

        _mainMenu = mainMenuDoc.rootVisualElement;
        _settingsMenu = settingsMenuDoc.rootVisualElement;
        _escapeMenu = pauseMenuDoc.rootVisualElement;

        _startButton = _mainMenu.Q<Button>("StartButton");
        _settingsButton = _mainMenu.Q<Button>("SettingsButton");
        _exitButton = _mainMenu.Q<Button>("ExitButton");
        _doSomethingButton = _settingsMenu.Q<Button>("MakeSmth");
        _goBackButton = _settingsMenu.Q<Button>("GoBack");
        _continueButton = _escapeMenu.Q<Button>("continueButton");
        _backToMainMenuButton = _escapeMenu.Q<Button>("backToMainMenu");

        // _mainMenu = root.Q<VisualElement>("MainMenu");
        // _settingsMenu = root.Q<VisualElement>("SettingsMenu");
        // _escapeMenu = root.Q<VisualElement>("EscapeMenu");

        _settingsMenu.style.display = DisplayStyle.None;
        _escapeMenu.style.display = DisplayStyle.None;
        Time.timeScale = 0f;
        _isGamePlaying = false;


        _startButton.RegisterCallback<ClickEvent>(OnStartButtonClicked);
        _settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClicked);
        _exitButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);
        _doSomethingButton.RegisterCallback<ClickEvent>(OnDoSmthButtonClicked);
        _goBackButton.RegisterCallback<ClickEvent>(OnGoBackButtonClicked);
        _continueButton.RegisterCallback<ClickEvent>(OnContinueButtonClicked);
        _backToMainMenuButton.RegisterCallback<ClickEvent>(OnBackToMainMenuClicked);

        ShowMainMenu();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && _isGamePlaying)
        {
            OpenMainMenu();
        }
    }

    private void OnStartButtonClicked(ClickEvent clickEvent)
    {
        _mainMenu.style.display = DisplayStyle.None;
        Time.timeScale = 1.5f;
        _isGamePlaying = true;

        SceneManager.LoadScene("GameScene");
        
    }

    private void OnSettingsButtonClicked(ClickEvent clickEvent)
    {
        // _mainMenu.style.display = DisplayStyle.None;
        // _escapeMenu.style.display = DisplayStyle.None;
        // _settingsMenu.style.display = DisplayStyle.Flex;
        ShowSettingsMenu();
    }

    private void OnExitButtonClicked(ClickEvent clickEvent)
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    private void OnGoBackButtonClicked(ClickEvent clickEvent)
    {
        _settingsMenu.style.display = DisplayStyle.None;
        _mainMenu.style.display = DisplayStyle.Flex;

        ShowMainMenu();
    }

    private void OnDoSmthButtonClicked(ClickEvent clickEvent)
    {
        // UnityEngine.Debug.Log("Congratulations! You're doing something!");
    }

    private void OnContinueButtonClicked(ClickEvent clickEvent)
    {
        _escapeMenu.style.display = DisplayStyle.None;
        Time.timeScale = 1.5f;
        _isGamePlaying = true;
    }

    private void OnBackToMainMenuClicked(ClickEvent clickEvent)
    {
        _escapeMenu.style.display = DisplayStyle.None;
        _mainMenu.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f;
        _isGamePlaying = false;
        ShowMainMenu();
    }

    private void OpenMainMenu()
    {
        if (_isGamePlaying)
        {
            Time.timeScale = 0f;
            _escapeMenu.style.display = DisplayStyle.Flex;
            _isGamePlaying = false;
        }
        else
        {
            Time.timeScale = 1.5f;
            _escapeMenu.style.display = DisplayStyle.None;
            _isGamePlaying = true;
        }
    }
    private void ShowMainMenu()
    {
        SwitchUI(mainMenuUI);
    }

    // Show the settings menu and switch UI if needed
    private void ShowSettingsMenu()
    {
        SwitchUI(settingsMenuUI);
    }

    // Switch between different UI screens dynamically
    private void SwitchUI(VisualTreeAsset newUI)
    {
        if (_currentUI != null)
        {
            _currentUI.RemoveFromHierarchy();
        }

        _currentUI = newUI.CloneTree();
        GetComponent<UIDocument>().rootVisualElement.Add(_currentUI);
    }
}