using System.Collections;
using System.Collections.Generic;

using Mirror;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : PawnController
{
    public PlayerNetworkManager manager;

    // Character and player data
    private Color playerColor;

    void Awake()
    {
        DontDestroyOnLoad(this);
        if (!manager) manager = GameObject.Find("PlayerManager").GetComponent<PlayerNetworkManager>();
    }

    void Start()
    {
        if (!manager) manager = GameObject.Find("PlayerManager").GetComponent<PlayerNetworkManager>();
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

    // Assign pawn to controller over the network
    [TargetRpc]
    public void TargetPossesPawn(NetworkIdentity pawnIdentity)
    {
        Debug.Log("In target posses");
        if (this.isLocalPlayer)
        {
            pawn = pawnIdentity.gameObject.GetComponent<Pawn>();
            pawn.StopShooting();

            // Switch input map
            PlayerInput input = GetComponent<PlayerInput>();
            if (input) input.SwitchCurrentActionMap("Gameplay");

            Debug.Log("MADE IT HERE!!");
        }
    }

    // DEBUG
    [TargetRpc]
    public void TargetPrint(int playerNum)
    {
        Debug.Log("PLAYER " + playerNum.ToString());
    }

    // ---------------
    // EVENT CALLBACKS
    // ---------------

    // Move action callback
    void OnMove(InputValue input)
    {
        if ((manager.mode == NetworkManagerMode.Offline || this.isLocalPlayer) && pawn)
            pawn.Move(input.Get<Vector2>());
    }

    // -------------
    // COMBAT EVENTS
    // -------------

    // Shoot action callback
    void OnShoot(InputValue input)
    {
        bool isShooting = input.Get<float>() > 0.0f ? true : false;

        if ((manager.mode == NetworkManagerMode.Offline || this.isLocalPlayer) && pawn)
        {
            if (isShooting) pawn.StartShooting();
            else pawn.StopShooting();
        }
    }

    // AbilityL action callback function
    void OnAbilityL(InputValue input)
    {
        if ((manager.mode == NetworkManagerMode.Offline || this.isLocalPlayer) && pawn)
        {
            if (input.Get<float>() > 0.0f) pawn.ActivateAbility(0);
            else pawn.DeactivateAbility(0);
        }
    }

    // AbilityR action callback function
    void OnAbilityR(InputValue input)
    {
        if ((manager.mode == NetworkManagerMode.Offline || this.isLocalPlayer) && pawn)
        {
            if (input.Get<float>() > 0.0f) pawn.ActivateAbility(1);
            else pawn.DeactivateAbility(1);
        }
    }

    // Pause action callback function
    void OnPause()
    {
        // if ((manager.mode == NetworkManagerMode.Offline || this.isLocalPlayer) && manager)
        //     manager.TogglePause(this);
    }

    // Switch action callback
    void OnSwitch()
    {
        // if ((manager.mode == NetworkManagerMode.Offline || this.isLocalPlayer) && manager)
        //     manager.SwitchCharacters();
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
