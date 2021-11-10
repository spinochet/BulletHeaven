using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoombaController : EnemyController
{
    private bool inBounds = false;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        controller.Move(transform.forward * speed * Time.deltaTime);
        SoundManager.Instance.Play("Roomba");


        if (CheckBorders() && inBounds)
            transform.rotation = AimAtPlayer();

        inBounds = CheckBounds();

        fireTimer += Time.unscaledDeltaTime;
        if (fireTimer > 1.0f / pawn.FireRate)
        {
            fireTimer = 0.0f;
            pawn.Shoot(transform.rotation);
        }
    }

    bool CheckBounds()
    {
        Vector3 position = Camera.main.WorldToScreenPoint(transform.position);
        bool up = position.y <= Camera.main.pixelHeight;
        bool down = position.y >= 0.0f;
        bool right = position.x <= Camera.main.pixelWidth;
        bool left = position.x >= 0.0f;

        return up && down && right && left;
    }

    bool CheckBorders()
    {
        Vector3 position = Camera.main.WorldToScreenPoint(transform.position);
        bool up = position.y >= Camera.main.pixelHeight;
        bool down = position.y <= 0.0f;
        bool right = position.x >= Camera.main.pixelWidth;
        bool left = position.x <= 0.0f;

        return up || down || right || left;
    }
}
