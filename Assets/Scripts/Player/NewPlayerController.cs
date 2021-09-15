using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NewPlayerController : MonoBehaviour
{
    [Header ("Movement")]
    [SerializeField] private float speed = 20.0f;

    [Header ("Combat")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 30.0f;
    [SerializeField] private GameObject burstPrefab;
    private GameObject burstBullet;
    private bool isShooting;
    private float shootTimer;
    private bool isBurstCharging;

    [Header ("Abilities")]
    [SerializeField] private Ability abilityL;
    private bool isAbilityL;
    [SerializeField] private Ability abilityR;
    private bool isAbilityR;

    [Header ("Stats")]
    [SerializeField] private float maxHP = 100.0f;
    [SerializeField] private float maxStamina = 100.0f;
    [SerializeField] private float staminaRecoveryRate = 5.0f;
    private float hp;
    private float stamina;
    private Slider hpBar;
    private Slider staminaBar;

    private CharacterController controller;
    private Vector3 movement;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        abilityR.SetOwner(gameObject.name);

        hp = maxHP;
        stamina = maxStamina;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        controller.Move((Vector3) movement * speed * Time.unscaledDeltaTime);

        // Shooting
        if (isShooting)
        {
            shootTimer += Time.unscaledDeltaTime;

            if (shootTimer > 1.0f / fireRate)
            {
                shootTimer = 0.0f;
                Instantiate(bulletPrefab, transform.position, transform.rotation);
            }
        }

        // Burst attack
        if (isBurstCharging)
        {
            burstBullet.transform.position = transform.position + (transform.forward * 5.0f);
        }

        // Abilities
        if (isAbilityL)
        {
            stamina -= abilityL.GetCost() * Time.unscaledDeltaTime;
            staminaBar.value = stamina / maxStamina;

            if (stamina <= 0.0f)
                abilityL.Deactivate();
        }

        if (isAbilityR)
        {
            stamina -= abilityR.GetCost() * Time.unscaledDeltaTime;
            staminaBar.value = stamina / maxStamina;

            if (stamina <= 0.0f)
                abilityR.Deactivate();
        }

        // Stats
        if (!isAbilityL && !isAbilityR && stamina < maxStamina)
        {
            stamina += staminaRecoveryRate * Time.unscaledDeltaTime;
            staminaBar.value = stamina / maxStamina;
        }
    }

    public void SetUI(Slider health, Slider stamina)
    {
        hpBar = health;
        staminaBar = stamina;
    }

    // Move action callback function
    void OnMove(InputValue input)
    {
        Vector2 inputVec = input.Get<Vector2>();
        movement.x = inputVec.x;
        movement.y = 0.0f;
        movement.z = inputVec.y;
    }

    // Shoot action callback
    void OnShoot(InputValue input)
    {
        isShooting = input.Get<float>() > 0.0f ? true : false;
    }

    // ShootTap action callback function
    void OnShootTap()
    {
    }

    // ShootHold action callback function
    void OnShootHold()
    {
        if (isShooting)
            shootTimer = 1.0f / fireRate;
    }

    // Burst action callback
    void OnBurst(InputValue input)
    {
        bool f = input.Get<float>() > 0.0f ? true : false;

        if (f)
        {
            burstBullet = Instantiate(burstPrefab, transform.position, transform.rotation);
            isBurstCharging = true;
        }
        else
        {
            burstBullet.GetComponent<BurstBullet>().Shoot();
            isBurstCharging = false;
        }
    }

    // AbilityL action callback function
    void OnAbilityL(InputValue input)
    {
        isAbilityL = input.Get<float>() > 0.0f ? true : false;

        if (isAbilityL && stamina > abilityL.GetCost())
        {
            isAbilityL = abilityL.Activate();
        }
        else
        {
            isAbilityL = false;
            abilityL.Deactivate();
        }
    }

    // AbilityR action callback function
    void OnAbilityR(InputValue input)
    {
        isAbilityR = input.Get<float>() > 0.0f ? true : false;

        if (isAbilityR && stamina > abilityR.GetCost())
        {
            isAbilityR = abilityR.Activate();

            if (!isAbilityR)
            {
                stamina -= abilityR.GetCost();
                staminaBar.value = stamina / maxStamina;
            }
        }
        else
        {
            isAbilityR = false;
            abilityR.Deactivate();
        }
    }

    public void TakeDamage(float damage)
    {
        hp -= damage;
        if (hp < 0.0f) hp = 0.0f;
        
        hpBar.value = hp / maxHP;
    }
}
