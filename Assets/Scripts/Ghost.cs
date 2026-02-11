using UnityEngine;
using System.Collections;

// The Ghost class is the base class for all ghost behaviors
// It is used to define the common properties and methods for all ghost behaviors
public class Ghost : MonoBehaviour
{
    public Movement movement { get; private set; }// The movement component of the ghost
    public GhostHome home { get; private set; }// The home component of the ghost
    public GhostScatter scatter { get; private set; }// The scatter behavior of the ghost
    public GhostChase chase { get; private set; }// The chase behavior of the ghost
    public GhostFrightened frightened { get; private set; } // The frightened behavior of the ghost
    public GhostBehavior initialBehavior; // The initial behavior of the ghost
    public int points = 200; // The points for the ghost
    public GameObject target; // The target to chase

    [Header("Sprite (child objects)")]
    public GameObject normalSprite;
    public GameObject frightenedSprite;
    public GameObject deadSprite;

    internal bool IsResetting { get; private set; }

// Awake is called when the script instance is being loaded this awake method is used to initialize the ghost
    private void Awake()
    {
        // Get the movement component of the ghost
        this.movement = GetComponent<Movement>();
        // Get the home component of the ghost
        this.home = GetComponent<GhostHome>();
        // Get the scatter component of the ghost
        this.scatter = GetComponent<GhostScatter>();
        // Get the chase component of the ghost
        this.chase = GetComponent<GhostChase>();
        // Get the frightened component of the ghost
        this.frightened = GetComponent<GhostFrightened>();
        // Default to scatter only if not set in Inspector (e.g. set to Ghost Home to start in home)
        if (this.initialBehavior == null)
            this.initialBehavior = this.scatter;
    }
// Start is called before the first frame update
public void Start()
{
    ResetState();
}

    private void Update()
    {
        // rotate the ghost based on the direction of the movement
        if (movement.enabled && movement.direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(movement.direction.y, movement.direction.x);
            // rotate the ghost based on the angle
            transform.rotation = Quaternion.AngleAxis(angle * Mathf.Rad2Deg, Vector3.forward);
        }
    }

// ResetState is used to reset the state of the ghost
    public void ResetState(){
        IsResetting = true; // set the is resetting flag to true
        this.gameObject.SetActive(true);
        this.movement.ResetState(); // reset the state of the movement
        this.movement.SetDirection(this.movement.initialDirection, true); // use initial direction at level/round start
        SetSpriteState(SpriteState.Normal); // set the sprite state to normal
        this.frightened.Disable(); // disable the frightened behavior
        this.chase.Disable(); // disable the chase behavior
        this.scatter.Disable(); // disable the scatter behavior
        this.home.Disable(); // disable the home behavior
        if (this.initialBehavior != null)
            this.initialBehavior.Enable(); // enable the initial behavior
        IsResetting = false; // set the is resetting flag to false
    }

    public enum SpriteState { Normal, Frightened, Dead }

    public void SetSpriteState(SpriteState state){
        if (normalSprite != null) normalSprite.SetActive(state == SpriteState.Normal); // set the normal sprite active if the state is normal
        if (frightenedSprite != null) frightenedSprite.SetActive(state == SpriteState.Frightened); // set the frightened sprite active if the state is frightened
        if (deadSprite != null) deadSprite.SetActive(state == SpriteState.Dead); // set the dead sprite active if the state is dead
    }

    public void EatenByPlayer(){
        if (!movement.enabled && deadSprite != null && deadSprite.activeSelf) return; // already eaten, avoid double-handling
        StopAllCoroutines(); // stop all coroutines 
        frightened.Disable();
        chase.Disable(); // disable the chase behavior
        scatter.Disable(); // disable the scatter behavior
        SetSpriteState(SpriteState.Dead); // set the sprite state to dead
        movement.enabled = false; // disable the movement
        Vector3 housePosition = home != null ? home.mazeStartPosition : transform.position; // get the house position
        housePosition.z = -1f; // set the z position to -1
        transform.position = housePosition; // set the position to the house position
        movement.rigidbody.position = new Vector2(housePosition.x, housePosition.y); // set the rigidbody position to the house position
        StartCoroutine(RespawnAfterEaten()); // start the respawn after eaten coroutine
    }

    private IEnumerator RespawnAfterEaten(){
        yield return new WaitForSeconds(3f);
        SetSpriteState(SpriteState.Normal); // set the sprite state to normal
        movement.rigidbody.position = new Vector2(transform.position.x, transform.position.y);
        movement.SetDirection(movement.initialDirection, true); // set the direction to the initial direction
        movement.enabled = true; // enable the movement
        scatter.Enable(); // enable the scatter behavior
    }
    // Only handle "ghost caught player" — "player ate ghost" is handled in GhostFrightened when that behavior is enabled
    // Don't call PacmanEaten if we're already in eaten state (GhostFrightened may have run first and disabled itself)
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsPlayer(collision.gameObject) && !frightened.enabled && !IsInEatenState())
            FindFirstObjectByType<GameManager>().PacmanEaten();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayer(other.gameObject) && !frightened.enabled && !IsInEatenState())
            FindFirstObjectByType<GameManager>().PacmanEaten();
    }

    private bool IsInEatenState()
    {
        return (deadSprite != null && deadSprite.activeSelf) || !movement.enabled;
    }

    internal static bool IsPlayer(GameObject go)
    {
        if (go == null) return false; // if the game object is null return false
        return go.layer == LayerMask.NameToLayer("Player") || go.GetComponent<Pacman>() != null; // if the game object is a player or the game object has a pacman component return true
    }
}

