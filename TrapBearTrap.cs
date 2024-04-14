using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBearTrap : MonoBehaviour
{
    private Animator anim;
    private bool trapFinished = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null && !trapFinished)
        {
            anim.SetTrigger("trap");
            anim.SetBool("trapFinished", true);
            Player player = collision.GetComponent<Player>();

            player.NoKnockbackDamage();
            trapFinished = true;
        }
    }
}
