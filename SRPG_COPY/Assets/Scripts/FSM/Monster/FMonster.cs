using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FMonster : FUnit
{
    // Start is called before the first frame update
    public enum UnitState
    {
        Idle,  Attack, Move, Hit, Die
    }
    UnitState state;
    void Start()
    {
        fsm = new FSM(new IdleState_M(this));
        state = UnitState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state) { 
            case UnitState.Idle:
                if (isDie())
                    fsm.ChangeState(new IdleState_M(this));
                else if(IsActive())
                    fsm.ChangeState(new MState_M(this));
                break;
            case UnitState.Move:
             if (CanMove())
                    fsm.ChangeState(new AState_M(this));
               break;
            case UnitState.Attack:
                if (CanAttack())
                    fsm.ChangeState(new IdleState_M(this));
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
    public bool IsActive()
    {
        if(GameManager.instance.turn==GameManager.TurnState.enemyTurn&& GameManager.instance.getselectMonster()==this)
            return true;
        return false;
    }
    public bool CanMove()
    {
        if(moveC>0)
            return false;
        return true;
    }
    public bool CanAttack()
    {
        if(atkC>0)
            return false;
        return true;
    }
    public bool isDie()
    {
        if(hp>0)
            return false;
        return true;
    }
}
