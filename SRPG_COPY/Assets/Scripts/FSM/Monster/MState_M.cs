using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MState_M : UnitState
{
    // Start is called before the first frame update
    FMonster monster;
    public MState_M(FUnit monster) : base(monster) { 
        this.monster = (FMonster)monster;
    }

    public override void DoingState()
    {
       
    }
    public override void EnterState()
    {
        Findplayer();
        monster.block();
        UIManager.instance.ShowInfoMenu(monster);
    }
    public override void ExitState()
    {
        UIManager.instance.HideBMenu();
    }
    
    //방향 탐색
   
    //플레이어 찾기 
    public void Findplayer()
    {
        GameManager.instance.cam.ChangeTarget(monster.gameObject);
        GameManager.instance.cam.ZoomIn();
        monster.moveC--;
        List<Tile> temp = new List<Tile>();
        Pathfinder path = new Pathfinder();
        temp = path.findPlayer(monster.unitTIle, GameManager.instance.FIndPlayer());


        monster.selectTile = temp[temp.Count - 1];
        if (temp.Count > monster.runAble)
            temp = temp.GetRange(0, monster.runAble);
        Debug.Log(temp[0].state);
        while (temp.Count > 0 && temp[temp.Count - 1].state != Tile.TileState.Idle)
            temp.RemoveAt(temp.Count - 1);
        if (temp != null)
        {
            monster.unitTIle.Setstate(Tile.TileState.Idle);
            monster.GoTo(temp);
        }

        GameManager.instance.cam.ZoomOut();
    }
}
