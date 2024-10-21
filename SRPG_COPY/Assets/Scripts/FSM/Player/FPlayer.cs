using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class FPlayer : FUnit
{
    // Start is called before the first frame update
    public enum UnitState
    {
        Idle, Attack, Move, Hit, Die,AT,MT
    }
    UnitState state;
    bool isClick=false;
    bool isCancel = false;
    void Start()
    {
        fsm = new FSM(new IdleState_P(this));
        state = UnitState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
       
          
        switch (state)
        {
            case UnitState.Idle:
                if (isDie())
                {
                    fsm.ChangeState(new DeadState_P(this));
                    state = UnitState.Die;
                }
                else if (IsActive())
                {
                    fsm.ChangeState(new MTstate_P(this));
                    state = UnitState.MT;
                   
                }
                break;
            case UnitState.MT:
                if (IsClick())
                {
                    fsm.ChangeState(new MState_P(this));
                    isClick = false;
                    state = UnitState.Move;
                }
                else if (IsCancel())
                {
                    fsm.ChangeState(new IdleState_P(this));
                    state = UnitState.Idle;
                    isCancel = false;
                }
                break;
            case UnitState.Move:
                if (IsBlock())
                    break;
                if (!CanMove())
                {
                    fsm.ChangeState(new ATstate_P(this));
                    state = UnitState.AT;
                }
                else
                {
                    fsm.ChangeState(new MTstate_P(this));
                    state= UnitState.MT;
                }
                break;
            case UnitState.AT:
                if (IsClick())
                {
                    fsm.ChangeState(new AState_P(this));
                    isClick = false;
                    state = UnitState.Attack;
                }
                else if (IsCancel())
                {
                     fsm.ChangeState(new IdleState_P(this));
                    state = UnitState.Idle;
                    isCancel = false;
                }
                break;
            case UnitState.Attack:
                if (IsBlock())
                    break;
                if (!CanAttack())
                {
                    fsm.ChangeState(new IdleState_P(this));
                    state = UnitState.Idle;
                }
                else
                {
                    fsm.ChangeState(new ATstate_P(this));
                    state = UnitState.AT;
                }
                break;
           
        }
        fsm.DoingState();
    }
    public bool IsCancel()
    {
        return false;
    }
    public bool getCancel()
    {
        if (state == UnitState.AT || state == UnitState.MT)
        {
            isCancel = true;
            return true;
        }
        else { return false; }

   
    }
    public void end()
    {
        state = UnitState.Idle;
    }
    public void getClick(Tile tile)
    {

        if (RangeTiles.Contains(tile))
        {
            if ((state == UnitState.MT && tile.state == Tile.TileState.Idle)||(state==UnitState.AT&&tile.on.GetComponent<FMonster>()!=null))
            {
                selectTile = tile;
                Debug.Log(selectTile.on);
                isClick = true;
            }
            
        }
        
    }
    public bool IsClick()
    {
        return isClick;
    }
    public bool IsActive()
    {
        if (GameManager.instance.turn == GameManager.TurnState.playerTurn && GameManager.instance.getSelectPlayer()==this)
            return true;
        return false;
    }

}
public class DeadState_P : UnitState
{
    // Start is called before the first frame update
    FPlayer player;
    public DeadState_P(FUnit player) : base(player)
    {
        this.player = (FPlayer)player;
    }
    // Start is called before the first frame update
    public override void DoingState()
    {

    }
    public override void EnterState()
    {
        GameManager.instance.RemoveUnit(player.gameObject);
        player.Dead();
    }
    public override void ExitState()
    {

    }


}
