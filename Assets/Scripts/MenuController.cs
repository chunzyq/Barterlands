using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void OnStartGameButtonClicked()
    {
        SceneManager.LoadScene("MainGameScene");
    }
    public void OnQuitButtoncClicked()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
