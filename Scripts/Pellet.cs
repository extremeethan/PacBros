using UnityEngine;

public class Pellet : MonoBehaviour
{
    //get Player
    private Player player;
    //get Collider2D
    private Collider2D col;
    //get Rigidbody2D
    private Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = FindFirstObjectByType<Player>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        // If the player enters the trigger, destroy the pellet
        if(col.IsTouching(player.GetComponent<Collider2D>()))
        {
            Destroy(gameObject);
        }
    }
}
