using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PawnController : MonoBehaviour
{
    protected Pawn pawn;

    // Assign pawn to controller
    public void AssignPawn(Pawn _pawn)
    {
        pawn = _pawn;

        // Switch input map
        PlayerInput input = GetComponent<PlayerInput>();
        if (input) input.SwitchCurrentActionMap("Gameplay");
    }

    // Assign pawn to controller
    // public void AssignPawn(GameObject pawn, HUDController hud = null)
    // {
    //     // Set up player HUD
    //     if (hud != null)
    //         hudController = hud;
    //     if (hudController != null)
    //         hudController.UpdatePortrait(pawn.GetComponent<CharacterData>().portrait.texture);

    //     // Set up player movement
    //     movementController = pawn.GetComponent<MovementController>();

    //     // Set up player stats
    //     statsController = pawn.GetComponent<StatsController>();
    //     if (statsController) statsController.AssignHUD(hudController);

    //     // Set up player bullets
    //     bulletController = pawn.GetComponent<BulletController>();
    //     if (bulletController) bulletController.StopShooting();

    //     // Set up player abilities
    //     abilityController = pawn.GetComponent<AbilityController>();
    //     if (abilityController)
    //     {
    //         abilityController.Deactivate(0);
    //         abilityController.Deactivate(1);
    //     }
    // }

    public void TogglePause(bool isPaused)
    {
        
    }
}
