using UnityEngine;

// Ghost starts in home; when they exit, teleport to maze start and switch to scatter
public class GhostHome : GhostBehavior
{
  [Header("Maze Start (teleport from home)")]
  public Vector3 mazeStartPosition = new Vector3(0f, 0f, 0f);

  private void OnDisable()
  {
    if (ghost == null || ghost.scatter == null)
      return;
    if (ghost.IsResetting)
      return;
    ghost.transform.position = mazeStartPosition;
    ghost.scatter.Enable();
  }
}
