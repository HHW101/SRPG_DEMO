using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
/* 유닛들을 조작하는 가상 클래스
 *  모든 유닛이 공유하는 움직임을 줌
 *  플레이어와 적이 차이를 느끼는 건 오버라이드 해주자
 * 
 */
public class UnitP : MonoBehaviour
{
    [SerializeField]
    protected float hp;
    [SerializeField]
    public int range;
    protected Animator animator;
    public bool isMoving;
    protected float moveSpeed = 2f;
    public int runAble;
    public int unitX;
    public int unitY;
    protected HashSet<Tile> RangeTiles = new HashSet<Tile>();
    public Tile unitTIle;
    public Direction dirU;
    public enum Direction
    {
        up, down, left, right
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
    }

    public virtual void Attack(GameObject a)
    {
       // GetRange(mode.attack);
    }
    public virtual void Damaged(float x)
    {
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
    public virtual void GetRange(mode s)
    {
        switch (s) {
            case mode.attack:
                RangeTiles = Grid.instance.GetRange(unitX, unitY, range);
                break;
            case mode.move:
                RangeTiles = Grid.instance.GetRange(unitX, unitY, runAble);
                break;
        }

        
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
        Grid.instance.cam.ZoomIn();
        foreach (Tile tile in t)
        {
            Debug.Log($"tile: X = {tile.getX()}, Y = {tile.getY()}");

            Vector3 direction = tile.gameObject.transform.position;
            isMoving = true;
        animator.SetBool("isRunning", true);
        Vector3 rot = direction - transform.position;
        transform.rotation = Quaternion.LookRotation(rot.normalized);
        while (Vector3.Distance(transform.position, direction) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, direction, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = direction;
        animator.SetBool("isRunning", false);
        isMoving = false;
            unitTIle = tile;
            
        }

        unitTIle.state = Tile.TileState.Occupied;
        unitTIle.on = gameObject;
        CheckDir();
        Grid.instance.cam.ZoomOut();
        if(TurnManager.instance.turn!=TurnManager.TurnState.enemyTurn)
            Grid.instance.canClick = true;
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
