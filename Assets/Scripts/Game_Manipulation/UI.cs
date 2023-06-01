using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    GameObject health_bar;
    Transform Health_Container;
    int amount_health_bars;
    int health_bar_offset=1;
    int player_current_health =0;
    GameObject Canvas;
    GameObject currentHealthBar = null;
    int num_bar_gone;
    bool hurt;
    bool wait;
    // Start is called before the first frame update
    void Start()
    {
        //Contains the health container that has not been duplicated
        health_bar = GameObject.FindWithTag("Health_Container");
        Canvas = this.gameObject;
        Health_Container = this.transform;
        num_bar_gone = Player_GamePlay.Max_player_health;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        amount_health_bars = Player_GamePlay.Max_player_health;
        player_current_health = Player_GamePlay.current_health;
        Make_health_bars(amount_health_bars);
        hurt_reaction(amount_health_bars,player_current_health);
        Get_Rid_of_bar();
        SlowMow();
        
    }

    void Make_health_bars(int  max_health){
       //If the amount of containers is not the same as our max health, then duplicate until there is enough
        if(GameObject.FindGameObjectsWithTag("Health_Container").Length != max_health || wait == true){
            if(wait == false){
                currentHealthBar =Instantiate(health_bar, new Vector3(health_bar.transform.position.x + 20*health_bar_offset, health_bar.transform.position.y,health_bar.transform.position.z), Quaternion.identity, Health_Container);
                currentHealthBar.transform.localScale = new Vector3(2,1,1);
                wait = true;
                
            }
            if(currentHealthBar != null){
                
                
                currentHealthBar.transform.localScale = Vector3.Lerp(currentHealthBar.transform.localScale, new Vector3(1f,1f,1f), .5f);
                if(currentHealthBar.transform.localScale == new Vector3(1f,1f,1f)){
                    wait = false;
                    health_bar_offset++;
                }
            }
            
            
            
        }
        
    }
    //Handles Slowmow Effect on Screen
    void SlowMow(){
        if(Time_Manager.FixedTheUpdate == true){
              GameObject.FindGameObjectWithTag("SlowMow").transform.localScale= Vector3.Lerp(GameObject.FindGameObjectWithTag("SlowMow").transform.localScale, new Vector3(18f,18f,18f),.05f);
        }else if(Time_Manager.FixedTheUpdate == false){
              GameObject.FindGameObjectWithTag("SlowMow").transform.localScale= Vector3.Lerp(GameObject.FindGameObjectWithTag("SlowMow").transform.localScale, new Vector3(0f,0f,0f),.2f);
        }
    }
    //Takes a health bar off when player takes damage
    void hurt_reaction(int maxhealth,int currenthealth){
        //Handles health bar removal when gettting hurt
        if(maxhealth > currenthealth && (maxhealth-currenthealth) != num_bar_gone){
            num_bar_gone = (maxhealth - currenthealth);
            //Debug.Log(num_bar_gone);
            hurt = true;
        }
    }
    //easiuer way to change transparency
    public static Color change_transparency(Color c, float desiredTransparency){
       c = new Color(c.r,c.g,c.b,0);
       return c;
    }

    public static void change_color(Color c,float r, float g, float b, float a){
       c = new Color(r,g,b,a);
    }
    //Gets rid of the appropriate bars of health
    void Get_Rid_of_bar(){
        //Little flair when getting hurt
        if(hurt == true){
            for(int x = 1; x<=num_bar_gone; x++){
                if(Canvas.transform.GetChild((amount_health_bars-x)+3).GetChild(1).GetComponent<Transform>().localScale != new Vector3(0,0,1)){
                    Canvas.transform.GetChild((amount_health_bars-x)+3).GetChild(1).GetComponent<Transform>().localScale = Vector2.Lerp(Canvas.transform.GetChild((amount_health_bars-x)+3).GetChild(1).GetComponent<Transform>().localScale, new Vector2(0,0), .2f);
                }
                if(x== (num_bar_gone)){
                    if(Canvas.transform.GetChild((amount_health_bars-x)+3).GetChild(1).GetComponent<Transform>().localScale == new Vector3(0,0,1)){
                        hurt = true;
                    }
                }
            }
          
            
        }
    }
}
