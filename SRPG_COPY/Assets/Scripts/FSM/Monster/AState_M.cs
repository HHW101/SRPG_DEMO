using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AState_M : UnitState
{
    // Start is called before the first frame update
    FMonster monster;
    public AState_M(FUnit monster) : base(monster)
    {
        this.monster = (FMonster)monster;
    }

    public override void DoingState()
    {
        if (monster.atkC > 0)
        {
            monster.atkC--;
            MonAttack();
        }
    }
    public override void EnterState()
    {
        MonAttack();
    }
    public override void ExitState()
    {
        UIManager.instance.HideBS();
    }

    private void MonAttack()
    {
        Pathfinder path = new Pathfinder();
        HashSet<Tile> temp = path.Range(monster.unitTIle, monster.range, Pathfinder.PathMode.mA);
        monster.atkC--;
        if (temp.Contains(monster.selectUnit.unitTIle))
            monster.Attack(monster.selectUnit.gameObject);
      
    }
 


}
