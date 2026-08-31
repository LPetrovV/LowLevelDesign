using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public bool isOpened { get; private set; } = false;
    public string chestID { get; private set; }

    public Animator anim;

    public GameObject itemPrefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        anim = GetComponent<Animator>();
        chestID ??= GlobalHelper.GenerateUniqueID(gameObject);
    }

    public void Interact()
    {
        if (!CanInteract()) return;
        OpenChest();
    }

    public void OpenChest()
    {
        if (!isOpened)
        {
            isOpened = true;
            anim.SetBool("isOpened", true);

            if (itemPrefab)
            {
                GameObject droppedItem = Instantiate(itemPrefab, transform.position + Vector3.up, Quaternion.identity);
            }
        }
    }

    public bool CanInteract()
    {
        return !isOpened;
    }
}
