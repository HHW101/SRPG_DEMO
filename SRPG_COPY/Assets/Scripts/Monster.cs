using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : UnitP
{
 
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
    public override void Attack()
    {
        base.Attack();
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
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {

            case MonsterState.Idle:
                if (TurnManager.instance.turn == TurnManager.TurnState.enemyTurn)
                    ChangeState(MonsterState.Move);
                break;
            case MonsterState.Attack:

                break;
            case MonsterState.Move:
                break;
            case MonsterState.Hit:
                
                break;
            case MonsterState.Die:
                
                break;
        }
     
    }
}
