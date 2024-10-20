using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AState_P : UnitState
{
    // Start is called before the first frame update
    FPlayer player;
    public AState_P(FUnit player) : base(player)
    {
        this.player = (FPlayer)player;
    }
    private Vector3 originPos, targetPos;

    public Tile selectTile;
    // Start is called before the first frame update
    public override void DoingState()
    {
        //Å½»ö
    }
    public override void EnterState()
    {
        player.Attack(selectTile.on);
    }
    public override void ExitState()
    {
        //ÀÌµ¿
    }
     public void getClick()
    {

    }
    
}
