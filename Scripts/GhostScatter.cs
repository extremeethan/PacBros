using UnityEngine;

public class GhostScatter : GhostBehavior
{
    private void OnDisable(){
        // Check if ghost and chase are initialized before accessing them
        // This prevents NullReferenceException during initialization
        if(this.ghost != null && this.ghost.chase != null){
            this.ghost.chase.Enable();
        }
    }
    private void OnTriggerEnter2D(Collider2D other){
        Node node = other.GetComponent<Node>();
        // If the node is not null, the ghost is not enabled, and the ghost is not frightened
        if(node != null && this.enabled && !this.ghost.frightened.enabled){
            // Set the next direction to a random available direction
            // Get a random index from the available directions list
            int index = Random.Range(0, node.availableDirections.Count);
            // If the random available direction is not the opposite of the current direction, set the next direction to the random available direction
            if(node.availableDirections[index] == -this.ghost.movement.direction && node.availableDirections.Count > 1){
                index++; // Increment the index
                // If the index is greater than the available directions count, set the index to 0
                if(index >= node.availableDirections.Count){
                    index = 0; // Set the index to 0
                }
            }
            // Set the next direction to the random available direction
            this.ghost.movement.SetDirection(node.availableDirections[index]);

        }
    
    }
}

