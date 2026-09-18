using System.Reflection.Metadata.Ecma335;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour, IInteractable
{
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    [SerializeField] private int damage;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] public int health = 100;
    private float cooldownTimer = Mathf.Infinity;

    private bool playerInSight = false;

    private Animator anim;
    private Health playerHealth;
    private EnemyPatrol enemyPatrol;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
    }

    private void Update()
    {
        cooldownTimer += Time.deltaTime;

        //Attack only when player in sight?
        if (PlayerInSight())
        {
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("meleeAttack");
            }
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();

        // bad code delete later
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit = 
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
            playerHealth = hit.transform.GetComponent<Health>();

        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    //bad code delete later
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInSight = true;
            Debug.Log("Player entered: " + collision.gameObject.name);
            DamagePlayer(damage);
        }
    }

    //bad code delete later
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInSight = false;
            //Debug.Log("Player exited: " + collision.gameObject.name);
        }
    }

    private void DamagePlayer(int damage)
    {
        //get player stats and decrease health
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        if (playerStats != null)
        {
            //Debug.Log("PlayerStats component found: " + playerStats.gameObject.name);
            playerStats.DecreaseHealth(damage);
            Debug.Log("Player health decreased by " + damage + ". Current health: " + playerStats.health);
        }
            
    }

    public bool CanInteract()
    {
        return true;
    }

    public void Interact()
    {
        if (health <= 0)
        {
            EnemyDeath();
        }
        health -= 20; // Example damage value
        Debug.Log("Enemy health decreased by 10. Current health: " + health);
    }

    public void EnemyDeath()
    {
        // Handle enemy death logic here
        Debug.Log("Enemy has died.");
        Destroy(gameObject);
    }
}
