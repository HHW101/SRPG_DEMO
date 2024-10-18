using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using static Pathfinder;

public class Monster : UnitP
{

  
    public bool isAttack = false;

    public Tile selectPlayer;
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
        StartCoroutine(AttackAni(a));
        
    }
    private IEnumerator AttackAni(GameObject target)
    {
        while(!animator.IsInTransition(0))
            yield return null;
        animator.SetBool("isAttack", false);
        GameManager.instance.TurnChange(GameManager.TurnState.playerTurn);
        isActive = false;
         atkC--;
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
        Destroy(gameObject);
    }
  
    private void MonAttack()
    {
        HashSet<Tile> temp = path.Range(unitTIle, range, Pathfinder.PathMode.mA);
        if (temp.Contains(selectPlayer))
            Attack(selectPlayer.gameObject);
        else
        {
            isActive = false;
            atkC--;

        }
    }
    public void Findplayer()
    {
        GameManager.instance.cam.ChangeTarget(gameObject);
        GameManager.instance.cam.ZoomIn();
      
        List<Tile> temp = new List<Tile>();
        Debug.Log(GameManager.instance.FIndPlayer().Count);
        temp = path.findPlayer(unitTIle , GameManager.instance.FIndPlayer());

        Debug.Log(temp.Count);
        temp[temp.Count-1] = selectPlayer;
        if(temp.Count>runAble)
            temp = temp.GetRange(0, runAble);
        
        while (temp.Count>0&&temp[temp.Count - 1].state!=Tile.TileState.Idle)
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
        if (!isSelected)
            return;
        
        switch (state)
        {

            case UnitState.Idle:
                if (hp <= 0)
                    ChangeState(UnitState.Die);
                 ChangeState(UnitState.Move);
                break;
            case UnitState.Move:
                if (isActive) { return; }
                if (moveC == 0)
                {
                    moveC++;
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
