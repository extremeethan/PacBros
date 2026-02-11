// Import Unity's core functionality for game objects, components, and MonoBehaviour
using UnityEngine;
// Import Unity's new Input System for handling keyboard and mouse input
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

// GameManager class that inherits from MonoBehaviour, allowing it to be attached to a GameObject
// This class manages the overall game state, score, lives, and game flow
public class GameManager : MonoBehaviour
{
    // Static property that provides a singleton instance of GameManager
    // 'get' is public (anyone can read it), 'set' is private (only this class can set it)
    // This ensures only one GameManager exists in the scene
    public static GameManager Instance { get; private set; }
    
    // Array of GameObject references to all ghost enemies in the game
    // Public so it can be assigned in the Unity Inspector
    public GameObject[] ghosts;
    
    // GameObject reference to the Pacman player character
    // Public so it can be assigned in the Unity Inspector
    public GameObject pacman;
    
    // Transform reference to the parent object containing all pellet GameObjects
    // Public so it can be assigned in the Unity Inspector
    public Transform pellets;

    // Audio
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip pelletSound;
    public AudioClip pacmanDeathSound;
    public AudioClip powerPelletSound;

    // Ghost multiplier that increases each time a ghost is eaten during power pellet mode
    // Starts at 1 and increases (1, 2, 3, 4...) for scoring multiplier effect
    // Public getter allows reading, private setter only allows internal modification
    public int ghostMultiplier {get; private set; } = 1;

    // Current game score - tracks points earned from eating pellets and ghosts
    // Public getter allows other scripts to read the score, private setter keeps it controlled
    public int score { get; private set; }
    
    // Current number of lives/remaining chances the player has
    // Public getter allows reading, private setter ensures lives are only changed through proper methods
    public int lives { get; private set; }

