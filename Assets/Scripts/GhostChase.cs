using UnityEngine;

public class GhostChase : GhostBehavior
{
private void OnDisable(){
        // Check if ghost and scatter are initialized before accessing them
        // This prevents NullReferenceException during initialization
        if(this.ghost != null && this.ghost.scatter != null){
            this.ghost.scatter.Enable();
        }
    }
    private void OnTriggerEnter2D(Collider2D other){
        Node node = other.GetComponent<Node>();
        // If the node is not null, the ghost is not enabled, and the ghost is not frightened
        if(node != null && this.enabled && !this.ghost.frightened.enabled){
            // Set the next direction to the target position through the shortest path
            Vector2 direction = Vector2.zero; // The direction to the target position
            float minDistance = float.MaxValue; // The minimum distance to the target position

            foreach(Vector2 availableDirection in node.availableDirections){
                // Get the new position by adding the available direction to the current position
                Vector3 newPosition = this.transform.position + new Vector3(availableDirection.x, availableDirection.y, 0.0f);
                float distance = (this.ghost.target.transform.position - newPosition).sqrMagnitude; // The distance to the target position 
                if(distance < minDistance){
                    direction = availableDirection;
                    minDistance = distance;
                }
            }
            // Set the next direction to the direction to the target position
            this.ghost.movement.SetDirection(direction);

        }
    
    }
}
