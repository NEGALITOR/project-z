using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Detection : MonoBehaviour
{

    private GameObject body;
    public static bool touching;
    public static int damage_multiplier = 0;
    public Animator anim;
    bool[] dealt_it = new bool[3];
    public static bool hurt_color;

    public GameObject camera_container;
    List<GameObject> enemies = new List<GameObject>();

    CameraShake cameraShake;
    GameObject punchingBag;
    

    
    // Start is called before the first frame update
    void Start()
    {
        body = this.gameObject;
        cameraShake = camera_container.GetComponent<CameraShake>();
       

        
    }
    //<-------------------------When the collider appears, say that they are touching and also make the indivisual enemy have a hurt variable true-------------------->
    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Hello?");
         if(other != null){
             if(other.gameObject.tag == "Enemy"){
                Debug.Log("TOuching Enemy on basic Attack!");
                enemies.Add(other.gameObject); 
                touching = true;
             }
             
         }
         if(other == null){
             Debug.Log("Nothing");
         }
        
         

      
         
         
    }
//<----------------------------Do the opposite when you are no longer next to the enemy------------------------------>
    void OnTriggerExit2D(Collider2D other){
        
        if(other!= null){
            if(other.gameObject.tag == "Enemy"){
                if(enemies.Contains(other.gameObject)){
                    enemies.RemoveAt(enemies.FindIndex(x => x == other.gameObject));
                }  
                
                touching = false;
            }
            
        }
        if(other == null){
            Debug.Log("Nothing");
        }
        

    }


    // Update is called once per frame
    void Update()
    {
       Debug.Log(body.GetComponent<CapsuleCollider2D>().enabled);
        if(touching == true)
        {
            damage();
        }
        if(touching ==false){
            //Make the collider dissaper
            
        }
       
        
        

        
       
        
    }
    //<-----------------------------------------Responsible for actually dealing the damage------------------------------------>
    void damage()
    {
        if(!Player_GamePlay.def_air_attack){
                //If the 1st attack animation is playing and you have not dealt damage yet
            if((anim.GetCurrentAnimatorStateInfo(0).IsTag("1") == true && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .5f) && dealt_it[0] == false){
                damage_multiplier = 1;
                hurt_color = true;
                dealt_it[0] = true;
                foreach(GameObject enemy in enemies){
                    enemy.GetComponent<EnemyMain>().TakingDamage(Player_GamePlay.basedamage*damage_multiplier,true);
                    enemy.GetComponent<EnemyMain>().Follow = false;
                }
                Debug.Log("Hello");
                Status_Effect.knockback_action(Player_GamePlay.rb,2,damage_multiplier);
                cameraShake.Shake(0.01f,0.2f);
                
                
                
            //same condition as above except for the 2nd attack
            }else if(anim.GetCurrentAnimatorStateInfo(0).IsTag("2") == true  && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .5f && dealt_it[1] == false)
            {
                damage_multiplier = 2;
                hurt_color = true;
                dealt_it[1]= true;
                foreach(GameObject enemy in enemies){
                    enemy.GetComponent<EnemyMain>().TakingDamage(Player_GamePlay.basedamage*damage_multiplier,true);
                    enemy.GetComponent<EnemyMain>().Follow = false;
                }
                Status_Effect.knockback_action(Player_GamePlay.rb,1.5f,damage_multiplier);
                cameraShake.Shake(0.01f,0.2f);
                
                
            //same condition as above except for the 3rd attack
            }else if(anim.GetCurrentAnimatorStateInfo(0).IsTag("3") == true  && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > .5f && dealt_it[2] == false)
            {
                damage_multiplier = 3;
                hurt_color = true;
                dealt_it[2] = true;
                foreach(GameObject enemy in enemies){
                    enemy.GetComponent<EnemyMain>().TakingDamage(Player_GamePlay.basedamage*damage_multiplier,true);
                    enemy.GetComponent<EnemyMain>().Follow = false;
                }
                cameraShake.Shake(0.01f,0.2f);
                
                
            //if you are not attacking
            }else{
                damage_multiplier = 0;
                if(anim.GetCurrentAnimatorStateInfo(0).IsTag("1") == false){
                    dealt_it[0] = false;
                    hurt_color = false;
                   
                }
                if(anim.GetCurrentAnimatorStateInfo(0).IsTag("2") == false){
                    dealt_it[1] = false;
                    hurt_color = false;
                 
                }
                if(anim.GetCurrentAnimatorStateInfo(0).IsTag("3") == false){
                    dealt_it[2] = false;
                    hurt_color = false;
                   
                }
                
            
                
            }
        }else if(Player_GamePlay.def_air_attack){
                //If the 1st attack animation is playing and you have not dealt damage yet
            if(anim.GetCurrentAnimatorStateInfo(0).IsTag("AirAttack") == true && dealt_it[0] == false){
                damage_multiplier = 1;
                hurt_color = true;
                dealt_it[0] = true;
                cameraShake.Shake(0.01f,0.2f);
            //if you are not attacking
            }else{
                damage_multiplier = 0;
                if(anim.GetCurrentAnimatorStateInfo(0).IsTag("AirAttack") == false){
                    dealt_it[0] = false;
                    hurt_color = false;
                }
               
                
            
                
            }
        }
        
        
        
         
        
    }
}
