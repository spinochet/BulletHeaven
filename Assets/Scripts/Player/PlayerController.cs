using System.Collections;
using System.Collections.Generic;

using Mirror;

using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : PawnController
{
    // Character and player data
    private Color playerColor;
    private int score = 0;

    // Movement
    private Vector3 movement = Vector3.zero;
    private float speed = 10.0f;
    private bool canMove = true;

    // Stats
    private HUDController hud;

    // Combat
    private bool isShooting = false;
    private float fireTimer = 0.0f;

    // Abilities
    private bool isAbilityL;
    private bool isAbilityR;

    void Awake()
    {
        DontDestroyOnLoad(this);
    }

    void Update()
    {
        if (pawn)
        {
            // Movement
            pawn.Move(movement);

            // Combat
            fireTimer += Time.unscaledDeltaTime;
            if (isShooting && fireTimer > 1.0f / pawn.FireRate)
            {
                fireTimer = 0.0f;
                pawn.Shoot(transform.rotation);
            }
        }
    }

    public override void Destroy(Pawn _pawn)
    {
        if (_pawn.partner)
        {
            Pawn newPawn = _pawn.partner.GetComponent<Pawn>();
            newPawn.SetVisibility(true);
            TargetPossesPawn(newPawn.transform.GetComponent<NetworkIdentity>());
        }
        else
        {
            // Something something...
            // Game over...
            // Something something...
        }

        Destroy(_pawn.gameObject);
    }

    // -----
    // SETUP
    // -----

    // Assign pawn to controller over the network
    [TargetRpc]
    public void TargetPossesPawn(NetworkIdentity pawnIdentity)
    {
        if (this.isLocalPlayer)
        {
            pawn = pawnIdentity.gameObject.GetComponent<Pawn>();
            pawn.pawnController = this;
            
            if (hud)
                pawn.AssignHUD(hud);

            // Switch input map
            PlayerInput input = GetComponent<PlayerInput>();
            if (input) input.SwitchCurrentActionMap("Gameplay");
        }
    }

    // Assign player's HUD
    [TargetRpc]
    public void TargetAssignHUD(HUDController _hud)
    {
        hud = _hud;
    }

    // ---------------
    // EVENT CALLBACKS
    // ---------------

    // Move action callback
    void OnMove(InputValue input)
    {
        if (this.isLocalPlayer)
        {
            Vector2 inputVec = input.Get<Vector2>();
            movement.x = inputVec.x;
            movement.z = inputVec.y;
        }
    }

    // -------------
    // COMBAT EVENTS
    // -------------

    // Shoot action callback
    void OnShoot(InputValue input)
    {
        isShooting = input.Get<float>() > 0.0f ? true : false;
    }

    // --------------
    // ABILITY EVENTS
    // --------------

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
        // if ((manager.mode == NetworkManagerMode.Offline || this.isLocalPlayer) && manager)
        //     manager.TogglePause(this);
        LevelManager.Instance.TogglePause();
    }

    // Switch action callback
    void OnSwitch()
    {
        if (this.isLocalPlayer)
            Switch();
    }

    [Command(requiresAuthority = false)]
    void Switch()
    {
        PlayerNetworkManager.Instance.SwitchCharacters(pawn);
    }

    // -----
    // SCORE
    // -----

    public void AddPoints(int points)
    {
        score += points;
        hud.UpdateScore(score);
    }

    public void ResetPoints()
    {
        // score = 0;
        // if (hudController)
        //     hudController.UpdateScore(score);
    }
}
