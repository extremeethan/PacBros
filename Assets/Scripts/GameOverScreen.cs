using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameOverScreen : MonoBehaviour
{
    public TMP_Text pointsText;
    public void Setup(int score)
    {
        gameObject.SetActive(true);
        Time.timeScale = 0f;
        pointsText.text = score.ToString() + "YOUR SCORE";
    }
}
