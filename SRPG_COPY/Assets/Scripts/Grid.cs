using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class Grid : MonoBehaviour
{
    [SerializeField] private Tile[,] grid;
    [SerializeField] private int GridX =10;
    [SerializeField] private int GridY =10;
    [SerializeField] private Tile tilePre;
    [SerializeField] private Player PlayerPre;
    [SerializeField] private float speed=2.0f;
    [SerializeField] private int runAble =3;
    private Tile selectTile;
   
    private HashSet<Tile> goTiles = new HashSet<Tile>();
    private Player player;
    private int TileX;
    private int TileY;
    private bool isRunning;
    public static Grid instance;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        isRunning = false;
        grid = new Tile[GridX, GridY];
        makeMap();
        SetPlayer(0,0);
        
    }
    private void Update()
    {
        if (selectTile==null&&Input.anyKeyDown){
            selectTile = grid[0 , 0];
            selectTile.SetPState(Tile.PState.Select);
            ShowSetTile();
            getCango(0, 0);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectTile.getX() < GridX - 1)
                ShiftSelect(1, 0);
            ShowSetTile();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectTile.getX() > 0)
                ShiftSelect(-1, 0);
            ShowSetTile();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectTile.getY() < GridY - 1)
                ShiftSelect(0, 1);
            ShowSetTile();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectTile.getY() > 0)
               ShiftSelect(0,-1);
            
            ShowSetTile();
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            isRunning = true;
            ClickTile();
        
        }

    }
    public void ShiftSelect(int x,int y)
    {
        if (!goTiles.Contains(grid[selectTile.getX() + x, selectTile.getY() + y]))
            return;
        if (goTiles.Contains(selectTile))
            selectTile.SetPState(Tile.PState.CanGO);
        else
            selectTile.SetPState(Tile.PState.Idle);
 
        selectTile = grid[selectTile.getX()+x, selectTile.getY() + y];
        selectTile.SetPState(Tile.PState.Select);
    }
    public void getCango(int x,int y)
    {
        int cnt = runAble;
        if (goTiles != null)
        {
            foreach (Tile t in goTiles)
            {
                if(t!=null)
                t.SetPState(Tile.PState.Idle);
            }
        }
        goTiles.Clear();
       
        goTiles.Add(getTile(x, y));
       
        for(int dx=-cnt;dx<=cnt; dx++)
        {
            for(int dy=-cnt;dy<=cnt; dy++)
            {
                if (math.abs(dx) + math.abs(dy) <= cnt) 
                if (getTile(x+dx,y+dy) != null)
                    goTiles.Add(getTile(x+dx,y+ dy));
                
            }
        }
        
        if (goTiles != null)
        {
            foreach (Tile t in goTiles)
            {
        
                if (t != null)
                    t.SetPState(Tile.PState.CanGO);
            }
        }
        

    }
    private void Cango(int x,int y,int cnt)
    {
        if (cnt == 0) return;
        for(int i = -1; i <= 1; i += 2)
        {
            
                if (getTile(x + i, y) != null )
                {
                    goTiles.Add(getTile(x + i, y));
                    Cango(x + i, y, cnt - 1);
            }
        }
        for (int i = -1; i <= 1; i += 2)
        {
            if (getTile(x , y+i) != null )
            {
                goTiles.Add(getTile(x, y+i));
                Cango(x , y+i, cnt - 1);
        
            }
        }
    }
    private void ClickTile()
    {
        movePlayer();
        getCango(selectTile.getX(),selectTile.getY());

        //StartCoroutine(MovePlayer(selectTile.getX(), selectTile.getY()));
    }
    private void movePlayer()
    {
        player.GoTo(selectTile.transform.position);
        Pathfinder path = new Pathfinder();
        //path.FindNext(player.playerTIle, selectTile);
        player.playerX=selectTile.getX();
        player.playerY=selectTile.getY();


    }
    public Tile getTile(int x, int y)
    {
        if(x< 0 || y < 0||x>=GridX||y>=GridY) return null;
        if (grid[x, y] == null) return null;
        //if (grid[x,y].Getstate()!=Tile.TileState.Idle) return null;
        return grid[x, y];
    }
   
    private void ShowSetTile()
    {   
       
        Debug.Log("현재 선택 X:"+selectTile.getX()+"Y:"+selectTile.getY());
    }
    private void makeMap()
    {

        for (int i = 0; i < GridX; i++)
        {
            for (int j = 0; j < GridY; j++)
            {
                grid[i, j] = Instantiate(tilePre, new Vector3(i*2, 0, j*2), Quaternion.Euler(new Vector3(90, 0, 0)));
                grid[i, j].SetCoord(i, j);
                Debug.Log(i+"확인"+j+grid[i, j]);
            }
        }
    }

    public void SetPlayer(int x, int y)
    {
        if (player == null)
        {
            player = Instantiate(PlayerPre, new Vector3(x, 0.5f, y), Quaternion.identity);
        }
      
        player.playerX = 0;
        player.playerY = 0;
        player.playerTIle = grid[x, y];
        grid[x,y].Setstate(Tile.TileState.Occupied);

    }



}
