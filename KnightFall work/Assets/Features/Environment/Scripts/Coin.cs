using UnityEngine;

public class Coin : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Add coin to player's inventory or score
            Debug.Log("Coin collected!");
            Destroy(gameObject);

            GameObject player = other.gameObject;

            SkillPointManager skillPointManager = player.GetComponentInChildren<SkillPointManager>();
            skillPointManager.AddSkillPoint();
        }
    }
}
