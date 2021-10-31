using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaController : EnemyController
{
    private Vector3 movement = Vector3.zero;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        movement = transform.forward;
    }

    // Update is called once per frame
    void Update()
    {
        controller.Move(movement * speed * Time.deltaTime);

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
        movement = Vector3.Reflect(movement, hit.normal);
        transform.rotation = Quaternion.LookRotation(movement);
    }
}
