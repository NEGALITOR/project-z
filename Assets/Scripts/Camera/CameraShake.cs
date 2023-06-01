using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    float shake_amo = 0;
    
    //Master Branch of Shake Ability
    public void Shake(float amount, float length){
        shake_amo = amount;
        InvokeRepeating("DoShake",0,0.01f);
        Invoke("stopshake",length);
    }
    //Continuesly Shakes the Camera at random positions
    void DoShake(){
        if(shake_amo > 0){
            Vector3 camPos = transform.position;
            float shake_amt_x = Random.value * shake_amo * 2 - shake_amo;
            float shake_amt_y =Random.value * shake_amo * 2 - shake_amo;
            camPos.x += shake_amt_x;
            camPos.y += shake_amt_y;

            transform.position = camPos;
           
        }
    }
    //Stops the Shake function and returns the camera to it's original position
    void stopshake(){
        CancelInvoke("DoShake");
        transform.localPosition = Vector3.zero;
    }

    
}
