using System.Collections;
using UnityEngine;

public class Entity : MonoBehaviour
{

    protected Animator anim;
    protected Rigidbody2D rb;
    protected Collider2D col;
    protected SpriteRenderer sr;

    [Header("Health Details")]
    [SerializeField] protected int maxHealth = 1;
    protected int currentHealth;
    [SerializeField] private Material damageMaterial;
    [SerializeField] private float damageFeedbackDuration = 0.2f;
    private Coroutine damageFeedbackCoroutine;

    [Header("Attack Details")]
    [SerializeField] protected float attackRadius;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask whatIsTarget;

    [Header("Movement Details")]
    [SerializeField] protected float moveSpeed = 8f;
    protected int facingDir = 1;
    protected bool facingRight = true;
    protected bool canMove = true;

    [Header("Collision Details")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    protected bool isGrounded;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponentInChildren<Animator>();
        sr = GetComponentInChildren<SpriteRenderer>();
        currentHealth = maxHealth;
    }

    protected virtual void Update()
    {
        HandleCollision();
        HandleMovement();
        HandleAnimation();
        HandleFlip();
    }

    public void DamageTargets()
    {
        Collider2D[] entityColliders = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, whatIsTarget);

        foreach (Collider2D entityCollider in entityColliders)
        {
            entityCollider.GetComponent<Entity>().TakeDamage();
        }
    }

    private void TakeDamage()
    {
        Debug.Log($"{gameObject.name} took damage!");
        currentHealth -= 1;
        PlayDamageFeedback();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void PlayDamageFeedback()
    {
        if (damageFeedbackCoroutine != null)
            StopCoroutine(DamageFeedbackCoroutine());
        StartCoroutine(DamageFeedbackCoroutine());
    }

    private IEnumerator DamageFeedbackCoroutine()
    {
        Material originalMaterial = sr.material;
        sr.material = damageMaterial;
        yield return new WaitForSeconds(damageFeedbackDuration);
        sr.material = originalMaterial;
    }

    protected virtual void Die()
    {
        anim.enabled = false;
        col.enabled = false;

        rb.gravityScale = 6;
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 15);

        Destroy(gameObject, 3f);
    }

    public virtual void EnableMovementAndJump(bool enable)
    {
        canMove = enable;
    }

    protected void HandleAnimation()
    {
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
    }

    protected virtual void HandleAttack()
    {
        if (isGrounded)
        {
            anim.SetTrigger("attack");
        }
    }

    protected virtual void HandleMovement()
    {
    }

    protected virtual void HandleCollision()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    protected virtual void HandleFlip()
    {
        if ((rb.linearVelocity.x > 0 && !facingRight) || (rb.linearVelocity.x < 0 && facingRight))
        {
            Flip();
        }
    }

    public void Flip()
    {
        transform.Rotate(0f, 180f, 0f);
        facingRight = !facingRight;
        facingDir *= -1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(0, -groundCheckDistance));
        if (attackPoint != null)
            Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
