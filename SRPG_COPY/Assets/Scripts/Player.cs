using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : UnitP
{
    public int playerX{ get;set; }
    public int playerY{ get; set; }

    private Vector3 originPos, targetPos;
  
    
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
        TurnManager.instance.TurnChange(TurnManager.TurnState.enemyTurn);
    }
    public override void Damaged(float x)
    {
        base.Damaged(x);  
    }
    public override void GoTo(Vector3 targetpostion)
    {
        base.GoTo(targetpostion);
      

    }
    
    public override void GetRange(mode s)
    {
        base.GetRange(s);
        Grid.instance.setGo(RangeTiles);
    }

    // Update is called once per frame

    void Update()
    {
        
    }
}
