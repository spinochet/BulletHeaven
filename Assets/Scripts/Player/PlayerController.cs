using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private MovementController movementController;
    [Header ("Movement")]
    [SerializeField] private float speed = 20.0f;

    private StatsController statsController;
    [Header ("Stats")]
    [SerializeField] private float maxHP = 100.0f;
    [SerializeField] private float maxStamina = 100.0f;
    [SerializeField] private float hpRegenRate = 0.0f;
    [SerializeField] private float staminaRegenRate = 5.0f;

    private BulletController bulletController;
    [Header ("Combat")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private GameObject burstPrefab;

    public AbilityController abilityController;
    [Header ("Abilities")]
    [SerializeField] private Ability abilityL;
    [SerializeField] private Ability abilityR;

    // Start is called before the first frame update
    void Start()
    {
        // Set up player movement
        movementController = gameObject.AddComponent(typeof(MovementController)) as MovementController;
        movementController.Setup(speed);

        // Set up player stats
        statsController = gameObject.AddComponent(typeof(StatsController)) as StatsController;
        statsController.Setup(maxHP, maxStamina, hpRegenRate, staminaRegenRate);

        // Set up player bullets
        bulletController = gameObject.AddComponent(typeof(BulletController)) as BulletController;
        bulletController.Setup(bulletPrefab, burstPrefab);

        // Set up player abilities
        abilityController = gameObject.AddComponent(typeof(AbilityController)) as AbilityController;
        abilityController.Setup(statsController, abilityL, abilityR);
    }

    // Move action callback
    void OnMove(InputValue input)
    {
        movementController.Move((Vector3) input.Get<Vector2>());
    }

    // Shoot action callback
    void OnShoot(InputValue input)
    {
        bool isShooting = input.Get<float>() > 0.0f ? true : false;

        if (isShooting)
            bulletController.StartShooting();
        else
            bulletController.StopShooting();
    }

    // AbilityL action callback function
    void OnAbilityL(InputValue input)
    {
        if (input.Get<float>() > 0.0f)
            abilityController.Activate(0);
        else
            abilityController.Deactivate(0);
    }

    // AbilityR action callback function
    void OnAbilityR(InputValue input)
    {
        if (input.Get<float>() > 0.0f)
            abilityController.Activate(1);
        else
            abilityController.Deactivate(1);
    }
}
