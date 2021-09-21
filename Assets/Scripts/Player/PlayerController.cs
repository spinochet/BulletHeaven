using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Class to store character data
    [System.Serializable]
    public class CharacterPreset
    {
        [Header ("Model")]
        public GameObject model;
        public RawImage portrait;
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

    // Player manager assigned to character
    private PlayerManager manager;

    // Controller scripts for player mechanics
    private MovementController movementController;
    private StatsController statsController;
    private BulletController bulletController;
    private AbilityController abilityController;

    // Character select scripts
    private PlayerCharacterSelect characterSelect;
    private bool isSelected;

    // Character data
    [SerializeField] private CharacterPreset presets;

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // ------------
    // PLAYER SETUP
    // ------------

    // Assign manager
    public void AssignManager(PlayerManager _manager)
    {
        manager = _manager;
    }
    
    // Initialize player for character selection
    public void Init(CharacterPreset _presets, PlayerCharacterSelect _characterSelect)
    {
        // Store variables
        presets = _presets;
        characterSelect = _characterSelect;

        // Set up player movement
        movementController = gameObject.AddComponent(typeof(MovementController)) as MovementController;
        movementController.Setup(10.0f);
    }

    // Initialize player for gameplay from character select screen
    public void Init()
    {
        // Set up player movement
        if (movementController) Destroy(movementController);
        movementController = gameObject.AddComponent(typeof(MovementController)) as MovementController;
        movementController.Setup(presets.speed);

        // Set up player stats
        if (statsController) Destroy(statsController);
        statsController = gameObject.AddComponent(typeof(StatsController)) as StatsController;
        statsController.Setup(presets.maxHP, presets.maxStamina, presets.hpRegenRate, presets.staminaRegenRate, presets.hpRegenCooldown, presets.staminaRegenCooldown);

        // Set up player bullets
        if (bulletController) Destroy(bulletController);
        bulletController = gameObject.AddComponent(typeof(BulletController)) as BulletController;
        bulletController.Setup(presets.bulletPrefab, presets.burstPrefab);

        // Set up player abilities
        if (abilityController) Destroy(abilityController);
        abilityController = gameObject.AddComponent(typeof(AbilityController)) as AbilityController;
        abilityController.Setup(statsController, presets.abilityL, presets.abilityR);

        // Spawn model
        GameObject model = Instantiate(presets.model, transform);
        model.transform.localScale= new Vector3(0.2f, 0.2f, 0.2f);

        // Switch input map
        PlayerInput input = GetComponent<PlayerInput>();
        if (input) input.SwitchCurrentActionMap("Gameplay");
    }

    // Assing hud
    public void AssignHUD(HUDController _hud)
    {
        statsController.AssignHUD(_hud);
        _hud.UpdatePortrait(presets.portrait.texture);
    }

    // ---------------
    // EVENT CALLBACKS
    // ---------------

    // Move action callback
    void OnMove(InputValue input)
    {
        if (movementController)
            movementController.Move(input.Get<Vector2>());
    }

    // ---------
    // UI EVENTS
    // ---------

    // Select action callback
    void OnSelect()
    {
        if (characterSelect)
        {
            bool lockMovement = characterSelect.SetReady(true);
            movementController.enabled = !lockMovement;

            isSelected = lockMovement;
            if (isSelected) presets = characterSelect.GetPreset();

            manager.IsReady();
        }
    }

    // Cancel action callback
    void OnCancel()
    {
        if (characterSelect)
        {
            bool lockMovement = characterSelect.SetReady(false);
            movementController.enabled = !lockMovement;
            isSelected = lockMovement;

            manager.IsReady();
        }
    }

    // Submit action callback
    void OnSubmit()
    {
        manager.LoadLevel("CharacterController");
    }

    public bool IsReady()
    {
        return isSelected;
    }

    // -------------
    // COMBAT EVENTS
    // -------------

    // Shoot action callback
    void OnShoot(InputValue input)
    {
        bool isShooting = input.Get<float>() > 0.0f ? true : false;

        if (bulletController)
        {
            if (isShooting) bulletController.StartShooting();
            else bulletController.StopShooting();
        }
    }

    // AbilityL action callback function
    void OnAbilityL(InputValue input)
    {
        if (abilityController)
        {
            if (input.Get<float>() > 0.0f) abilityController.Activate(0);
            else abilityController.Deactivate(0);
        }
    }

    // AbilityR action callback function
    void OnAbilityR(InputValue input)
    {
        if (abilityController)
        {
            if (input.Get<float>() > 0.0f) abilityController.Activate(1);
            else abilityController.Deactivate(1);
        }
    }
}
