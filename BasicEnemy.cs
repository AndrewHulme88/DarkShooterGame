using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : Enemy
{
    // Update is called once per frame
    void Update()
    {
        CollisionChecks();
        WalkAround();

        //anim.SetFloat("xVelocity", rb.velocity.x);
    }
}
