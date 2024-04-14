using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : DamageController
{
    protected Animator anim;
    protected Rigidbody2D rb;

    protected int facingDirection = -1;

    public int enemyMaxHealth = 1;
    public int enemyCurrentHealth;
    [SerializeField] private float enemyDeathDelay = 1f;

    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected LayerMask whatToIgnore;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected Transform enemySprite;
    [SerializeField] protected float playerDetectDistance = 10f;
    protected bool wallDetected;
    protected bool groundDetected;
    [SerializeField] protected LayerMask whatIsPlayer;
    protected bool playerDetected;
    protected GameObject player;
    [SerializeField] private bool facingRight;

    //protected Transform player;
    [HideInInspector] public bool invincible;

    [Header("FX")]
    //[SerializeField] private GameObject deathFx;

    [Header("Move info")]
    [SerializeField] protected float speed;
    [SerializeField] protected float idleTime = 2;
    protected float idleTimeCounter;


    protected bool canMove = true;
    protected bool aggressive;

    protected virtual void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;

        anim = enemySprite.GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        InvokeRepeating("FindPlayer", 0, .5f);
        FindPlayer();

        if (groundCheck == null)
            groundCheck = transform;
        if (wallCheck == null)
            wallCheck = transform;

        if (facingRight)
            Flip();
    }

    private void IsDead()
    {
        GetComponent<Collider2D>().enabled = false;
    }

    private void FindPlayer()
    {
        //if (player != null)
            //return;

        //if (Player.instance != null)
            player = FindObjectOfType<Player>().gameObject;
    }

    protected virtual void WalkAround()
    {
        if (idleTimeCounter <= 0 && canMove)
            rb.velocity = new Vector2(speed * facingDirection, rb.velocity.y);
        else
            rb.velocity = new Vector2(0, rb.velocity.y);

        idleTimeCounter -= Time.deltaTime;


        if (wallDetected || !groundDetected)
        {
            idleTimeCounter = idleTime;
            Flip();
        }
    }

    public virtual void Damage()
    {
        //if (!invincible)
        // {
        enemyCurrentHealth--;

        if (enemyCurrentHealth > 0)
                anim.SetTrigger("gotHit");
            else
            {
                canMove = false;
                anim.SetTrigger("death");
                IsDead();
                Invoke("DestroyMe", enemyDeathDelay);
            }
        //}
    }
    public void DestroyMe()
    {
        //if (deathFx != null)
        //{
         //   GameObject newDeathFx = Instantiate(deathFx, transform.position, transform.rotation);
           // Destroy(newDeathFx, .3f);
       // }

       // if (GetComponent<Enemy_DropController>() != null)
       //     GetComponent<Enemy_DropController>().DropFruits();
       // else
          //  Debug.LogWarning("You don't have !Enemy_DropController! on the enemy!");


        Destroy(gameObject);
    }


    protected virtual void Flip()
    {
        facingDirection = facingDirection * -1;
        transform.Rotate(0, 180, 0);
    }

    protected virtual void CollisionChecks()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
        wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGround);
        playerDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, playerDetectDistance, whatIsPlayer);
    }

    protected virtual void OnDrawGizmos()
    {
        if (groundCheck != null)
            Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));

        if (wallCheck != null)
        {
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y));
            Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + playerDetectDistance * facingDirection, wallCheck.position.y));
        }

    }
}
