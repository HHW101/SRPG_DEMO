using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class Grid : MonoBehaviour
{
    Tile[,] grid;
    [SerializeField] private int GridX =10;
    [SerializeField] private int GridY =10;
    [SerializeField] private Tile tilePre;
    private void Awake()
    {
        makeMap();
    }

    private void makeMap()
    {
        Tile[,] grid = new Tile[GridX, GridY];
        for (int i = 0; i < GridX; i++)
        {
            for (int j = 0; j < GridY; j++)
            {
                grid[i, j] = Instantiate(tilePre, new Vector3(i*2, 0, j*2), Quaternion.Euler(new Vector3(90, 0, 0)));
                grid[i, j].SetCoord(i, j);
            }
        }
    }



}
