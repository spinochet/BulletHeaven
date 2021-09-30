using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PawnController : MonoBehaviour
{
    // Controller scripts for player mechanics
    protected MovementController movementController;
    protected StatsController statsController;
    protected BulletController bulletController;
    protected AbilityController abilityController;
    protected HUDController hudController;

    // Assign pawn to controller
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
        bulletController.StopShooting();

        // Set up player abilities
        abilityController = pawn.GetComponent<AbilityController>();
        abilityController.Deactivate(0);
        abilityController.Deactivate(1);

        // Switch input map
        PlayerInput input = GetComponent<PlayerInput>();
        if (input) input.SwitchCurrentActionMap("Gameplay");
    }

    // --------
    // MOVEMENT
    // --------

    // Enable pawn movement
    public void EnableMovement()
    {
        if (movementController)
            movementController.enabled = true;
    }

    // Disable pawn movement
    public void DisableMovement()
    {
        if (movementController)
            movementController.enabled = false;
    }

    // ------
    // COMBAT
    // ------

    // Take damage
    public void Damage(float damage)
    {
        statsController.ModifyHealth(-damage);
    }
}
