using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class Pawn : NetworkBehaviour
{
    private HUDController hud;
    public bool isEnemy;

    // Character Data
    [Header ("Character Data")]
    [SerializeField] private string name;
    [SerializeField] private Sprite portrait;

    // Movement
    [Header ("Movement")]
    [SerializeField] private float speed = 10.0f;
    private CharacterController controller;
    private Vector3 movement;
    private bool isMove = true;

    // Stats
    [Header ("Stats")]
    [SerializeField] private float maxHP = 100.0f;
    [SerializeField] private float hpRegenRate = 0.0f;
    [SerializeField] private float hpRegenCooldown = 0.0f;

    [Space (10)]
    [SerializeField] private float maxStamina = 100.0f;
    [SerializeField] private float staminaRegenRate = 1.0f;
    [SerializeField] private float staminaRegenCooldown = 2.5f;

    private float hp;
    private float stamina;
    private float hpTimer;
    private float staminaTimer;

    // Combat
    [Header ("Combat")]
    [SerializeField] private GameObject bulletPrefab;
    Bullet bullet;
    private bool isShooting;
    private float fireTimer;
    private System.Random rnd;
    public int firingArcAngle = 30;

    // Abilities
    [Header ("Abilities")]
    [SerializeField] private Ability abilityL;
    [SerializeField] private Ability abilityR;
    private bool isAbilityL;
    private bool isAbilityR;

    // -----
    // SETUP
    // -----

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        // Stats
        hp = maxHP;
        stamina = maxStamina;

        // Combat
        bullet = bulletPrefab.GetComponent<Bullet>();
        rnd = new System.Random();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Movement
        controller = GetComponent<CharacterController>();
    }

    // Assign player's HUD
    public void AssignHUD(HUDController _hud)
    {
        hud = _hud;
    }

    // Update is called once per frame
    void Update()
    {
        // Movement
        if (isMove)
            controller.Move(movement * speed * Time.unscaledDeltaTime);

        // Abilities
        hpTimer += Time.unscaledDeltaTime;
        staminaTimer += Time.unscaledDeltaTime;

        // Recover health and stamina
        if (hp < maxHP && hpTimer > hpRegenCooldown)
            hp += hpRegenRate * Time.unscaledDeltaTime;
        if (stamina < maxStamina && staminaTimer > staminaRegenCooldown) 
            stamina += staminaRegenRate * Time.unscaledDeltaTime;

        // Update HUD
        if (hud)
        {
            hud.UpdateHealth(hp / maxHP);
            hud.UpdateStamina(stamina / maxStamina);
        }

        // Combat
        if (isEnemy)
            fireTimer += Time.deltaTime;
        else
            fireTimer += Time.unscaledDeltaTime;

        if (isShooting)
        {
            if (fireTimer > 1.0f / bullet.GetFireRate())
            {
                fireTimer = 0.0f;
                Shoot(transform.position, transform.rotation);
            }
        }

        // Abilities
        if (isAbilityL)
        {
            ConsumeStamina(abilityL.GetCost() * Time.unscaledDeltaTime);

            if (stamina <= 0.0f)
                abilityL.Deactivate();
        }
        else if (isAbilityR && abilityR != null)
        {
            ConsumeStamina(abilityR.GetCost() * Time.unscaledDeltaTime);

            if (stamina <= 0.0f)
                abilityR.Deactivate();
        }
    }

    // --------
    // MOVEMENT
    // --------

    // Move character in XY plane
    public void Move(Vector2 moveVector)
    {
        movement.x = moveVector.x;
        movement.z = moveVector.y;
    }

    public void EnableMovement(bool move)
    {
        isMove = move;
    }

    // -----
    // STATS
    // -----

    // Add health to player
    public void TakeDamage(float value)
    {
        hp -= value;
        hpTimer = 0.0f;

        if (hp <= 0) Destroy(gameObject);
    }

    // Consume player stamina
    public void ConsumeStamina(float value)
    {
        stamina -= value;
        staminaTimer = 0.0f;
    }

    // ------
    // COMBAT
    // ------

    // Start shooting
    public void StartShooting()
    {
        if (fireTimer > 1.0f / bullet.GetFireRate())
        {
            fireTimer = 0.0f;
            Shoot(transform.position, transform.rotation);
        }

        isShooting = true;
    }

    [Command(requiresAuthority = false)]
    public void Shoot(Vector3 position, Quaternion rotation)
    {
        int randDegree = rnd.Next(firingArcAngle);
        int ranSign = rnd.NextDouble() > 0.5f ? 1 : -1;
        GameObject b;
        if (isEnemy) {
            b = Instantiate(bullet.gameObject, position, rotation *= Quaternion.Euler(0, ranSign * randDegree, 0));
        } else {
            b = Instantiate(bullet.gameObject, position, rotation);
        }
        
        NetworkServer.Spawn(b);
    }

    // Stop shooting
    public void StopShooting()
    {
        isShooting = false;
    }

    // ---------
    // ABILITIES
    // ---------

    // Activate chosen ability
    public void ActivateAbility(int i)
    {
        if (i == 0 && abilityL != null && stamina > abilityL.GetCost())
        {
            abilityL.Activate();

            if (!abilityL.IsOverTime())
                ConsumeStamina(abilityL.GetCost());
            else
                isAbilityL = true;
        }
        else if (i == 1 && abilityR != null && stamina > abilityR.GetCost())
        {
            abilityR.Activate();

            if (!abilityR.IsOverTime())
                ConsumeStamina(abilityR.GetCost());
            else
                isAbilityR = true;
        }
    }

    // Deactivate chosen ability
    public void DeactivateAbility(int i)
    {
        if (i == 0 && isAbilityL)
        {
            isAbilityL = false;
            abilityL.Deactivate();
        }
        else if (i == 1 && isAbilityR)
        {
            isAbilityR = false;
            abilityR.Deactivate();
        }
    }
}
