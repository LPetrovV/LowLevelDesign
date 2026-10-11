using UnityEngine;

public class RespawnHandler : MonoBehaviour
{
    public static Vector3 RespawnLocation { get; private set; } = Vector3.zero;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void SetRespawnLocation(Vector3 location)
    {
        RespawnLocation = location;
        Debug.Log("Respawn location set to: " + RespawnLocation);
    }

    public void Respawn()
    {
        if (player != null)
        {
            player.transform.position = RespawnLocation;
        }
    }
}
