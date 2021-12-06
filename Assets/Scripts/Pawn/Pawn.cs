using System.Collections;
using System.Collections.Generic;
using UnityEngine; 

public class Pawn : MonoBehaviour
{
    private HUDController hud;
    public PawnController pawnController;
    public bool IsPossesed { get { return pawnController != null; } }
    public bool IsPlayerPawn { get { return pawnController is PlayerController; } }
    public GameObject partner;

    // Character Data
    [Header ("Character Data")]
    [SerializeField] private string name;
    [SerializeField] private Sprite portrait;
    [SerializeField] private GameObject model;
    private int currentLevel = 0;

    public GameObject Model { get { return model; } }
    public Sprite Portrait { get { return portrait; } }

    // Movement
    [Header ("Movement")]
    [SerializeField] private float speed = 10.0f;
    private CharacterController controller;

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
    [SerializeField] private float fireRate;
    private Vector3 aim;

    public float FireRate { get { return fireRate; } }

    public Bullet bullet;
    public Bullet.BulletLevel powerUp;
    private bool isShooting;
    private float fireTimer;

    // Abilities
    [Header ("Abilities")]
    [SerializeField] private Ability abilityL;
    [SerializeField] private Ability abilityR;
    public bool isAbilityL;
    public bool isAbilityR;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        // Movement 
        controller = GetComponent<CharacterController>();
        
        // Stats
        hp = maxHP;
        stamina = maxStamina;

        // Combat
        if (bulletPrefab)
            bullet = bulletPrefab.GetComponent<Bullet>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 adjustedPosition = transform.position;
        adjustedPosition.y = 0.0f;
        transform.position = adjustedPosition;

        // Update move
        if (!model.activeSelf && partner)
        {
            transform.position = partner.transform.position;
        }

        // Abilities
        hpTimer += Time.unscaledDeltaTime;
        staminaTimer += Time.unscaledDeltaTime;

        // Recover health and stamina
        if (hp < maxHP && hpTimer > hpRegenCooldown)
            hp += hpRegenRate * Time.unscaledDeltaTime;
        if (stamina < maxStamina && staminaTimer > staminaRegenCooldown) 
            stamina += staminaRegenRate * Time.unscaledDeltaTime;

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

    // -----
    // SETUP
    // -----

    // Assign partner
    public void AssignPartner(Pawn _partner)
    {
        if (_partner != null)
            partner = _partner.gameObject;
        else
            partner = null;
    }

    // Set visibility of model
    public void SetVisibility(bool visible)
    {
        model.SetActive(visible);
    }

    // --------
    // MOVEMENT
    // --------

    // Move character in XY plane
    public void Move(Vector3 moveVector)
    {
        controller.Move(moveVector * speed * Time.unscaledDeltaTime);

        Vector3 adjustedPosition = transform.position;
        adjustedPosition.y = 0.0f;
        transform.position = adjustedPosition;
    }

    // Pause pawn's animation
    public void ToggleAnimation(bool playing)
    {
        if (playing)
            model.GetComponent<Animator>().speed = 1.0f;
        else
            model.GetComponent<Animator>().speed = 0.0f;
    }

    // -----
    // STATS
    // -----

    // Get current hp as a percentage
    public float GetHP()
    {
        return hp / maxHP;
    }

    // Reduce health to pawn
    public float TakeDamage(float value)
    {
        if (model.activeSelf)
        {
            hp -= value;
            hpTimer = 0.0f;

            if (hp <= 0)
            {
                SoundManager.Instance.Play(name + " Death");
                if (pawnController) {
                    pawnController.Destroy(this);
                } else {
                    Destroy(gameObject);
                }
                    
            }
            else
            {
                SoundManager.Instance.Play(name + " Hurt");
            }
        }

        return hp;
    }

    // Restore health to pawn
    public float RestoreHealth(float value)
    {
        if (model.activeSelf)
        {
            hp += value;
            if (hp >= maxHP)
                hp = maxHP;
        }

        return hp;
    }

    // Get current hp as a percentage
    public float GetStamina()
    {
        return stamina / maxStamina;
    }

    // Consume player stamina
    public void ConsumeStamina(float value)
    {
        stamina -= value;
        staminaTimer = 0.0f;

        if (stamina > maxStamina)
            stamina = maxStamina;
        if (stamina < 0.0f)
            stamina = 0.0f;
    }

    // ------
    // COMBAT
    // ------

    // Ask server to spawn bullet prefab
    public void Shoot(Quaternion aim)
    {
        int numBullets = bullet.GetNumBullets(currentLevel) + powerUp.numBullets;
        float bulletSpacing = bullet.GetBulletSpacing(currentLevel) + powerUp.bulletSpacing;
        float bulletAngle = bullet.GetBulletAngle(currentLevel) + powerUp.angle;

        float spaceFactor = (numBullets - 1.0f) / 2.0f;
        Vector3 startSpawn = transform.position + (Vector3.left * bulletSpacing * spaceFactor);
        float startAngle = 0;
        float angleStep = 0;

        if (numBullets > 1)
        {
            startAngle = -bulletAngle;
            angleStep = (bulletAngle * 2) / (numBullets - 1);
        }

        for (int i = 0; i < numBullets; ++i)
        {
            GameObject b = Instantiate(bullet.gameObject, startSpawn + (Vector3.right * bulletSpacing * i), aim * Quaternion.Euler(0.0f, startAngle + (angleStep * i), 0.0f));
            if (pawnController is PlayerController)
                b.GetComponent<Bullet>().SetOwner((PlayerController) pawnController, powerUp, currentLevel);
        }
    }

    // Ask server to spawn bomb prefab
    public void Bomb(Vector3 target)
    {
        GameObject b = Instantiate(bullet.gameObject, target, Quaternion.identity);
    }

    // Power up bullets
    public void PowerUp(Bullet.BulletLevel level, float duration)
    {
        powerUp = level;
    }

    // ---------
    // ABILITIES
    // ---------

    // Activate chosen ability
    public void ActivateAbility(int i)
    {
        if (i == 0 && abilityL != null && stamina > abilityL.GetCost())
        {
            abilityL.Activate(currentLevel);

            if (!abilityL.IsOverTime())
                ConsumeStamina(abilityL.GetCost());
            else
                isAbilityL = true;
        }
        else if (i == 1 && abilityR != null && stamina > abilityR.GetCost())
        {
            abilityR.Activate(currentLevel);

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
