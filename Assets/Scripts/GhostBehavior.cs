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
    public float duration;

    private void Awake()
    {
        // Get the Ghost component attached to this GameObject
        this.ghost = this.GetComponent<Ghost>();
        // Disable the behavior by default
        this.enabled = false;
    }

    public void Enable()
    {
    // Enable the behavior for the duration of the behavior
      Enable(this.duration);
    }
    public virtual void Enable(float duration)
    {
        // Enable the behavior
        this.enabled = true;
        // Invoke the Disable method after the duration of the behavior
        Invoke(nameof(Disable), duration);
    }
// Disable the behavior
    public virtual void Disable()
    {
    this.enabled = false;
    }
}
