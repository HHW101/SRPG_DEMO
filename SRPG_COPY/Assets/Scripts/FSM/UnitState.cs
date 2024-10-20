using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitState 
{
    protected FUnit _monster;
    // Start is called before the first frame update
    protected UnitState(FUnit monster)
    {
        _monster = monster;
    }
    public abstract void EnterState();
    public abstract void DoingState();
    public abstract void ExitState();
}
