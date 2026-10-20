using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

// Put ONE of these on a GameObject in SampleScene (e.g. GameController).
// It gathers the game state into SaveData, and applies a slot back after a load.
public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    float playTime;

    // Snapshot, taken when the scene starts, of everything that can be destroyed during play.
    // A coin/enemy whose object has been destroyed by save time was collected/killed.
    readonly Dictionary<Coin, string> coinIds = new Dictionary<Coin, string>();
    readonly Dictionary<Enemy, string> enemyIds = new Dictionary<Enemy, string>();

    void Awake() { Instance = this; }

    void OnDestroy()
    {
        if (Instance == this) Instance = null;
    }

    void Update() { playTime += Time.deltaTime; }   // scaled time, so the paused menu isn't counted

    IEnumerator Start()
    {
        int slot = SaveSystem.PendingLoadSlot;
        SaveSystem.PendingLoadSlot = -1;

        bool loading = slot >= 0 && SaveSystem.HasSave(slot);
        if (!loading) SaveSystem.CurrentSlot = -1;   // plain "New Game": no slot until the player saves

        yield return null;   // wait a frame so every other Start() has run (slots, chest IDs, spawn points...)

        foreach (Coin c in FindObjectsByType<Coin>(FindObjectsSortMode.None))
            coinIds[c] = StableId(c.transform);
        foreach (Enemy e in FindObjectsByType<Enemy>(FindObjectsSortMode.None))
            enemyIds[e] = StableId(e.transform);

        if (loading) Apply(SaveSystem.Read(slot));
    }

    // ---------- Saving ----------

    // Saves into the slot the player is using. Handy for checkpoints/autosave.
    public void QuickSave()
    {
        if (SaveSystem.CurrentSlot >= 0) SaveToSlot(SaveSystem.CurrentSlot);
        else Debug.Log("QuickSave skipped: no save slot chosen yet. Save once from the menu first.");
    }

    public void SaveToSlot(int slot)
    {
        PlayerStats stats = PlayerStats.Instance;
        if (stats == null)
        {
            Debug.LogError("Save failed: no PlayerStats in the scene.");
            return;
        }

        var data = new SaveData
        {
            savedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
            playTimeSeconds = playTime,
            playerPosition = stats.transform.position,
            level = stats.level,
            xp = stats.xp,
            coins = stats.coins,
            health = stats.health,
            maxHealth = stats.maxHealth,
            speed = stats.speed,
            dexterity = stats.dexterity,
            combat = stats.combat
        };

        if (SkillPointManager.Instance != null)
            data.skillPoints = SkillPointManager.Instance.AvailableSkillPoints;

        var inventory = FindAnyObjectByType<InventoryController>();
        if (inventory != null) data.inventory = inventory.GetItemNames();

        // Include inactive: the skill menu is hidden while you play.
        foreach (var u in FindObjectsByType<SkillUpgrader>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            data.skills.Add(new SkillSave { id = StableId(u.transform), value = (int)u.skillSlider.value });

        foreach (var chest in FindObjectsByType<Chest>(FindObjectsSortMode.None))
            if (chest.isOpened && !string.IsNullOrEmpty(chest.chestID))
                data.openedChests.Add(chest.chestID);

        foreach (var lever in FindObjectsByType<Lever>(FindObjectsSortMode.None))
            if (lever.isInteracted && !string.IsNullOrEmpty(lever.gateID))
                data.pulledLevers.Add(lever.gateID);

        foreach (var pair in coinIds)
            if (pair.Key == null) data.collectedCoins.Add(pair.Value);   // destroyed = collected

        foreach (var pair in enemyIds)
        {
            Enemy e = pair.Key;
            if (e == null || e.health <= 0)
                data.deadEnemies.Add(pair.Value);
            else
                data.enemyHealth.Add(new EnemySave { id = pair.Value, health = e.health });
        }

        var respawn = FindAnyObjectByType<RespawnHandler>();
        if (respawn != null)
        {
            SpawnPoint active = respawn.spawnPoints.Find(sp => sp != null && sp.IsActivated());
            if (active != null)
            {
                data.hasRespawn = true;
                data.respawnPosition = active.transform.position;
            }
        }

        var confiner = FindAnyObjectByType<CinemachineConfiner2D>();
        if (confiner != null && confiner.BoundingShape2D != null)
            data.cameraBounds = confiner.BoundingShape2D.name;

        SaveSystem.Write(slot, data);
        SaveSystem.CurrentSlot = slot;
        Debug.Log($"Saved to slot {slot}: {SaveSystem.SlotPath(slot)}");
    }

    // ---------- Loading ----------

    void Apply(SaveData d)
    {
        if (d == null) return;

        PlayerStats stats = PlayerStats.Instance;
        if (stats == null) return;

        playTime = d.playTimeSeconds;

        // Stats are restored directly. Do NOT replay SkillUpgrader.TryUpgrade, or bonuses double up.
        stats.level = Mathf.Max(1, d.level);
        stats.xp = d.xp;
        stats.coins = d.coins;
        if (d.maxHealth > 0) stats.maxHealth = d.maxHealth;
        stats.health = Mathf.Clamp(d.health, 1, stats.maxHealth);
        stats.speed = d.speed;
        stats.dexterity = d.dexterity;
        stats.combat = d.combat;
        stats.UpdateHealthBar();    // the HUD doesn't poll, so tell it
        stats.NotifyXpChanged();

        if (SkillPointManager.Instance != null)
            SkillPointManager.Instance.SetSkillPoints(d.skillPoints);

        // Skill sliders + their "x/y" labels
        var savedSkills = new Dictionary<string, int>();
        foreach (var s in d.skills) savedSkills[s.id] = s.value;
        foreach (var u in FindObjectsByType<SkillUpgrader>(FindObjectsInactive.Include, FindObjectsSortMode.None))
        {
            if (savedSkills.TryGetValue(StableId(u.transform), out int v))
            {
                u.skillSlider.value = v;
                if (u.counterText != null) u.counterText.text = $"{v}/{u.skillSlider.maxValue}";
            }
        }

        var inventory = FindAnyObjectByType<InventoryController>();
        if (inventory != null) inventory.LoadItems(d.inventory);

        var openedChests = new HashSet<string>(d.openedChests);
        foreach (var chest in FindObjectsByType<Chest>(FindObjectsSortMode.None))
            if (openedChests.Contains(chest.chestID)) chest.LoadOpened();

        // Pulling a lever the same way the player does restores both the lever and the gate it moves.
        var pulled = new HashSet<string>(d.pulledLevers);
        foreach (var lever in FindObjectsByType<Lever>(FindObjectsSortMode.None))
            if (pulled.Contains(lever.gateID) && !lever.isInteracted) lever.Interact();

        var collected = new HashSet<string>(d.collectedCoins);
        foreach (var pair in coinIds)
            if (pair.Key != null && collected.Contains(pair.Value))
                Destroy(pair.Key.gameObject);

        var dead = new HashSet<string>(d.deadEnemies);
        var enemyHp = new Dictionary<string, int>();
        foreach (var e in d.enemyHealth) enemyHp[e.id] = e.health;
        foreach (var pair in enemyIds)
        {
            Enemy enemy = pair.Key;
            if (enemy == null) continue;

            if (dead.Contains(pair.Value))
            {
                // Same thing Enemy.Die does: remove the whole patrol group.
                EnemyPatrol patrol = enemy.GetComponentInParent<EnemyPatrol>();
                Destroy(patrol != null ? patrol.gameObject : enemy.gameObject);
            }
            else if (enemyHp.TryGetValue(pair.Value, out int hp))
            {
                enemy.health = hp;
            }
        }

        // Respawn point (also restores its "activated" sprite)
        var respawn = FindAnyObjectByType<RespawnHandler>();
        if (respawn != null && d.hasRespawn)
        {
            foreach (var sp in respawn.spawnPoints)
            {
                if (sp != null && Vector3.Distance(sp.transform.position, d.respawnPosition) < 0.01f)
                {
                    respawn.SetRespawnLocation(sp);
                    break;
                }
            }
        }

        // Player position + camera (same warp MapTransition does)
        Vector3 oldPos = stats.transform.position;
        stats.transform.position = d.playerPosition;
        var rb = stats.GetComponent<Rigidbody2D>();
        if (rb != null) rb.linearVelocity = Vector2.zero;

        var confiner = FindAnyObjectByType<CinemachineConfiner2D>();
        if (confiner != null && !string.IsNullOrEmpty(d.cameraBounds))
        {
            foreach (var col in FindObjectsByType<PolygonCollider2D>(FindObjectsInactive.Include, FindObjectsSortMode.None))
            {
                if (col.name == d.cameraBounds)
                {
                    confiner.BoundingShape2D = col;
                    confiner.InvalidateBoundingShapeCache();
                    break;
                }
            }
        }

        var vcam = FindAnyObjectByType<CinemachineCamera>();
        if (vcam != null)
            vcam.OnTargetObjectWarped(stats.transform, stats.transform.position - oldPos);

        Debug.Log("Game loaded.");
    }

    // An ID from the object's place in the hierarchy (names + sibling indices).
    // Unlike a position-based ID it still matches after the object has moved (patrolling enemies, bobbing coins).
    static string StableId(Transform t)
    {
        string id = t.name + "[" + t.GetSiblingIndex() + "]";
        while (t.parent != null)
        {
            t = t.parent;
            id = t.name + "[" + t.GetSiblingIndex() + "]/" + id;
        }
        return id;
    }
}
