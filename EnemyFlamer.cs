using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlamer : Enemy
{
    [SerializeField] private GameObject attackZone;
    [SerializeField] private float spawnHitBoxDelay = 0.5f;
    [SerializeField] private float removeHitBoxDelay = 1f;

    private void Update()
    {
        CollisionChecks();

        if (!playerDetected)
        {
            WalkAround();
            anim.SetFloat("xVelocity", rb.velocity.x);
        }

        if(playerDetected)
        {
            anim.SetTrigger("attack");
            rb.velocity = new Vector2(0, rb.velocity.y);
            Invoke("SpawnHitBox", spawnHitBoxDelay);
        }


    }

    private void SpawnHitBox()
    {
        attackZone.SetActive(true);
        if (!playerDetected)
        {
            Invoke("RemoveHitBox", removeHitBoxDelay);
        }
    }

    private void RemoveHitBox()
    {
        attackZone.SetActive(false);
        idleTimeCounter = idleTime;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
