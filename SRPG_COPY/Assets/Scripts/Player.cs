using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameManager;
using static Monster;
using static UnityEngine.GraphicsBuffer;

public class Player : UnitP
{
 

    private Vector3 originPos, targetPos;
   
    public Tile selectTile;
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
    }
    void Start()
    {
        
    }
   
    public override void Attack(GameObject a)
    {
        base.Attack(a);
        atkC--;
        Debug.Log($"플레이어가 {a}를 공격");
        foreach (Tile t in RangeTiles)
        {
            t.SetPState(Tile.PState.Idle);
        }
        if (atkC == 0)
        {
            UIManager.instance.HideBMenu();
            GameManager.instance.PlayerTurnChange();
        }
        //GameManager.instance.ChangeInputMode(GameManager.InputMode.s);

    }
    public override void Damaged(float x)
    {
        base.Damaged(x);  
    }
    public override void GoTo(Vector3 targetpostion)
    {
        base.GoTo(targetpostion);
      

    }
    protected override void ThinkAttack()
    {
        
        state = UnitState.AttackThink;
       
    }
    

    public override void GetRange(Pathfinder.PathMode s)
    {
        base.GetRange(s);
    
        //GameManager.instance.setGo(RangeTiles);
    }
        public void movePlayer(Tile selectTile)
      {
        Debug.Log($"확인: {unitTIle.getX()}{unitTIle.getY()}");
        unitTIle.Setstate(Tile.TileState.Idle);
        List<Tile> temp = new List<Tile>();
        temp = path.FindNext(unitTIle, selectTile, Pathfinder.PathMode.pM);
        moveC--;
        if (temp != null)
            GoTo(temp);
        foreach(Tile t in RangeTiles)
        {
            t.SetPState(Tile.PState.Idle);
        }
    }
    // Update is called once per frame
    //private void AttackT()
    //{
    //    if (Input.GetKeyDown(KeyCode.Z))
    //    {
    //        if (!isClick)
    //        {
    //            selectTile.SetPState(Tile.PState.Select);

    //            GetRange(Pathfinder.PathMode.pA);
    //            isClick = true;
    //        }
    //        else if (selectTile.state == Tile.TileState.Occupied)
    //        {
    //            Attack(selectTile.on);
    //            isClick = false;
                
    //        }
    //    }
    //}
  
    void Update()
    {
        if (!isSelected)
            return;
      
       
    }
}
