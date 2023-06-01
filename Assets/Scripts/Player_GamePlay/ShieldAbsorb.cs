using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ShieldAbsorb : MonoBehaviour
{
    //Absorbs the damage being contained in the shield
    public static int AbsorbedDamage = 0;
    void OnTriggerEnter2D(Collider2D other){
        if(other.GetComponent<Touch_me_and_I_will_damage_you>() != null && !Player_GamePlay.blastActivated){
            AbsorbedDamage += other.GetComponent<Touch_me_and_I_will_damage_you>().DamageToPlayer;
            Debug.Log("hit");
        }else if(Player_GamePlay.blastActivated && other.gameObject.tag == "Enemy"){
            other.GetComponent<EnemyMain>().TakingDamage(AbsorbedDamage,false);
            Player_GamePlay.blastActivated = false;
        }
        
    }
    void Update(){
        if(Player_GamePlay.ShieldIsActive){
            this.GetComponent<SpriteRenderer>().material.SetInt("_Multiplier", (AbsorbedDamage*2)+1);
        }else{
            this.GetComponent<SpriteRenderer>().material.SetInt("_Multiplier", 0);
        }
        
    }
    
}
