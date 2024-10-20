using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

public class FUnit : MonoBehaviour
{
    // Start is called before the first frame update

    [Header("유닛 능력치")]
    [SerializeField]
    public float maxhp;
    public float hp;
    [SerializeField]
    public int range; //사거리
    public int runAble; //이동거리
    [SerializeField]
    public int moveC = 1; //이동력
    [SerializeField]
    public int atkC = 1; //공격기회
    public Animator animator;
    public float moveSpeed = 2f;
    protected bool isMove;
    [SerializeField]
    public float atk = 5; //공격력
    public FUnit selectUnit;
    public int unitNum; //유닛 이름
    public HashSet<Tile> RangeTiles = new HashSet<Tile>(); //사거리
    public Tile unitTIle; //현재 가진 올라온 타일
    public Direction dirU; //방향 
    public Tile selectTile;
    public bool isSelected=false;
    public bool isFinish=false;
    //스킬 관련
    public List<Skill> skill = new List<Skill>();
    protected FSM fsm; //유한상태머신
    public enum Direction
    {
        up, down, left, right
    }
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", false);
        animator.SetBool("isHit", false);
        animator.SetBool("isDie", false);
        animator.SetBool("isAttack", false);
    }
    public void GReset()
    {
        hp = maxhp;
        moveC = 1;
        atkC = 1;
    
    }
    public void TReset()
    {
        moveC = 1;
        atkC = 1;
    }
    public bool CanMove()
    {
        if (moveC > 0)
            return true;
        return false;
    }
    public bool CanAttack()
    {
        if (atkC > 0)
            return true;
        return false;
    }
    public bool isDie()
    {
        if (hp > 0)
            return false;
        return true;
    }
    public void block()
    {
        isMove = true;
    }
    public void UnLock()
    {
        isMove = false;
    }
    public bool IsBlock()
    {
        return isMove;
    }
    public void GetRange(Pathfinder.PathMode s)
    {
        Pathfinder path = new Pathfinder();
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
    public void CheckDir()
    {
        float rot = gameObject.transform.rotation.eulerAngles.y;
        if (rot <= 10f && rot >= -10f)
        {
            dirU = FUnit.Direction.up;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        else if (rot <= 190f && rot >= 170f)
        {
            dirU = FUnit.Direction.down;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (rot <= 280f && rot >= 260f)
        {
            dirU = FUnit.Direction.left;
            transform.rotation = Quaternion.Euler(0, 270, 0);
        }
        else
        {
            dirU = FUnit.Direction.right;
            transform.rotation = Quaternion.Euler(0, 90, 0);
        }
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
    //코루틴 사용
    public virtual void GoTo(List<Tile> t)
    {

        StartCoroutine(MovePlayer(t));

    }

    public void Attack(GameObject a)
    {
        // UIManager.instance.ShowBattleScene(a.GetComponent<UnitP>(), monster);
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
        UnLock();

    }
    private IEnumerator MovePlayer(List<Tile> t)
    {
        GameManager.instance.cam.ZoomIn();
        foreach (Tile tile in t)
        {
            Debug.Log($"tile: X = {tile.getX()}, Y = {tile.getY()}");

            Vector3 direction = tile.gameObject.transform.position;
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
            unitTIle = tile;

        }
        yield return new WaitForSeconds(1f);

        unitTIle.state = Tile.TileState.Occupied;
        unitTIle.on = gameObject;
        CheckDir();
        GameManager.instance.cam.ZoomOut();
        UnLock();

    }

}
