using UnityEngine;
using UnityEngine.InputSystem;

public class Pacman : MonoBehaviour
{
    public Movement movement { get; private set; }
    
    // Get the movement component
    private void Awake(){
        this.movement = GetComponent<Movement>();
    }
    // Update is called once per frame
    private void Update(){
        // Set the direction of the movement based on the input
        if (Keyboard.current == null) return;
        
        if(Keyboard.current[Key.W].wasPressedThisFrame || Keyboard.current[Key.UpArrow].wasPressedThisFrame){
        this.movement.SetDirection(Vector2.up);
    }
    else if(Keyboard.current[Key.S].wasPressedThisFrame || Keyboard.current[Key.DownArrow].wasPressedThisFrame){
        this.movement.SetDirection(Vector2.down);
    }
    else if(Keyboard.current[Key.A].wasPressedThisFrame || Keyboard.current[Key.LeftArrow].wasPressedThisFrame){
        this.movement.SetDirection(Vector2.left);
    }
    else if(Keyboard.current[Key.D].wasPressedThisFrame || Keyboard.current[Key.RightArrow].wasPressedThisFrame){
        this.movement.SetDirection(Vector2.right);
    }
    // Rotate the pacman based on the direction using the angle and the vector3 forward
    // Calculate the angle in radians
    float angle = Mathf.Atan2(this.movement.direction.y, this.movement.direction.x);
    // Rotate the pacman based on the angle Radians to Degrees using the angle and the vector3 forward
    this.transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
    }
    public void ResetState(){ // this is the reset state method
        this.gameObject.SetActive(true); // set the game object to active
        this.movement.ResetState(); // reset the state of the movement
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ghost g = other.GetComponent<Ghost>();
        if (g != null && g.frightened != null && g.frightened.enabled)
            FindFirstObjectByType<GameManager>().GhostEaten(g);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Ghost g = collision.gameObject.GetComponent<Ghost>();
        if (g != null && g.frightened != null && g.frightened.enabled)
            FindFirstObjectByType<GameManager>().GhostEaten(g);
    }
}
