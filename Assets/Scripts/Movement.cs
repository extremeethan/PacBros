using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Movement : MonoBehaviour
{
    public float speed = 8.0f; // The speed of the movement
    public float speedMultiplier = 1.0f; // The speed multiplier of the movement

    public Vector2 initialDirection; // The initial direction of the movement
    public LayerMask obstacleLayer; // The layer mask of the obstacles  

    public Vector2 direction { get; private set; } // The current direction of the movement
    public Vector2 nextDirection { get; private set; } // The next direction of the movement
    public Vector3 startingPosition { get; private set; } // The starting position of the movement
    public new Rigidbody2D rigidbody { get; private set; } // The rigidbody component
    // Awake is called when the script instance is being loaded
    private void Awake()
    {// Get the rigidbody component
        this.rigidbody = GetComponent<Rigidbody2D>();
        // Set the starting position to the current position
        this.startingPosition = this.transform.position;
    }
    // Reset the state of the movement
    private void Start()
    {
        ResetState();
    }

    // Update is called once per frame
    // Check if the next direction is not zero and set the direction to the next direction if it is not zero
    private void Update()
    {
        if(this.nextDirection != Vector2.zero){
            // Set the direction to the next direction if it is not zero
            SetDirection(this.nextDirection, true);
        }
    }
    // Reset the state of the movement
    public void ResetState()
    {
        // Set the speed multiplier to 1.0f
        this.speedMultiplier = 1.0f;
        // Set the direction to the initial direction
        this.direction = this.initialDirection;
        // Set the next direction to zero
        this.nextDirection = Vector2.zero;
        // Set the position to the starting position and keep rigidbody in sync
        this.transform.position = this.startingPosition;
        this.rigidbody.position = new Vector2(this.startingPosition.x, this.startingPosition.y);
        this.rigidbody.bodyType = RigidbodyType2D.Dynamic;
    }
    private void FixedUpdate()
    {
        // Get the current position
        Vector2 position = this.rigidbody.position;
        // Get the next position by multiplying the direction by the speed and the speed multiplier and the time fixed delta time
        Vector2 translation = this.direction * this.speed * this.speedMultiplier * Time.fixedDeltaTime;
        // Set the position to the next position
        this.rigidbody.MovePosition(position + translation);
    }
    // Set the direction of the movement, forced for ghost movement
    public void SetDirection(Vector2 direction, bool forced = false)
    {
        if( forced || !Occupied(direction)){
            // Set the direction to the new direction
            this.direction = direction;
            // Set the next direction to zero
            this.nextDirection = Vector2.zero;
        }
        else{
            // Set the next direction to the new direction
            this.nextDirection = direction;
        }
    }
    // Check if the path is occupied
    public bool Occupied(Vector2 direction)
    {
        // Cast a box cast to check if the path is occupied the position of the object, the size of the box, the angle of the box, the direction of the box, the distance of the box, and the layer mask of the obstacles
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.75f, 0.0f, direction, 1.5f, this.obstacleLayer);
        // Return true if the path is occupied, false otherwise
        return hit.collider != null;
    }
}
