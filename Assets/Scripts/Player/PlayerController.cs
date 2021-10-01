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

    void Awake()
    {
        DontDestroyOnLoad(this);
        
        Pawn[] pawns = GameObject.FindObjectsOfType(typeof(Pawn)) as Pawn[];
        foreach (Pawn pawn in pawns)
        {
            CompanionController ai = pawn.gameObject.GetComponent<CompanionController>();
            if (ai && ai.enabled)
            {
                ai.enabled = false;
                PossesPawn(pawn);
                break;
            }
        }
    }

    // Assign pawn to controller
    public void PossesPawn(Pawn _pawn)
    {
        pawn = _pawn;
        pawn.StopShooting();

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
        if (this.isLocalPlayer && pawn)
            pawn.Move(input.Get<Vector2>());
    }

    // -------------
    // COMBAT EVENTS
    // -------------

    // Shoot action callback
    void OnShoot(InputValue input)
    {
        bool isShooting = input.Get<float>() > 0.0f ? true : false;

        if (this.isLocalPlayer && pawn)
        {
            if (isShooting) pawn.StartShooting();
            else pawn.StopShooting();
        }
    }

    // AbilityL action callback function
    void OnAbilityL(InputValue input)
    {
        if (this.isLocalPlayer && pawn)
        {
            if (input.Get<float>() > 0.0f) pawn.ActivateAbility(0);
            else pawn.DeactivateAbility(0);
        }
    }

    // AbilityR action callback function
    void OnAbilityR(InputValue input)
    {
        if (this.isLocalPlayer && pawn)
        {
            if (input.Get<float>() > 0.0f) pawn.ActivateAbility(1);
            else pawn.DeactivateAbility(1);
        }
    }

    // Pause action callback function
    void OnPause()
    {
        if (this.isLocalPlayer && manager)
            manager.TogglePause(this);
    }

    // Switch action callback
    void OnSwitch()
    {
        if (this.isLocalPlayer && manager)
            manager.SwitchCharacters();
    }

    // -----
    // SCORE
    // -----

    public void ResetPoints()
    {
        // score = 0;
        // if (hudController)
        //     hudController.UpdateScore(score);
    }
}
