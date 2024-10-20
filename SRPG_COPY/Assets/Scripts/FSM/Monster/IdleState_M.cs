using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState_M : UnitState
{
    FMonster monster;
    // Start is called before the first frame update
    public IdleState_M(FUnit monster) : base(monster) {
        this.monster = (FMonster)monster;
    }

    public override void DoingState()
    {
        
    }
    public override void EnterState()
    {
        if (!monster.CanAttack())
        {
            GameManager.instance.enemyTurnChange();
        }
    }
    public override void ExitState()
    {

    }
    

}
