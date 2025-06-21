using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSceneLoadManager : MonoBehaviour
{

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Update()
    {
        if (MenuController.Instance.isPaused == false)
        {
            if (Input.GetKeyDown(KeyCode.M))
            {
                Scene currentScene = SceneManager.GetActiveScene();

                if (currentScene.name == "MainGameScene")
                {
                    SceneManager.LoadScene("MapScene");
                }
                else if (currentScene.name == "MapScene")
                {
                    SceneManager.LoadScene("MainGameScene");
                }
            }
        }
    }
}
