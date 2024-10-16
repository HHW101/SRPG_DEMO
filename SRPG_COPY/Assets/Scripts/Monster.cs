using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Monster : UnitP
{
    public bool isActive=true;
    public enum MonsterState
    {
        Idle, Attack, Move, Hit,Die
    }
    public MonsterState state;
    // Start is called before the first frame update
    protected  override void  Awake()
    {
        base.Awake();
      
        animator.SetBool("isHit", false);
        animator.SetBool("isDie", false);
        animator.SetBool("isAttack",false);
    }
    void Start()
    {
        
    }
    public override void Attack(GameObject a)
    {
        base.Attack(a);
        
    }
    public override void Damaged(float x)
    {
        base.Damaged(x);
    }
    public override void GoTo(Vector3 targetpostion)
    {
        base.GoTo(targetpostion);

    }
    private void Dead()
    {
        Destroy(gameObject);
    }
    public void ChangeState(MonsterState _state)
    {
        state= _state;
    }
    public void Findplayer()
    {
        Pathfinder path = new Pathfinder();
        List<Tile> temp = new List<Tile>();
        temp = path.FindNext(Grid.instance.FIndPlayer().unitTIle, unitTIle);
        if(temp.Count>runAble)
            temp = temp.GetRange(0, runAble);

        if (temp != null)
            GoTo(temp);
        unitX = unitTIle.getX();
        unitY = unitTIle.getY();
    }
    // Update is called once per frame
    void Update()
    {
        if (TurnManager.instance.turn != TurnManager.TurnState.enemyTurn)
            return;
        switch (state)
        {

            case MonsterState.Idle:
                if(isActive)
                    ChangeState(MonsterState.Move);
                break;
            case MonsterState.Attack:
                 
                break;
            case MonsterState.Move:
                Findplayer();
           
                ChangeState(MonsterState.Attack);
                break;
            case MonsterState.Hit:
                
                break;
            case MonsterState.Die:
                
                break;
        }
     
    }
}
