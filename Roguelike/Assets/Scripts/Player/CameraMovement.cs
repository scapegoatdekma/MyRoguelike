using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
 
    public Transform target;
    public Vector3 offset;
    public float movingSpeed;


    private void FixedUpdate()
    {
        offset = new Vector3(target.transform.position.x, target.transform.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, offset, movingSpeed*Time.fixedDeltaTime);
    }
    void Start()
    {
        
    }


    void Update()
    {
       
    }
}
