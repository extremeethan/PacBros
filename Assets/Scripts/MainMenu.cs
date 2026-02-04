using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void Playgame()
    {
        SceneManager.LoadSceneAsync("Level_01");
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
