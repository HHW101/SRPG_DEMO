using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

    private int tileX;
    private int tileY;
    public int gCost;
    public int hCost;
    public bool canGo;
    public int moveCost = 1;
    public GameObject selectTile;
    public GameObject selectTilePre;
    private GameObject goTile;
    public GameObject goTilePre;
    public Tile parent;
    public GameObject on;
    [SerializeField]
    public bool upB= false;
    [SerializeField]
    public bool downB = false;
    [SerializeField] public bool leftB = false;
    [SerializeField] public bool rightB  = false;
    public int fCost { get {  return gCost+hCost; } }
     public enum TileState
    {
        Idle,
        Block,
        Occupied

    }
    public bool getDir(int i)
    {
        //상 하 우 좌 순서
        switch (i)
        {
            case 0:
                return upB;
               
            case 1:
                return downB;
                case 2:
                return rightB;
            case 3:
                return leftB;
            default:
                return false;
        }
    }
    void Update()
    {
        if (state != TileState.Occupied)
            on = null;
    }
    public enum PState
    {
        Idle,
        CanGO,
        Select

    }
    public TileState state =TileState.Idle;
    public PState pState;
    public void SetCoord(int x, int y)
    {
        tileX = x;
        tileY = y;
        goTile = Instantiate(goTilePre, gameObject.transform.position + new Vector3(0, 0.1f, 0), Quaternion.Euler(new Vector3(90,0,0)));
        goTile.SetActive(false);
        selectTile = Instantiate(selectTilePre, gameObject.transform.position + new Vector3(0, 0.1f, 0), Quaternion.Euler(new Vector3(90, 0, 0)));
        selectTile.SetActive(false);
        //Setstate(TileState.Idle);
    }
    public void SetPState(PState _pState)
    {
        pState = _pState;
        switch (pState) { 
        case PState.Idle:
                goTile.SetActive(false);
                selectTile.SetActive(false);
                break;
            case PState.Select:
                goTile.SetActive(false);
                selectTile.SetActive(true);
                break;
            case PState.CanGO:
                goTile.SetActive(true);
                selectTile.SetActive(false);
                break;
        }
       
    }
    public int getX()
    {
        return tileX;
    }
    public int getY() { 
        return tileY;
    }

    public void Setstate(TileState _state) {
        state = _state;
        if(state!=TileState.Occupied)
            on = null;
    }
    public TileState Getstate()
    {
        return state; 
    }


}
