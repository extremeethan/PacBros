using UnityEngine;
using TMPro; // this is the text mesh pro library
// this script is used to show the score and lives on the screen 

// To get this to work make a canvas with two text objects named score and lives and put this script on the canvas then drag each of those text objects into the scoreText and livesText variables in the inspector
public class ScoreAndLives : MonoBehaviour
{
   // this is the text for the score
   public TMP_Text scoreText;
   // this is the text for the lives
   public TMP_Text livesText;

   // this is the method to update the score and lives every frame
   private void Update()
   {
       // this is the method to check if the game manager is null
       if (GameManager.Instance == null) return;
       // this is the method to update the score
       if (scoreText != null) scoreText.text = "Score: " + GameManager.Instance.score;
       // this is the method to update the lives
       if (livesText != null) livesText.text = "Lives: " + GameManager.Instance.lives;
   }
}
