using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[InitializeOnLoad]
public static class StartSceneManager
{
    // Укажите путь к сцене, которую хотите запускать при нажатии Play
    private const string startScenePath = "Assets/Scenes/MainMenuScene.unity";

    // Храним путь к сцене, которая была открыта до нажатия Play,
    // чтобы вернуться к ней, когда выйдем из режима Play.
    private static string previousScenePath;

    static StartSceneManager()
    {
        // Подписываемся на событие изменения состояния PlayMode
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        switch (state)
        {
            case PlayModeStateChange.ExitingEditMode:
                // Перед тем, как Unity войдёт в PlayMode, запоминаем текущую сцену
                previousScenePath = EditorSceneManager.GetActiveScene().path;

                // Открываем желаемую "стартовую" сцену
                if (!string.IsNullOrEmpty(startScenePath))
                {
                    EditorSceneManager.OpenScene(startScenePath);
                }
                break;

            case PlayModeStateChange.EnteredEditMode:
                // Когда возвращаемся из PlayMode в EditMode, восстанавливаем предыдущую сцену
                if (!string.IsNullOrEmpty(previousScenePath))
                {
                    EditorSceneManager.OpenScene(previousScenePath);
                }
                break;
        }
    }
}
