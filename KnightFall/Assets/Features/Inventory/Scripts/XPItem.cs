using UnityEngine;

public class XPItem : Item
{
    public int xpBuff = 50;
    
    public override int getBuff()
    {
        return this.xpBuff;
    }

    public override void Use(PlayerStats playerStats)
    {
        playerStats.AddXP(xpBuff);
    }
}
