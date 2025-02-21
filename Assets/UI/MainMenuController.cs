using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuController : MonoBehaviour
{
    public UIDocument uiDocument;
    private VisualElement rootElement;

    void Start() {

        rootElement = uiDocument.rootVisualElement;

        var startButton = rootElement.Q<Button>("startButton");
        var settingsButton = rootElement.Q<Button>("settingsButton");
        var exitButton = rootElement.Q<Button>("exitButton");

        startButton.clicked += StartGame;
        settingsButton.clicked += OpenSettings;
        exitButton.clicked += ExitGame;
    }

    void StartGame()
    {
        Debug.Log("Game Started!");
    }
    void OpenSettings()
    {
        Debug.Log("Настройки открыты!");
    }
    void ExitGame()
    {
        Debug.Log("Выход из игры!");
        Application.Quit();
    }
}
