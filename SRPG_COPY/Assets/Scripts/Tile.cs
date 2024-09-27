using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{

     private float tileX;
     private float tileY;
   
    public void SetCoord(float x, float y)
    {
        tileX = x;
        tileY = y;
    }

}
