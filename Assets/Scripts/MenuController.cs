using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu = null;
    public void OnStartGameButtonClicked()
    {
        SceneManager.LoadScene("MainGameScene");
    }
    public void OnQuitButtoncClicked()
    {
        UnityEditor.EditorApplication.isPlaying = false;
    }
}
