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
    Pathfinder path = new Pathfinder();
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
        Debug.Log($"플레이어가 {a}를 공격");

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
        Debug.Log($"위치 확인 부분: {RangeTiles.Count}");
        //GameManager.instance.setGo(RangeTiles);
    }
        public void movePlayer(Tile selectTile)
      {
        List<Tile> temp = new List<Tile>();
        temp = path.FindNext(unitTIle, selectTile, Pathfinder.PathMode.pM);
        if (temp != null)
            GoTo(temp);
    }
    // Update is called once per frame
    private void AttackT()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (!isClick)
            {
                selectTile.SetPState(Tile.PState.Select);

                GetRange(Pathfinder.PathMode.pA);
                isClick = true;
            }
            else if (selectTile.state == Tile.TileState.Occupied)
            {
                Attack(selectTile.on);
                isClick = false;
                
            }
        }
    }
  
    void Update()
    {
        if (!isSelected)
            return;
      
       
    }
}
