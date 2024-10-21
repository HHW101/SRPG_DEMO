using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.Mathematics;
using Unity.VisualScripting;

using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using static UnitP;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    
    [SerializeField] private Tile[,] grid;
    [SerializeField] private int GridX =10;
    [SerializeField] private int GridY =10;
    [SerializeField] private Tile tilePre;
    [SerializeField] private FPlayer PlayerPre;
    [SerializeField] private FMonster MonsterPre;
    [SerializeField] private float speed=2.0f;
    [SerializeField] private Vector2[] monspawnPos = new Vector2[5];
    [SerializeField] private Vector2[] playerPos = new Vector2[1];
    [SerializeField] private Vector2[] obspawnPos = new Vector2[5];
    [SerializeField] private GameObject obspre;
    private Tile selectTile;
    private FPlayer selectPlayer;
    private FMonster selectMonster;
    public Camera cam;
    private List<FPlayer> player = new List<FPlayer>();
    private List<FMonster> monster = new List<FMonster>();
    private int TileX;
    private int TileY;
    private bool isRunning;
    public static GameManager instance;
    Pathfinder path;
    public TurnState turn;
    public int turnN=0;
    //private int NowPNum =-1;
    private int NowMNum =-1;

    public enum TurnState
    {
        start, playerTurn, enemyTurn, end
    }
    public InputMode inputmode;
    public enum InputMode
    {
        Player, Map, Menu, block
    }
    public void ChangeInputMode(InputMode mode)
    {
        inputmode = mode;
    }
    public Vector3 SelectCamera()
    {
        return selectTile.transform.position;
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
    public void StartTurn()
    {
        turnN++;
        Debug.Log(turnN);
        foreach(FPlayer p in player)
            p.TReset();
        foreach(FMonster m in monster)
            m.TReset();
        TurnChange(TurnState.playerTurn);
    }
    public FMonster getselectMonster()
    {
        return selectMonster;
    }
    public FPlayer getSelectPlayer()
    {
        return selectPlayer;
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
       
        GameStart();
        path = new Pathfinder();
        inputmode = InputMode.Map;
    }
   void Start()
    {
       
    }
    public void GameStart()
    {
        UIManager.instance.HideInfo();
        turn=TurnState.start;
        turnN = 0;
        SetPlayer();
        SetMonster();
        StartTurn();
    }
    public void TurnChange(TurnState state)
    {
        turn = state;
        if(state==TurnState.enemyTurn)
            ChangeInputMode(InputMode.block);
    }
  
  
    public void getClick()
    {
        selectPlayer.getClick(selectTile);
    }
    public void getCancel()
    {
        if (selectPlayer.getCancel())
        {
            MoveTile();
            setSelect(selectPlayer.unitTIle);
            selectPlayer = null;
            ChangeInputMode(InputMode.Map);
           
        }
    }

    public void setSelect(Tile t)
    {
        if (selectTile != null)
            selectTile.SetPState(Tile.PState.Idle);
        selectTile = t;
        selectTile.SetPState(Tile.PState.Select);
     }

    public void ShiftSelect(int x,int y,int mode) // 0: 맵 기준 타일 선택 1: 플레이어 기준 타일 선택
    {
        UIManager.instance.HideInfo();
            
        int nx=selectTile.getX()+x;
        int ny=selectTile.getY()+y;
      
        if (getTile(nx,ny) == null)
            return;
        if (mode==1&&!selectPlayer.RangeTiles.Contains(grid[nx, ny]))
            return;
        if (mode == 1 && selectPlayer.RangeTiles.Contains(selectTile))
        {
            selectTile.SetPState(Tile.PState.CanGO);
            Debug.Log($"{nx}:{ny}확인{selectPlayer.RangeTiles.Count}");
        }
        else
        {
            selectTile.SetPState(Tile.PState.Idle);
            Debug.Log($"{nx}:{ny}2");
        }
        selectTile = grid[nx, ny];
        selectTile.SetPState(Tile.PState.Select);
      
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
        
        if (IsPend()!=null)
        {
            selectPlayer = null;
            setSelect(IsPend().unitTIle);
           
            ChangeInputMode(InputMode.Map);
        }
        else
        {
            selectPlayer = null;
            TurnChange(TurnState.enemyTurn);
            Debug.Log("적턴 시작");
            enemyTurnChange();
            //NowPNum=-1;
        }
    }
   
    public FPlayer IsPend()
    {
        foreach(FPlayer p in player)
        {
            if(p.moveC!=0)
                return p;
        }
        return null;
    }
    public void SelectPlayer()
    {   
         
        Debug.Log("선택");
        if (selectTile.on == null)
        {
            UIManager.instance.ShowTile(selectTile);
        }
        else if (selectTile.on.GetComponent<FPlayer>() != null&& selectTile.on.GetComponent<FPlayer>().atkC>0)
        {
            selectPlayer = selectTile.on.GetComponent<FPlayer>();
            setSelect(selectPlayer.unitTIle);
            //  UIManager.instance.ShowBMenu(selectPlayer);
            //Debug.Log($"{NowPNum}:{player[NowPNum].RangeTiles.Count}");
            cam.ChangeTarget(selectPlayer.gameObject);
           
        }
        else
        {
            UIManager.instance.ShowMonster(selectTile.on.GetComponent<FUnit>());
        }
            //ChangeInputMode(GameManager.InputMode.Player);
        
    }
    public void Attackstart()
    {
        selectPlayer.GetRange(Pathfinder.PathMode.pA);
        setGo(selectPlayer.RangeTiles);
        ChangeInputMode (GameManager.InputMode.Player);
    }
    private int GetAbleMonN()
    {
        int cnt = 0;
        foreach(FMonster mon in monster)
        {
            if (mon.atkC > 0)
                cnt++;
        }
        return cnt;
    }
    public void enemyTurnChange()
    {


        if (IsMend()!=null)
        {
            //setSelect(monster[++NowMNum].unitTIle);
            selectMonster=IsMend();
           
        }
        else
        {

            selectMonster = null;
            StartTurn();
            PlayerTurnChange();
            NowMNum = -1;
        }
    }
    public FMonster IsMend()
    {
        foreach (FMonster p in monster)
        {
            if (p.atkC != 0)
                return p;
        }
        return null;
    }
    public void MoveTile()
    {
        HashSet<Tile> goTiles = selectPlayer.RangeTiles;
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
   public int Isgameend()
    {
        if (player.Count == 0)
            return 0;
        if(monster.Count == 0)
            return 1;
        return -1;
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

        foreach (FPlayer p in player)
        {
            Destroy(p);
        }
        player.Clear();
        for (int i = 0; i <playerPos.Length; i++)
        {

            int x = (int)playerPos[i].x;
            int y = (int)playerPos[i].y;
            player.Add(Instantiate(PlayerPre, new Vector3(x * 4, 0.2f, y * 4), Quaternion.identity));
            player[i].unitTIle = grid[x, y];
            grid[x, y].Setstate(Tile.TileState.Occupied);
            grid[x, y].on = player[i].gameObject;
            player[i].unitNum = i;
           
            player[i].SetDirection(FUnit.Direction.right);
            player[i].GReset();
        }
        setSelect(player[0].unitTIle);
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
    public List<FPlayer> FIndPlayer()
    {
        return player;
    }

    public void SetMonster()
    {
        foreach (FMonster mon in monster)
        {
            Destroy(mon);
        }
        monster.Clear();
        for (int i = 0; i < monspawnPos.Length; i++)
        {
            
            int x = (int)monspawnPos[i].x;
            int y = (int)monspawnPos[i].y;
            monster.Add(Instantiate(MonsterPre, new Vector3(x*4, 0.2f, y*4), Quaternion.identity));
            monster[i].unitTIle = grid[x, y];
            grid[x, y].Setstate(Tile.TileState.Occupied);
            grid[x, y].on = monster[i].gameObject;
            monster[i].unitNum = i;
            monster[i].GReset();
            if (x > y)
                monster[i].SetDirection(FUnit.Direction.left);
            else
                monster[i].SetDirection(FUnit.Direction.down);
        }
       
    }
    public void RemoveUnit(GameObject unit)
    {
        if(unit.GetComponent<FPlayer>() != null)
            player.Remove(unit.GetComponent<FPlayer>());
        else
            monster.Remove(unit.GetComponent<FMonster>());
        if(Isgameend()!=-1)
            Gameend(Isgameend());

    }
    public void Gameend(int x)
    {
        TurnChange(TurnState.end);
        foreach(FMonster m in monster)
        {
            m.end();
        }
        foreach (FPlayer p in player)
        {
            p.end();
        }
        ChangeInputMode(InputMode.block);
        UIManager.instance.HideBS();
        
        if(x==0)
            UIManager.instance.end(false);
        else
            UIManager.instance.end(true);
    }

}
