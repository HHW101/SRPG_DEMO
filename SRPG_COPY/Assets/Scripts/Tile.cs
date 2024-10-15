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
    public GameObject selectTile;
    public GameObject selectTilePre;
    private GameObject goTile;
    public GameObject goTilePre;
    public Tile parent;
    public GameObject on;
    public int fCost { get {  return gCost+hCost; } }
     public enum TileState
    {
        Idle,
        Block,
        Occupied

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
    public TileState state;
    public PState pState;
    public void SetCoord(int x, int y)
    {
        tileX = x;
        tileY = y;
        goTile = Instantiate(goTilePre, gameObject.transform.position + new Vector3(0, 0.1f, 0), gameObject.transform.rotation);
        goTile.SetActive(false);
        selectTile = Instantiate(selectTilePre, gameObject.transform.position + new Vector3(0, 0.1f, 0), gameObject.transform.rotation);
        selectTile.SetActive(false);
        Setstate(TileState.Idle);
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
    }
    public TileState Getstate()
    {
        return state; 
    }


}
