using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapSceneLoader : MonoBehaviour
{
    [Header("Название сцены")]
    private string _mapSceneName = "MapScene";
    private string _mainSceneName = "MainGameScene";

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M) && MenuController.Instance.isPaused == false) // todo save between scenes
        {
            string current = SceneManager.GetActiveScene().name;
            string next = current == _mapSceneName ? _mainSceneName : _mapSceneName;
            SceneManager.LoadScene(next);
        }
    }
}
