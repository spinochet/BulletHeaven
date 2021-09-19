using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // Class to store character data
    [System.Serializable]
    public class CharacterPreset
    {
        [Header ("Model")]
        public GameObject model;
        public string name;

        [Header ("Movement")]
        public float speed = 10.0f;

        [Header ("Stats")]
        public float maxHP = 100.0f;
        public float hpRegenRate = 0.0f;
        public float hpRegenCooldown = 0.0f;
        [Space (10)]
        public float maxStamina = 100.0f;
        public float staminaRegenRate = 5.0f;
        public float staminaRegenCooldown = 0.5f;

        [Header ("Combat")]
        public GameObject bulletPrefab;
        public GameObject burstPrefab;

        [Header ("Abilities")]
        public Ability abilityL;
        public Ability abilityR;
    }

    // Controller scripts for player mechanics
    private MovementController movementController;
    private StatsController statsController;
    private BulletController bulletController;
    private AbilityController abilityController;

    // Character data
    [SerializeField] private CharacterPreset presets;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // ------------
    // PLAYER SETUP
    // ------------
    
    // Initialize player for character selection
    public void Init()
    {
        // Set up player movement
        movementController = gameObject.AddComponent(typeof(MovementController)) as MovementController;
        movementController.Setup(10.0f);
    }

    // Initialize player for gameplay
    public void Init(CharacterPreset _presets)
    {
        presets = _presets;

        // Set up player movement
        movementController = gameObject.AddComponent(typeof(MovementController)) as MovementController;
        movementController.Setup(presets.speed);

        // Set up player stats
        statsController = gameObject.AddComponent(typeof(StatsController)) as StatsController;
        statsController.Setup(presets.maxHP, presets.maxStamina, presets.hpRegenRate, presets.staminaRegenRate, presets.hpRegenCooldown, presets.staminaRegenCooldown);

        // Set up player bullets
        bulletController = gameObject.AddComponent(typeof(BulletController)) as BulletController;
        bulletController.Setup(presets.bulletPrefab, presets.burstPrefab);

        // Set up player abilities
        abilityController = gameObject.AddComponent(typeof(AbilityController)) as AbilityController;
        abilityController.Setup(statsController, presets.abilityL, presets.abilityR);
    }

    // Assing hud
    public void AssignHUD(HUDController _hud)
    {
        statsController.AssignHUD(_hud);
    }

    // ---------------
    // EVENT CALLBACKS
    // ---------------

    // Move action callback
    void OnMove(InputValue input)
    {
        movementController.Move(input.Get<Vector2>());
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
