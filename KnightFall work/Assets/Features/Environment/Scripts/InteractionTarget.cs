using UnityEngine;

public class InteractionTarget : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float moveSpeed = 2f;
    protected Vector3 nextPosition;

    public int delaySeconds = 2;

    void Start()
    {
        nextPosition = pointA.position;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, nextPosition, moveSpeed * Time.deltaTime);
    }

    public virtual void Activate()
    {
        if (nextPosition == pointA.position)
            {
                nextPosition = pointB.position;
            }
        else
             {
                nextPosition = pointA.position;
             }
        
    }
}