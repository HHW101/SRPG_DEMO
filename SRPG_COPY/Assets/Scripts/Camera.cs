using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform Target;
    public float offsetX;
    public float offsetY;
    public float offsetZ;
    // Start is called before the first frame update
    void Start()
    {
        Target = Grid.instance.FIndPlayer().gameObject.transform;
    }

    // Update is called once per frame
 
    private void FixedUpdate()
    {
        transform.position=
            new Vector3(Target.position.x+offsetX, Target.position.y+offsetY, Target.position.z+offsetZ);
        
    }
}
