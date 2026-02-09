// using = import a namespace. UnityEngine = Unity's core types (MonoBehaviour, Transform, Vector3, etc.)
using UnityEngine;
// System.Collections = contains IEnumerator, needed for coroutines (yield return)
using System.Collections;

// public class ClassName : BaseClass = define a class that inherits from GhostBehavior (which inherits MonoBehaviour)
public class GhostHome : GhostBehavior
{
  // public Type name; = field visible in Inspector, assignable in Editor. Transform = position/rotation/scale of a GameObject
  // "inside" = the point inside the ghost home (center of the pen)
  public Transform inside;
  // "outside" = the point just outside the home (where the ghost exits to the maze)
  public Transform outside;

  // private void MethodName() = Unity message, called automatically when this component is enabled (checkbox turned on)
  private void OnEnable(){
    // StopAllCoroutines() = cancels every coroutine started by this MonoBehaviour (inherited from MonoBehaviour)
    StopAllCoroutines();
  }

  // OnDisable() = Unity message, called when this component is disabled (checkbox turned off)
  private void OnDisable(){
    // StartCoroutine(IEnumerator) = starts a coroutine; the method runs over multiple frames and can yield. ExitTransition() returns an IEnumerator
    StartCoroutine(ExitTransition());
  }

  // private IEnumerator MethodName() = method that can be used as a coroutine; IEnumerator = interface that supports yield return
  private IEnumerator ExitTransition(){
    // yield return null; = pause this coroutine until next frame, then continue. Lets other scripts' Awake() run first
    yield return null;

    // this.ghost = reference (from base GhostBehavior) to the Ghost component on this GameObject
    // .movement = the Movement component on the Ghost. SetDirection(Vector2, bool) = set movement direction and optionally reset position
    // Vector2.up = (0, 1), i.e. upward
    this.ghost.movement.SetDirection(Vector2.up, true);
    // .rigidbody = the Rigidbody2D on the ghost. isKinematic = true means physics won't move it (we'll move transform ourselves)
    this.ghost.movement.rigidbody.isKinematic = true;
    // .enabled = false = turn off the Movement script so it doesn't override our manual position changes
    this.ghost.movement.enabled = false;

    // Vector3 = (x, y, z). this.transform = this GameObject's Transform. .position = world-space position
    Vector3 position = this.transform.position;

    // float = decimal number. 0.5f = 0.5 as a float (f suffix required in C#)
    float duration = 0.5f;
    // elapsed = how much time has passed in the current lerp (in seconds)
    float elapsed = 0.0f;
    // while (condition) { } = loop until condition is false. elapsed < duration = run for 0.5 seconds
    while(elapsed < duration){
      // Vector3.Lerp(a, b, t) = linear interpolation: a when t=0, b when t=1. t = elapsed/duration goes 0→1 over 0.5s
      // this.inside.position = world position of the "inside" Transform
      Vector3 newPosition = Vector3.Lerp(position, this.inside.position, elapsed / duration);
      // .z = keep the same depth so the ghost stays on the correct sorting layer
      newPosition.z = position.z;
      // Assign the new position to the ghost's transform (moves the ghost)
      this.ghost.transform.position = newPosition;
      // Time.deltaTime = seconds since last frame. elapsed += ... = add to elapsed each frame
      elapsed += Time.deltaTime;
      // yield return null; = wait one frame, then continue the while loop
      yield return null;
    }

    // Reset elapsed for the second part of the animation (inside → outside)
    elapsed = 0.0f;
    // Second while loop: animate from inside to outside over 0.5 seconds
    while(elapsed < duration){
      // Lerp from inside.position to outside.position; t = elapsed/duration (0 to 1)
      Vector3 newPosition = Vector3.Lerp(this.inside.position, this.outside.position, elapsed / duration);
      // Keep z the same
      newPosition.z = position.z;
      // Apply position to ghost
      this.ghost.transform.position = newPosition;
      elapsed += Time.deltaTime;
      yield return null;
    }
    // Random.value = random float 0.0 to 1.0. condition ? a : b = if true use a else b. So -1 or 1 for x, 0 for y (left or right)
    this.ghost.movement.SetDirection(new Vector2(Random.value < 0.5f ? -1.0f : 1.0f, 0.0f), true);
    // isKinematic = false = physics can move the ghost again
    this.ghost.movement.rigidbody.isKinematic = false;
    // Re-enable the Movement script so the ghost moves normally
    this.ghost.movement.enabled = true;

  }
}
