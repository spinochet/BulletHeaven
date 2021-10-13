using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PawnController
{
    // TEMP BEHAVIORS
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        if (!pawn)
            pawn = GetComponent<Pawn>();
    }

    // Update is called once per frame
    void Update()
    {
        pawn.Move(Vector2.down * Time.timeScale);

        timer += Time.deltaTime;
        if (timer > 0.5f)
        {
            timer = 0.0f;
            float shoot = Random.Range(0.0f, 1.0f);
            if (shoot < 0.4f)
                pawn.StartShooting();
            else
                pawn.StopShooting();
        }

        if (Time.deltaTime == 0.0f)
        {
            pawn.StopShooting();
        }
    }
}
