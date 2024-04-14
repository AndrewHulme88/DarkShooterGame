using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSpikes : DamageController
{
    [SerializeField] private float animationDelay = 3f;
    private Animator anim;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        InvokeRepeating("PlayAnimation", animationDelay, animationDelay);
    }

    private void PlayAnimation()
    {
        anim.SetTrigger("animate");
    }
}
