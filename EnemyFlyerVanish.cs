using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFlyerVanish : Enemy
{
    [Header("FlyerVanish specific")]
    [SerializeField] private float activeTime;
    private float activeTimeCounter = 4;
    [SerializeField] private float appearDelay = 1f; 
    [SerializeField] private SpriteRenderer sr;

    [SerializeField] private float[] xOffset;
    protected override void Start()
    {
        base.Start();
        aggressive = true;
        invincible = true;


    }

    void Update()
    {

        if (player == null)
        {
            anim.SetTrigger("disappear");
            Disappear();
            return;
        }

        activeTimeCounter -= Time.deltaTime;
        idleTimeCounter -= Time.deltaTime;

        if (activeTimeCounter > 0)
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);

        if (activeTimeCounter < 0 && idleTimeCounter < 0 && aggressive)
        {
            anim.SetTrigger("disappear");
            Invoke("Disappear", appearDelay);
            aggressive = false;
            idleTimeCounter = idleTime;
        }

        if (activeTimeCounter < 0 && idleTimeCounter < 0 && !aggressive)
        {
            ChoosePosition();
            anim.SetTrigger("appear");
            Invoke("Appear", appearDelay);
            aggressive = true;
            activeTimeCounter = activeTime;
        }

        FlipController();
    }

    private void FlipController()
    {
        if (player == null)
            return;

        if (facingDirection == -1 && transform.position.x < player.transform.position.x)
            Flip();
        else if (facingDirection == 1 && transform.position.x > player.transform.position.x)
            Flip();
    }

    private void ChoosePosition()
    {
        float _xOffset = xOffset[Random.Range(0, xOffset.Length)];
        float _yOffset = Random.Range(-10, 10);
        transform.position = new Vector2(player.transform.position.x + _xOffset, player.transform.position.y + _yOffset);
    }

    public void Disappear() => sr.enabled = false;
    public void Appear() => sr.enabled = true;



    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (aggressive)
            base.OnTriggerEnter2D(collision);
    }
}
