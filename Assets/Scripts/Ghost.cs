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

// ResetState is used to reset the state of the ghost
    public void ResetState(){
        IsResetting = true;
        this.gameObject.SetActive(true);
        this.movement.ResetState();
        SetSpriteState(SpriteState.Normal);
        this.frightened.Disable();
        this.chase.Disable();
        this.scatter.Disable();
        this.home.Disable();
        if (this.initialBehavior != null)
            this.initialBehavior.Enable();
        IsResetting = false;
    }

    public enum SpriteState { Normal, Frightened, Dead }

    public void SetSpriteState(SpriteState state){
        if (normalSprite != null) normalSprite.SetActive(state == SpriteState.Normal);
        if (frightenedSprite != null) frightenedSprite.SetActive(state == SpriteState.Frightened);
        if (deadSprite != null) deadSprite.SetActive(state == SpriteState.Dead);
    }

    public void EatenByPlayer(){
        StopAllCoroutines();
        frightened.Disable();
        chase.Disable();
        scatter.Disable();
        SetSpriteState(SpriteState.Dead);
        movement.enabled = false;
        Vector3 housePosition = home != null ? home.mazeStartPosition : transform.position;
        transform.position = housePosition;
        StartCoroutine(RespawnAfterEaten());
    }

    private IEnumerator RespawnAfterEaten(){
        yield return new WaitForSeconds(3f);
        SetSpriteState(SpriteState.Normal);
        movement.enabled = true;
        scatter.Enable();
    }
    // Only handle "ghost caught player" — "player ate ghost" is handled in GhostFrightened when that behavior is enabled
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsPlayer(collision.gameObject) && !frightened.enabled)
            FindObjectOfType<GameManager>().PacmanEaten();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsPlayer(other.gameObject) && !frightened.enabled)
            FindObjectOfType<GameManager>().PacmanEaten();
    }

    internal static bool IsPlayer(GameObject go)
    {
        if (go == null) return false;
        return go.layer == LayerMask.NameToLayer("Player") || go.GetComponent<Pacman>() != null;
    }
}

