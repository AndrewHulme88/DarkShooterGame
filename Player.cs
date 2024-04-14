using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("Health")]
    public int playerMaxHealth = 3;
    public int playerCurrentHealth;

    [Header("Shooting")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private Transform bulletOriginUp;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shootTime;
    [SerializeField] private GameObject gunshotFXPrefab;
    [SerializeField] private Transform gunshotFXTransform;
    [SerializeField] private float gunshotDestroyTime = 0.8f;
    [SerializeField] private float bulletDestroyTime = 1f;
    private float shootCooldownTimer;

    [Header("Particles")]
    //[SerializeField] private ParticleSystem dustFx;
   // private float dustFxTimer;

    [Header("Move info")]
    public float moveSpeed;
    public float jumpForce;
    public float doubleJumpForce;

    private float defaultJumpForce;
    private bool canDoubleJump = true;
    private bool canMove;
    //private bool canBeControlled;
    //private bool readyToLand;

    [SerializeField] private float bufferJumpTime;
    private float bufferJumpCounter;

    [SerializeField] private float cayoteJumpTime;
    private float cayoteJumpCounter;
    private bool canHaveCayoteJump;

    //private float defaultGravityScale;
    [Header("Knockback info")]
    [SerializeField] private Vector2 knockbackDirection;
    [SerializeField] private float knockbackTime;
    [SerializeField] private float knockbackProtectionTime;

    private bool isKnocked;
    private bool canBeKnocked = true;

    [Header("Collision info")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private Transform playerSprite;
    private RaycastHit2D isGrounded;

    private bool facingRight = true;
    private int facingDirection = 1;

    [Header("Controlls info")]
    //public VariableJoystick joystick;
    private float movingInput;
    private float vInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = playerSprite.GetComponent<Animator>();

        playerCurrentHealth = playerMaxHealth;
        defaultJumpForce = jumpForce;
    }

    void Update()
    {
        AnimationControllers();

        if (isKnocked)
            return;
        
        InputChecks();
        FlipController();
        CollisionChecks();
        //CheckForEnemy();

        shootCooldownTimer -= Time.deltaTime;
        bufferJumpCounter -= Time.deltaTime;
        cayoteJumpCounter -= Time.deltaTime;

        if (isGrounded)
        {
            if (!canDoubleJump)
            {
                canDoubleJump = true;
            }

            canMove = true;

            if (bufferJumpCounter > 0)
            {
                bufferJumpCounter = -1;
                Jump();
            }

            canHaveCayoteJump = true;

            //if (readyToLand)
            //{
            //    dustFx.Play();
            //    readyToLand = false;
            //}
        }
        else
        {
            //if (!readyToLand)
           //     readyToLand = true;

            if (canHaveCayoteJump)
            {
                canHaveCayoteJump = false;
                cayoteJumpCounter = cayoteJumpTime;
            }
        }
        Move();
    }

    private void AnimationControllers()
    {
        bool isMoving = Mathf.Abs(rb.velocity.x) > 0.1f;

        anim.SetBool("isKnocked", isKnocked);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGrounded", isGrounded);
        //anim.SetBool("canBeControlled", canBeControlled);
        anim.SetFloat("yVelocity", rb.velocity.y);
    }

    private void InputChecks()
    {
        //if (!canBeControlled)
            //return;

        //if (testingOnPc)
        //{
            movingInput = Input.GetAxisRaw("Horizontal");
            vInput = Input.GetAxisRaw("Vertical");
        //}
        //else
       // {
        //    movingInput = joystick.Horizontal;
        //    vInput = joystick.Vertical;
       // }

        if (Input.GetButtonDown("Jump"))
        {
            JumpButton();
        }

        if (Input.GetAxis("Vertical") > 0 && Input.GetButtonDown("Fire1"))
        {
            ShootUp();
        }
        else if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    private void ShootUp()
    {
        if (shootCooldownTimer > 0)
        {
            return;
        }

        Quaternion upRotation = Quaternion.Euler(0, 0, 90);
        GameObject newBullet = Instantiate(bulletPrefab, bulletOriginUp.position, upRotation);
        newBullet.GetComponent<Bullet>().SetupSpeed(0, bulletSpeed);
        Destroy(newBullet, bulletDestroyTime);

        shootCooldownTimer = shootTime;
    }

    private void Shoot()
    {
        if (shootCooldownTimer > 0f)
        {
            return;
        }

        anim.SetTrigger("shoot");

        GameObject newBullet = Instantiate(bulletPrefab, bulletOrigin.transform.position, bulletOrigin.transform.rotation);
        newBullet.GetComponent<Bullet>().SetupSpeed(bulletSpeed * facingDirection, 0);
        Destroy(newBullet, bulletDestroyTime);
        GameObject newGunShotFX = Instantiate(gunshotFXPrefab, gunshotFXTransform.transform.position, gunshotFXTransform.transform.rotation);
        Destroy(newGunShotFX, gunshotDestroyTime);

        shootCooldownTimer = shootTime;
    }

    //public void ReturnControll()
   // {
        //rb.gravityScale = defaultGravityScale;
        //canBeControlled = true;
    //}

    public void JumpButton()
    {
        if (!isGrounded)
        {
            bufferJumpCounter = bufferJumpTime;
        }

        if (isGrounded || cayoteJumpCounter > 0)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            canMove = true;
            canDoubleJump = false;
            jumpForce = doubleJumpForce;
            Jump();
            jumpForce = defaultJumpForce;
        }
    }

    public void Knockback(Transform damageTransform)
    {
        //AudioManager.instance.PlaySFX(9);

        if (!canBeKnocked)
            return;

       // if (GameManager.instance.difficulty > 1)
       //     PlayerManager.instance.OnTakingDamage();

        //PlayerManager.instance.ScreenShake(-facingDirection);
        isKnocked = true;
        canBeKnocked = false;

        PlayerDamage();

        //#region Define horizontal direction for knockback
        int hDirection = 0;
        if (transform.position.x > damageTransform.position.x)
            hDirection = 1;
        else if (transform.position.x < damageTransform.position.x)
            hDirection = -1;
        //#endregion

        rb.velocity = new Vector2(knockbackDirection.x * hDirection, knockbackDirection.y);

        Invoke("CancelKnockback", knockbackTime);
        Invoke("AllowKnockback", knockbackProtectionTime);
    }

    public void PlayerDamage()
    {
        playerCurrentHealth--;

        //if (playerCurrentHealth < 1)
        //{
           // PlayerManager.instance.KillPlayer();
        //}
    }

    private void CancelKnockback()
    {
        isKnocked = false;
    }

    private void AllowKnockback()
    {
        canBeKnocked = true;
    }

    public void NoKnockbackDamage()
    {
        if(!canBeKnocked)
            return;

        isKnocked = true;
        canBeKnocked = false;

        PlayerDamage();

        rb.velocity = Vector2.zero;

        Invoke("CancelKnockback", knockbackTime);
        Invoke("AllowKnockback", knockbackProtectionTime);
    }

    private void Move()
    {
        if (canMove)
            rb.velocity = new Vector2(moveSpeed * movingInput, rb.velocity.y);
    }

    private void Jump()
    {
        //AudioManager.instance.PlaySFX(3);
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);

        //if (isGrounded)
        //    dustFx.Play();
    }

    public void Push(float pushForce)
    {
        rb.velocity = new Vector2(rb.velocity.x, pushForce);
    }

    private void FlipController()
    {
        //dustFxTimer -= Time.deltaTime;
        if (facingRight && rb.velocity.x < -0.1f)
            Flip();
        else if (!facingRight && rb.velocity.x > 0.1f)
            Flip();
    }

    private void Flip()
    {
        //if (dustFxTimer < 0)
       //{
         //   dustFx.Play();
        //    dustFxTimer = .7f;
        //}

        facingDirection = facingDirection * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
        //Gizmos.DrawWireSphere(enemyCheck.position, enemyCheckRadius);
    }
}
