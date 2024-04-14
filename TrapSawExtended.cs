using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapSawExtended : DamageController
{
    private Animator anim;

    [SerializeField] private Transform[] movePoint;
    [SerializeField] private float speed;

    private int movePointIndex;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("isWorking", true);
        transform.position = movePoint[0].position;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint[movePointIndex].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, movePoint[movePointIndex].position) < 0.15f)
        {
            movePointIndex++;


            if (movePointIndex >= movePoint.Length)
            {
                movePointIndex = 0;
            }
        }
    }
}
