using UnityEngine;
using System.Collections.Generic;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    private readonly List<Slot> slots = new List<Slot>();
    public int slotCount;
    public List<GameObject> itemPrefabs = new List<GameObject>();
    private readonly List<Item> inventoryItems = new List<Item>();

    public PlayerStats playerStats;

    void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            slots.Add(slot);
            if (i < itemPrefabs.Count)
            {
                GameObject itemObject = Instantiate(itemPrefabs[i], slot.transform);
                itemObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

                Item item = itemObject.GetComponent<Item>();
                if (item != null)
                {
                    slot.currentItem = item;
                    inventoryItems.Add(item);
                }

            }
        }
    }

    public bool AddItem(GameObject prefab)
    {
        Slot free = slots.Find(s => s.currentItem == null);
        if (free == null) return false; // inventory full

        GameObject itemObject = Instantiate(prefab, free.transform);
        itemObject.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        Item item = itemObject.GetComponent<Item>();
        if (item == null) { Destroy(itemObject); return false; }

        free.currentItem = item;
        inventoryItems.Add(item);
        return true;
    }

    public bool useItem(Item item)
    {
        Debug.Log("item used");
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

        Slot usedSlot = slots.Find(s => s.currentItem == item);
        if (usedSlot != null) usedSlot.currentItem = null;

        Destroy(item.gameObject);

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