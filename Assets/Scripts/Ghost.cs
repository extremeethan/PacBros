using UnityEngine;
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
        // Get the initial behavior of the ghost
        this.initialBehavior = this.scatter;
    }
// Start is called before the first frame update
public void Start()
{
    ResetState();
}

// ResetState is used to reset the state of the ghost
    public void ResetState(){
        // Set the ghost to active
        this.gameObject.SetActive(true);
        // Reset the movement state of the ghost
        this.movement.ResetState();
        // Disable the frightened state of the ghost
        this.frightened.Disable();
        // Disable the chase state of the ghost
        this.chase.Disable();
        // Enable the scatter state of the ghost
        this.scatter.Enable();
        if(this.home != this.initialBehavior){
            // Disable the home state of the ghost
            this.home.Disable();
        }
        if(this.initialBehavior != null){
            // Enable the initial behavior of the ghost
            this.initialBehavior.Enable();
        }
    }
// OnCollisionEnter2D is called when the ghost collides with another object
    private void OnCollisionEnter2D(Collision2D collision){
        // If the ghost collides with the player
        if(collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if(this.frightened.enabled){
                // If the ghost is frightened, it is eaten by the player
                // The GameManager is found using the FindObjectOfType method
                // The GhostEaten method is called to add points to the score
                FindObjectOfType<GameManager>().GhostEaten(this);
            }
            else{
                // If the ghost is not frightened, the player is eaten by the ghost
                // The GameManager is found using the FindObjectOfType method
                // The PacmanEaten method is called to subtract a life from the player
                FindObjectOfType<GameManager>().PacmanEaten();
            }
        }
    }
}

