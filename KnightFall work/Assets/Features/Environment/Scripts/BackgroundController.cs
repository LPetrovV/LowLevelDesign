using UnityEngine;

public class BackgroundController : MonoBehaviour
{
    private float startPosx;
    private float startPosy;
    public GameObject cam;
    //0 = move normally, 1 = won't move
    public float parallaxEffect;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPosx = transform.position.x;
        startPosx = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float distancex = cam.transform.position.x*parallaxEffect;
        float distancey = cam.transform.position.y*parallaxEffect;
        transform.position = new Vector3(startPosx+distancex, startPosy+distancey, transform.position.z);
    }
}
