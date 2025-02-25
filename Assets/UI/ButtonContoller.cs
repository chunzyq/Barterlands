using UnityEngine.UIElements;
using UnityEngine;

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

    private void OnEnable()
    {

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        _startButton = root.Q<Button>("StartButton");
        _settingsButton = root.Q<Button>("SettingsButton");
        _exitButton = root.Q<Button>("ExitButton");
        _doSomethingButton = root.Q<Button>("MakeSmth");
        _goBackButton = root.Q<Button>("GoBack");
        _continueButton = root.Q<Button>("continueButton");
        _backToMainMenuButton = root.Q<Button>("backToMainMenu");

        _mainMenu = root.Q<VisualElement>("MainMenu");
        _settingsMenu = root.Q<VisualElement>("SettingsMenu");
        _escapeMenu = root.Q<VisualElement>("EscapeMenu");

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
        
    }

    private void OnSettingsButtonClicked(ClickEvent clickEvent)
    {
        _settingsMenu.style.display = DisplayStyle.Flex;
    }

    private void OnExitButtonClicked(ClickEvent clickEvent)
    {
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }

    private void OnGoBackButtonClicked(ClickEvent clickEvent)
    {
        _settingsMenu.style.display = DisplayStyle.None;
    }

    private void OnDoSmthButtonClicked(ClickEvent clickEvent)
    {
        Debug.Log("Congratulations! You're doing something!");
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
}
