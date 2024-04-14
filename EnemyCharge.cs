using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharge : Enemy
{
    [Header("Charge specific")]
    [SerializeField] private float agroSpeed;
    [SerializeField] private float shockTime;
    private float shockTimeCounter;



    protected override void Start()
    {
        base.Start();
        invincible = true;
    }


    // Update is called once per frame
    void Update()
    {
        CollisionChecks();
        AnimatorControllers();

        if (!playerDetected)
        {
            WalkAround();
            return;
        }


        if (playerDetected)
            aggressive = true;

        if (!aggressive)
        {
            WalkAround();
        }

        else
        {
            if (!groundDetected)
            {
                aggressive = false;
                Flip();
            }

            rb.velocity = new Vector2(agroSpeed * facingDirection, rb.velocity.y);

            if (wallDetected && invincible)
            {
                invincible = false;
                shockTimeCounter = shockTime;
            }



            if (shockTimeCounter <= 0 && !invincible)
            {
                invincible = true;
                Flip();
                aggressive = false;
            }

            shockTimeCounter -= Time.deltaTime;
        }


    }

    private void AnimatorControllers()
    {
        anim.SetBool("invincible", invincible);
        anim.SetFloat("xVelocity", rb.velocity.x);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
