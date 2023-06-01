using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirAttackReaction : MonoBehaviour
{
    public int airalDamage;
    //Hurt Enemy if they touch you during a air attack
    void OnTriggerEnter2D(Collider2D other){
        if(other != null && other.gameObject.tag == "Enemy" ){
            other.GetComponent<EnemyMain>().TakingDamage(airalDamage,true);
            other.GetComponent<EnemyMain>().Follow = false;
        }else if(other == null){
             Debug.Log("Nothing");
        }
       
        
    }
}
