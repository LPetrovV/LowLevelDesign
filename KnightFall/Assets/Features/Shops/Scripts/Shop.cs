using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private KeyCode interactKey = KeyCode.P;

    private bool playerInRange = false;


    void Start(){
        shopPanel.SetActive(false); // start with the menu gones
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            shopPanel.SetActive(!shopPanel.activeSelf);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            shopPanel.SetActive(false); // walk away
        }
    }
}