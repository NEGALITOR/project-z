using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_GamePlay : Abilities_Script
{

    //<-----------------------Variables---------------------->

    //used for effecting movement
    public static Rigidbody2D rb;
    //used for what animation to use
    public Animator anim;

    //used for movement speed
    float moving_speed = 1.5f;

    //used for jumping
    public float Jump_Force;
    private bool Jumped;
    //Dash Line
    private LineRenderer lr;
    
    public static SpriteRenderer sr;
    float Sprint_Speed;
    float distToGround;
    float distToWall;
    public static bool Combat_Mode;
    int combo_num = 0;
    bool combo_active;
    public float time_alloated_between_combo;
    float time;
    bool attacking;
    public static bool attacked = false;
    bool combo_complete;
    public GameObject attack;
    private Collider2D attack_capsule;
    public static int basedamage;
    public int user_damage;
    float capped_velocity;
    
  
    float max_velocity;
    bool slide_crouch;

    Vector2 inital_collider_sz;
    Vector2 inital_collider_off;

    public PhysicsMaterial2D physic;
    private BoxCollider2D bc;

    bool possible_air_attack;
    public static bool def_air_attack,air_combo_finisher;
    static public int Max_player_health = 5,current_health;

    private int air_combo_num;


    public static bool hasBeenDamage,Gamplay_mode = false;


    Vector3 LightSpeedDirection;
    Vector3[] hitPointAlongLine, pointAlongLine;

    
    
    int linepoints, currentlinepoint, storedlinepoints,reflectionpoints = 1;
    float[] counter, storedDistance;
    public float dashLineDrawSpeed;
    bool hasHit;

    Vector3 reflectiondir; 
    bool[] lineDrawn;
    Ray2D ray;
    RaycastHit2D hit;

    float distance_allowed_travel = 5;

    float initial_distance_allowed_travel;
    bool LightSpeedDashCondition;
    public static bool DoDash;

    
    Vector3[] dashpositions;
    bool ability_active;

    float jumpercounter;
    public float jumptime;

    public float dashCoolDown = 5, shieldCooldown = 5;
    float dashCurrentCooldown;
    float shieldCurrentCooldDown;
    public static bool use_ability;

    public static bool AllowMovement = true;

    public static bool ShieldIsActive,blastActivated;
    bool shieldbroken;
    public int shieldBreakPoint;

    
    



   
    

    // Start is called before the first frame update
    void Start()
    {

        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();
        lr = this.GetComponentInChildren<LineRenderer>();
        cameraShake = camera_container.GetComponent<CameraShake>();
        time = time_alloated_between_combo;
        lineDrawn = new bool[reflectionpoints+1];
        counter = new float[reflectionpoints+1];
        hitPointAlongLine = new Vector3[reflectionpoints+1];
        pointAlongLine = new Vector3[reflectionpoints+1];
        storedDistance = new float[reflectionpoints+1];
        attack_capsule = attack.GetComponent<Collider2D>();
        bc = this.GetComponent<BoxCollider2D>();
        inital_collider_sz = bc.size;
        inital_collider_off = bc.offset;
        current_health = Max_player_health;
        Sprint_Speed = moving_speed*2;
        initial_distance_allowed_travel = distance_allowed_travel;
        
    }

    // Update is called once per frame
    void Update()
    {
    //This allows control of switch modes form the game master
        if(Gamplay_mode == true){
            //<--------------------------------Animation Variables------------------------>
            distToGround = GetComponent<Collider2D>().bounds.extents.y;
            distToWall = GetComponent<Collider2D>().bounds.extents.x;
            anim.SetFloat("Speed_x", Mathf.Abs(rb.velocity.x));
            anim.SetBool("Jumping?", Jumped);
            anim.SetBool("OnGround?",Check_Ground());
            anim.SetFloat("Velocity_Y",rb.velocity.y);
            if(Input.GetKeyDown(KeyCode.J) && ability_active == false){
                if(Combat_Mode == false){
                    Combat_Mode = true;
                    
                }
                   
                attacked = true;

                
               
            }
            if(Input.GetKeyDown(KeyCode.N)){
                Max_player_health++;
            }
        
            anim.SetInteger("Combo_Num", combo_num);
            anim.SetBool("Attacked", attacked);
            anim.SetBool("Combo_Complete", combo_complete);
            anim.SetBool("Slide_or_Crouch",slide_crouch);
            anim.SetBool("Air_Attack?", def_air_attack);
            anim.SetBool("Air_Finisher?", air_combo_finisher);
            anim.SetInteger("Air_Combo_Num",air_combo_num);
            anim.SetBool("In_Combat?",Combat_Mode);
            basedamage = user_damage;
            Attack_Combo();
            LightSpeedDash();
            Shield();
            if(Input.GetKey(KeyCode.L)){
                use_ability = true;
            }
            if(use_ability == true){
                
                Explosion(1f);
            }else if(use_ability == false){
                Reset();
            }
           
            Quit();
            
        }
         
    }
    void FixedUpdate()
    {
        if(Gamplay_mode == true){
            //<-----------------------------Core Functions--------------------->
            StartCoroutine(Basic_Movement());
            Jump();
            slide();
            StartCoroutine(Air_Attack());
           
            
        }
       
        
        
        
        
    }
    void LateUpdate(){
        attacked = false;
    }
    //Handles basic movement controls
    IEnumerator Basic_Movement()
    {
        if(combo_num == 0 && LightSpeedDashCondition == false && DoDash == false && AllowMovement == true){
           
    //<------------------------------------------------Move a static Velocity when moving left or right, Walking/Sprinting (Might Change to Force Later(Newly Added!!!))----------------------->
            if((Input.GetAxis("Sprint")==0 || Check_Ground() == false) && slide_crouch == false){
                if(Input.GetAxis("Horizontal") > 0 ){
                    rb.velocity = new Vector2(moving_speed, rb.velocity.y);
                    sr.flipX = false;
                    attack_capsule.offset = new Vector2(Mathf.Abs(attack_capsule.offset.x),attack_capsule.offset.y);
                    
                }
                else if(Input.GetAxis("Horizontal")<0 ){
                    rb.velocity =new Vector2(-moving_speed, rb.velocity.y);
                    yield return new WaitUntil(() => (rb.velocity.x <= 0)); 
                    sr.flipX = true;
                    attack_capsule.offset = new Vector2(-Mathf.Abs(attack_capsule.offset.x),attack_capsule.offset.y);
                }else{
                    rb.velocity=new Vector2(0,rb.velocity.y);
                }
                
            }else if(Input.GetAxis("Sprint") > 0 && Check_Ground() && attacked == false ){
                    
                if(Input.GetAxis("Horizontal") > 0){
                    rb.velocity =new Vector2(Sprint_Speed,rb.velocity.y);
                    sr.flipX = false;
                }else if(Input.GetAxis("Horizontal")<0 ){
                    rb.velocity =new Vector2(-Sprint_Speed,rb.velocity.y);
                    sr.flipX = true;
                }else{
                    rb.velocity =new Vector2(0,rb.velocity.y);
                }
            }
           
        }
        
       
        
        
    }
    //<-------------------------------Jumping by Force------------------------------>
    void Jump(){
        if(slide_crouch == false){
            if(Input.GetKey(KeyCode.W) && Check_Ground() == true && LightSpeedDashCondition == false && Jumped == false && AllowMovement == true){
                Jumped = true;
                jumpercounter = jumptime;
                rb.velocity = new Vector2(rb.velocity.x,Jump_Force);
                
            
            }else if(Check_Ground() == false && Input.GetKey(KeyCode.W) == false){
                Jumped = false;
            }
            if(Input.GetKey(KeyCode.W)  && Jumped == true){
                if(jumpercounter > 0){
                    rb.velocity = new Vector2(rb.velocity.x,Jump_Force);
                    jumpercounter -= Time.deltaTime;
                    
                }else{
                    Jumped = false;
                }
            }
          
            
        }
       
       
       
      

       
        
        
    }
    //Allows the player to quit game
    void Quit(){
        if(Input.GetKeyDown(KeyCode.Escape)){
            Application.Quit();
        }
        
    }
    //<-------------------------------------------Checking if bottom of character is on ground by using raycasting---------------------------->
    bool Check_Ground(){
        return Physics2D.Linecast(new Vector2(this.transform.position.x,(this.transform.position.y-distToGround)-.03f),new Vector2(this.transform.position.x, ((this.transform.position.y-distToGround)-.03f)-.1f));
    }
    int Check_Wall(){
        if(Physics2D.Linecast(new Vector2(((this.transform.position.x + distToWall) + .03f), this.transform.position.y),new Vector2(((this.transform.position.x +distToWall) + .03f)+.01f, this.transform.position.y))==true || Physics2D.Linecast(new Vector2(((this.transform.position.x + distToWall) + .03f), this.transform.position.y+.06f),new Vector2(((this.transform.position.x +distToWall) + .03f)+.01f, this.transform.position.y+.06f))==true){
            return 1;
        }
        if(Physics2D.Linecast(new Vector2(((this.transform.position.x - distToWall) - .03f), this.transform.position.y),new Vector2(((this.transform.position.x -distToWall) - .03f)-.01f, this.transform.position.y))==true || Physics2D.Linecast(new Vector2(((this.transform.position.x + distToWall) + .03f), this.transform.position.y+.06f),new Vector2(((this.transform.position.x +distToWall) + .03f)+.01f, this.transform.position.y+.06f))==true){
            return -1;
        }
        else{
            return 0;
        }
    }
    //<-------------------------Attack Combo, reset if not pressed in a certain amount of time----------------------->
    void Attack_Combo(){
        if(Combat_Mode == true){
            if(attacking == true){
                Timer();
                
            }
         
            if(time != 0 && Input.GetAxis("Normal Attack")>0 && combo_active == false){
                if(Check_Ground() && def_air_attack == false){
                    combo_num++;
                    
                    air_combo_num =0;
                }else if(Check_Ground() == false){
                    air_combo_num ++;
                    combo_num = 0;
                }
                
                
                combo_active = true;
                time = time_alloated_between_combo;
                attacking = true;
            }
            if(time <= 0){
                combo_num = 0;
               
                combo_complete = true;
            }
            if(Input.GetAxis("Normal Attack") == 0){
                combo_active = false;
            }
            if(combo_num >= 3 || air_combo_num >= 3){
                combo_complete = true;
            }
            if(combo_num == 0){
                combo_complete = false;
            }
            if(anim.GetCurrentAnimatorStateInfo(0).IsTag("3") == true || anim.GetCurrentAnimatorStateInfo(0).IsTag("6") == true){
                combo_num=0;
            }
                
            
            
            if(attacked == true){
                if(attack.GetComponent<CapsuleCollider2D>().enabled != true && Check_Ground() && def_air_attack == false){
                    attack.GetComponent<CapsuleCollider2D>().enabled = true;
                }
                if(possible_air_attack != true){
                    possible_air_attack = true;
                }
                
            } 
           
            
            

        }
        
    }
    //<-----------------Timer for Attack Combo---------------------->
    float Timer(){
        time = time - Time.deltaTime;
        return time;
    }
    //Hanldes the slide ability
    void slide(){
        if(Check_Ground()){
            if(Input.GetAxis("Slide/Crounch") > 0){
                slide_crouch =true;
           
            }else if(Input.GetAxis("Slide/Crounch") == 0){
                slide_crouch = false;
            }
            if(slide_crouch == true && (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Horizontal")< 0)){
                physic.friction = .2f;
                bc.size =change_size(.5f,0.19f);
                bc.offset = new Vector2(0,-.04f);
            }
            if(slide_crouch == false){
                physic.friction = .4f;
                bc.size = inital_collider_sz;
                bc.offset = inital_collider_off;
            }
        }
        
    }
    Vector2 change_size(float new_width, float new_height){
        return new Vector2(new_width,new_height);
    }
    //<--------------------Handles Air_Combos & Attack--------->
    IEnumerator Air_Attack(){
        if(Check_Ground() == false && possible_air_attack == true && slide_crouch == false){
            this.transform.GetChild(2).GetComponent<CircleCollider2D>().enabled = true;
            def_air_attack =true;
            
        }else if(Check_Ground() || possible_air_attack == false){
            this.transform.GetChild(2).GetComponent<CircleCollider2D>().enabled = false;
            possible_air_attack = false;
            def_air_attack = false;
           
            
           
        }
        if(possible_air_attack == true){
            yield return new WaitForSeconds(.5f);
            possible_air_attack = false;
        }
        
       
       
        

        
    }
    
    
    //Light Speed Dash Controller
    void LightSpeedDash(){
        DrawLine();
        DashControls();
        StartCoroutine(DashAction());
        
       

    }
    //Handles Direction of Dash
    void DashControls(){
         if(Input.GetAxis("Ability") > 0 && ((Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0) || LightSpeedDashCondition == true) && DoDash == false && dashCurrentCooldown <= 0 && AllowMovement == true){
            LightSpeedDashCondition = true;
            if(Input.GetAxis("Horizontal") > 0){
                this.transform.GetChild(2).transform.Rotate(new Vector3(0, 0, -3));
            }
            if(Input.GetAxis("Horizontal") < 0){
                this.transform.GetChild(2).transform.Rotate(new Vector3(0,0,3));
            }
            time_Manager.DoSlowMotion(.5f,1f);
            

        }else{
            LightSpeedDashCondition = false;
            for(int x =0; x< counter.Length; x++){
                counter[x] = 0;
            }
            for(int x =0; x< lineDrawn.Length; x++){
                lineDrawn[x] = false;
            }
            if(dashCurrentCooldown > 0){
                dashCurrentCooldown = dashCurrentCooldown - Time.deltaTime;
            }
        }
        
    }
    
    //Handles the literal Dash
    IEnumerator DashAction(){
        if(lr.positionCount > 1){
             if(LightSpeedDashCondition == true && Input.GetAxis("Normal Attack") > 0 && DoDash == false){
                 dashpositions = new Vector3[lr.positionCount];
                 lr.GetPositions(dashpositions);
                 storedlinepoints = lr.positionCount;
                 //this.GetComponent<BoxCollider2D>().isTrigger = true;
                 rb.bodyType = RigidbodyType2D.Kinematic;
                 DoDash = true;
                 ability_active = true;
                 

             }
             
           
        }
        if(DoDash == true){
            Mathf.Clamp(currentlinepoint, 1, storedlinepoints);
            rb.bodyType = RigidbodyType2D.Kinematic;
            this.transform.GetChild(2).GetComponent<TrailRenderer>().enabled = true;
            transform.position = Vector2.MoveTowards(transform.position,dashpositions[currentlinepoint],.3f);
           
            if(new Vector2(transform.position.x, transform.position.y) == new Vector2(dashpositions[currentlinepoint].x,dashpositions[currentlinepoint].y)){
                currentlinepoint+=1;
                
            }
            
            if(currentlinepoint == storedlinepoints){
                dashCurrentCooldown = dashCoolDown;
                DoDash = false;
            }
             
            

            
        }
        if(DoDash == false){
            currentlinepoint = 1;
            dashpositions = null;
            this.GetComponent<BoxCollider2D>().isTrigger = false;
            rb.bodyType = RigidbodyType2D.Dynamic;
            this.transform.GetChild(2).GetComponent<TrailRenderer>().enabled = false;
            if(ability_active == true){
                yield return new WaitForSeconds(.5f);
                ability_active = false;
            }

            
        }
       
    }
    //Draws the line of the dash direction
    void DrawLine(){
        if(sr.flipX == true){
            LightSpeedDirection = -this.transform.GetChild(2).transform.right;
        }else if(sr.flipX == false){
            LightSpeedDirection =this.transform.GetChild(2).transform.right;
        }
        
        if(LightSpeedDashCondition == true && DoDash == false){
            ray = new Ray2D(transform.position,LightSpeedDirection); 
            Debug.DrawRay(transform.position, LightSpeedDirection*100, Color.green);
            linepoints = reflectionpoints;
            lr.positionCount = linepoints;
            lr.SetPosition(0,transform.position + new Vector3(0,0,1));  
            for(int i = 0; i<=reflectionpoints; i++){
                if(i==0){
                    Debug.Log(lineDrawn[i]);
                    hit = Physics2D.Raycast(ray.origin, ray.direction, initial_distance_allowed_travel);
                    //If Line hits a wall
                    if(hit){
                        
                        
                        reflectiondir = Vector3.Reflect(LightSpeedDirection,hit.normal);
                        distance_allowed_travel = initial_distance_allowed_travel - Vector2.Distance(hit.point,ray.origin);
                        
                        
                        if(lineDrawn[i]){
                            if(Vector2.Distance(hitPointAlongLine[i],lr.GetPosition(0)) > Vector2.Distance(hit.point,ray.origin)){
                                counter[i] = Vector2.Distance(hit.point,ray.origin)-.1f;  
                                storedDistance[i] = Vector2.Distance(hit.point,ray.origin); 
                            }
                        }
                        if(reflectionpoints==1)  
                        {  
                            //add a new vertex to the line renderer  
                            lr.positionCount = ++linepoints;  
                        }
                        if(counter[i] < Vector2.Distance(hit.point,ray.origin)){
                            counter[i] += .1f/(dashLineDrawSpeed);
                            float y = Mathf.Lerp(0,Vector2.Distance(hit.point,ray.origin),counter[i]*3);
                            hitPointAlongLine[i] = y * LightSpeedDirection+lr.GetPosition(0);
                        }
                        if(Vector2.Distance(hitPointAlongLine[i],lr.GetPosition(0)) >= Vector2.Distance(hit.point,ray.origin) && !lineDrawn[i]){
                            lineDrawn[i] = true;
                        }
                        
                        lr.SetPosition(i+1,hitPointAlongLine[i]); 
                        

                        ray = new Ray2D(hit.point, reflectiondir);
                        Debug.DrawRay(hit.point, hit.normal*3, Color.blue);    
                        Debug.DrawRay(hit.point, reflectiondir*100, Color.magenta); 
                        
                       

                    }else{
                        if(linepoints > 2 || lineDrawn[i]){
                            linepoints = 1;
                            for(int x = 0; x< lineDrawn.Length; x++){
                               lineDrawn[x] = false;
                            }
                            counter[i] = storedDistance[i]/10;

                        }
                        if(reflectionpoints==1)  
                        {  
                            //add a new vertex to the line renderer  
                            lr.positionCount = ++linepoints;  
                            
                        }
                        
                        
                        if(counter[i] < distance_allowed_travel){
                            counter[i] += .1f/dashLineDrawSpeed;
                            float x = Mathf.Lerp(0,distance_allowed_travel,counter[i]);
                            pointAlongLine[i] = x * LightSpeedDirection+lr.GetPosition(0);
                            //Debug.Log("Counter: " + counter[i] + " x: " + x + " pointAlongLine" + pointAlongLine[i]);
                            
                        }
                        lr.SetPosition(i+1,pointAlongLine[i]);
                        
                        

                        
                        
                    }
                        
                        
                    
                }else{
                        hit = Physics2D.Raycast(ray.origin, ray.direction, distance_allowed_travel);
                        if(hit){
                            
                            //reflectionpoints++;
                                //the refletion direction is the reflection of the ray's direction at the hit normal 
                            if(lineDrawn[i-1]){
                                if(distance_allowed_travel > 0){
                                    reflectiondir = Vector3.Reflect(reflectiondir,hit.normal);  
                                    distance_allowed_travel = distance_allowed_travel - Vector2.Distance(hit.point,ray.origin);
                                    
                                    //cast the reflected ray, using the hit point as the origin and the reflected direction as the direction  
                                    
                                    if(lineDrawn[i]){
                                        if(Vector2.Distance(hitPointAlongLine[i],lr.GetPosition(i)) > Vector2.Distance(hit.point,ray.origin)){
                                            counter[i] = Vector2.Distance(hit.point,ray.origin)-.1f;   
                                        }
                                    }
                                    if(counter[i] < Vector2.Distance(hit.point,ray.origin)){
                                        counter[i] += .1f/(dashLineDrawSpeed);
                                        float y = Mathf.Lerp(0,Vector2.Distance(hit.point,ray.origin),counter[i]*3);
                                        hitPointAlongLine[i] = y * (Vector3)ray.direction+lr.GetPosition(i);
                                        
                                    }
                                    if(Vector2.Distance(hitPointAlongLine[i],lr.GetPosition(0)) >= Vector2.Distance(hit.point,ray.origin) && !lineDrawn[i]){
                                        lineDrawn[i] = true;
                                    }
                                    ray = new Ray2D(hit.point,reflectiondir);  
                                    //Draw the normal - can only be seen at the Scene tab, for debugging purposes  
                                    Debug.DrawRay(hit.point, hit.normal*3, Color.blue);  
                                    //represent the ray using a line that can only be viewed at the scene tab  
                                    Debug.DrawRay(hit.point, reflectiondir*100, Color.red);  
                
                                    
                                    lr.positionCount = ++linepoints;
                                
                
                                    
                                    //set the position of the next vertex at the line renderer to be the same as the hit point  
                                    lr.SetPosition(i+1,hitPointAlongLine[i]);
                                }
                            } 
                          
                        }else{
                            if(lineDrawn[i-1]){
                                if(counter[i] < distance_allowed_travel){
                                    counter[i] += .1f/dashLineDrawSpeed;
                                    float x = Mathf.Lerp(0,distance_allowed_travel,counter[i]);
                                    pointAlongLine[i] = x * (Vector3)ray.direction+lr.GetPosition(i);
                            
                                }
                                lr.positionCount = ++linepoints; 
                                lr.SetPosition(i+1,hitPointAlongLine[i]);
                            }
                            
                            
                            
                        
                        }
                        
                }
               
            }
            distance_allowed_travel = initial_distance_allowed_travel;
        }else if(LightSpeedDashCondition == false || DoDash == true){
            lr.positionCount = 0;
        }
       
        
        
    }
    //Shield Root Controller
    void Shield(){
        CastShield();
        ReleaseShieldExplode();
    }
    //Casts the Shield and Handles anything that could stop it's casting
    void CastShield(){
        CircleCollider2D shieldbody =this.transform.GetChild(4).GetComponent<CircleCollider2D>();
        if(Input.GetKey(KeyCode.S) && Input.GetAxis("Ability") > 0 && shieldCurrentCooldDown <= 0 ){
            ShieldIsActive = true;
            shieldbody.enabled = true;
            shieldbody.radius = Mathf.Lerp(shieldbody.radius,0.44f,.2f);
        }else if(shieldbody.radius > 0.02f && blastActivated == false && ShieldIsActive == true){
            shieldbody.radius = Mathf.Lerp(shieldbody.radius, 0.01f, .2f);
        }else if(ShieldIsActive == true && shieldbody.radius < .02f){
            ShieldAbsorb.AbsorbedDamage = 0;
            shieldbody.enabled = false;
            shieldCurrentCooldDown = shieldCooldown;
            ShieldIsActive = false;
        }
        if(blastActivated == true){
            shieldbody.radius *= 8f;
        }
        if(ShieldAbsorb.AbsorbedDamage >= shieldBreakPoint){
            ShieldAbsorb.AbsorbedDamage = 0;
            shieldbroken = true;
            shieldCurrentCooldDown = shieldCooldown;
        }
        if(shieldbroken == true){
            if(Status_Effect.Stunned(rb,.5f)){
                shieldbroken = false;
            }
        }
        if(shieldCurrentCooldDown> 0){
            shieldCurrentCooldDown -= Time.deltaTime;
        }
        
    }
    //Hanldes Shield Explosion if Player Activates
    void ReleaseShieldExplode(){
        PointEffector2D shieldBlast = this.transform.GetChild(4).GetComponent<PointEffector2D>();
        if(ShieldIsActive == true && Input.GetAxis("Normal Attack") > 0 && blastActivated == false && shieldCurrentCooldDown <=0){
            shieldBlast.forceMagnitude += (shieldBlast.forceMagnitude*ShieldAbsorb.AbsorbedDamage)*2;
            blastActivated = true;
            shieldCurrentCooldDown = shieldCooldown;
            
        }else{
            shieldBlast.forceMagnitude = 5;
        }
    }
    
    

    
    

}

