using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Pathfinder;

public class Monster : UnitP
{

  
    public bool isAttack = false;
    private global::UnitState Mstate;
    public Player selectPlayer;
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
        StartCoroutine(AttackAni(a));

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
        isActive = false;
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
        StartCoroutine(DeadAni());
    }
    private IEnumerator DeadAni()
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0).Length);
        GameManager.instance.RemoveUnit(gameObject);
        Destroy(gameObject);
    }
  
    private void MonAttack()
    {
        HashSet<Tile> temp = path.Range(unitTIle, range, Pathfinder.PathMode.mA);
        atkC--;
        if (temp.Contains(selectPlayer.unitTIle))
            Attack(selectPlayer.gameObject);
        else
        {
           isActive = false;
           

        }
    }
    public void Findplayer()
    {
        GameManager.instance.cam.ChangeTarget(gameObject);
        GameManager.instance.cam.ZoomIn();
        moveC--;
        List<Tile> temp = new List<Tile>();
        temp = path.findPlayer(unitTIle , GameManager.instance.FIndPlayer());

  
        selectPlayer= temp[temp.Count-1].on.GetComponent<Player>();
        if(temp.Count>runAble)
            temp = temp.GetRange(0, runAble);
        Debug.Log(temp[0].state);
        while (temp.Count > 0 && temp[temp.Count - 1].state!=Tile.TileState.Idle)
            temp.RemoveAt(temp.Count-1);
        if (temp != null)
        {
            unitTIle.Setstate(Tile.TileState.Idle);
            GoTo(temp);
        }
        
        GameManager.instance.cam.ZoomOut();
    }
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {

            case UnitState.Idle:
                if (hp <= 0)
                    ChangeState(UnitState.Die);
                if (!isSelected)
                    return;
                if (atkC == 0 && moveC == 0)
                    GameManager.instance.enemyTurnChange();
                else
                    ChangeState(UnitState.Move);
                break;
            case UnitState.Move:
                if (isActive) { return; }
                if (moveC == 0)
                {
                    //moveC++;
                    ChangeState(UnitState.Attack);
                }
                isActive = true;
                Findplayer();


                break;
            case UnitState.AttackThink:
                break;
            case UnitState.Attack:
                if (isActive) { return; }
                isActive = true;
                if (atkC==0)
                {
                    ChangeState(UnitState.Idle);
                }
                else
                    MonAttack();
               
                
                
               
                break;
        
            case UnitState.Hit:
                
                break;
            case UnitState.Die:
                Dead();
                break;
        }
     
    }
}
