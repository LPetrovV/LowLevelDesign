using System.Collections;
using UnityEngine;

// One script now owns the enemy's health, attack, hurt and death, and drives the
// Animator's bool parameters: moving / attack / hurt / dead.
public class Enemy : MonoBehaviour
{
    [Header("Combat")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] public int health = 100;
    [SerializeField] private int xpReward = 50;

    [Header("Animation timing (seconds)")]
    [Tooltip("How far into the swing the damage lands. Scrub attack.anim to find the hit frame.")]
    [SerializeField] private float hitTime = 0.3f;
    [SerializeField] private float attackClipLength = 0.67f;
    [SerializeField] private float hurtClipLength = 0.42f;
    [SerializeField] private float deathClipLength = 0.82f;

    private float cooldownTimer = Mathf.Infinity;
    private Animator anim;
    private EnemyPatrol enemyPatrol;

    private bool isDead;
    private bool isAttacking;
    private bool isHurt;
    private Coroutine attackRoutine;
    private Coroutine hurtRoutine;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        if (isDead) return;

        cooldownTimer += Time.deltaTime;
        bool playerSeen = PlayerInSight();

        if (playerSeen && !isAttacking && !isHurt && cooldownTimer >= attackCooldown)
            attackRoutine = StartCoroutine(AttackRoutine());

        // Stand still while the player is in sight, mid-swing, or staggered.
        if (enemyPatrol != null)
            enemyPatrol.enabled = !(playerSeen || isAttacking || isHurt);

    }

    // ---------- Attack ----------
    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        anim.SetBool("attack", true);

        // The animator may still be blending out of "moving", so wait until the
        // attack state has really started (1s safety timeout so we can never hang).
        float waited = 0f;
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("attack") && waited < 1f)
        {
            waited += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(hitTime);
        DealDamage();
        yield return new WaitForSeconds(Mathf.Max(0f, attackClipLength - hitTime));

        anim.SetBool("attack", false);
        isAttacking = false;
        cooldownTimer = 0f;   // cooldown counts from the end of the swing
    }

    private void DealDamage()
    {
        if (isDead || !PlayerInSight()) return;   // player may have dodged out of range
        if (PlayerStats.Instance != null)
            PlayerStats.Instance.DecreaseHealth(damage);
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        return hit.collider != null;
    }

    private void OnDrawGizmos()
    {
        if (boxCollider == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    // ---------- Taking damage ----------
    public void TakeDamage(int amount)
    {
        if (isDead) return;

        health -= amount;
        if (health <= 0)
        {
            Die();
            return;
        }

        // A hit interrupts the swing.
        if (attackRoutine != null) StopCoroutine(attackRoutine);
        anim.SetBool("attack", false);
        isAttacking = false;
        cooldownTimer = 0f;

        if (hurtRoutine != null) StopCoroutine(hurtRoutine);
        hurtRoutine = StartCoroutine(HurtRoutine());
    }

    private IEnumerator HurtRoutine()
    {
        isHurt = true;
        anim.SetBool("hurt", true);
        yield return new WaitForSeconds(hurtClipLength);
        anim.SetBool("hurt", false);
        isHurt = false;
    }

    // ---------- Death ----------
    public void EnemyDeath() => Die();   // kept so existing callers still work

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        StopAllCoroutines();

        if (enemyPatrol != null) enemyPatrol.enabled = false;
        if (boxCollider != null) boxCollider.enabled = false;   // corpse can't be hit or block anything

        // Order matters: the controller checks "hurt" before "dead", so clear it first.
        anim.SetBool("attack", false);
        anim.SetBool("hurt", false);
        anim.SetBool("moving", false);
        anim.SetBool("dead", true);

        // If you added the XP system, uncomment this:
        if (PlayerStats.Instance != null) PlayerStats.Instance.AddXP(xpReward);

        // Let the death animation play, then remove the whole patrol group.
        GameObject root = enemyPatrol != null ? enemyPatrol.gameObject : gameObject;
        Destroy(root, deathClipLength + 0.5f);
    }
}