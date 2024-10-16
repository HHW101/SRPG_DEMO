using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] private Tile[,] grid;
    [SerializeField] private int GridX =10;
    [SerializeField] private int GridY =10;
    [SerializeField] private Tile tilePre;
    [SerializeField] private Player PlayerPre;
    [SerializeField] private Monster MonsterPre;
    [SerializeField] private float speed=2.0f;
    [SerializeField] private Vector2[] monspawnPos = new Vector2[5];
    [SerializeField] private Vector2[] obspawnPos = new Vector2[5];
    [SerializeField] private GameObject obspre;
    public bool canClick =true;
    private Tile selectTile;
    public Camera cam;
    private HashSet<Tile> goTiles = new HashSet<Tile>();
    private Player player;
    private Monster[] monster=new Monster[3];
    private int TileX;
    private int TileY;
    private bool isRunning;
    public static GameManager instance;
    Pathfinder path;


    public void resetF()
    {
        foreach(var tile in grid)
        {
            if (tile == null)
                continue;
            tile.gCost = 0;
            tile.hCost = 0;
            tile.parent = null;
        }
    }
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
        SetPlayer(0,2);
        SetMonster();
        path = new Pathfinder();
    }
    private void Update()
    {
        if (!canClick)
            return;
        switch (TurnManager.instance.turn)
        {
            case TurnManager.TurnState.pMoveTurn:
                
                GameManager.instance.cam.ChangeTarget(player.gameObject);
                Moving();
                
                break;
            case TurnManager.TurnState.pAttackTurn:
                Attack();
               
                break;
        }
    }

    public void Attack()
    {
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (selectTile.getX() < GridX - 1)
                ShiftSelect(1, 0);
       
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectTile.getX() > 0)
                ShiftSelect(-1, 0);
          
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectTile.getY() < GridY - 1)
                ShiftSelect(0, 1);
        
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectTile.getY() > 0)
                ShiftSelect(0, -1);
            ;
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!isRunning)
            {
                selectTile.SetPState(Tile.PState.Select);
            
                player.GetRange(UnitP.mode.attack);
                isRunning = true;
            }
            else if(selectTile.state==Tile.TileState.Occupied)
            {
                player.Attack(selectTile.on);
                canClick = false;
                isRunning = false;
            }
        }
      
    }
    private void Moving()
    {
        if (selectTile == null && Input.anyKeyDown)
        {
            setSelect(player.unitTIle);
            
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
                ShiftSelect(1, 0);
          
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
                ShiftSelect(-1, 0);
            
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
                ShiftSelect(0, 1);
            
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
                ShiftSelect(0, -1);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!isRunning)
            {
                selectTile.SetPState(Tile.PState.Select);
                //getCango(selectTile.getX(), selectTile.getY());
                player.GetRange(UnitP.mode.move);
                isRunning = true;
            }
            else
            {
                ClickTile();
                isRunning = false;
                canClick = false;
            }
        }
    }
    public void setSelect(Tile t)
    {
        selectTile = t;
        selectTile.SetPState(Tile.PState.Select);
    }
    public void ShiftSelect(int x,int y)
    {
        int nx=selectTile.getX()+x;
        int ny=selectTile.getY()+y;
        if (nx<0||ny<0|| nx>GridX||ny>GridY|| grid[nx,ny] == null)
            return;
        if (!goTiles.Contains(grid[nx, ny]))
            return;
        if (goTiles.Contains(selectTile))
            selectTile.SetPState(Tile.PState.CanGO);
        else
            selectTile.SetPState(Tile.PState.Idle);
 
        selectTile = grid[nx, ny];
        selectTile.SetPState(Tile.PState.Select);
        UIManager.instance.ShowTile(selectTile);
    }
    public HashSet<Tile> GetRange(int x, int y, int cnt)
    {
        HashSet<Tile> range = new HashSet<Tile>();
        Debug.Log($"{x}:{y}{getTile(x, y)}");
        range= path.Range(getTile(x,y), cnt);
        //range.Add(getTile(x, y));

        //for (int dx = -cnt; dx <= cnt; dx++)
        //{
        //    for (int dy = -cnt; dy <= cnt; dy++)
        //    {
        //        if (math.abs(dx) + math.abs(dy) <= cnt)
        //            if (getTile(x + dx, y + dy) != null)
        //                range.Add(getTile(x + dx, y + dy));

        //    }
        //}
        return range;
    }
    public void setGo(HashSet<Tile> tiles)
    {
        if (goTiles != null)
        {
            foreach (Tile t in goTiles)
            {
                if (t != null)
                    t.SetPState(Tile.PState.Idle);
            }
        }
        goTiles.Clear();
        goTiles = tiles;
        if (goTiles != null)
        {
            foreach (Tile t in goTiles)
            {

                if (t != null)
                    t.SetPState(Tile.PState.CanGO);
            }
        }
        selectTile.SetPState(Tile.PState.Select);
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
        TurnManager.instance.TurnChange(TurnManager.TurnState.pAttackTurn);
        if (goTiles != null)
        {
            foreach (Tile t in goTiles)
            {
                if (t != null)
                    t.SetPState(Tile.PState.Idle);
            }
        }
        goTiles.Clear();


        //StartCoroutine(MovePlayer(selectTile.getX(), selectTile.getY()));
    }
    private void movePlayer()
    {
        //player.GoTo(selectTile.transform.position);
        
        List<Tile> temp = new List<Tile>();    
        temp=path.FindNext(player.unitTIle, selectTile);
        if (temp != null)
            player.GoTo(temp);
        player.unitX=selectTile.getX();
        player.unitY=selectTile.getY();
       

    }
    public Tile getTile(int x, int y)
    {
        if(x< 0 || y < 0||x>=GridX||y>=GridY) return null;
       
        if (grid[x, y] == null) return null;

        //if (grid[x,y].Getstate()!=Tile.TileState.Idle&&TurnManager.instance.turn!=TurnManager.TurnState.pAttackTurn) return null;
        if (grid[x, y].Getstate() == Tile.TileState.Block) {
            Debug.Log(x + " "+y);
            return null; }
        return grid[x, y];
    }
   
  
    private void makeMap()
    {
      
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");
     
        foreach (GameObject tile in tiles) {
           
            Vector3 tilePositon = tile.transform.position;
            int x= Mathf.RoundToInt(tilePositon.x)/4;
            int y= Mathf.RoundToInt(tilePositon.z)/4;
           
            if (x >= 0 && x < GridX && y >= 0 && y < GridY)
            {
                // 타일을 2차원 배열에 저장
                grid[x, y] = tile.GetComponent<Tile>();
                grid[x, y].SetCoord(x, y);
            }
        }
        // 좌표값을 출력하여 확인
        //for (int i = 0; i < GridX; i++)
        //{
        //    for (int j = 0; j < GridY; j++)
        //    {
        //        if (grid[i, j] != null)
        //        {
        //            Debug.Log($"Tile at grid[{i}, {j}] is {grid[i, j].name}");
        //        }
        //    }
        //}
        //for (int i = 0; i < GridX; i++)
        //{
        //    for (int j = 0; j < GridY; j++)
        //    {

        //        grid[i, j] = Instantiate(tilePre, new Vector3(i*2, 0, j*2), Quaternion.Euler(new Vector3(90, 0, 0)));
        //        grid[i, j].SetCoord(i, j);
        //        Debug.Log(i+"확인"+j+grid[i, j]);
        //    }
        //}
        //foreach(Vector2 i in obspawnPos)
        //{
        //    grid[(int)i.x,(int)i.y].Setstate(Tile.TileState.Block);
        //    Instantiate(obspre, grid[(int)i.x, (int)i.y].gameObject.transform.position + new Vector3(0, 0.1f, 0),quaternion.identity);
        //}
    }

    public void SetPlayer(int x, int y)
    {
        if (player == null)
        {
            player = Instantiate(PlayerPre, new Vector3(x * 4, 0.2f, y * 4), Quaternion.identity);
        }
      
        player.unitX = x; player.unitY = y;
        
        player.unitTIle = grid[x, y];
        grid[x,y].Setstate(Tile.TileState.Occupied);
        grid[x,y].on = player.gameObject;
        player.SetDirection(UnitP.Direction.right);

    }
    public Player FIndPlayer()
    {
        return player;
    }
    public void SetMonster()
    {
        
        for (int i = 0; i < monspawnPos.Length; i++)
        {
            
            int x = (int)monspawnPos[i].x;
            int y = (int)monspawnPos[i].y;
            monster[i] = Instantiate(MonsterPre, new Vector3(x*4, 0.2f, y*4), Quaternion.identity);
            monster[i].unitTIle = grid[x, y];
            grid[x, y].Setstate(Tile.TileState.Occupied);
            grid[x, y].on = monster[i].gameObject;
            monster[i].monsterNum = i;
            monster[i].unitX = x;
            monster[i].unitY = y;
            if (x > y)
                monster[i].SetDirection(Monster.Direction.left);
            else
                monster[i].SetDirection(Monster.Direction.down);
        }
       
    }
  


}
