using UnityEngine.UIElements;
using UnityEngine;
using System.ComponentModel.Design.Serialization;

public class ButtonContoller : MonoBehaviour
{

    private VisualElement settingsMenu;
    private VisualElement mainMenu;
    private VisualElement escapeMenu;
    private bool isGamePlaying = false;
    private void OnEnable()
    {

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button startButton = root.Q<Button>("StartButton");
        Button settingsButton = root.Q<Button>("SettingsButton");
        Button exitButton = root.Q<Button>("ExitButton");
        Button doSmth = root.Q<Button>("MakeSmth");
        Button goBack = root.Q<Button>("GoBack");
        Button continueButton = root.Q<Button>("continueButton");
        Button backToMainMenu = root.Q<Button>("backToMainMenu");

        mainMenu = root.Q<VisualElement>("MainMenu");
        settingsMenu = root.Q<VisualElement>("SettingsMenu");
        escapeMenu = root.Q<VisualElement>("EscapeMenu");


        startButton.RegisterCallback<ClickEvent>(OnStartButtonClicked);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClicked);
        exitButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);
        doSmth.RegisterCallback<ClickEvent>(OnDoSmthButtonClicked);
        goBack.RegisterCallback<ClickEvent>(OnGoBackButtonClicked);
        continueButton.RegisterCallback<ClickEvent>(OnContinueButtonClicked);
        backToMainMenu.RegisterCallback<ClickEvent>(OnBackToMainMenuClicked);

        settingsMenu.style.display = DisplayStyle.None;
        escapeMenu.style.display = DisplayStyle.None;
        Time.timeScale = 0f;
        isGamePlaying = false;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isGamePlaying) // доделать чтобы escape не открывался в settings + mainMenu
        {
            OpenMainMenu();
        }
    }

    private void OnStartButtonClicked(ClickEvent clickEvent)
    {
        Debug.Log("Play the game!");
        mainMenu.style.display = DisplayStyle.None;
        Time.timeScale = 1.5f;
        isGamePlaying = true;
        
    }
    private void OnSettingsButtonClicked(ClickEvent clickEvent)
    {
        settingsMenu.style.display = DisplayStyle.Flex;
        Debug.Log("Open the settings!");
    }
    private void OnExitButtonClicked(ClickEvent clickEvent)
    {
        Debug.Log("Exit the game!");
        Application.Quit();
        UnityEditor.EditorApplication.isPlaying = false;
    }
    private void OnGoBackButtonClicked(ClickEvent clickEvent)
    {
        settingsMenu.style.display = DisplayStyle.None;
    }
    private void OnDoSmthButtonClicked(ClickEvent clickEvent)
    {
        Debug.Log("Congratulations! You're doing something!");
    }
    private void OnContinueButtonClicked(ClickEvent clickEvent)
    {
        escapeMenu.style.display = DisplayStyle.None;
        Time.timeScale = 1.5f;
        isGamePlaying = true;
    }
    private void OnBackToMainMenuClicked(ClickEvent clickEvent)
    {
        escapeMenu.style.display = DisplayStyle.None;
        mainMenu.style.display = DisplayStyle.Flex;
        Time.timeScale = 0f;
        isGamePlaying = false;
    }
    private void OpenMainMenu()
    {
        if (isGamePlaying)
        {
            Time.timeScale = 0f;
            escapeMenu.style.display = DisplayStyle.Flex;
            isGamePlaying = false;
        }
        else
        {
            Time.timeScale = 1.5f;
            escapeMenu.style.display = DisplayStyle.None;
            isGamePlaying = true;
        }
    }
}
