using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float pushForce = 20;
    [SerializeField] private float resetDelay = 1f;
    [SerializeField] private bool canBeUsed = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<Player>() != null && canBeUsed)
        {
            canBeUsed = false;
            GetComponent<Animator>().SetTrigger("activate");
            collision.GetComponent<Player>().Push(pushForce);
            Invoke("CanUseAgain", resetDelay);
        }

    }

    private void CanUseAgain() => canBeUsed = true;
}
