using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherController : EnemyController
{
    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance.IsScrolling)
            controller.Move(Vector3.forward * -speed * Time.deltaTime);

        fireTimer += Time.unscaledDeltaTime;
        if (fireTimer > 1.0f / pawn.FireRate)
        {
            fireTimer = 0.0f;
            pawn.Shoot(AimAtPlayer());
        }
    }
}
