using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : EnemyFollowScript
{
    public int knockback_multiplier;
    [SerializeField]
    public int health;

    bool give_color;
    
    Color hurt_Color = new Color(1f,0.245283f,0.245283f,1f);

    public bool knockback;
    public bool friction;
    public float knockback_speed;
    public float drag_speed;
    public bool colorChanged;

    

    

    

    
    //<---------------------------------------Deal Damage to this indivisual character--------------------->
    public void TakingDamage(int Damage, bool knockBack){
        health -= Damage;
        knockback = knockBack;
        if(knockback == true){
            if(Status_Effect.knockback_action(this.GetComponent<Rigidbody2D>(),knockback_speed,knockback_multiplier) == true){
                knockback = false;
                friction = true;
            }   
        }
        this.GetComponent<SpriteRenderer>().color = hurt_Color;
        colorChanged = true;
        
    }

    
    //<------------------------------Change the color of the attacked-------------------------->
    public void AssignColor(Color type_color, Color inital_Color, bool going_backward){
        switch(going_backward){
            case false:
                if(inital_Color != type_color){
                    this.GetComponent<SpriteRenderer>().color = Color.Lerp(inital_Color,type_color,.5f);
                }
                break;
            case true:
                if(new Color(1f,1f,1f,1f) != inital_Color){
                    this.GetComponent<SpriteRenderer>().color = Color.Lerp(inital_Color, new Color(1f,1f,1f,1f),.2f);
                }else{
                    colorChanged = false;
                }
                break;
                
        }
        
        
        
    }

    public void Death(){
        if(health <= 0){
            Destroy(this.gameObject);
        }
    }

   
    
}
