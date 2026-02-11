using UnityEngine;

public class PowerPellet : Pellet
{
    public float duration = 8f;
    // override the Eat method to call the PowerPelletEaten method in the GameManager
    protected override void Eat()
    {
        var gm = FindFirstObjectByType<GameManager>();
        if (gm != null) gm.PowerPelletEaten(this);
    }
}
