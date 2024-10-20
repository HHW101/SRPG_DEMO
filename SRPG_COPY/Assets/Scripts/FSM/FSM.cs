using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM 
{
    public UnitState currentState;
    // Start is called before the first frame update
     public AState_M astate_m;
    public MState_M mstate_m;
    public IdleState_M idlestate_m;
    public AState_P astate_p;
    public MState_P mstate_p;
    public IdleState_P idlestate_p;
    public ATstate_P atstate_P;
    public MTstate_P mtstate_p;
    public FSM(UnitState state)
    {
        currentState = state;

    }
 
    public void DoingState()
    {
        if(currentState != null) 
            currentState.DoingState();
    }

    public void ChangeState(UnitState state)
    {

        if (currentState == state)
        {
          
            return;
        }
        if (currentState != null)
        {
            currentState.ExitState();
            Debug.Log($"{state}나감");
        }
        currentState = state;
        currentState.EnterState();
        Debug.Log($"{state}들어감");
    }

}
