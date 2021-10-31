using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PawnController
{
    // TEMP BEHAVIORS
    protected CharacterController controller;
    [SerializeField] protected float speed = 2.5f;

    protected float fireTimer;
    protected float cooldownTimer;
    public float shootFrequency = 0.3f;

    protected float delay = 100.0f;
    protected float delayTimer = 0.0f;

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

    // Return rotation to aim at player
    public Quaternion AimAtPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Character");

        Vector3 dir = Vector3.zero;
        float closest = 1000000;

        foreach (GameObject player in players)
        {
            Vector3 temp_dir = player.transform.position - transform.position;

            if (temp_dir.magnitude < closest)
            {
                dir = temp_dir;
                closest = dir.magnitude;
            }
        }

        return Quaternion.LookRotation(dir);
    }

    // Set delay so every enemy is slightly off
    public void SetDelay(float _delay)
    {
        delay = _delay;
        delayTimer = 0.0f;

        Debug.Log(delay);
    }

    // Destroy object
    public override void Destroy(Pawn _pawn)
    {
        Destroy(gameObject);
    }
}
