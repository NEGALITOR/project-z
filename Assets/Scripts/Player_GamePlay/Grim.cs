using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;


public class Grim : GrimFollow
{
    
    float distanceFromCharacter = 1.95f;
    GameObject lockedEnemy;
    public GameObject bullet;
    public VisualEffect System;
    bool shootAgain = true;

    void Update(){
        shoot();
    }
    void shoot(){
        selectTarget();
        StartCoroutine(doShoot());
    }
    
    void selectTarget(){
        foreach(GameObject enemy in EnemyInRange.enemies){
            if(Vector2.Distance(player.position, enemy.transform.position)< distanceFromCharacter){
                lockedEnemy = enemy;
                distanceFromCharacter = Vector2.Distance(player.position,enemy.transform.position);
            }
        }
        if(EnemyInRange.enemies.Count == 0){
            lockedEnemy = null;
        }
        if(lockedEnemy != null){
            if(Vector2.Distance(player.position,lockedEnemy.transform.position) > distanceFromCharacter){
                distanceFromCharacter = 1.95f;
            }
        }
    }
    IEnumerator doShoot(){
        if(Input.GetKey(KeyCode.I) && lockedEnemy != null && shootAgain){
            Instantiate(bullet,this.transform.position,Quaternion.identity).GetComponent<HomingBullets>().targetEnemy = lockedEnemy;
            System.Play();
            shootAgain = false;
            yield return new WaitForSeconds(.2f);
            shootAgain = true;
        }
    }
}
