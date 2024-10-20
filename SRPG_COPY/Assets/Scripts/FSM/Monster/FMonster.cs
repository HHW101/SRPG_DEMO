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
                {
                    fsm.ChangeState(new DeadState_M(this));
                    
                }
                else if (IsActive())
                {
                    fsm.ChangeState(new MState_M(this));
                    state = UnitState.Move;
                }
                break;
            case UnitState.Move:
                if (IsBlock())
                    return;
                if (!CanMove())
                {
                    fsm.ChangeState(new AState_M(this));
                    state = UnitState.Attack;
                }
                
               break;
            case UnitState.Attack:
               
                if (IsBlock())
                    break;
                if (!CanAttack())
                {
                      
                        fsm.ChangeState(new IdleState_M(this));
                    state = UnitState.Idle;
                }
                break;
        }
        fsm.DoingState();
    }
 
    public bool IsActive()
    {
        if(GameManager.instance.turn==GameManager.TurnState.enemyTurn&& GameManager.instance.getselectMonster()==this)
            return true;
        return false;
    }
    
}

public class DeadState_M : UnitState
{
    // Start is called before the first frame update
    FMonster monster;
    public DeadState_M(FUnit monster) : base(monster)
    {
        this.monster = (FMonster)monster;
    }
    // Start is called before the first frame update
    public override void DoingState()
    {

    }
    public override void EnterState()
    {
        monster.Dead();
    }
    public override void ExitState()
    {
        
    }


}

