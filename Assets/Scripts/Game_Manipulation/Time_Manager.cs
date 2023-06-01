using UnityEngine;

public class Time_Manager : MonoBehaviour
{
    
    public static float slowdownLength =  .1f;
    float inital_delta_time;
    public static bool FixedTheUpdate = false;
    bool copied = false;

   //Brings time back to normal if time has been slowed down
    void Update(){
        Time.timeScale += (1/slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f,1f);
        if(Time.timeScale == 1 && FixedTheUpdate == true){
            Time.fixedDeltaTime = inital_delta_time;
            FixedTheUpdate = false;
            
        }
       
        
       
        
       
    }
    //Actually slows down time for a certain amount of time
    public void DoSlowMotion(float slowdownFactor, float slowdownlength){
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale*0.02f;
        FixedTheUpdate = true;
        slowdownLength = slowdownlength;

      
        
    }
    //Copies the original fixedDeltaTime Value
    void FixedUpdate(){
        if(copied == false){
            inital_delta_time = Time.fixedDeltaTime;
            copied = true;
        }
    }
}
