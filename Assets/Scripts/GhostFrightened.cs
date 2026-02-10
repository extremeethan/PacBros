using UnityEngine;

public class GhostFrightened : GhostBehavior
{
    float _ignoreEatUntil;

    public override void Enable(float duration)
    {
        base.Enable(duration);
        ghost.SetSpriteState(Ghost.SpriteState.Frightened);
        _ignoreEatUntil = Time.time + 1f;
    }

    public override void Disable()
    {
        base.Disable();
        ghost.SetSpriteState(Ghost.SpriteState.Normal);
        ghost.scatter.Enable();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!enabled) return;

        Node node = other.GetComponent<Node>();
        if (node != null)
        {
            Vector2 direction = Vector2.zero;
            float maxDistance = float.MinValue;
            Vector3 targetPos = ghost.target != null ? ghost.target.transform.position : transform.position;
            foreach (Vector2 availableDirection in node.availableDirections)
            {
                Vector3 newPosition = transform.position + new Vector3(availableDirection.x, availableDirection.y);
                float distance = (targetPos - newPosition).sqrMagnitude;
                if (distance > maxDistance)
                {
                    direction = availableDirection;
                    maxDistance = distance;
                }
            }
            ghost.movement.SetDirection(direction);
        }
        else if (Ghost.IsPlayer(other.gameObject) && Time.time >= _ignoreEatUntil)
        {
            FindFirstObjectByType<GameManager>().GhostEaten(ghost);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!enabled || Time.time < _ignoreEatUntil) return;
        if (Ghost.IsPlayer(collision.gameObject))
            FindFirstObjectByType<GameManager>().GhostEaten(ghost);
    }
}
