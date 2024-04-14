using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFloater : Enemy
{
    private bool ceillingDetected;

    [Header("Floater specific")]
    [SerializeField] private float ceillingDistance;
    [SerializeField] private float groundDistance;
    [SerializeField] private float flyDelay = 0.5f;

    [SerializeField] private float flyUpForce;
    [SerializeField] private float flyDownForce;
    private float flyForce;

    private bool canFly = true;


    protected override void Start()
    {
        base.Start();
        flyForce = flyUpForce;
        InvokeRepeating("FlyUpEvent", flyDelay, flyDelay);
    }

    private void Update()
    {
        CollisionChecks();

        if (ceillingDetected)
            flyForce = flyDownForce;
        else if (groundDetected)
            flyForce = flyUpForce;

        if (wallDetected)
            Flip();
    }

    public override void Damage()
    {
        canFly = false;
        rb.velocity = new Vector2(0, 0);
        rb.gravityScale = 0;
        base.Damage();
    }

    public void FlyUpEvent()
    {
        if (canFly)
            rb.velocity = new Vector2(speed * facingDirection, flyForce);
    }

    protected override void CollisionChecks()
    {
        base.CollisionChecks();

        ceillingDetected = Physics2D.Raycast(transform.position, Vector2.up, ceillingDistance, whatIsGround);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + ceillingDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundDistance));
    }
}
