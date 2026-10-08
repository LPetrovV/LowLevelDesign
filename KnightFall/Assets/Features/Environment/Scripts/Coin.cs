using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Add coin to player's stats
            //Debug.Log("Coin collected!");
            Destroy(gameObject);

            GameObject player = other.gameObject;

            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            playerStats.coins += coinValue;
        }
    }
}
