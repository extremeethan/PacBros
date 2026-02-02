using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Teleporter otherTeleporter; // The other teleporter
    private bool playerInside = false; // Whether the player is inside the teleporter
    private bool enemyInside = false; // Whether the enemy is inside the teleporter
    
  private void OnTriggerEnter2D(Collider2D other){ // When the player enters the teleporter
    // If the player is not inside the teleporter and the player is the player
    if(other.CompareTag("Player") && !playerInside || other.CompareTag("Enemy") && !enemyInside){
        // Get the player component
        Player player = other.GetComponent<Player>();
        Enemy enemy = other.GetComponent<Enemy>();
        // If the player is not null and the other teleporter is not null
        if(player != null && otherTeleporter != null || enemy != null && otherTeleporter != null){
            // Set the player or enemy inside the other teleporter
            playerInside = true;
            enemyInside = true;
            otherTeleporter.playerInside = true;
            otherTeleporter.enemyInside = true;
            // Teleport the player to the other teleporter
            other.transform.position = otherTeleporter.transform.position;
            enemy.transform.position = otherTeleporter.transform.position;
            }
        }
    }
    // When the player exits the teleporter
    private void OnTriggerExit2D(Collider2D other){ // When the player exits the teleporter
        // If the player is the player
        if(other.CompareTag("Player")){
            // Set the player or enemy inside the other teleporter
            playerInside = false;
            enemyInside = false;
        }
        if(other.CompareTag("Enemy")){
            // Set the enemy inside the other teleporter
            enemyInside = false;
            otherTeleporter.enemyInside = false;
        }
    }
  }

