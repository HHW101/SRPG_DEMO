using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MTstate_P : UnitState
{
    // Start is called before the first frame update
    FPlayer player;
    public MTstate_P(FUnit player) : base(player)
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
        Debug.Log("MTµé¾î°¨");
        Thinkmove();
    }
    public override void ExitState()
    {
        Debug.Log("MT³ª¿È");
    }
    public void Thinkmove()
    {
        // setGo(player[NowPNum].RangeTiles);
        player.GetRange(Pathfinder.PathMode.pM);
        GameManager.instance.setGo(player.RangeTiles);
        UIManager.instance.battleMenu.SetActive(false);
        GameManager.instance.setSelect(player.unitTIle);
        GameManager.instance.ChangeInputMode(GameManager.InputMode.Player);
    }

}




