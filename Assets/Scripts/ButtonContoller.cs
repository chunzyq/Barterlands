using UnityEngine.UIElements;
using UnityEngine;

public class ButtonContoller : MonoBehaviour
{

    private VisualElement settingsMenu;
    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        Button startButton = root.Q<Button>("StartButton");
        Button settingsButton = root.Q<Button>("SettingsButton");
        Button exitButton = root.Q<Button>("ExitButton");
        Button doSmth = root.Q<Button>("MakeSmth");
        Button goBack = root.Q<Button>("GoBack");
        settingsMenu = root.Q<VisualElement>("SettingsMenu");

        startButton.RegisterCallback<ClickEvent>(OnStartButtonClicked);
        settingsButton.RegisterCallback<ClickEvent>(OnSettingsButtonClicked);
        exitButton.RegisterCallback<ClickEvent>(OnExitButtonClicked);
        doSmth.RegisterCallback<ClickEvent>(OnDoSmthButtonClicked);
        goBack.RegisterCallback<ClickEvent>(OnGoBackButtonClicked);

        settingsMenu.style.display = DisplayStyle.None;
        Time.timeScale = 0f;
    }

    private void OnStartButtonClicked(ClickEvent clickEvent)
    {
        Debug.Log("Play the game!");
        
    }
    private void OnSettingsButtonClicked(ClickEvent clickEvent)
    {
        settingsMenu.style.display = DisplayStyle.Flex;
        Debug.Log("Open the settings!");
    }
    private void OnExitButtonClicked(ClickEvent clickEvent)
    {
        Debug.Log("Exit the game!");
    }
    private void OnGoBackButtonClicked(ClickEvent clickEvent)
    {
        settingsMenu.style.display = DisplayStyle.None;
    }
    private void OnDoSmthButtonClicked(ClickEvent clickEvent)
    {
        Debug.Log("Congratulations! You're doing something!");
    }
}
