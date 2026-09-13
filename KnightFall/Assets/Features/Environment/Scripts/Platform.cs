using UnityEngine;
using System.Collections;
public class Platform : InteractionTarget
{
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    
    public override void Activate()
    {
        //Debug.Log("Activating platform!");
        StartCoroutine(DelayedPlatform(delaySeconds));
        
    }

    IEnumerator DelayedPlatform(int seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (nextPosition == pointA.position)
            {
                nextPosition = pointB.position;
            }
        else
             {
                nextPosition = pointA.position;
             }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.collider.transform.SetParent(null);
        }
    }
}
