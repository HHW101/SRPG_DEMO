using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerX{ get;set; }
    public int playerY{ get; set; }
    private bool isMoving;
    private Vector3 originPos, targetPos;
    private float moveSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void GoTo(int x)
    {
        switch (x) { 
        case 0:
                StartCoroutine(MovePlayer(Vector3.forward*2));
                break;
        case 1:
                StartCoroutine(MovePlayer(Vector3.back*2));
                break;
        case 2:
                StartCoroutine(MovePlayer(Vector3.right*2));
                break;
        case 3:
                StartCoroutine(MovePlayer(Vector3.left*2));
                break;
        
        }

    }
    private IEnumerator MovePlayer(Vector3 direction)
    {
        isMoving = true;
        float elapsedTime = 0;
        originPos = transform.position;
        targetPos = transform.position+direction;
        while (elapsedTime < moveSpeed) {
            
            transform.position = Vector3.Lerp(originPos, targetPos, elapsedTime/moveSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.position =targetPos;
        isMoving = false;
    }
    // Update is called once per frame

    void Update()
    {
        
    }
}
