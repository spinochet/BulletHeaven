using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PawnController
{
    // TEMP BEHAVIORS
    private CharacterController controller;
    private float speed = 2.5f;


    private float fireTimer;
    public float shootFrequency = 0.3f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!pawn)
            pawn = GetComponent<Pawn>();
    }

    // Update is called once per frame
    void Update()
    {
        if (LevelManager.Instance.IsScrolling)
            controller.Move(Vector3.forward * -speed * Time.deltaTime);

        fireTimer += Time.unscaledDeltaTime;
        if (fireTimer > 1.0f / pawn.FireRate)
        {
            fireTimer = 0.0f;
            GameObject[] players = GameObject.FindGameObjectsWithTag("Character");
            Vector3 dir = players[0].transform.position - transform.position;
            pawn.Shoot(Quaternion.LookRotation(dir));
        }
    }
}
