using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;
    public int health = 100;
    public int speed = 5;
    public int dexterity = 5;
    public int combat = 5;

    void Awake()
    {
        Instance = this;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            health = 0;
            PlayerDeath();
        }
    }

    public void DecreaseHealth(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            health = 0;
            PlayerDeath();
        }
    }

    public void PlayerDeath()
    {
        //respawn player and reset health
        health = 100;

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
    }
}
