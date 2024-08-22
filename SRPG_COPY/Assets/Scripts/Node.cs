using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour
{

    public bool canGo;
    public GameObject tile;
    // Start is called before the first frame update
    void Start()
    {
        Instantiate(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
