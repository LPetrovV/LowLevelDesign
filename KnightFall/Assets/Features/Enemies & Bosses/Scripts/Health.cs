using UnityEngine;

public class Health : MonoBehaviour
{

    public int  health = 3;

    [Header("Enemy Animator")]
    [SerializeField] private Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Dead()
    {
        anim.SetBool("moving", false);
        anim.SetBool("dead", true);
    }

    private void takeDamage(int amount)
    {
        anim.SetBool("hurt", true);
        health -= amount;

        if(health <= 0)
        {
            health = 0;
            Dead();
        }
    }
}
