using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_P : UnitState
{
    // Start is called before the first frame update
    FPlayer player;
    public IdleState_P(FUnit player) : base(player)
    {
        this.player = (FPlayer)player;
    }
    private Vector3 originPos, targetPos;

    public Tile selectTile;
    // Start is called before the first frame update
    public override void DoingState()
    {
       
    }
    public override void EnterState()
    {
        if(!player.CanAttack())
            GameManager.instance.PlayerTurnChange();
    }
    public override void ExitState()
    {
        
        GameManager.instance.ChangeInputMode(GameManager.InputMode.Player);
        UIManager.instance.InfoUI.SetActive(true);
    }
    public void getClick()
    {

    }
}
