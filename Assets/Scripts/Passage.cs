using UnityEngine;

public class Passage : MonoBehaviour
{
    public Transform connection;// The connection to the other passage
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        // object other is the object that entered the trigger
        // destination is the position of the destination
        // set the position of the object other to the position of the destination
        Vector3 position = other.transform.position;// The position of the object other
        position.x = this.connection.position.x;// The x position of the connection
        position.y = this.connection.position.y;// The y position of the connection
        other.transform.position = position;// Set the position of the object other to the position of the connection

    }
}
