using UnityEngine;

public class Pellet : MonoBehaviour
{
    public int points = 10;

    protected virtual void Eat()// protected virtual void Eat() is a virtual method that can be overridden by subclasses
    {
        FindFirstObjectByType<GameManager>().PelletEaten(this);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player")){
            Eat();
        }

    }
}