    bool _gameStarted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
        }
        else
        {
            // Copy this level's audio to the persistent Instance before we're destroyed
            if (pelletSound != null) Instance.pelletSound = pelletSound;
            if (pacmanDeathSound != null) Instance.pacmanDeathSound = pacmanDeathSound;
            if (powerPelletSound != null) Instance.powerPelletSound = powerPelletSound;
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main Menu" || scene.name == "GameOver" || scene.name == "YouWin")
        {
            _gameStarted = false;
            return;
        }
        if (!scene.name.Contains("Level")) return;

        StartCoroutine(BindLevelRefsNextFrame());
    }

    System.Collections.IEnumerator BindLevelRefsNextFrame()
    {
        yield return null; // Wait one frame so the scene is fully active
        var p = FindFirstObjectByType<Pacman>();
        pacman = p != null ? p.gameObject : null;
        var ghostList = FindObjectsByType<Ghost>(FindObjectsSortMode.None);
        if (ghostList != null && ghostList.Length > 0)
        {
            ghosts = new GameObject[ghostList.Length];
            for (int i = 0; i < ghostList.Length; i++)
                ghosts[i] = ghostList[i].gameObject;
        }
        var firstPellet = FindFirstObjectByType<Pellet>();
        pellets = firstPellet != null ? firstPellet.transform.parent : null;

        if (!_gameStarted)
        {
            _gameStarted = true;
            SetScore(0);
            SetLives(3);
        }
        NewRound();
    }

   // Start is called before the first frame update, after Awake()
   private void Start(){
    // Only run level setup when this scene has gameplay refs. Skip on Main Menu / Game Over / YouWin.
    if (pellets != null && pacman != null && ghosts != null && ghosts.Length > 0)
        NewGame();
   }
   
   // Update is called once per frame (typically 60 times per second)
   // This continuously checks for player input to restart the game when it's over
   private void Update()
   {
    // Check if any key on the keyboard was pressed this frame
    // First checks if Keyboard.current exists (keyboard is available)
    // Then checks if anyKey.wasPressedThisFrame (any key was just pressed)
    bool anyKeyPressed = (Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame);
    bool escKeyPressed = (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame);
    // If the escape key was pressed
    if(escKeyPressed){
        // Quit the game
        QuitGame();
    }
    // If the player has no lives left (game over) AND any key was pressed
    if(this.lives <= 0 && anyKeyPressed){
        // Restart the game by calling NewGame() to reset everything
        NewGame();
    }
   }
   private void QuitGame(){
    Debug.Log("Quitting game");
    // Quit the game
    Application.Quit();
   }

  // Initializes a new game from scratch - resets score, lives, and starts first round
  private void NewGame(){
    // Reset the score to 0 at the start of a new game
    SetScore(0);
    // Set the player's lives to 3 (standard Pacman starting lives)
    SetLives(3);
    // Start the first round by resetting pellets and game objects
    NewRound();
  }
  
  // Sets the score to a specific value
  // Private method ensures score can only be changed through controlled methods
  private void SetScore(int newScore){
    // Update the score property with the new value
    this.score = newScore;
  }
  
  // Sets the number of lives to a specific value
  // Private method ensures lives can only be changed through controlled methods
  private void SetLives(int newLives){
    // Update the lives property with the new value
    this.lives = newLives;
  }

  // Starts a new round - reactivates all pellets and resets game object positions
  private void NewRound(){
    if (this.pellets == null) return; // Not in a gameplay scene (e.g. Main Menu)
    foreach (Transform pellet in this.pellets){
        // Reactivate each pellet GameObject so it can be collected again
        // SetActive(true) makes the GameObject visible and interactive
        pellet.gameObject.SetActive(true);
    }
    ResetState();
}
  
  // Resets the game state for a new round - reactivates Pacman and ghosts, resets multiplier
  private void ResetState(){
    if (ghosts == null || pacman == null) return;
    ResetGhostMultiplier();
    
    for (int i = 0; i < this.ghosts.Length; i++)
    {
        if (this.ghosts[i] == null) continue;
        Ghost g = this.ghosts[i].GetComponent<Ghost>();
        if (g != null) g.ResetState();
    }
    Pacman p = this.pacman.GetComponent<Pacman>();
    if (p != null) p.ResetState();
 }
  
  // Called when the game is over (player has no lives left)
  // Deactivates all game objects to show the game over state
  private void GameOver()
  {
    if (ghosts != null)
    {
        Debug.Log("[GameManager] GameOver() - deactivating ghosts");
        for (int i = 0; i < this.ghosts.Length; i++)
        {
            if (this.ghosts[i] != null)
                this.ghosts[i].SetActive(false);
        }
    }
    if (pacman != null)
    {
        Debug.Log("[GameManager] GameOver() - deactivating Pacman");
        this.pacman.SetActive(false);
    }
    SceneManager.LoadScene("GameOver");
  }
  
  // Block "ghost eaten" for this long after a power pellet is eaten (stops ghosts dying with no contact)
  public static float PowerPelletEatenTime { get; private set; } = -999f;
  const float GhostEatBlockSeconds = 2f;

  public void GhostEaten(Ghost ghost){
    if (Time.time - PowerPelletEatenTime < GhostEatBlockSeconds)
      return;
    int points = ghost.points * this.ghostMultiplier;
    SetScore(this.score + points);
    this.ghostMultiplier++;
    ghost.EatenByPlayer();
}
 
 // Called when Pacman is eaten by a ghost
 // Handles life loss and either resets the round or ends the game
 public void PacmanEaten(){
    if (pacman == null) return;
    Debug.Log("PacmanEaten called");

    if (audioSource != null && pacmanDeathSound != null)
    {
        audioSource.PlayOneShot(pacmanDeathSound);
    }

        // Immediately hide Pacman when eaten
        // SetActive(false) makes Pacman invisible and stops movement
        if (this.pacman != null) this.pacman.SetActive(false);
    
    // Decrease the player's lives by 1
    // Uses current lives minus 1
    SetLives(this.lives -1);
    
    // Check if the player still has lives remaining
    if(this.lives > 0){
        // If lives remain, wait 3 seconds then reset the game state
        // Invoke schedules ResetState() to be called after 3 seconds
        // This gives a brief pause before respawning
        Invoke(nameof(ResetState), 3f);
    }
    // If no lives remain (lives <= 0)
    else{
        // Output a debug message to the console for troubleshooting
        Debug.Log("GameOver called");
        // Call GameOver() to deactivate everything and show game over state
        GameOver();
    }
}
 
 // Called when a regular pellet is eaten by Pacman
 // Handles pellet deactivation, scoring, and checks for round completion
 public void PelletEaten(Pellet pellet){
    if (pellet == null) return;
        // pellet sound
        if (audioSource != null && pelletSound != null)
        {
            audioSource.PlayOneShot(pelletSound);
        }
        // Deactivate the pellet GameObject so it disappears and can't be eaten again
        // SetActive(false) makes it invisible and disables its collider
        pellet.gameObject.SetActive(false);
    
    // Add the pellet's point value to the current score
    // Uses the current score plus the pellet's points property
    SetScore(this.score + pellet.points);
    
    // Check if there are any remaining pellets in the scene
    // Only proceed if pellets Transform exists AND no pellets remain
    if(this.pellets != null && !HasRemainingPellets()){
        // Output debug message when all pellets are collected
        Debug.Log("All pellets eaten - loading next level");
        
        if (this.pacman != null) this.pacman.SetActive(false);
        
        // Wait 3 seconds then load the next level (Level_02)
        Invoke(nameof(LoadNextLevel), 3f);
    }
}
 
 // Called when a power pellet is eaten by Pacman
 // Handles power pellet effects like making ghosts vulnerable
 public void PowerPelletEaten(PowerPellet powerPellet){
    if (powerPellet == null) return;

        if (audioSource != null && powerPelletSound != null)
        {
            audioSource.PlayOneShot(powerPelletSound);
        }
        // First handle it like a regular pellet (deactivate, add points, check for round completion)
        // PowerPellet inherits from Pellet, so this works correctly
        PelletEaten(powerPellet);
    
    // Schedule the ghost multiplier to reset after the power pellet duration expires
    // Invoke calls ResetGhostMultiplier() after powerPellet.duration seconds
    // This ensures the multiplier resets when the power pellet effect ends
    Invoke(nameof(ResetGhostMultiplier), powerPellet.duration);
    
    PowerPelletEatenTime = Time.time;
    if (ghosts == null || ghosts.Length == 0) return;
    for (int i = 0; i < ghosts.Length; i++)
    {
        if (ghosts[i] == null) continue;
        Ghost g = ghosts[i].GetComponent<Ghost>();
        if (g != null && g.chase != null && g.scatter != null && g.frightened != null)
        {
            g.chase.Disable();
            g.scatter.Disable();
            g.frightened.Enable(powerPellet.duration);
        }
    }
}

