using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : PawnController
{
    // TEMP BEHAVIORS
    protected CharacterController controller;
    [SerializeField] protected float speed = 2.5f;
    [SerializeField] protected float contactDamage = 20.0f;
    [SerializeField] protected float contactKnockback = 20.0f;
    [SerializeField] protected float contactCooldown = 5.0f;
    protected float contactTimer;

    protected float fireTimer;
    protected float cooldownTimer;
    public float shootFrequency = 0.3f;

    protected float delay = 100.0f;
    protected float delayTimer = 0.0f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        contactTimer = contactCooldown;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!pawn)
            pawn = GetComponent<Pawn>();
    }

    // Return position of nearest player
    public Vector3 GetNearestPlayer()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Character");

        Vector3 target = Vector3.zero;
        float closest = 1000000;

        foreach (GameObject player in players)
        {
            Vector3 dir = player.transform.position - transform.position;

            if (dir.magnitude < closest)
            {
                closest = dir.magnitude;
                target = player.transform.position;
            }
        }

        return target;
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
    }

    // Destroy object
    public override void Destroy(Pawn _pawn)
    {
        Destroy(gameObject);
    }

    // OnControllerColliderHit is called when the controller hits a collider while performing a Move.
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (contactTimer >= contactCooldown)
            if (hit.gameObject.tag == "Character")
            {
                contactTimer = 0.0f;
                hit.transform.GetComponent<Pawn>().TakeDamage(contactDamage);
                // Player knockback goes here
            }
    }
}
