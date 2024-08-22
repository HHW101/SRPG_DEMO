using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    private float speed=5;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    void GetNode()
    {

    }

    void move()
    {
        rb.velocity = Vector3.right*Input.GetAxis("Horizontal") * speed+Vector3.forward*Input.GetAxis("Vertical")*speed;
    }
}
