using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    // Player Speed
    [SerializeField] private float speed = 5f;
    // Default Vector2 for movement
    private Vector2 movement;
    // Default Rigidbody2D for movement
    private Vector2 lastMovement;
    private Rigidbody2D rb;

    // Player Score
    [SerializeField] private int score = 0;
    // Player Lives
    [SerializeField] private int lives = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        lastMovement = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Get input using InputSystem
        movement = Vector2.zero;
        // If W or Up Arrow is pressed, move up
        if(Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed)
        {
            movement.y = 1;
        }
        // If S or Down Arrow is pressed, move down
        if(Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed)
        {
            movement.y = -1;
        }
        // If A or Left Arrow is pressed, move left
        if(Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed)
        {
            movement.x = -1;
        }
        // If D or Right Arrow is pressed, move right
        if(Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            movement.x = 1;
        }
       // Only update lastMovement when there is input
       if(movement != Vector2.zero)
       {
        lastMovement = movement;
       }
       // If there is no input, keep moving in the last movement direction
       else
       {
        movement = lastMovement;
       }
       //Move Player
       transform.position += (Vector3)movement * speed * Time.deltaTime;
    }

}
