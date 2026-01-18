using UnityEngine;

public class Player : Entity
{

    [SerializeField] private float jumpForce = 8f;
    private float xInput;
    private bool canJump = true;

    protected override void Update()
    {
        HandleInput();
        base.Update();
    }

    protected override void HandleMovement()
    {
        if (canMove)
        {
            rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    private void HandleInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            TryToJump();
        }

        //Mouse0 is left click or use "V" to attack
        if (Input.GetKeyDown(KeyCode.Mouse0) || Input.GetKeyDown(KeyCode.V))
        {
            HandleAttack();
        }
    }

    private void TryToJump()
    {
        if (isGrounded && canJump)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    public override void EnableMovementAndJump(bool enable)
    {
        base.EnableMovementAndJump(enable);
        canJump = enable;
    }

    protected override void Die()
    {
        base.Die();
        UI.instance.EnableGameOverUI();
    }
}
