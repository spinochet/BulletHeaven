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
    private int score;

    // Spawn players in level
    public void AssignPawn(GameObject pawn, HUDController hud = null)
    {
        // Set up player HUD
        if (hud != null)
            hudController = hud;
        if (hudController != null)
            hudController.UpdatePortrait(pawn.GetComponent<CharacterData>().portrait.texture);

        // Set up player movement
        movementController = pawn.GetComponent<MovementController>();

        // Set up player stats
        statsController = pawn.GetComponent<StatsController>();
        statsController.AssignHUD(hudController);

        // Set up player bullets
        bulletController = pawn.GetComponent<BulletController>();
        bulletController.AssignOwner(this);
        bulletController.StopShooting();

        // Set up player abilities
        abilityController = pawn.GetComponent<AbilityController>();
        abilityController.Deactivate(0);
        abilityController.Deactivate(1);

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

    // Switch action callback
    void OnSwitch()
    {
        manager.SwitchCharacters();
    }

    // ----
    // TEMP
    // ----

    [SerializeField] private int health = 10;

    public void Damage()
    {
        statsController.ModifyHealth(-10);
    }

    public void AddPoints()
    {
        score += 100;
        hudController.UpdateScore(score);
    }
}
