using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpitter : Enemy
{
    [Header("Spitter specific")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletOrigin;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shootDelay = 0.1f;


    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        CollisionChecks();
        idleTimeCounter -= Time.deltaTime;

        if (!playerDetected)
            return;

        if (idleTimeCounter < 0 && playerDetected)
        {
            idleTimeCounter = idleTime;
            anim.SetTrigger("attack");
            Invoke("AttackEvent", shootDelay);
        }
    }

    private void AttackEvent()
    {
        GameObject newBullet = Instantiate(bulletPrefab, bulletOrigin.transform.position, bulletOrigin.transform.rotation);
        newBullet.GetComponent<Bullet>().SetupSpeed(bulletSpeed * facingDirection, 0);
        Destroy(newBullet, 3f);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
    }
}
