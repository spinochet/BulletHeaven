using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionController : PawnController
{
    // TEMP BEHAVIOR
    float timer;
    int score;
    Vector2 movement;

    // Update is called once per frame
    void Update()
    {
        if (Time.timeScale > 0.0f)
        {
            timer += Time.unscaledDeltaTime;

            if (timer > 0.5f)
            {
                timer = 0.0f;

                // Random movement
                float dir = Random.Range(0.0f, 1.0f);
                if (dir < 0.25f)
                    movement = Vector2.left;
                else if (dir < 0.5f)
                    movement = Vector2.right;
                else
                    movement = Vector2.zero;

                // Random shooting
                float shoot = Random.Range(0.0f, 1.0f);
                if (shoot < 0.6f)
                    pawn.StartShooting();
                else
                    pawn.StopShooting();
            }

            // pawn.Move(movement);
        }
        else
        {
            pawn.Move(Vector3.zero);
            pawn.StopShooting();
        }
    }

    // public void AddPoints()
    // {
    //     score += 100;
    //     hudController.UpdateScore(score);
    // }
}
