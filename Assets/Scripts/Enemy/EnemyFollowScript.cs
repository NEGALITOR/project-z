using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public abstract class EnemyFollowScript : Touch_me_and_I_will_damage_you
{
    public Transform player;

    public float chaseDistance, coolDistance;

    Vector2 target;
    Vector2 startPosition;

    public bool Follow;

    public float speed = 200f;
    public float nextWaypointDistance = 3f;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndofPath = false;

    Seeker seeker;
    Rigidbody2D rb;

    public void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        startPosition = transform.position;

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
        if(Follow)
        {
            if (Vector2.Distance(transform.position, player.position) <= chaseDistance)
            {
                target = player.position;
            }
            else
            {
                target = startPosition;
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
