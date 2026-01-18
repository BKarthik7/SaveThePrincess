using UnityEngine;

public class Princess : Entity
{
    private Transform player;

    protected override void Awake() {
        base.Awake();
        player = FindFirstObjectByType<Player>().transform;
    }

    protected override void Update()
    {
        HandleFlip();
    }

    protected override void HandleFlip()
    {

        if (player == null) return;

        if ((player.transform.position.x > 0 && !facingRight) || (player.transform.position.x < 0 && facingRight))
        {
            Flip();
        }
    }

    protected override void Die()
    {
        base.Die();
        UI.instance.EnableGameOverUI();
    }
}
