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
    [SerializeField] private Vector2[] playerPos = new Vector2[1];
    [SerializeField] private Vector2[] obspawnPos = new Vector2[5];
    [SerializeField] private GameObject obspre;
    private Tile selectTile;
    public Camera cam;
    private Player[] player=new Player[1];
    private Monster[] monster=new Monster[3];
    private int TileX;
    private int TileY;
    private bool isRunning;
    public static GameManager instance;
    Pathfinder path;
    private int monNum;
    private int playerNum;
    public TurnState turn;
    private int NowPNum =-1;
    private int NowMNum =-1;
    public enum TurnState
    {
        start, playerTurn, enemyTurn, end
    }

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
        grid = new Tile[GridX, GridY];
        makeMap();
        SetPlayer();
        SetMonster();
        path = new Pathfinder();
        monNum = 3;
        playerNum = 1;
    }
    public void TurnChange(TurnState state)
    {
        turn = state;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            TurnChange(TurnState.playerTurn);
        if (Input.GetKeyDown(KeyCode.X))
            TurnChange(TurnState.enemyTurn);
     
    }
    private void StartGame()
    {
        setSelect(player[NowPNum].unitTIle);
    }
    public void ClickTile()
    {
        Debug.Log(NowPNum);
        if (player[NowPNum].state==UnitP.UnitState.MoveThink)
            player[NowPNum].movePlayer(selectTile);
    }
    public void setSelect(Tile t)
    {
        if (selectTile != null)
            selectTile.Setstate(Tile.TileState.Idle);
        selectTile = t;
        selectTile.SetPState(Tile.PState.Select);
        cam.ChangeTarget(player[NowPNum].gameObject);
    }
    public void goToTile(Tile tile)
    {
        selectTile.SetPState(Tile.PState.Idle);
        selectTile = tile;
        selectTile.SetPState(Tile.PState.Select);
        UIManager.instance.ShowTile(selectTile);
    }
    public void ShiftSelect(int x,int y,int mode) // 0: 맵 기준 타일 선택 1: 플레이어 기준 타일 선택
    {
            
        int nx=selectTile.getX()+x;
        int ny=selectTile.getY()+y;
        Debug.Log($"{nx}:{ny}");
        if (getTile(nx,ny) == null)
            return;
        if (mode==1&&!player[NowPNum].RangeTiles.Contains(grid[nx, ny]))
            return;
        if (mode == 1 && player[NowPNum].RangeTiles.Contains(selectTile))
            selectTile.SetPState(Tile.PState.CanGO);
        else
            selectTile.SetPState(Tile.PState.Idle);
 
        selectTile = grid[nx, ny];
        selectTile.SetPState(Tile.PState.Select);
        UIManager.instance.ShowTile(selectTile);
    }
    public void startmove()
    {
        setGo(player[NowPNum].RangeTiles);
        UIManager.instance.battleMenu.SetActive(false);
    }
    public void setGo(HashSet<Tile> tiles)
    {
      
        if (tiles != null)
        {
            foreach (Tile t in tiles)
            {

                if (t != null)
                    t.SetPState(Tile.PState.CanGO);
            }
        }
        selectTile.SetPState(Tile.PState.Select);
    }
 
    
    public void PlayerTurnChange()
    {
        setSelect(player[++NowPNum].unitTIle);
        player[NowPNum].GetRange(Pathfinder.PathMode.pM);
        UIManager.instance.ShowBMenu(player[NowPNum]);
        Debug.Log($"{NowPNum}:{player[NowPNum].RangeTiles.Count}");

    }
    private void MoveTile()
    {
        HashSet<Tile> goTiles = player[NowPNum].RangeTiles;
        if (goTiles != null)
        {
            foreach (Tile t in goTiles)
            {
                if (t != null)
                    t.SetPState(Tile.PState.Idle);
            }
        }
        goTiles.Clear();
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

    public void SetPlayer() {
        for (int i = 0; i <playerPos.Length; i++)
        {

            int x = (int)playerPos[i].x;
            int y = (int)playerPos[i].y;
            player[i] = Instantiate(PlayerPre, new Vector3(x * 4, 0.2f, y * 4), Quaternion.identity);
            player[i].unitTIle = grid[x, y];
            grid[x, y].Setstate(Tile.TileState.Occupied);
            grid[x, y].on = player[i].gameObject;
            player[i].unitNum = i;

            
            player[i].SetDirection(Monster.Direction.right);
         
        }
        //
        //    if (player == null)
        //    {
        //        player = Instantiate(PlayerPre, new Vector3(x * 4, 0.2f, y * 4), Quaternion.identity);
        //    }


        //    player.unitTIle = grid[x, y];
        //    grid[x,y].Setstate(Tile.TileState.Occupied);
        //    grid[x,y].on = player.gameObject;
        //    player.SetDirection(UnitP.Direction.right);


    }
    public Player FIndPlayer()
    {
        return player[NowPNum];
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
            monster[i].unitNum = i;
   
            if (x > y)
                monster[i].SetDirection(Monster.Direction.left);
            else
                monster[i].SetDirection(Monster.Direction.down);
        }
       
    }
  


}
