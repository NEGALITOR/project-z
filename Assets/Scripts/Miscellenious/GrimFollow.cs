using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class GrimFollow : MonoBehaviour
{
    public static int grimDamage;
    public int damagePower;
    public Transform player;
    public Vector2 posOffset;

    public float chaseDistance, coolDistance;

    Vector2 target;
    Vector2 startPosition;

    public bool isMoveing;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    public void Start()
    {
        grimDamage = damagePower;
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();


        InvokeRepeating("UpdatePath", 0f, .5f);
        seeker.StartPath(rb.position, target, OnPathComplete);
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
    }

    public void FixedUpdate()
    {
        if(isMoveing)
        {
            if (Vector2.Distance(transform.position, (player.position+(Vector3)posOffset)) <= chaseDistance)
            {
                target = player.position+(Vector3)posOffset;
            }
            else
            {
                this.transform.position = player.position;
            }

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
    }
}
