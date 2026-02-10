using UnityEngine;
using UnityEngine.SceneManagement;

public class backbuttonscript : MonoBehaviour
{
    // Audio
    public AudioSource audioSource;
    public AudioClip menuButtonSound;

    public void Back_button()
    {
        // Play button sound
        if (audioSource != null && menuButtonSound != null)
        {
            audioSource.PlayOneShot(menuButtonSound);
        }

        SceneManager.LoadSceneAsync("Main Menu");
    }
}
