using UnityEngine;
using System.Collections.Generic;

public class RespawnHandler : MonoBehaviour
{
    public static Vector3 RespawnLocation { get; private set; } = Vector3.zero;
    public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();

    private GameObject player;

    

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        spawnPoints = new List<SpawnPoint>(GetComponentsInChildren<SpawnPoint>());

    }

    public void SetRespawnLocation(SpawnPoint spawnPoint)
    {
        DeactivateAllSpawnPoints(); // Deactivate all spawn points before activating the new one
        RespawnLocation = spawnPoint.transform.position;
        spawnPoint.ActivateSpawnPoint(); // Activate the new spawn point
        Debug.Log("Respawn location set to: " + RespawnLocation);
    }

    public void DeactivateAllSpawnPoints()
    {
        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.IsActivated())
            {
                spawnPoint.DeactivateSpawnPoint();
            }
        }
    }

    public void Respawn()
    {
        if (player != null)
        {
            player.transform.position = RespawnLocation;
        }
    }
}
