using UnityEngine;
using System.Collections.Generic;

public class Puzzle : MonoBehaviour
{
    List<GameObject> immediateChildren = new List<GameObject>();
    List<Vector3> initialPositions = new List<Vector3>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Transform child in transform)
        {
            immediateChildren.Add(child.gameObject);
            initialPositions.Add(child.position);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResetPuzzle()
    {
        for (int i = 0; i < immediateChildren.Count; i++)
        {
            immediateChildren[i].transform.position = initialPositions[i];
        }
    }
}
