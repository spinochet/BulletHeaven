using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public PlayerManager manager;

    // Controller scripts for player mechanics
    private MovementController movementController;
    private StatsController statsController;
    private BulletController bulletController;
    private AbilityController abilityController;
    private HUDController hudController;

    // Character and player data
    private Color playerColor;
    private PlayerData presets;

    // Load character presets
    public void LoadCharacter(PlayerData _presets)
    {
        presets = _presets;
    }

    // Spawn players in level
    public void SpawnPlayer(Vector3 spawnPoint)
    {
        transform.position = spawnPoint;

        // Set up player movement
        if (movementController) Destroy(movementController);
        movementController = gameObject.AddComponent(typeof(MovementController)) as MovementController;
        movementController.Setup(presets.speed, presets.dashDist);

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

    // ---------------
    // EVENT CALLBACKS
    // ---------------

    // Move action callback
    void OnMove(InputValue input)
    {
        if (movementController)
            movementController.Move(input.Get<Vector2>());
    }

    public void EnableMovement()
    {
        if (movementController)
            movementController.enabled = true;
    }

    public void DisableMovement()
    {
        if (movementController)
            movementController.enabled = false;
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

    // Dash action callback function
    void OnDash()
    {
        if (movementController)
        {
            movementController.Dash();
        }
    }

    // Pause action callback function
    void OnPause()
    {
        hudController.TogglePause();
    }
}
