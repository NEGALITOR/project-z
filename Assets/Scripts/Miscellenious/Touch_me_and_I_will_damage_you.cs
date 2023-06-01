using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Touch_me_and_I_will_damage_you : MonoBehaviour
{
    public Time_Manager time_Manager;
    public int DamageToPlayer;
    
    public void OnTriggerStay2D(Collider2D other){
        if(Player_GamePlay.hasBeenDamage == false && other.gameObject.tag == "Player"){
            StartCoroutine(Deal_Damage_To_Player(other));
        }
       
        
    }
    IEnumerator Deal_Damage_To_Player(Collider2D other){
        
    //If the thing that is touching is the player then deal damage and have invisa frames to allow redemption
        if(Enemy_Detection.touching == false && other.gameObject.tag != "Enemy" && other.gameObject.tag != "Enemy_Detector" && other.gameObject.tag != "Abilites"&& Player_GamePlay.DoDash == false && Player_GamePlay.def_air_attack == false){
            Player_GamePlay.current_health-=DamageToPlayer;
        
            time_Manager.DoSlowMotion(0f,.5f);
            Player_GamePlay.hasBeenDamage = true;
        
            if(other.GetComponentInParent<SpriteRenderer>().color.a == 1){
                other.GetComponentInParent<SpriteRenderer>().color = new Color(other.GetComponentInParent<SpriteRenderer>().color.r,other.GetComponentInParent<SpriteRenderer>().color.g,other.GetComponentInParent<SpriteRenderer>().color.b,.5f );
                    
            }
                
            
            yield return new WaitForSeconds(2);
            Player_GamePlay.hasBeenDamage = false;
            other.GetComponentInParent<SpriteRenderer>().color = new Color(other.GetComponentInParent<SpriteRenderer>().color.r,other.GetComponentInParent<SpriteRenderer>().color.g,other.GetComponentInParent<SpriteRenderer>().color.b,1 );
        }   
        
        
        
    }
}
