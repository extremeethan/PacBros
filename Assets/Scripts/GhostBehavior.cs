using UnityEngine;
[RequireComponent(typeof(Ghost))]
// The GhostBehavior class is the base class for all ghost behaviors
// It is used to define the common properties and methods for all ghost behaviors
// It is also used to define the different ghost behaviors
public class GhostBehavior : MonoBehaviour
{

    // This is the base class for all ghost behaviors
    // It is used to define the common properties and methods for all ghost behaviors
    public Ghost ghost { get; private set; }
    // This is the duration of the behavior
    public float duration;

    private void Awake()
    {
        // Get the Ghost component attached to this GameObject
        this.ghost = this.GetComponent<Ghost>();
        // Disable the behavior by default
        this.enabled = false;
    }

    // This is the method to enable the behavior
    public void Enable()
    {
    // Enable the behavior for the duration of the behavior
      Enable(this.duration);
    }
    // This is the method to enable the behavior for a specific duration
    public virtual void Enable(float duration)
    {
        // Enable the behavior
        this.enabled = true;
        // Invoke the Disable method after the duration of the behavior
        Invoke(nameof(Disable), duration);
    }
    // This is the method to disable the behavior
    public virtual void Disable()
    {
    this.enabled = false;
    }
}
