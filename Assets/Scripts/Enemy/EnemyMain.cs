using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMain : Enemy
{
    public bool hurt_me;
    //Main Class of an enemy
    void Update()
    {
        Death();
        if(friction == false){
            Follow = true;
        }
        if(friction == true){
            if(Status_Effect.friction(this.GetComponent<Rigidbody2D>(), drag_speed) == true){
                friction = false;
            }
        }
        if(colorChanged == true){
            AssignColor(new Color(1f,1f,1f,1f),this.GetComponent<SpriteRenderer>().color, colorChanged);
        }

        
        
        
    }
}
