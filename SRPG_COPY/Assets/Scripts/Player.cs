using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    public int playerX{ get;set; }
    public int playerY{ get; set; }
    public bool isMoving;
    private Vector3 originPos, targetPos;
    private float moveSpeed = 2f;
    private Animator animator;
    // Start is called before the first frame update
    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("isRunning", false);
    }
    void Start()
    {
        
    }
    public void GoTo(Vector3 targetpostion)
    {
       
           StartCoroutine(MovePlayer(targetpostion));
      

    }
    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;
        animator.SetBool("isRunning", true);
        Vector3 rot = direction- transform.position;
        transform.rotation = Quaternion.LookRotation(rot.normalized);
        while (Vector3.Distance(transform.position, direction) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, direction, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position=direction;
        animator.SetBool("isRunning", false);
        isMoving = false;
    }
    // Update is called once per frame

    void Update()
    {
        

    }
}
