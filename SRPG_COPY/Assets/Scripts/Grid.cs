using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
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
    private Tile selectTile;
    private Player player;
    private int TileX;
    private int TileY;
private bool isRunning;
  
    private void Awake()
    {
        isRunning = false;
        grid = new Tile[GridX, GridY];
        makeMap();
        SetPlayer(0,0);
    }
    private void Update()
    {
        if (selectTile==null&&Input.anyKeyDown){
            selectTile = grid[0 , 0];
            ShowSetTile();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if(selectTile.getX()< GridX-1)
                selectTile = grid[selectTile.getX()+1, selectTile.getY()];
            ShowSetTile();
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (selectTile.getX() > 0)
                selectTile = grid[selectTile.getX() - 1, selectTile.getY()];
            ShowSetTile();
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (selectTile.getY() < GridY - 1)
                selectTile = grid[selectTile.getX(), selectTile.getY()+1];
            ShowSetTile();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (selectTile.getY() > 0)
                selectTile = grid[selectTile.getX(), selectTile.getY()-1];
            ShowSetTile();
        }
        if (Input.GetKeyDown(KeyCode.Z)) {
            isRunning = true;
            ClickTile();
        
        }

    }
    private void ClickTile()
    {
        movePlayer();
        //StartCoroutine(MovePlayer(selectTile.getX(), selectTile.getY()));
    }
    private void movePlayer()
    {
        int pX = selectTile.getX()-player.playerX;
        int pY = selectTile.getY()-player.playerY;
        if (pX > 0)
        {
            for (int i = 0; i < pX; i++)
            {
                player.GoTo(2);
                Debug.Log("우");
            }
        }
        else
        {
            for (int i = 0; i < -pX; i++)
            {
                player.GoTo(3);
                Debug.Log("좌");
            }
        }
        if (pY > 0)
        {
            for (int i = 0; i < pY; i++)
            {
                player.GoTo(0);
                Debug.Log("전진");
            }
        }
        else
        {
            for (int i = 0; i < -pY; i++)
            {
                player.GoTo(1);
                Debug.Log("후진");
            }
        }
        player.playerX=selectTile.getX();
        player.playerY=selectTile.getY();


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
            player = Instantiate(PlayerPre, new Vector3(x, 1, y), Quaternion.identity);
        }
      
        player.playerX = 0;
        player.playerY = 0;
        grid[x,y].Setstate(Tile.TileState.Occupied);

    }



}
