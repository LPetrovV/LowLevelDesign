using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 8f;

    [Header("Jump")]
    public float jumpForce = 14f;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;

    [Header("Attack")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayer;
    public float attackCooldown = 0.4f;
    public int attackDamage = 10;
    private float lastAttackTime;

    [Header("Hurt")]
    public float stunDuration = 0.3f;
    public float invincibilityDuration = 1f;
    public Vector2 knockbackForce = new Vector2(6f, 5f);
    private bool isStunned;
    private bool isInvincible;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private PlayerStats stats;
    private float moveInput;
    private bool isGrounded;
    private bool jumpPressed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        stats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        animator.SetBool("IsGrounded", isGrounded);

        if (isStunned)
        {
            moveInput = 0;
            animator.SetFloat("Speed", 0);
            return;
        }

        moveInput = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump")) jumpPressed = true;

        if (moveInput > 0)
        {
            spriteRenderer.flipX = false;
            attackPoint.localPosition = new Vector3(Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, 0);
        }
        else if (moveInput < 0)
        {
            spriteRenderer.flipX = true;
            attackPoint.localPosition = new Vector3(-Mathf.Abs(attackPoint.localPosition.x), attackPoint.localPosition.y, 0);
        }

        animator.SetFloat("Speed", Mathf.Abs(moveInput));

        if (Input.GetKeyDown(KeyCode.E) && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);
        foreach (Collider2D col in hitEnemies)
        {
            Enemy enemy = col.GetComponentInParent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(attackDamage);
        }
    }

    public void TakeDamage(int amount, Vector2 damageSourcePosition)
    {
        if (isInvincible) return;

        if (stats != null) stats.DecreaseHealth(amount);
        animator.SetTrigger("Hurt");

        float dir = transform.position.x >= damageSourcePosition.x ? 1f : -1f;
        rb.linearVelocity = new Vector2(dir * knockbackForce.x, knockbackForce.y);

        StartCoroutine(HurtRoutine());
    }

    IEnumerator HurtRoutine()
    {
        isStunned = true;
        isInvincible = true;

        yield return new WaitForSeconds(stunDuration);
        isStunned = false;

        float remaining = invincibilityDuration - stunDuration;
        float timer = 0f;
        while (timer < remaining)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        spriteRenderer.enabled = true;
        isInvincible = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void FixedUpdate()
    {
        if (!isStunned)
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        if (jumpPressed)
        {
            if (isGrounded && !isStunned)
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpPressed = false;
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.fixedDeltaTime;
        }
        else if (rb.linearVelocity.y > 0 && !Input.GetButton("Jump") && !isStunned)
        {
            rb.linearVelocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.fixedDeltaTime;
        }
    }
}