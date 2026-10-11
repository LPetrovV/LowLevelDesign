using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayUseItemTest
{
    private GameObject player;
    private GameObject gameController;

    private PlayerStats playerStats;
    private InventoryController inventoryController;

    [SetUp]
    public void SetUp()
    {
        player = new GameObject("TestPlayer");
        playerStats = player.AddComponent<PlayerStats>().GetComponent<PlayerStats>();

        gameController = new GameObject("TestGameController");
        inventoryController =
            gameController.AddComponent<InventoryController>();

        inventoryController.playerStats = playerStats;
    }

    [TearDown]
    public void TearDown()
    {
        Object.DestroyImmediate(player);
        Object.DestroyImmediate(gameController);
    }

    [UnityTest]
    public IEnumerator UseHealthItem_IncreasesHealth()
    {
        int initialHealth = playerStats.health;
        HealthItem healthItem = inventoryController.newHealthItem();
        Assert.IsNotNull(healthItem, "Health item must exist.");
        int buff = healthItem.getBuff();          // capture before it is destroyed

        inventoryController.useItem(healthItem);
        yield return null;

        Assert.AreEqual(initialHealth + buff, playerStats.health);
    }

    [UnityTest]
    public IEnumerator UseXPItem_IncreasesXP()
    {
        int initialXP = playerStats.xp;
        XPItem xpItem = inventoryController.newXPItem();
        Assert.IsNotNull(xpItem, "XP item must exist.");
        int buff = xpItem.getBuff();

        inventoryController.useItem(xpItem);
        yield return null;

        Assert.AreEqual(initialXP + buff, playerStats.xp);
    }

    [UnityTest]
    public IEnumerator UseItem_InventoryDecreasesItemCount()
    {
        int initialItems = inventoryController.itemPrefabs.Count;   // 0
        HealthItem healthItem = inventoryController.newHealthItem();
        Assert.IsNotNull(healthItem, "Health item must exist.");
        Assert.AreEqual(initialItems + 1, inventoryController.itemPrefabs.Count); // added

        bool used = inventoryController.useItem(healthItem);
        yield return null;

        Assert.IsTrue(used);
        Assert.AreEqual(initialItems, inventoryController.itemPrefabs.Count);     // removed again
    }
}