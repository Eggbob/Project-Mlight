using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public GameObject groundPrefab;
    GameObject parentGrid;

    public Vector2 gridWorldSize;

    Node[,] grid;

    public void CreateGrid()
    {
        if (parentGrid != null)
            Destroy(parentGrid);
        parentGrid = new GameObject("parentGrid");

        grid = new Node[(int)gridWorldSize.x, (int)gridWorldSize.y];
        Vector3 worldBottomLeft = Vector3.zero - Vector3.right * gridWorldSize.x / 2 - Vector3.forward
            * gridWorldSize.y/2;

        for(int x = 0; x< (int)gridWorldSize.x; x++)
        {
            for(int y=0; y< (int)gridWorldSize.y; y++)
            {
                Vector3 worldPoint = worldBottomLeft + Vector3.right * (x + 0.5f) + Vector3.forward *
                    (y + 0.5f);

                GameObject obj = Instantiate(groundPrefab, worldPoint, Quaternion.Euler(90, 0, 0));
                obj.transform.parent = parentGrid.transform;
                grid[x, y] = new Node(obj, true, x, y);
            }
        }
    }
}
