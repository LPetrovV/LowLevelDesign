using UnityEngine;
using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;

public class MapTransition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundary;

   [SerializeField] GameObject puzzleObject;
    CinemachineConfiner2D confiner;
    public Transform transportLocation;
    [SerializeField] Direction direction;
    public float offset = 1f;
    enum Direction
    {
        Up,
        Down,
        Left,
        Right,
        Teleport
    }

    

    private void Awake()
    {
        confiner = FindAnyObjectByType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
    {
        confiner.BoundingShape2D = mapBoundary;
        confiner.InvalidateBoundingShapeCache();
        
        Vector3 oldPos = collision.transform.position;
        UpdatePlayerPosition(collision.gameObject);
        Vector3 delta = collision.transform.position - oldPos;

        var vcam = FindAnyObjectByType<CinemachineCamera>();
        vcam.OnTargetObjectWarped(collision.transform, delta);

        // reset puzzle if the player is entering a new map
        if (puzzleObject != null)
        {
            Puzzle puzzle = puzzleObject.GetComponent<Puzzle>();
            if (puzzle != null)
            {
                puzzle.ResetPuzzle();
            }
        }
    }
        
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        if(direction == Direction.Teleport)
        {
            player.transform.position = transportLocation.position;
            return;
        }
        

        Vector3 additivePos = player.transform.position;

        switch (direction)
        {
            case Direction.Up:
                additivePos.y += offset;
                break;
            case Direction.Down:
                additivePos.y -= offset;
                break;
            case Direction.Left:
                additivePos.x -= offset;
                break;
            case Direction.Right:
                additivePos.x += offset;
                break;
        }

        player.transform.position = additivePos;
        
    }
}
