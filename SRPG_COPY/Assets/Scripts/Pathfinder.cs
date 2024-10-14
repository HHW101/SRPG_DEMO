using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{

    [SerializeField]
    public Tile startTile;
    public Tile endTile;

    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void FindNext(Tile startTile, Tile endTile)
    {
        Tile currentTIle=startTile;
        List<Tile> openTiles = new List<Tile>();
        HashSet<Tile> closedTiles = new HashSet<Tile>();
        openTiles.Add(startTile);
        bool fin = false;
        while (!fin)
        {
            for (int i = -1; i <= 1; i+=2)
            {
                for (int j = -1; j <= 1; j+=2)
                {
                    if (Grid.instance.getTile(currentTIle.getX()+i, currentTIle.getY()+j) != null)
                        openTiles.Add(Grid.instance.getTile(currentTIle.getX()+i, currentTIle.getY()+j));
                }
            }
        }
        return;
    }
    int GetDistance(Tile tile) {
        return 10; 
    
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
