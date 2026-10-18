using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    public int coins = 0;
    public int xp = 0;
    public int health = 80;

    // addition made during health bar UI
    public int maxHealth = 100; 
    private HealthBarUI healthBar;

    public int speed = 5;
    public int dexterity = 5;
    public int combat = 5;

    public int respawnHealth = 100;

    void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthBar = FindFirstObjectByType<HealthBarUI>();
        UpdateHealthBar();
    }

    // Update is called once per frame
    void Update()
    {
        // debug and test of the health bar
        if (Input.GetKeyDown(KeyCode.K))
            DecreaseHealth(10);

        if (health <= 0)
            Die();
    }

    private void Die()
    {
        health = 0;
        PlayerDeath();
    }

    public void DecreaseHealth(int amount)
    {
        health -= amount;
        if (health <= 0)
            Die();
        UpdateHealthBar();
    }
    
    public void Heal(int amount)
    {
        health = Mathf.Min(health + amount, maxHealth);
        UpdateHealthBar();
    }

    public void PlayerDeath()
    {
        //respawn player and reset health
        

        GameObject respawnHandler = GameObject.FindGameObjectWithTag("Respawn");

        if (respawnHandler != null)
        {
            RespawnHandler handler = respawnHandler.GetComponent<RespawnHandler>();

            if (handler != null)
            {
                handler.Respawn();
            }
            else
            {
                Debug.LogError("Respawn object does not have a RespawnHandler component.");
            }
        }
        else
        {
            Debug.LogError("No GameObject with the 'Respawn' tag was found.");
        }

        // half coins on death 
        coins = coins/2;
        
        health = respawnHealth;
        UpdateHealthBar();
    }

    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    public void UpdateHealthBar()
    {
        if (healthBar == null) return;
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(health);
    }
}
