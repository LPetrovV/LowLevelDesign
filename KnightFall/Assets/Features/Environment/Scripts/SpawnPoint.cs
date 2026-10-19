using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   [SerializeField] private Sprite SpriteActivated;

    private SpriteRenderer spriteRenderer;
    private RespawnHandler respawnHandler;

    private bool isActivated = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        respawnHandler = GetComponentInParent<RespawnHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        

        if (collision.CompareTag("Player"))
        {
            //Debug.Log("Player has entered the spawn point.");

            ActivateSpawnPoint();

            BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();
            boxCollider.enabled = false; // Disable the collider to prevent reactivation
        }
    }

    private void ActivateSpawnPoint()
    {
        spriteRenderer.sprite = SpriteActivated;
        isActivated = true;

        // Set this spawn point as the new respawn location
        respawnHandler.SetRespawnLocation(transform.position);
    }

    public bool IsActivated()
    {
        return isActivated;
    }
}
