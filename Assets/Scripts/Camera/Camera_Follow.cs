using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{
    public Transform character_position;
    public Vector3 offset;
    public float smoothSpeed= 0.125f;

    
    // Start is called before the first frame update
    void FixedUpdate()
    {
        Follow_Player();
    }

    void Follow_Player(){
    //<------------------------Follow the leader/ player--------------->
        //where I want the camera to be
        Vector3 desireposition = character_position.position + offset ;
        //where the camera currently is
        Vector3 smoothposition = Vector3.Lerp(transform.position,desireposition,smoothSpeed);
        //setting the position as smoothposition
        transform.position = smoothposition;
    }
    
}
