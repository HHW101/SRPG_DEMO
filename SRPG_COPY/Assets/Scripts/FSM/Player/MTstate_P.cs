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
        UIManager.instance.ShowInfoMenu(player);
        Thinkmove();
    }
    public override void ExitState()
    {
        UIManager.instance.HideBMenu();
        Debug.Log("MT");
    }
    public void Thinkmove()
    {
        // setGo(player[NowPNum].RangeTiles);
        player.GetRange(Pathfinder.PathMode.pM);
        GameManager.instance.setGo(player.RangeTiles);
        UIManager.instance.InfoUI.SetActive(false);
        GameManager.instance.setSelect(player.unitTIle);
        GameManager.instance.ChangeInputMode(GameManager.InputMode.Player);
    }

}




