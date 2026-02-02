using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public int points = 200;
    [SerializeField] private float speed = 2f;
    private Player player;
    private Vector2 movement;
    void Start(){
        player = FindFirstObjectByType<Player>();
    }
    void Update(){
        if(player != null){
            // calculate the direction to the player
            Vector2 direction = (player.transform.position - transform.position).normalized;
            // move towards the player 
            movement = direction;
            // move the enemy
            transform.position += (Vector3)movement * speed * Time.deltaTime;
    }
}
}
