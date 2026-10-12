using UnityEngine;

public class HealthItem : Item
{
    public int healthBuff = 20;
    public override int getBuff()
    {
        return this.healthBuff;
    }

    public override void Use(PlayerStats playerStats)
    {
        playerStats.health += healthBuff;
    }
}
