using UnityEngine;
using System.Collections.Generic;

public class Node : MonoBehaviour
{
    public LayerMask obstacleLayer; // The layer mask of the obstacles
    public List<Vector2> availableDirections {get; private set;} // The available directions for the node

    private void Start() // this is the start method
    {
        // Initialize the available directions list
        this.availableDirections = new List<Vector2>();
        // Check the available directions for the node
        CheckAvailableDirections(Vector2.up);
        CheckAvailableDirections(Vector2.down); // check the available directions for the node down
        CheckAvailableDirections(Vector2.left); // check the available directions for the node left
        CheckAvailableDirections(Vector2.right); // check the available directions for the node right
    }

    // Check the available directions for the node
    private void CheckAvailableDirections(Vector2 direction){
        // Cast a box cast to check if the path is occupied the position of the object, the size of the box, the angle of the box, the direction of the box, the distance of the box, and the layer mask of the obstacles
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.5f, 0.0f, direction, 1f, this.obstacleLayer);
        // If the raycast hits nothing, add the direction to the available directions
        if(hit.collider == null){
            this.availableDirections.Add(direction);
        }
    }
}