using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public GameObject ground;
    public bool walkable;
    public int gridX;
    public int gridY;

    public Node(GameObject _ground, bool _walkable, int _gridX, int _gridY)
    {
        ground = _ground;
        walkable = _walkable;
        gridX = _gridX;
        gridY = _gridY;
    }
}
