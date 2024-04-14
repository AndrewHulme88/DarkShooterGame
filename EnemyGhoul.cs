using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGhoul : Enemy
{
    [SerializeField] private bool isAwake;
    [SerializeField] private float checkRadius;
    [SerializeField] private float wakeUpDelay = 1f;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        CollisionChecks();

        playerDetected = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);

        if(playerDetected && !isAwake)
        {
            anim.SetTrigger("wakeUp");
            Invoke("WakeUp", wakeUpDelay);
        }

        if (isAwake)
        {
            WalkAround();

            anim.SetFloat("xVelocity", rb.velocity.x);
        }
    }

    private void WakeUp()
    {
        isAwake = true;
        anim.SetBool("isAwake", isAwake);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
