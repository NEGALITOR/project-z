using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class HomingBullets : MonoBehaviour
{
    public GameObject targetEnemy;

    Vector2 target;
    Vector2 startPosition;

    public float speed = 200f;
    bool reducedSpeed;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;

    Seeker seeker;
    Rigidbody2D rb;
    
    float multiplier;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        startPosition = transform.position;
        multiplier = Random.Range(3,1);
        speed = speed *multiplier;

        InvokeRepeating("UpdatePath", 0f, .5f);
        seeker.StartPath(rb.position, target, OnPathComplete);
    }

    public void Update(){
        if(!reducedSpeed){
            speed = speed/multiplier;
            reducedSpeed = true;
        }
    }

    public void UpdatePath()
    {
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target, OnPathComplete);
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
        if(targetEnemy == null){
            Destroy(this.gameObject);
        }
    }

    public void FixedUpdate()
    {
        target = targetEnemy.transform.position;

        if (path == null)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndofPath = true;
            return;
        }
        else
        {
            reachedEndofPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        
    }
    public void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Enemy"){
            other.GetComponent<EnemyMain>().TakingDamage(Grim.grimDamage,false);
            Destroy(this.gameObject);
        }
    }
   
    
}
