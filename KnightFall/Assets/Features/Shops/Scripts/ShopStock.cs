using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopStock : MonoBehaviour
{
    public List<GameObject> itemsForSale = new List<GameObject>();

    void Start()
    {
        foreach (GameObject itemPrefab in itemsForSale)
        {
            GameObject shopItem = Instantiate(itemPrefab, transform);
            Item item = shopItem.GetComponent<Item>();

            Button button = shopItem.AddComponent<Button>();
            button.onClick.AddListener(() => Buy(item));
        }
    }

    void Buy(Item item)
    {
        PlayerStats stats = PlayerStats.Instance;
        if (stats == null) return;

        if (stats.coins >= item.getPrice())
        {
            stats.coins -= item.getPrice();
            Debug.Log("Bought " + item.getName());
        }
        else
        {
            Debug.Log("Not enough coins for " + item.getName());
        }
    }
}