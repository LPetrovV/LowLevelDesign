using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillSave
{
    public string id;
    public int value;
}

[Serializable]
public class EnemySave
{
    public string id;
    public int health;
}

// Everything that goes into one save slot file (JSON).
[Serializable]
public class SaveData
{
    public int version = 3;

    // Shown in the slot list
    public string savedAt;
    public float playTimeSeconds;

    // Player
    public Vector3 playerPosition;
    public int level = 1;
    public int xp, coins, health, maxHealth, speed, dexterity, combat;
    public int skillPoints;

    // World
    public bool hasRespawn;
    public Vector3 respawnPosition;
    public string cameraBounds;

    public List<string> inventory = new List<string>();        // item names
    public List<SkillSave> skills = new List<SkillSave>();     // skill slider values
    public List<string> openedChests = new List<string>();
    public List<string> collectedCoins = new List<string>();
    public List<string> deadEnemies = new List<string>();
    public List<EnemySave> enemyHealth = new List<EnemySave>();  // living enemies
    public List<string> pulledLevers = new List<string>();
}
