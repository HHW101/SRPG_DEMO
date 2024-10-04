using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

     private int tileX;
     private int tileY;
     public enum TileState
    {
        Idle,
        Block,
        Occupied

    }
    public TileState state;
    public void SetCoord(int x, int y)
    {
        tileX = x;
        tileY = y;
        Setstate(TileState.Idle);
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



}
