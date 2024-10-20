using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class MState_P : UnitState
{
    FPlayer player;
    public MState_P(FUnit player) : base(player)
    {
        this.player = (FPlayer)player;
    }
    private Vector3 originPos, targetPos;


    // Start is called before the first frame update
    public override void DoingState()
    {
     
    }
    public override void EnterState()
    {
        movePlayer(player.selectTile);
        player.block();
    }
    public override void ExitState()
    {
        
    }
    
  
    public void movePlayer(Tile selectTile)
    {
        Pathfinder path = new Pathfinder();
        GameManager.instance.ChangeInputMode(GameManager.InputMode.block);
        player.unitTIle.Setstate(Tile.TileState.Idle);
        List<Tile> temp = new List<Tile>();
        temp = path.FindNext(player.unitTIle, selectTile, Pathfinder.PathMode.pM);
        player.moveC--;
        if (temp != null)
            player.GoTo(temp);
        foreach (Tile t in player.RangeTiles)
        {
            t.SetPState(Tile.PState.Idle);
        }
    }
}
