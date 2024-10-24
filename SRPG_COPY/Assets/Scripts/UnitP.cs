using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

/* 
 *  모든 유닛이 공유하는 움직임을 줌
 *  플레이어와 적이 차이를 느끼는 건 오버라이드 해주자
 * 
 */
public class UnitP : MonoBehaviour
{
    [SerializeField]
    public float maxhp;
    public float hp;
    
    [SerializeField]
    public int range;
    protected Animator animator;
    public bool isMoving;
    public bool isActive=false;
    public bool isSelected = false;
    protected float moveSpeed = 2f;
    public int runAble;
    [SerializeField]
    protected float atk =5;
    [SerializeField]
    public int moveC = 1;
    [SerializeField]
    public int atkC = 1;
    public int unitNum;
    public HashSet<Tile> RangeTiles = new HashSet<Tile>();
    public Tile unitTIle;
    public Direction dirU;
    protected Pathfinder path = new Pathfinder();
    public UnitState state;
    protected bool isClick=false ;
    protected bool isMove=false;
    //스킬 관련
    public List<Skill> skill = new List<Skill>();

    
    public enum UnitState
    {
        Idle, AttackThink,Attack, Move,MoveThink, Hit, Die
    }
    public void ChangeState(UnitState _state)
    {
        state = _state;
    }
    public enum Direction
    {
        up, down, left, right
    }
    public void GReset()
    {
        hp = maxhp;
        moveC = 1;
        atkC=1;
        state=UnitState.Idle;
    }
    public void TReset()
    {
        moveC = 1;
        atkC = 1;
        state = UnitState.Idle;
    }

    public void SetDirection(Direction direction)
    {
        dirU = direction;
        switch (direction)
        {
            case Direction.up:
                transform.rotation = Quaternion.Euler(0, 0, 0);  
                break;
            case Direction.down:
                transform.rotation = Quaternion.Euler(0, 180, 0);  
                break;
            case Direction.left:
                transform.rotation = Quaternion.Euler(0, 270, 0);  
                break;
            case Direction.right:
                transform.rotation = Quaternion.Euler(0, 90, 0);  
                break;
           
                    
        }
    }
    public enum mode
    {
        attack,move
    }
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", false);
        animator.SetBool("isHit", false);
        animator.SetBool("isDie", false);
        animator.SetBool("isAttack", false);
    }

    public virtual void Attack(GameObject a)
    {
       // UIManager.instance.ShowBattleScene(a.GetComponent<UnitP>(), this);
       
        Debug.Log("확인");
       
    }
  
    public virtual void Damaged(float x)
    {
        UIManager.instance.getDamage(hp, hp - x, maxhp);
        hp -= x;
    }
    public virtual void GoTo(List<Tile> t)
    {
       
            StartCoroutine(MovePlayer(t));
        
    }
    public virtual void GoTo(Vector3 t)
    {

        StartCoroutine(MovePlayer(t));

    }

    public virtual void GetRange(Pathfinder.PathMode s)
    {

        if (s == Pathfinder.PathMode.pA || s == Pathfinder.PathMode.mA)
        {
            RangeTiles = path.Range(unitTIle, range, s);
           // ChangeState(UnitState.AttackThink);
        }
        else
        {
            RangeTiles = path.Range(unitTIle, runAble, s);
           
        }
        Debug.Log($":{RangeTiles.Count}");
    }
    private IEnumerator MovePlayer(Vector3 direction)
    {
       
           
            isMoving = true;
            animator.SetBool("isRunning", true);
            Vector3 rot = direction - transform.position;
            transform.rotation = Quaternion.LookRotation(rot.normalized);
            while (Vector3.Distance(transform.position, direction) > 0.01f)
            {
                transform.position = Vector3.MoveTowards(transform.position, direction, moveSpeed * Time.deltaTime);
                yield return null;
            }
            transform.position = direction;
            animator.SetBool("isRunning", false);
            isMoving = false;
        
    }

    private IEnumerator MovePlayer(List<Tile> t)
    {
        GameManager.instance.cam.ZoomIn();
        foreach (Tile tile in t)
        {
            Debug.Log($"tile: X = {tile.getX()}, Y = {tile.getY()}");

            Vector3 direction = tile.gameObject.transform.position;
            isMoving = true;
        animator.SetBool("isRunning", true);
        Vector3 rot = direction - transform.position;
        transform.rotation = Quaternion.LookRotation(rot.normalized);
        while (Vector3.Distance(transform.position, direction) > 0.03f)
        {
            transform.position = Vector3.MoveTowards(transform.position, direction, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = direction;
        animator.SetBool("isRunning", false);
        isMoving = false;
            unitTIle = tile;
            
        }
        yield return new WaitForSeconds(1f);
        
        unitTIle.state = Tile.TileState.Occupied;
        unitTIle.on = gameObject;
        CheckDir();
        GameManager.instance.cam.ZoomOut();
        isActive = false;
        if (moveC == 0)
        {
            if (this is Player)
            {
              //  UIManager.instance.ShowBAMenu(this);
                state = UnitState.AttackThink;
                Debug.Log("순서확인");
                GameManager.instance.ChangeInputMode(GameManager.InputMode.block);
            }
            else
            {
                state = UnitState.Attack;
            }
        }
        else
        {
            if (this is Player) 
            GameManager.instance.ChangeInputMode(GameManager.InputMode.Player);
        
        }
    }
 
    private void CheckDir()
    {
        float rot = gameObject.transform.rotation.eulerAngles.y;
        if(rot<=10f&&rot >= -10f)
        {
            dirU = Direction.up;
                transform.rotation = Quaternion.Euler(0, 0, 0);
         }
        else if (rot <= 190f && rot >= 170f)
        {
            dirU = Direction.down;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (rot <= 280f && rot >= 260f)
        {
            dirU = Direction.left;
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else
        {
            dirU = Direction.right;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
    }
    // Update is called once per frame
    
}
