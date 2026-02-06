using UnityEngine;
using UnityEngine.SceneManagement;

public class backbuttonscript : MonoBehaviour
{
    public void Back_button()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }
}
