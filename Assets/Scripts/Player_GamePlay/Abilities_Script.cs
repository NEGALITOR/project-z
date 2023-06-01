using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public abstract class Abilities_Script : MonoBehaviour
{
    // Start is called before the first frame update
    GameObject AbilityEffect;
    CircleCollider2D sc;
    BoxCollider2D bac;
    SpriteRenderer sra;
    VisualEffect system;
    Ability_Detector ad;
    public GameObject camera_container;
    public CameraShake cameraShake;
    public Time_Manager time_Manager;
    bool initiate;
    public static string ability_name;
    public static bool ability_activated;
    bool particlestarted;
    
    //Explosion Aiblity
    public void Explosion(float range){
        if(initiate == false){
            Player_GamePlay.AllowMovement = false;
            
            AbilityEffect = GameObject.FindWithTag("Abilites");
            sc = AbilityEffect.AddComponent<CircleCollider2D>();
            sra =AbilityEffect.AddComponent<SpriteRenderer>();
            sra.enabled = false;
            ability_name = "Explosion!";
            createParitclesys(ability_name);
            cameraShake.Shake(0.01f,1.5f);
            
            Sprite explosion = Resources.Load<Sprite>("download (3)");
            sra.sprite = explosion;
            sc.isTrigger = true;
            sc.radius = .26f;
            sra.drawMode = SpriteDrawMode.Sliced;
            
            
            initiate = true;
            
            
        }
        Player_GamePlay.sr.material.SetColor("_OutlineColour",Color.Lerp(Player_GamePlay.sr.material.GetColor("_OutlineColour"),new Color(246,17,17,255),.0002f));
        if(system != null){
            if(particlestarted == false && system.aliveParticleCount > 0){
                particlestarted = true;
            }
            if(system.aliveParticleCount ==0 && particlestarted == true && ability_activated == false){
                if(Time.timeScale==1 && ability_activated == false){
                    ability_activated = true;
                    particlestarted = false;
                }
                
            }
            
            
        }
        
       
        
       
        
     
       
       if(ability_activated == true && Time.timeScale==1){
           
           if(sc.radius == .26f){
                if(Player_GamePlay.current_health ==1){
                    Player_GamePlay.current_health = 0;
                }else{
                    Player_GamePlay.current_health -= Mathf.RoundToInt((Player_GamePlay.current_health*.5f));
                }
                sra.enabled = true;
                ad = AbilityEffect.AddComponent<Ability_Detector>();
           }
             if(sc.radius >= (range-.1f) ){
                Player_GamePlay.AllowMovement = true;
                Destroy(sc);
                Destroy(sra);
                Destroy(ad);
                Destroy(system);
                Player_GamePlay.use_ability = false;
                ability_activated = false;   
            }else{
                sc.radius =Mathf.Lerp(sc.radius,range, .1f);
                sra.size = new Vector2(sc.radius*2, sc.radius*2);
                
            }
       } 
      
        
    }
    //Stun Ability
    public void TaserTime(float range){
        if(initiate == false){
            AbilityEffect = GameObject.FindWithTag("Abilites");
            bac= AbilityEffect.AddComponent<BoxCollider2D>();
            ability_name = "Taser Time";
            bac.isTrigger = true;
            bac.size = new Vector2(range, 0.37f);
            initiate = true;
            


        }
        Status_Effect.Stunned(Player_GamePlay.rb,5f);
    }

    public void Reset(){
        initiate = false;
        Player_GamePlay.sr.material.SetColor("_OutlineColour",Color.Lerp(Player_GamePlay.sr.material.GetColor("_OutlineColour"),new Color(0.1259473f,0.1122865f,0.1122865f,1f),.05f));
    }
    //Calls Particle system files
    void createParitclesys(string ability){
        system = AbilityEffect.AddComponent<VisualEffect>();
        if(ability == "Explosion!"){
            if(system !=null){
                VisualEffectAsset Charge =Resources.Load<VisualEffectAsset>("ParticleSystems/Charge");
                system.visualEffectAsset = Charge;
                Gradient gradient1 = new Gradient();
                Gradient gradient2 = new Gradient();
                gradient1.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.red, 1.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
                );
                gradient2.SetKeys(
                    new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(new Color(1f, 0.6f, 0f), 1.0f) },
                    new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 0.0f), new GradientAlphaKey(1.0f, 1.0f) }
                );
                system.SetGradient("Gradient1",gradient1);
                system.SetGradient("Gradient2", gradient2);
            }
            
            
        }
    }
    
}
