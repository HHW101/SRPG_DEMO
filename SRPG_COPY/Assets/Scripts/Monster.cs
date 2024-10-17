using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Monster : UnitP
{

    public int monsterNum;
    public bool isAttack = false;
    public enum MonsterState
    {
        Idle, Attack, Move, Hit,Die
    }
    public MonsterState state;
    // Start is called before the first frame update
    protected  override void  Awake()
    {
        base.Awake();
      
      
    }
    void Start()
    {
        
    }
    public override void Attack(GameObject a)
    {
        base.Attack(a);
        Vector3 p = gameObject.transform.position;
        animator.SetBool("isAttack", true);
        Debug.Log($"{gameObject}가 {a}를 공격");
        
        animator.SetBool("isAttack", false);
        TurnManager.instance.TurnChange(TurnManager.TurnState.pMoveTurn);
        isActive = false;
        ChangeState(MonsterState.Attack);
    }
    public override void Damaged(float x)
    {
        base.Damaged(x);
        animator.SetBool("isHit", true);
        animator.SetBool("isHit", false);
    }
    public override void GoTo(Vector3 targetpostion)
    {
        base.GoTo(targetpostion);

    }
    private void Dead()
    {
        unitTIle.Setstate(Tile.TileState.Idle);
        animator.SetBool("isDie", true);
        Destroy(gameObject);
    }
    public void ChangeState(MonsterState _state)
    {
        state= _state;
    }
    private void MonAttack()
    {
        HashSet<Tile> temp = GameManager.instance.GetRange(unitX, unitY, range, Pathfinder.PathMode.mA);
        Debug.Log("몬스터 공격"+temp.Count+" "+temp);
        Debug.Log($"가지고 있나? {temp.Contains(GameManager.instance.FIndPlayer().unitTIle)}");
        if (temp.Contains(GameManager.instance.FIndPlayer().unitTIle))
            Attack(GameManager.instance.FIndPlayer().gameObject);
        else
        {
            isActive = false;
            TurnManager.instance.TurnChange(TurnManager.TurnState.pMoveTurn);
            ChangeState(MonsterState.Idle);
            
        }
    }
    public void Findplayer()
    {
        GameManager.instance.cam.ChangeTarget(gameObject);
        GameManager.instance.cam.ZoomIn();
        Pathfinder path = new Pathfinder();
        List<Tile> temp = new List<Tile>();
        temp = path.FindNext(unitTIle, GameManager.instance.FIndPlayer().unitTIle,Pathfinder.PathMode.mM);
        Debug.Log(temp.Count);
        if(temp.Count>runAble)
            temp = temp.GetRange(0, runAble);
        
        while (temp[temp.Count - 1].state!=Tile.TileState.Idle)
            temp.RemoveAt(temp.Count-1);
        if (temp != null)
            GoTo(temp);
        
        GameManager.instance.cam.ZoomOut();
    }
    // Update is called once per frame
    void Update()
    {
        if (TurnManager.instance.turn != TurnManager.TurnState.enemyTurn||monsterNum!=0)
            return;
        
        switch (state)
        {

            case MonsterState.Idle:
                if (hp <= 0)
                    ChangeState(MonsterState.Die);
                 ChangeState(MonsterState.Move);
                break;
            case MonsterState.Attack:
                if (isActive) { return; }
                isActive = true;
                MonAttack();
               
                break;
            case MonsterState.Move:
                if (isActive) { return; }
                ChangeState(MonsterState.Attack);
                isActive = true;

                Findplayer();
                
                
                break;
            case MonsterState.Hit:
                
                break;
            case MonsterState.Die:
                Dead();
                break;
        }
     
    }
}
