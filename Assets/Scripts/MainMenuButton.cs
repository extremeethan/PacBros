using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuButton : MonoBehaviour
{

    public void MMButton()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }

}
