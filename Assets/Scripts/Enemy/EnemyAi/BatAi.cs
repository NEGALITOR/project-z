using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAi : MonoBehaviour
{
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        this.GetComponent<EnemyMain>().Follow = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
