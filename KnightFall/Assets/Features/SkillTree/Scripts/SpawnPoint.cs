using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   [SerializeField] private Sprite SpriteActivated;
   private Sprite SpriteDeactivated;

    private SpriteRenderer spriteRenderer;
    private RespawnHandler respawnHandler;

    private bool isActivated = false;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SpriteDeactivated = spriteRenderer.sprite; // Store the initial sprite as the deactivated state
        respawnHandler = GetComponentInParent<RespawnHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActivated)
    {
        if (respawnHandler != null)
        {
            respawnHandler.SetRespawnLocation(this);
        }
    }
    }

    public void ActivateSpawnPoint()
    {
        spriteRenderer.sprite = SpriteActivated;
        isActivated = true;
    }
    
    public void DeactivateSpawnPoint()
    {
        spriteRenderer.sprite = SpriteDeactivated;
        isActivated = false;
    }

    public bool IsActivated()
    {
        return isActivated;
    }
}
