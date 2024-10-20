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
        atkC--;
        UIManager.instance.HideBMenu();
    
        GameManager.instance.ChangeInputMode(InputMode.block);
        foreach (Tile t in RangeTiles)
        {
            t.SetPState(Tile.PState.Idle);
        }
        base.Attack(a);

        StartCoroutine(AttackAni(a));

        //GameManager.instance.ChangeInputMode(GameManager.InputMode.s);

    }
    IEnumerator AttackAni(GameObject target)
    {
        Debug.Log("확인2");
        animator.SetBool("isAttack", true);
        target.GetComponent<UnitP>().Damaged(atk);
        yield return new WaitForSeconds(5f);
      
        Debug.Log($"{gameObject}가 {target}를 공격");
        animator.SetBool("isAttack", false);
        UIManager.instance.HideBS();
        if (atkC == 0)
        {

            GameManager.instance.PlayerTurnChange();
        }
        else
        {
          //  UIManager.instance.ShowBAMenu(this);
        }
    }


   
    public override void Damaged(float x)
    {
        base.Damaged(x);  
    }
    public override void GoTo(Vector3 targetpostion)
    {
        base.GoTo(targetpostion);
      

    }


    public override void GetRange(Pathfinder.PathMode s)
    {
        base.GetRange(s);
    
        //GameManager.instance.setGo(RangeTiles);
    }
        public void movePlayer(Tile selectTile)
      {
  
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
