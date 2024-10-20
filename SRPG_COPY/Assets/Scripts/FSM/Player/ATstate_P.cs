using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ATstate_P : UnitState
{
    // Start is called before the first frame update
    FPlayer player;
    public ATstate_P(FUnit player):base(player)
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
        ThinkAttack();
    }
    public override void ExitState()
    {
        //ÀÌµ¿
    }
    public void ThinkAttack()
    {
        // setGo(player[NowPNum].RangeTiles);
        player.GetRange(Pathfinder.PathMode.pA);
        GameManager.instance.setGo(player.RangeTiles);
        UIManager.instance.HideBMenu();
        GameManager.instance.setSelect(player.unitTIle);
        GameManager.instance.ChangeInputMode(GameManager.InputMode.Player);
    }

}
