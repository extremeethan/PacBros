using UnityEngine;

public class GhostScatter : GhostBehavior
{
    private void OnDisable(){
        // Check if ghost and chase are initialized before accessing them
        // This prevents NullReferenceException during initialization
        if(this.ghost != null && this.ghost.chase != null){
            // Enable the chase behavior
            this.ghost.chase.Enable(); // enable the chase behavior
        }
    }
    private void OnTriggerEnter2D(Collider2D other){
        // Get the node component from the other collider
        Node node = other.GetComponent<Node>(); // get the node component from the other collider
        // If the node is not null, this is enabled, and the ghost is not frightened
        if(node != null && this.enabled && !this.ghost.frightened.enabled){
            // Set the next direction to a random available direction
            // Filter out the opposite direction to prevent going back and forth
            Vector2 oppositeDirection = -this.ghost.movement.direction; // get the opposite direction
            // Create a list to store the valid directions
            System.Collections.Generic.List<Vector2> validDirections = new System.Collections.Generic.List<Vector2>(); // create a list to store the valid directions
            // Loop through the available directions and add the directions to the valid directions list
            foreach(Vector2 dir in node.availableDirections){
                // Only exclude opposite direction if there are other options
                if(dir != oppositeDirection || node.availableDirections.Count == 1){
                    // Add the direction to the valid directions list
                    validDirections.Add(dir); // add the direction to the valid directions list
                }
            }
            
            // Get a random index from the valid directions list
            int index = Random.Range(0, validDirections.Count); // get a random index from the valid directions list
            // Set the next direction to the random available direction
            this.ghost.movement.SetDirection(validDirections[index]); // set the next direction to the random available direction   

        }
    
    }
}

