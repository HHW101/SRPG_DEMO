using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Grid : MonoBehaviour
{
    Node[,] grid;
    [SerializeField] int length =25;
    [SerializeField] int width =25;
    [SerializeField] float cellSize =1f;

    private void Start()
    {
       GenerateGrid();
    }

    private void GenerateGrid()
    {
        grid = new Node[width, length];
        CheckCanGoNode();
    }
    private void CheckCanGoNode()
    {
        for (int y = 0; y < length; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector3 pos = getWP(x, y);
                int mask = 1 << LayerMask.NameToLayer("Obstacle");
                bool CanGo=!Physics.CheckBox(pos, Vector3.one * cellSize / 2, quaternion.identity, mask);
                grid[x, y] = new Node();
                grid[x, y].canGo = CanGo;
            }
        }
    }
    private Vector3 getWP(int x,int y)
    {
        return new Vector3(transform.position.x + x * cellSize, 0, transform.position.z + y * cellSize);
    }
    private void OnDrawGizmos()
    {
        if(grid == null) { return; }
        for (int y = 0; y < length; y++)
        {
             for(int x = 0; x < width; x++)
            {
                Vector3 pos = getWP(x, y);
                Gizmos.color = grid[x,y].canGo ? Color.white : Color.red;
                Gizmos.DrawCube(pos, Vector3.one * 0.2f);
            }
        }
    }

}
