using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInRange : MonoBehaviour
{
    float time;
    public float desired_time_alloted;
    public static List<GameObject> enemies = new List<GameObject>();
    int amount_in_range;

  
    void Update(){
        Deactivate_Combat();
        
    }

    void OnTriggerEnter2D (Collider2D other)
    {
        //If there is an enemy in range add 1 to the amount of enemies in Range 
         if(other.gameObject.tag == "Enemy"){
            enemies.Add(other.gameObject);
            if(Player_GamePlay.Combat_Mode == false){
                Player_GamePlay.Combat_Mode = true;
            }
            amount_in_range++;
         }
         
    }

    void OnTriggerExit2D(Collider2D other){
        //If an enemy leaves subtract 1 from the amount of enemies in Range, this allows us to stay in combat if there are multiple enemies involed
        if(other.gameObject.tag == "Enemy"){
            if(enemies.Contains(other.gameObject)){
                enemies.RemoveAt(enemies.FindIndex(x => x == other.gameObject));
            }
            amount_in_range--;
        }
        

    }

    float Timer(){
        time = time - Time.deltaTime;
        return time;
    }

//<--------------Handles the deactivation of being in combat-------->
    void Deactivate_Combat(){
        if(Player_GamePlay.Combat_Mode == true){
            if(amount_in_range <= 0){
                Timer();
            }
            if(time <= 0){
                Player_GamePlay.Combat_Mode = false;
            }
            if(amount_in_range > 0 && time > 0 && time < desired_time_alloted){
                time = desired_time_alloted;
            }
        }else if(Player_GamePlay.Combat_Mode == false){
             
            if(time != desired_time_alloted){
              
                time = desired_time_alloted;
               
            }
        }
    }
}
