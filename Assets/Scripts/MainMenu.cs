using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Audio
    public AudioSource audioSource;
    public AudioClip menuButtonSound;

    public void Playgame()
    {
        // play button sound
        if (audioSource != null && menuButtonSound != null)
        {
            audioSource.PlayOneShot(menuButtonSound);
        }

        SceneManager.LoadSceneAsync("Level_01");
    }

    public void Credits()
    {
        // play button sound
        if (audioSource != null && menuButtonSound != null)
        {
            audioSource.PlayOneShot(menuButtonSound);
        }

        SceneManager.LoadSceneAsync("Credits");
    }

    public void QuitGame()
    {
        // play button sound
        if (audioSource != null && menuButtonSound != null)
        {
            audioSource.PlayOneShot(menuButtonSound);
        }

        Application.Quit();
    }
}

