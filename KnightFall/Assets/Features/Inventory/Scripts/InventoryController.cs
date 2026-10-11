using UnityEngine;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;
    public List<GameObject> itemPrefabs = new List<GameObject>();
    private readonly List<Item> inventoryItems = new List<Item>();

    public PlayerStats playerStats;

    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            if (i < itemPrefabs.Count)
            {
                GameObject itemObject = Instantiate(itemPrefabs[i], slot.transform);
                itemObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
                slot.currentItem = itemObject;

                Item item = itemObject.GetComponent<Item>();
                if (item != null)
                    inventoryItems.Add(item);
            }
        }
    }

    public bool useItem(Item item)
    {
        if (item == null)
            return false;

        if (playerStats == null)
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
                playerStats = player.GetComponent<PlayerStats>();
        }

        if (playerStats == null)
        {
            Debug.LogError("PlayerStats not found.");
            return false;
        }

        item.Use(playerStats);

        inventoryItems.Remove(item);
        itemPrefabs.Remove(item.gameObject);

        Destroy(item.gameObject); // now only destroys the item's own object

        return true;
    }

    public HealthItem newHealthItem() => CreateItem<HealthItem>("HealthItem");

    public XPItem newXPItem() => CreateItem<XPItem>("XPItem");

    // Each item gets its own child GameObject
    private T CreateItem<T>(string objectName) where T : Item
    {
        GameObject itemObject = new GameObject(objectName);
        itemObject.transform.SetParent(transform);

        T newItem = itemObject.AddComponent<T>();
        itemPrefabs.Add(itemObject);
        inventoryItems.Add(newItem);
        return newItem;
    }
}