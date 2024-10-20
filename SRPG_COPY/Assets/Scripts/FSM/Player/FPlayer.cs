using System.Collections;
using System.Collections.Generic;
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
    private void Awake()
    {
        
    }
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
                    fsm.ChangeState(new IdleState_P(this));
                    state = UnitState.Idle;
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
                    fsm.ChangeState(new ATstate_P(this));
                    isClick = false;
                    state = UnitState.Attack;
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
    public void Damaged(float x)
    {
        UIManager.instance.getDamage(hp, hp - x, maxhp);
        hp -= x;
        animator.SetBool("isHit", true);
        animator.SetBool("isHit", false);
    }
    public void getClick(Tile tile)
    {

        if (RangeTiles.Contains(tile))
        {
            if ((state == UnitState.MT && tile.state == Tile.TileState.Idle)||(state==UnitState.AT&&tile.on.GetComponent<FMonster>()!=null))
            {
                selectTile = tile;
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
