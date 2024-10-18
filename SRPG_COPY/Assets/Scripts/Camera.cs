using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject Target;
    public float offsetX;
    public float offsetY;
    public float offsetZ;
    private Vector3  defaultCam = new Vector3(27f,20f,3f);
    // Start is called before the first frame update
    void Start()
    {
        //Target = GameManager.instance.FIndPlayer().gameObject;
    }

    // Update is called once per frame
    public void ZoomIn()
    {
        offsetX = offsetX / 2;
        offsetY=offsetY/2;
        offsetZ = offsetZ / 2;
    }
    public void ZoomOut()
    {
        offsetX = offsetX * 2;
        offsetY=offsetY*2;
        offsetZ = offsetZ * 2;
    }
    public void ChangeTarget(GameObject t)
    {
        Target = t;
    }
    public void mapMode()
    {
        transform.position=new Vector3(offsetX, offsetY+10f, offsetZ)+GameManager.instance.SelectCamera();
    }
    private void FixedUpdate()
    {
        if (GameManager.instance.inputmode==GameManager.InputMode.Map)
            mapMode();
        else
        {
            if (Target != null)
                transform.position =
                new Vector3(Target.transform.position.x + offsetX, Target.transform.position.y + offsetY, Target.transform.position.z + offsetZ);
            else
                transform.position = defaultCam;
        }
    }
}
