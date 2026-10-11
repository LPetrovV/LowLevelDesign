using UnityEngine;

public class Item : MonoBehaviour
{
    public int price;
    public string itemName;

    public int getPrice()
    {
        return this.price;
    }

    public string getName()
    {
        return this.itemName;
    }

    public virtual void Use(PlayerStats playerStats)
    {
        return;
    }
}
