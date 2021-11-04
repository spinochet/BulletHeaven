using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : EnemyController
{
    [SerializeField] private int bullets = 5;
    [SerializeField] private float fireCooldown = 1.0f;

    private int bulletsFired = 0;
    private bool isFiring = true;

    // Update is called once per frame
    void Update()
    {
        // if (LevelManager.Instance.IsScrolling) {
            controller.enabled = true;
            controller.Move(Vector3.forward * -speed * Time.deltaTime);
        // }
            

        // Combat
        delayTimer += Time.deltaTime;
        if (delayTimer >= delay)
        {
            fireTimer += Time.deltaTime;

            if (isFiring && fireTimer > 1.0f / pawn.FireRate && bulletsFired < bullets)
            {
                fireTimer = 0.0f;
                ++bulletsFired;
                if (pawn.Model.GetComponent<Animator>()) {
                    pawn.Model.GetComponent<Animator>().Play("Attack");
                }
                pawn.Shoot(AimAtPlayer());
            }
            else if (bulletsFired >= bullets)
            {
                cooldownTimer += Time.deltaTime;

                if (cooldownTimer >= fireCooldown)
                {
                    cooldownTimer = 0.0f;
                    bulletsFired = 0;
                }
            }
        }
    }
}
