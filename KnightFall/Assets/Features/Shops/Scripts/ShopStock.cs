using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopStock : MonoBehaviour
{
    public List<GameObject> itemsForSale = new List<GameObject>();
    public InventoryController Inventory;

    void Start()
    {
        foreach (GameObject itemPrefab in itemsForSale)
        {
            GameObject shopItem = Instantiate(itemPrefab, transform);

            Item item = shopItem.GetComponent<Item>();

            Button button = shopItem.AddComponent<Button>();
            button.targetGraphic = shopItem.GetComponent<Image>();   // <-- add (item prefab needs an Image)
            GameObject prefab = itemPrefab;                          // <-- add
            button.onClick.AddListener(() => Buy(item, prefab));
        }
    }

    void Buy(Item item, GameObject prefab)
    {
        PlayerStats stats = PlayerStats.Instance;
        if (stats == null) return;

        if (stats.coins >= item.getPrice())
        {
            if (Inventory.AddItem(prefab))
            {
                stats.coins -= item.getPrice();
                Debug.Log("Bought " + item.getName());
            }
            else
            {
                Debug.Log("Inventory full");
            }
        }
        else
        {
            Debug.Log("Not enough coins for " + item.getName());
        }
    }
}