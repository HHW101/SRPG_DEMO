using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{

    [SerializeField] private Tile[,] path;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public Tile FindNext(Tile tile)
    {
        return path[0,0];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
