using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Status_Effect : MonoBehaviour
{
   static float  stun_duration;
   static bool stunned;
    
    //<-------------------Stun an object------------------>
    public static bool Stunned(Rigidbody2D body, float duration){
        if(stunned == false){
            if(body.gameObject.tag == "Player"){
                Player_GamePlay.AllowMovement = false;
                stunned = true;
                stun_duration = duration;
            }
            return false;
        }else if(stunned == true){
            stun_duration -= Time.deltaTime;
            if(stun_duration <= 0){
                stunned = false;
                if(body.gameObject.tag == "Player"){
                    Player_GamePlay.AllowMovement = true;
                }
                return true;
            }
            return false;
        }else{
            return false;
        }
      
    }
    //<--------------Recoils this enemy when getting hit by final hit----------------->
    public static bool knockback_action(Rigidbody2D body, float knockback_speed, int multiplier){
        if(Player_GamePlay.sr.flipX == false){
            body.velocity = new Vector2(knockback_speed, body.velocity.y);
        }else if(Player_GamePlay.sr.flipX == true){
            body.velocity = new Vector2(-knockback_speed, body.velocity.y);
        }
        return true;
     
           
        
        
        
    }
    //friction for objects that aren't dynamic with the rigidbody
    public static bool friction(Rigidbody2D body, float frictionAmount){
        if(body.velocity.x > 0.1){
            body.velocity = new Vector2(body.velocity.x - frictionAmount, body.velocity.y);
            return false;
            
        }else if(body.velocity.x < -0.1){
            body.velocity = new Vector2(body.velocity.x + frictionAmount, body.velocity.y);
            return false;
        }else{
            body.velocity = new Vector2(0,body.velocity.y);
            return true;
        }

    }

   
}
