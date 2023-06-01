using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Detector : MonoBehaviour
{
    //Certain Effects to Enemies per Enemy
    
    public void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Enemy"){
           
            if(Abilities_Script.ability_name == "Explosion!"){
                other.GetComponentInParent<EnemyMain>().hurt_me = true;
                Enemy_Detection.damage_multiplier = 99;
                
            }else if(Abilities_Script.ability_name == "Taser Time"){
                Debug.Log("hi");
                Status_Effect.Stunned(other.gameObject.GetComponentInParent<Rigidbody2D>(),5f);
            }
        }
    }
}
