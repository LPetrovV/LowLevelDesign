using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
   [SerializeField] private Sprite SpriteActivated;
   private Sprite SpriteDeactivated;

    private SpriteRenderer spriteRenderer;
    private RespawnHandler respawnHandler;

    private bool isActivated = false;

    private GameObject mapInstance;
    private SpriteRenderer mapSpriteRenderer;

    public Material unlitMaterial;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SpriteDeactivated = spriteRenderer.sprite; // Store the initial sprite as the deactivated state
        respawnHandler = GetComponentInParent<RespawnHandler>();

        //create map instance
        mapInstance = new GameObject("SpawnPointMapInstance");
        mapSpriteRenderer = mapInstance.AddComponent<SpriteRenderer>();

        mapSpriteRenderer.sprite = spriteRenderer.sprite;
        mapSpriteRenderer.material = unlitMaterial;

        mapInstance.transform.position = transform.position;
        mapInstance.transform.localScale = new Vector3(2f, 2f, 1f); // Adjust the scale as needed
        mapInstance.transform.Translate(0f, 0.5f, 0f, Space.World);

        // set the map instance layer to map layer
        int mapLayer = LayerMask.NameToLayer("Map");
        mapInstance.layer = mapLayer;
        mapSpriteRenderer.sortingLayerName = "Map";
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
        mapSpriteRenderer.sprite = SpriteActivated;
        isActivated = true;
    }
    
    public void DeactivateSpawnPoint()
    {
        spriteRenderer.sprite = SpriteDeactivated;
        mapSpriteRenderer.sprite = SpriteDeactivated;
        isActivated = false;
    }

    public bool IsActivated()
    {
        return isActivated;
    }
}
