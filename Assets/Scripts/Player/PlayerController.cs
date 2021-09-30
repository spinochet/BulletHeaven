using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : PawnController
{
    public PlayerManager manager;

    // Character and player data
    private Color playerColor;
    private int score;

    // ---------------
    // EVENT CALLBACKS
    // ---------------

    // Move action callback
    void OnMove(InputValue input)
    {
        if (movementController)
            movementController.Move(input.Get<Vector2>());
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
        if (hudController)
            hudController.TogglePause();
    }

    // Switch action callback
    void OnSwitch()
    {
        if (manager)
            manager.SwitchCharacters();
    }

    // -----
    // SCORE
    // -----

    public void AddPoints(int points)
    {
        score += points;
        if (hudController)
            hudController.UpdateScore(score);
    }

    public void ResetPoints()
    {
        score = 0;
        if (hudController)
            hudController.UpdateScore(score);
    }
}
