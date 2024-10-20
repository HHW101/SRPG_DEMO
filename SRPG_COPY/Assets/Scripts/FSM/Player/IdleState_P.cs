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
        Debug.Log("idle��");
    }
    public override void ExitState()
    {
        Debug.Log("idle����");
        GameManager.instance.ChangeInputMode(GameManager.InputMode.Player);
        UIManager.instance.battleMenu.SetActive(true);
    }
    public void getClick()
    {

    }
}
