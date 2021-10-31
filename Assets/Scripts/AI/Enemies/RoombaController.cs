using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaController : EnemyController
{
    // private Vector3 movement = Vector3.zero;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        controller.Move(transform.forward * speed * Time.deltaTime);

        fireTimer += Time.unscaledDeltaTime;
        if (fireTimer > 1.0f / pawn.FireRate)
        {
            fireTimer = 0.0f;
            pawn.Shoot(transform.rotation);
        }
    }

    // OnControllerColliderHit is called when the controller hits a collider while performing a Move.
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        transform.rotation = AimAtPlayer();
    }
}