// Checks if there are any active pellets remaining in the scene
// Returns true if at least one pellet is still active, false if all are collected
private bool HasRemainingPellets(){
    // Find all Pellet components in the entire scene, regardless of hierarchy
    // FindObjectsByType searches all GameObjects for Pellet components
    // FindObjectsSortMode.None means don't sort the results (faster)
    Pellet[] allPellets = FindObjectsByType<Pellet>(FindObjectsSortMode.None);
    
    // Loop through each Pellet component found in the scene
    foreach (Pellet pellet in allPellets){
        // Check if this pellet exists (not null) AND is currently active
        // gameObject.activeSelf checks if the GameObject is active in the hierarchy
        if(pellet != null && pellet.gameObject.activeSelf){
            // Found at least one active pellet, so return true immediately
            // No need to check the rest - we know pellets remain
            return true; // Found an active pellet
        }
    }
    // If we've checked all pellets and none were active, return false
    // This means all pellets have been collected
    return false;
}

// Resets the ghost multiplier back to 1
// Called when power pellet effect ends or a new round starts
private void ResetGhostMultiplier(){
    // Set the multiplier back to its starting value of 1
    // This means the next ghost eaten will give base points (no multiplier)
    this.ghostMultiplier = 1;
}
// Loads the next level or YouWin when Level 2 is beaten
private void LoadNextLevel(){
    string currentScene = SceneManager.GetActiveScene().name;
    if (currentScene == "Level_02")
        SceneManager.LoadScene("YouWin");
    else
        SceneManager.LoadScene("Level_02");
}
}
