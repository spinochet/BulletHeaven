using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Mirror;

public class Pawn : NetworkBehaviour
{
    private HUDController hud;
    public PawnController pawnController;
    public GameObject partner;

    // Character Data
    [Header ("Character Data")]
    [SerializeField] private string name;
    [SerializeField] private Sprite portrait;
    [SerializeField] private GameObject model;
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

    Bullet bullet;
    private bool isShooting;
    private float fireTimer;

    // Abilities
    [Header ("Abilities")]
    [SerializeField] private Ability abilityL;
    [SerializeField] private Ability abilityR;
    private bool isAbilityL;
    private bool isAbilityR;

    // Awake is called when the script instance is being loaded.
    void Awake()
    {
        // Movement 
        controller = GetComponent<CharacterController>();
        
        // Stats
        hp = maxHP;
        stamina = maxStamina;

        // Combat
        bullet = bulletPrefab.GetComponent<Bullet>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Update move
        if (!model.activeSelf)
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

        // Update HUD
        if (hud && model.activeSelf)
        {
            hud.UpdateHealth(hp / maxHP);
            hud.UpdateStamina(stamina / maxStamina);
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

    // -----
    // SETUP
    // -----

    // Assign player's HUD
    public void AssignHUD(HUDController _hud)
    {
        hud = _hud;
        hud.UpdatePortrait(portrait.texture);
    }

    // Assign partner
    public void AssignPartner(NetworkIdentity _partner)
    {
        partner = _partner.gameObject;
        Debug.Log(partner.gameObject.name);
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
    }

    // -----
    // STATS
    // -----

    // Add health to player
    public void TakeDamage(float value)
    {
        if (model.activeSelf)
        {
            hp -= value;
            hpTimer = 0.0f;

            if (hp <= 0)
            {
                SoundManager.Instance.Play(name + " Death");
                if (pawnController)
                    pawnController.Destroy(this);
                else
                    Destroy(gameObject);
            }
            else
            {
                SoundManager.Instance.Play(name + " Hurt");
            }
        }
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

    // Ask server to spawn bullet prefab
    [Command(requiresAuthority = false)]
    public void Shoot(Quaternion aim)
    {
        GameObject b = Instantiate(bullet.gameObject, transform.position, aim);
        NetworkServer.Spawn(b);
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
