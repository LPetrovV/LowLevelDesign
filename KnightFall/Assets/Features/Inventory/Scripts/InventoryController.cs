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
    public List<GameObject> itemCatalog = new List<GameObject>();

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



    public List<string> GetItemNames()
    {
        var names = new List<string>();
        foreach (var s in slots)
            if (s.currentItem != null) names.Add(s.currentItem.getName());
        return names;
    }

    public void LoadItems(List<string> names)
    {
        foreach (var s in slots)
        {
            if (s.currentItem != null) Destroy(s.currentItem.gameObject);
            s.currentItem = null;
        }
        inventoryItems.Clear();

        foreach (string n in names)
        {
            GameObject prefab = itemCatalog.Find(p => p != null && p.GetComponent<Item>() != null
                                                      && p.GetComponent<Item>().getName() == n);
            if (prefab != null) AddItem(prefab);
            else Debug.LogWarning($"Saved item '{n}' not found in Item Catalog.");
        }
    }
}